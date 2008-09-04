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
require 'uri'
require 'rtmp_packet'
require 'function_call'
require 'bandwidth_function'
require 'thread'

module RTMP

	class NetConnection
		Frame = Struct.new("Frame",:timer,:size,:data_type,:obj)

		def initialize()
			@bytes_out = 0
			@msg_out = 0
			@frames_in = {}
			@frames_out = {}
			@writing_queue = Queue.new
			@authorized = false
			@on_connect_action = Proc.new(){}
			@on_error_action = Proc.new(){|err|}
			@on_disconnect_action = Proc.new(){}
			@request_id = 1
			@request_array = {}
			@bandwidth_in = nil
			@bandwith_out = nil
			@end_point = nil
		end
		
		attr_accessor :end_point
		
		def get_last_timer(frame_num)
			Thread.critical = true
			if @last_timer_frames.nil?
				@last_timer_frames = {}
			end
			if ! @last_timer_frames.has_key? frame_num
				@last_timer_frames[frame_num] = (Time.new.to_f*1000).to_i
				dif_time = 0
			else
				n_time = (Time.new.to_f*1000).to_i
				dif_time = n_time - @last_timer_frames[frame_num]
				@last_timer_frames[frame_num] = n_time
			end
			Thread.critical = false
			return dif_time
		end
		
		def connect(uri,*args)
			uri_arr = URI.parse(uri)
			@uri = uri
			if uri_arr.scheme == 'rtmp'
				if uri_arr.port.nil?
					@port = '1935'
				else
					@port = uri_arr.port
				end
				@host = uri_arr.host
				@path = uri_arr.path
				if @path[0] == 0x2F
					@path = @path[1..-1]
				end
				@reading_thread = Thread.new do 
					begin
						@socket = TCPSocket.new(@host,@port)
						@socket.write("\03"+("a"*1536))
						@socket.read(1)
						qq = @socket.read(1536)
						@socket.write(qq)
						@socket.read(1536)
						t_amf = FunctionCall.new("connect",1,args,{"app"=>@path,"flashVer"=>"LNX 7.0.0","swfUrl"=>"http://toto.com/","tcUrl"=>@uri}).serialize
						l = t_amf.length
						if l >= 128
							t_amf = t_amf[0..127]+[195].pack("C")+t_amf[128..-1]
						end
						@request_array[1] = Proc.new do |f_obj|
							if f_obj.method == "_result"
								if f_obj.args[0]['code'] == "NetConnection.Connect.Success"
									process_writing
									on_connect
								else
									on_error(f_obj.args[0])
									disconnect
								end
							elsif f_obj.method == "_error"
								on_error(f_obj.args[0])
								disconnect
							end
						end
						@socket.write("\03"+setMediumInt(0)+setMediumInt(l)+[20].pack("C")+"\0\0\0\0"+t_amf)
						@connected = true
						while @connected
							begin
								pack = get_packet
								puts "pack"
							rescue Exception
								@connected = false
								disconnect
							else
								pack.extend BandwidthExtension
								pack.extend FunctionCallExtension
								case pack.data_type
								when 20
									t_func = pack.parsed_data
									t_func.request_id = t_func.request_id.to_i
									case t_func.method
									when "_result"
										on_returned_result(t_func)
									when "onStatus"
										on_returned_error(t_func)
									when "_error"
										on_returned_error(t_func)
									else
									end
								when 3
								
								when 4
									if !pack.parsed_data.response
										write_packet(RTMPPacket.new(2,get_last_timer(2),Ping.new(pack.parsed_data.value,true).serialize,4,0))
									end
								when 5
									@bandwidth_in = pack.parsed_data.value
								when 6
									@bandwidth_out = pack.parsed_data.value
									write_packet(RTMPPacket.new(2,get_last_timer(2),ServerBandwidth.new(@bandwidth_out).serialize,5,0))
								else
								end
							end
						end
					rescue Exception => obj
						puts obj
						puts obj.backtrace
					end
				end
			end
		end

		def on_returned_result(func)
			if func.request_id != 0
				if @request_array.has_key? func.request_id
					Thread.critical = true
					t_handle = @request_array.delete(func.request_id)
					Thread.critical = false
					t_handle.call(func)
				end
			end
		end
		
		def on_returned_error(func)
			if func.request_id != 0
				if @request_array.has_key? func.request_id
					Thread.critical = true
					t_handle = @request_array.delete(func.request_id)
					Thread.critical = false
					t_handle.call(func)
				end
			else
				on_error(func.args[0])
			end
		end
		
		def process_request(func)
			obj = @end_point
			meth_arr = method.split("/")
			begin
				until meth_arr.length <= 2
					m_name = meth_arr.shift.intern
					if obj.respond_to? m_name
						obj = obj.send(m_name)
					else
						raise StandardError, "property not found"
					end
				end
				if obj.respond_to? m_name
					res = obj.send(m_name,*func.args)
				else
					raise StandardError, "method not found"
				end
			rescue Exception => err
				t_func = FunctionCall.new("_error",func.request_id,[{level=>"error",code=>"NetConnection.Call.Failed",description=>err.to_s}])
			else
				t_func = FunctionCall.new("_result",func.request_id,[res])
			end
			write_packet(RTMPPacket.new(3,get_last_timer(3),t_func.serialize,20,0))
		end
		
		def send_function(func,frame=3,obj=0,&action)
			Thread.critical = true
				if action.nil?
					r_id = 0
				else
					@request_id += 1
					
					r_id = @request_id
					@request_array[r_id] = action
				end
			Thread.critical = false
			func.request_id = r_id
			write_packet(RTMPPacket.new(frame,get_last_timer(frame),func.serialize,20,obj))
		end
		
		def call(meth,*args,&action)
			act = action
			send_function(FunctionCall.new(meth.to_s,0,args)) do |obj|
				act.call(obj.args[0])
			end
		end
		
		def on_connect(&action)
			if action.nil?
				@on_connect_action.call()
			else
				@on_connect_action = action
			end
		end
		
		def on_error(err=nil,&action)
			if action.nil?
				@on_error_action.call(err)
			else
				@on_error_action = action
			end
		end
		
		def on_disconnect(&action)
			if action.nil?
				@on_disconnect_action.call()
			else
				@on_disconnect_action = action
			end
		end
		
		def get_packet
				f_byte = @socket.read(1)
				first_number = f_byte.unpack("C")[0]
				#puts first_number/16
				packet_type = first_number >> 6
				frame_number = first_number & 0x3F
				if frame_number == 0
					frame_number = @socket.read(1).unpack("C")[0]
				elsif frame_number == 1
					frame_number = @socket.read(2).unpack("n")[0]
				end
				if ! @frames_in.has_key? frame_number
					@frames_in[frame_number] = Frame.new(0,0,0,0)
					if packet_type != 0
						raise StandardError, "packet error"
					end
				end
				case packet_type
				when 0
					@frames_in[frame_number].timer = getMediumInt()
					@frames_in[frame_number].size = getMediumInt()
					@frames_in[frame_number].data_type = @socket.read(1).unpack("C")[0]
					@frames_in[frame_number].obj = @socket.read(4).unpack("N")[0]
				when 1
					@frames_in[frame_number].timer = getMediumInt()
					@frames_in[frame_number].size = getMediumInt()
					@frames_in[frame_number].data_type = @socket.read(1).unpack("C")[0]
				when 2
					@frames_in[frame_number].timer = getMediumInt()
				when 3
				
				else

				end
				packet_str = @socket.read(@frames_in[frame_number].size)
				return RTMPPacket.new(	frame_number,
							@frames_in[frame_number].timer,
							packet_str,
							@frames_in[frame_number].data_type,
							@frames_in[frame_number].obj)
		end
		def getMediumInt
			num_array = @socket.read(3).unpack("C*")
			num = num_array[0]<< 16 ^ num_array[1]<< 8 ^ num_array[2]
			return num
		end
		def setMediumInt(num)
			return [num].pack("N")[1,3]
		end

		def process_writing
			@writing_thread = Thread.new do
				begin
					while @connected do
						t_packet,to_close = @writing_queue.pop
						t_packet.size = t_packet.data.length
						if ! @frames_out.has_key? t_packet.frame
							@frames_out[t_packet.frame] = Frame.new(0,0,0,0)
							packet_type = 0
						else
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
						if t_f >= 64 && t_f <= 255
							packet_str = "" << [(packet_type*64)].pack('C') << [t_packet.frame].pack('C')
						elsif t_f > 255
							t_f = 1
							packet_str = "" << [((packet_type*64)+1)].pack('C') << [t_packet.frame].pack('n')
						elsif t_f < 2
							raise StandardError,"packet Error"
						else
							packet_str = "" << [((packet_type*64)+t_packet.frame)].pack('C')
						end
						case packet_type
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
						when 0
							t_frame.timer = t_packet.timer
							t_frame.data_type = t_packet.data_type
							t_frame.size = t_packet.size
							t_frame.obj = t_packet.obj
							packet_str << setMediumInt(t_packet.timer)
							packet_str << setMediumInt(t_packet.size)
							packet_str << t_packet.data_type
							packet_str << [t_packet.obj].pack("N")
						else
						
						end
						packet_str << t_packet.data
						@socket.write(packet_str)
						#@bytes_out += packet_str.length
						#@msg_out += 1
						if to_close
							disconnect
						end
					end
				rescue Exception => obj
					puts obj
					puts obj.backtrace
				end
			end
		end
		def write_packet (packet,to_close=false)
			if packet.class == RTMPPacket
				@writing_queue.push [packet,to_close]
			else
				raise "not a Packet !"
			end
		end
		
		def disconnect()
			@connected = false
			if !@writing_thread.nil?
				Thread.kill(@writing_thread) if Thread.current != @writing_thread
			end
			Thread.kill(@reading_thread) if Thread.current != @reading_thread
			@socket.close if not @socket.closed?
			on_disconnect()
		end
	end

end

net_co = RTMP::NetConnection.new

net_co.on_connect do 
	puts "connected"
end

net_co.on_disconnect do
	puts "disconnected"
end

net_co.on_error do |err|
	puts "rejected"
end

net_co.connect('rtmp://192.168.0.3/test')

net_co.call(:appendText,"qq","www") do |res|
	puts res
end

sleep