############################################################################
#    Copyright (C) 2003/2004 by yannick connan                             #
#    yannick@dazzlebox.com                                                 #
#                                                                          #
#    This program is free software; you can redistribute it and#or modify  #
#    it under the terms of the GNU Library General Public License as       #
#    published by the Free Software Foundation; either version 2 of the    #
#    License, or (at your option) any later version.                       #
#                                                                          #
#    This program is distributed in the hope that it will be useful,       #
#    but WITHOUT ANY WARRANTY; without even the implied warranty of        #
#    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the         #
#    GNU General Public License for more details.                          #
#                                                                          #
#    You should have received a copy of the GNU Library General Public     #
#    License along with this program; if not, write to the                 #
#    Free Software Foundation, Inc.,                                       #
#    59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.             #
############################################################################
#!/usr/bin/env ruby

require 'socket'
require 'rtmp_packet'
require "thread"

module RTMP

	# TCPSocket extension
	module RTMPSession

		def connect(serv)
			@server = serv
			@serving = true
			@connected = false
			@on_packet_action = Proc.new(){}
			@frames_in = {}
			@frames_out = {}
			@on_disconnect_action = Proc.new(){}
			@bytes_in = 0
			@bytes_out = 0
			@msg_in = 0
			@msg_out = 0
			@msg_dopped = 0
			do_not_reverse_lookup = true
			@ip = addr[3]
			@protocol = @server.protocol
			@first_packet = nil
			@writing_queue = Queue.new
			@long_to_close = false
			process_reading
		end
		def to_close?
			return @long_to_close
		end
		def server=(serv)
			Thread.critical = true
			@server.unbind(self)
			@server = serv
			Thread.critical = false
		end
		attr_reader :bytes_in,:bytes_out,:first_packet,:msg_in,:msg_out,:msg_dropped,:ip,:protocol
		Frame = Struct.new("Frame",:timer,:size,:data_type,:obj)
		
		## read an UI24 from the session
		def getMediumInt
			num_array = read(3).unpack("C*")
			num = num_array[0]<< 16 ^ num_array[1]<< 8 ^ num_array[2]
			return num
		end
		
		## return an UI24 from a number
		def setMediumInt(num)
			return [num].pack("N")[1,3]
		end
		
		## get a RTMP_PACKET from the socket
		def get_packet
				first_number = read(1).unpack("C")[0]
				# get the 'mask' property
				packet_mask = first_number >> 6
				# get the 'frame1' property
				frame_number = first_number & 0x3F
				if frame_number == 0
					# if frame1 is equal to 0 then 'frame' is equal to 'frame2'
					frame_number = read(1).unpack("C")[0]
				elsif frame_number == 1
					# if frame1 is equal to 1 then 'frame' is equal to 'frame3'
					frame_number = read(2).unpack("n")[0]
				end
				# init a 'frame stream' if it doesn't exist yet
				if ! @frames_in.has_key? frame_number
					@frames_in[frame_number] = Frame.new(0,0,0,0)
					if packet_mask != 0
						raise StandardError, "packet error"
					end
				end
				# for logging purpose
				@bytes_in += 1
				
				# reads the 'time', 'datasize', 'rtmpdatatype' and 'streamid' properties from the socket
				# and put them into the 'frame stream' archive
				
				case packet_mask
				when 0
					@frames_in[frame_number].timer = getMediumInt()
					@frames_in[frame_number].size = getMediumInt()
					@frames_in[frame_number].data_type = read(1).unpack("C")[0]
					@frames_in[frame_number].obj = read(4).unpack("N")[0]
					@bytes_in += 11
				when 1
					@frames_in[frame_number].timer = getMediumInt()
					@frames_in[frame_number].size = getMediumInt()
					@frames_in[frame_number].data_type = read(1).unpack("C")[0]
					@bytes_in += 7
				when 2
					@frames_in[frame_number].timer = getMediumInt()
					@bytes_in += 3
				when 3
				
				else
				
				end
				# fix the CONNECTION_PACKET bug when its size is larger than 128 bytes (see caution 4.4.6)
				if ! @connected
					data_length = @frames_in[frame_number].size
				
					if data_length < 129
						data = read(data_length)
					elsif data_length == 129
						data = read(data_length+1)
						data = data[0..-2]
					else data_length > 129
						data = read(data_length+1)
						data = data[0..127] << data[129..-1]
					end
				else
					data = read(@frames_in[frame_number].size)
				end
				# for logging purpose
				@bytes_in += data.length
				@msg_in += 1
				# return a RTMP_PACKET with all its properties (implicit ones included)
				return RTMPPacket.new(	frame_number,
							@frames_in[frame_number].timer,
							data,
							@frames_in[frame_number].data_type,
							@frames_in[frame_number].obj)
		end
		# the main reading loop
		def process_reading
			# open a thread
			@reading_thread = Thread.new do
				begin
					# session handshake (see section 4.1)
					# gets the 'ClientFirstByte'
					f_byte = read(1)
					
					# gets the 'ClientHandcheckReceiving'
					c_handcheck = read(1536)
					
					# generates 'SERVER_HANDCHECK'
					s_handcheck = "a"*1536
					
					# sends 'ServerFirstByte', 'ServerHandcheckSending' and 'ClientHandcheckSending'
					write ( "\3" << s_handcheck << c_handcheck )
					
					# get the 'ServerHandcheckReceiving'
					read(1536)
					# handshake done
					
					# get the 'CONNECTION_PACKET'
					@first_packet = get_packet
					@connected = true
					# starts the writing loop
					process_writing
	
					# process the 'CONNECTION_PACKET' and notice the server
					@server.on_session(self)
					
					# enter the reading loop
					while @connected do
						# fires an event each time the session receives a packet
						on_packet(get_packet)
					end
				rescue Exception => obj
					disconnect(obj)
				end
			end
		end
		
		# the main writing loop
		
		def process_writing
			@writing_thread = Thread.new do
				begin
					while @connected do
						# gets the next RTMP_PACKET to write from the event queue
						t_packet,to_close = @writing_queue.pop
						t_packet.size = t_packet.data.length
						# init a 'frame stream' if it doesn't exist yet
						if ! @frames_out.has_key? t_packet.frame
							@frames_out[t_packet.frame] = Frame.new(0,0,0,0)
							packet_mask = 0
						else
							
							# determine the 'mask' property
							t_frame = @frames_out[t_packet.frame]
							packet_mask = 0
							if (t_frame.obj == t_packet.obj)
								packet_mask += 1 
							end
							if (t_frame.data_type == t_packet.data_type) and (t_frame.size == t_packet.size)
								packet_mask += 1 
								if (t_frame.timer == t_packet.timer)
									packet_mask += 1 
								end
							end
						end
						t_frame = @frames_out[t_packet.frame]

						t_f = t_packet.frame
						
						# writes the 'frame' property, can be 'frame1' or 'frame1'+'frame2' or 'frame1'+'frame3'
						if t_f >= 64 && t_f <= 255
							packet_str = "" << [(packet_mask*64)].pack('C') << [t_packet.frame].pack('C')
						elsif t_f > 255
							t_f = 1
							packet_str = "" << [((packet_mask*64)+1)].pack('C') << [t_packet.frame].pack('n')
						elsif t_f < 2
							raise StandardError,"packet Error"
						else
							packet_str = "" << [((packet_mask*64)+t_packet.frame)].pack('C')
						end
						
						# writes the 'time', 'datasize', 'rtmpdatatype' and 'streamid' properties to the socket
						# and update the 'frame stream' last values
						case packet_mask
						when 2
							t_frame.timer = t_packet.timer
							packet_str << setMediumInt(t_packet.timer)
						when 1
							t_frame.timer = t_packet.timer
							t_frame.data_type = t_packet.data_type
							t_frame.size = t_packet.size
							packet_str << setMediumInt(t_packet.timer)
							packet_str << setMediumInt(t_packet.size)
							packet_str << t_packet.data_type
						else
							t_frame.timer = t_packet.timer
							t_frame.data_type = t_packet.data_type
							t_frame.size = t_packet.size
							t_frame.obj = t_packet.obj
							packet_str << setMediumInt(t_packet.timer)
							packet_str << setMediumInt(t_packet.size)
							packet_str << t_packet.data_type
							packet_str << [t_packet.obj].pack("N")
						end
						
						packet_str << t_packet.data
						# write the 'rtmpdata' property to the socket
						write(packet_str)
						
						
						@bytes_out += packet_str.length
						@msg_out += 1
						
						# disconnect if asked to
						if to_close
							disconnect
						end
					end
				rescue Exception => obj
					disconnect(obj)
				end
			end
		end
		def write_packet (packet,to_close=false)
			if packet.class == RTMPPacket
				if not @long_to_close
					@writing_queue.push [packet,to_close]
					if to_close
						@long_to_close = true
					end
					return true
				else
					return false
				end
			else
				raise "not a Packet !"
			end
		end
		def on_packet(pack)
			@on_packet_action.call(pack)
		end
		def each_packet (&action)
			@on_packet_action = action
		end
		def disconnect(reason=nil)
			@long_to_close = true
			@connected = false
			Thread.kill(@writing_thread) if Thread.current != @writing_thread
			Thread.kill(@reading_thread) if Thread.current != @reading_thread
			close if not closed?
			on_disconnect(reason)
			@server.unbind self
		end
		def on_disconnect(reason=nil,&action)
			if action.nil?
				@on_disconnect_action.call(self,reason)
			else
				@on_disconnect_action = action
			end
		end
	end
end