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

require 'com_session'
require 'rtmp_server'
require 'function_call'
require 'bandwidth_function'

module RTMP

	class ComServer < RTMPServer
		#attr_reader :bytes_in,:bytes_out,:serving,:protocol
		def initialize(reg,*args)
			@registar = reg
			super(*args)
		end
		def on_session(session)
			session.first_packet.extend RTMP::FunctionCallExtension
			@registar.process_session(session)
		end
	end

end
