require 'rtmp_server'
require 'function_call'
require 'bandwidth_function'
require 'com_registar'
require 'com_server'
require 'rexml/document'

config_file = REXML::Document.new File.new('config.xml')
default_registar = nil
registars = {}
config_file.elements.each("root/registars/registar") do |reg_doc|
	r_id = reg_doc.attributes["id"]
	r_reg = RTMP::Registar.new
	reg_doc.elements.each("virtualhost") do |vr_doc|
		
		t_vr = RTMP::VirtualHost.new vr_doc.attributes["path"]
		vr_doc.elements.each do |appli_doc|
			case appli_doc.name
			
			when 'appliRoot'
				t_appli = RTMP::AppliRootRegistar.new(appli_doc.attributes["path"],appli_doc.attributes["root"])
				t_vr.append_appli_registar(t_appli,appli_doc.attributes["path"].nil?)
			else
			
			end
		end
		r_reg.append_virtual_host(t_vr,vr_doc.attributes["path"].nil?)
	end
	if r_id.nil?
		default_registar = r_reg
	else
		registars[r_id] = r_reg
	end
end

servers = []

config_file.elements.each("root/servers") do |srv_doc|
	srv_doc.elements.each() do |s_doc|
	
		case s_doc.name
			
		when 'RTMPserver'
			if s_doc.attributes["host"].nil?
				t_host = "0.0.0.0"
			else
				t_host = s_doc.attributes["host"]
			end
			if s_doc.attributes["port"].nil?
				t_port = 1935
			else
				t_port = s_doc.attributes["port"].to_i
			end
			if s_doc.attributes["registar"].nil?
				t_reg = default_registar
			else
				t_reg = registars[s_doc.attributes["registar"]]
			end
			t_srv = RTMP::ComServer.new(t_reg,t_host,t_port)
			t_srv.serve
			servers.push t_srv
		else
			
		end
	end
end
sleep
