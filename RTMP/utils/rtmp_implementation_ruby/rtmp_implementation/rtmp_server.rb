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
require 'rtmp_session'
require 'thread'

module RTMP

	class RTMPServer < TCPServer
		attr_reader :bytes_in,:bytes_out,:serving,:protocol
		def initialize(*args)
			@sockets = []
			@on_session_action = Proc.new(){}
			@bytes_out = 0
			@bytes_in = 0
			@start_time = nil
			@protocol = "rtmp"
			@on_session_queue = Queue.new
			
			#init the TCPServer
			super(*args)
		end
		def on_session(session)
			@on_session_queue.push session
		end
		def each_session (&action)
			while @serving do
				sess = @on_session_queue.pop
				action.call(sess)
			end
		end
		def unbind(sess)
			@sockets.delete sess
		end
		
		def serve
			@start_time = Time.new
			@serving = true
			@thread = Thread.new do
				begin
					# start the serving loop
					while @serving do
						# wait for a new TCPSession
						session = accept
						# extend the new TCPSession to a RTMPSession (see rtmp_session.rb)
						session.extend RTMPSession
						#init the RTMPSession extension
						session.connect(self)
					end
				rescue Interrupt
					shutdown
				end
			end
		end
		def shutdown
			@serving = false
			@sockets.each do |session|
				session.disconnect
			end
		end
	end

end
