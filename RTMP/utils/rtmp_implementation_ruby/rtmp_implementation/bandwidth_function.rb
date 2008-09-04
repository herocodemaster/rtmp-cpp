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


module RTMP

	class Ping
		def initialize (value,response)
			@value = value
			@response = response
		end
		def _load (t_data)
			n_data = StringIO.new t_data
			@response = n_data.read(2).unpack('n')[0]
			@value = n_data.read(4).unpack('N')[0]
			if @response == 6
				@response = false
			else
				@response = true
			end
		end
		def serialize
			if @response
				num = 7
			else
				num = 6
			end
			return [num,@value].pack('nN')
		end
		def to_s
			t_str = "value : "<<@value.to_s<<"\n"
			t_str << "response : "<<@response.to_s<<"\n"
		end
		attr_writer :value,:response
		attr_reader :value,:response
	end

	class ClientBandwidth
		def initialize (value,mist=nil)
			@value = value
			@mist = mist
		end
		def _load (t_data)
			n_data = StringIO.new t_data
			@value = n_data.read(4).unpack('N')[0]
			@mist = n_data.read(1).unpack('C')[0]
		end
		def serialize
			return [@value,@mist].pack('NC')
		end
		def to_s
			t_str = "value : "<<@value.to_s<<"\n"
			t_str << "mist : "<<@mist.to_s<<"\n"
		end
		attr_writer :value,:mist
		attr_reader :value,:mist
	end


	class ServerBandwidth
		def initialize (value)
			@value = value
		end
		def _load (t_data)
			n_data = StringIO.new t_data
			@value = n_data.read(4).unpack('N')[0]
		end
		def serialize
			return [@value].pack('N')
		end
		def to_s
			t_str = "value : "<<@value.to_s<<"\n"
			return t_str
		end
		attr_writer :value
		attr_reader :value
	end
	
	class Mistery
		def initialize (value)
			@value = value
		end
		def _load (t_data)
			n_data = StringIO.new t_data
			@value = n_data.read(4).unpack('N')[0]
		end
		def serialize
			return [@value].pack('N')
		end
		def to_s
			t_str = "value : "<<@value.to_s<<"\n"
			return t_str
		end
		attr_writer :value
		attr_reader :value
	end
	

	module BandwidthExtension
		def BandwidthExtension.extend_object(obj)
			if obj.respond_to? :register_datatype
				super
				obj.register_datatype(5) do |t_data|
					n_func = ServerBandwidth.allocate
					n_func._load t_data
					n_func
				end
				obj.register_datatype(6) do |t_data|
					n_func = ClientBandwidth.allocate
					n_func._load t_data
					n_func
				end
				obj.register_datatype(3) do |t_data|
					n_func = Mistery.allocate
					n_func._load t_data
					n_func
				end
				obj.register_datatype(4) do |t_data|
					n_func = Ping.allocate
					n_func._load t_data
					n_func
				end
			else
				raise "this object doesn't have the \"register_datatype\" method"
			end
		
		end
	end

end