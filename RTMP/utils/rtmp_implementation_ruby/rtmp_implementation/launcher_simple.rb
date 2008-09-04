require 'rtmp_server'
require 'function_call'
require 'bandwidth_function'

server = RTMP::RTMPServer.new("0.0.0.0",1935)
server.serve
# iterator called each time the server receives a new session
server.each_session do |session|
	#extend this RTMPPacket (used by RTMPPacket#parsed_data)
	session.first_packet.extend RTMP::FunctionCallExtension
	f_function_call = session.first_packet.parsed_data
	
	$stdout.puts "new session"
	
	# define what to do each time this session receives a new packet
	def session.on_packet(packet)
	
		#extend this RTMPPacket (used by RTMPPacket#parsed_data)
		packet.extend RTMP::BandwidthExtension
		packet.extend RTMP::FunctionCallExtension
		$stdout.puts "on packet "+packet.data_type.to_s
	end
	
	# define what to do each time this session disconnect
	session.on_disconnect do |session,error|
		$stdout.puts error.class
		$stdout.puts error
		$stdout.puts error.backtrace
	end
	
	# accept the new session, send an 'ACCEPT_PACKET' (see section 4.4.7)
	
	# make a FunctionCall object
	method_name = "_result"
	request_id = f_function_call.request_id
	connection_status = 	{
					"level"=>"status",
					"code"=>"NetConnection.Connect.Success",
					"description"=>"Connection succeeded."
				}
	arguments = [connection_status]
	
	# serialize this FunctionCall object to a string
	return_string = RTMP::FunctionCall.new(method_name, request_id, arguments).serialize
	
	# make the 'ACCEPT_PACKET' and send it to the client
	frame = 3
	timer = 0
	data_type = 20
	obj = 0
	session.write_packet(RTMP::RTMPPacket.new(frame,timer,return_string,data_type,obj))
end



