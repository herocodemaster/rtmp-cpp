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

require 'rtmp_session'
require 'thread'
require 'function_call'
require 'bandwidth_function'
require 'messages'

module RTMP

	module ComSession
		def com_init(app,cli)
			@app_container = app
			
			@client = cli
			@request_array = {}
			@request_id = 1
		end
		attr_reader :client,:app_connected,:request_array
		
		
		def on_packet(pack)
			pack.extend BandwidthExtension
			pack.extend FunctionCallExtension
			pack.parsed_data
			case pack.data_type
			when 20
				@app_container.pass_packet(self,pack)
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

		def on_disconnect(reason=nil)
			@app_container.pass_disconnect(self,reason)
		end
		
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
		
		def send_function(func,frame=3,obj=0,&action)
			Thread.critical = true
				if action.nil?
					func.request_id = r_id if func.request_id.nil?
				else
					@request_id += 1
					
					func.request_id = @request_id
					@request_array[r_id] = action
				end
			Thread.critical = false
			t_f = func.serialize
			write_packet(RTMPPacket.new(frame,get_last_timer(frame),t_f,20,obj))
		end
		
		def call(meth,*args,&action)
			act = action
			send_function(FunctionCall.new(meth.to_s,0,args)) do |obj|
				act.call(obj.args[0])
			end
		end
	end
end