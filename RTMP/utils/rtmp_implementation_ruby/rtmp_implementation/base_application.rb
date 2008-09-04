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

module RTMP

	class BaseApplication
		def initialize(app)
			@__app__ = app
		end
		def acceptConnection 
			
		end
		def clients
			return @__app__.connected_clients.dup
		end
		def disconnect(cli) 
			if @__app__.connected_clients.include? self
				@__app__.connected_clients.delete self
				sess = @__app__.find_session(self)
				if not sess.nil?
					@__app__.connected_sessions.delete sess
					sess.disconnect
				end
				return true
			else
				return false
			end
		end
		def hostname 
			return @__app__.host
		end
		def name 
			return @__app__.name
		end
		def onAppStart 
			
		end
		def onAppStop 
			
		end
		def onConnect(*args)
			return true
		end
		def onConnectAccept(cli)
			
		end
		def onConnectReject(cli) 
			
		end
		def onDisconnect(cli) 
			
		end
		def onStatus(err)
			
		end
		def registerClass 
			
		end
		def registerProxy 
			
		end
		def rejectConnection 
			
		end
		def server 
			return "RTMP4Ruby 0.1 server"
		end
		def running?
			return true
		end
	end
end