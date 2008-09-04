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
require 'base_application'
require 'base_client'
require 'rtmp_server'
require 'function_call'
require 'bandwidth_function'
require 'thread'
require 'uri'

module RTMP

	class VirtualHost
		def initialize(id_obj=nil)
			@id_obj = id_obj
			@appli_array = []
			@default_appli = nil
			@sess_queue = Queue.new
			@continue = true
			@thread = Thread.new do
				while @continue
					begin
					t_sess = @sess_queue.pop
					t_app = t_sess.first_packet.parsed_data.first_arg['app']
					session_loop(t_sess,t_app)
					rescue StandardError => obj
						puts obj.inspect
						puts obj.backtrace
					end
				end
			end
		end
		
		def session_loop(sess,app)
			@appli_array.each do |obj|
				if obj.correct? app
					obj.process_session(sess)
					return nil
				end
			end
			if @default_appli.nil?
				raise StandardError, "no default application defined"
			else
				@default_appli.process_session(sess)
			end
		end
		
		def append_appli_registar(appli_registar,default=false)
			if default
				@default_appli = appli_registar
			else
				@appli_array.push appli_registar
			end
		end
		
		def process_session(sess)
			@sess_queue.push sess
		end
		
		def correct?(t_name)
			if @obj_id == t_name
				return true
			else
				return false
			end
		end
	end
			
	class AppliClassContainer
		def initialize(eval_str)
			@active_instances = {}
			@module = Module.new()
			@module.module_eval %q{
				class Application < RTMP::BaseApplication
						
				end
				class Client < RTMP::BaseClient
						
				end
			}
			@module.module_eval eval_str
			#end
		end
		def instanced?(key)
			hh = @active_instances.has_key? key
			if hh
				if @active_instances[key].application.running?
					return true
				else
					@active_instances.delete key
					return false
				end
			else
				return false
			end
		end
		def [](key)
			return @active_instances[key]
		end
		def init_instance(inst_name,n_name)
			@active_instances[inst_name] = AppliInstanceContainer.new(@module,n_name)
		end
	end
	
	class AppliInstanceContainer
		def initialize(mod,n_name)
			@module = mod
			@app = @module.const_get(:Application).new(self)
			@app.onAppStart
			@running = true
			@queue = Queue.new
			@pre_clients = []
			@connected_clients = []
			@connected_sessions = []
			@request_array = {}
			@name = n_name
			@host = nil
			@thread = Thread.new(@app) do |application|
				serve
			end 
		end
		attr_reader :connected_clients,:connected_sessions, :pre_clients,:name,:host
		def running?
			return @running
		end
		
		def application
			return @app
		end
		def find_session(cli)
			@connected_session.each do |sess|
				if sess.client == cli
					return sess
				end
			end
			return nil
		end
		
		def on_connect(sess)
			args = [sess.client]+ sess.first_packet.parsed_data.args
			l = args.length
			if @app.respond_to? :onConnect
				num_args = @app.method(:onConnect).arity
				if (l == num_args || num_args == -1)||(num_args < -1 && (-l+1) > num_args)
				
					return @app.onConnect(*args)
				elsif l > num_args
					return @app.onConnect(*args[0..num_args])
				else
					return false
				end
			else
				return nil
			end
		end

		
		def refuse_connection(sess)
			@app.onConnectReject(sess.client,*sess.first_packet.parsed_data.args)
			frame = 3
			timer = 0
			data_type = 20
			obj = 0
			request_id = sess.first_packet.parsed_data.request_id
			return_string = RTMP::FunctionCall.new("_result", request_id,[Messages::Rejected]).serialize
			sess.write_packet(RTMP::RTMPPacket.new(frame,timer,return_string,data_type,obj),true)
		end
		
		def accept_connection(sess)
			frame = 3
			timer = 0
			data_type = 20
			obj = 0
			request_id = sess.first_packet.parsed_data.request_id
			return_string = RTMP::FunctionCall.new("_result", request_id,[Messages::Success]).serialize
			sess.write_packet(RTMP::RTMPPacket.new(frame,timer,return_string,data_type,obj))
			Thread.critical = true
			@pre_clients.delete sess
			@connected_clients.push sess.client
			@connected_sessions.push sess
			Thread.critical = false
			@app.onConnectAccept(sess.client,*sess.first_packet.parsed_data.args)
		end
		
		def connect_session(sess)
			sess.extend RTMP::ComSession
			cli = @module.const_get(:Client).new(self,sess)
			sess.com_init(self,cli)
			@pre_clients.push sess
			@queue.push [:connect,sess,nil]
		end
		
		def pass_packet(sess,pack)
			@queue.push [:packet,sess,pack]
		end
		
		def pass_disconnect(sess,reason)
			@queue.push [:disconnect,sess,reason]
		end
		def shutdown(urgent=false)
			if urgent
				@running = false
				@thread.exit
				@app.onAppStop
			else
				@queue.push [:shutdown,nil,nil]
			end
		end
		def on_returned_result(sess,func)
			if func.request_id != 0
				if sess.request_array.has_key? func.request_id
					Thread.critical = true
					t_handle = sess.request_array.delete(func.request_id)
					Thread.critical = false
					t_handle.call(func)
				end
			end
		end

		def on_returned_error(func)
			if func.request_id != 0
				if sess.request_array.has_key? func.request_id
					Thread.critical = true
					t_handle = sess.request_array.delete(func.request_id)
					Thread.critical = false
					t_handle.call(func)
				end
			else
				@app.onStatus(func.args[0])
			end
		end
		
		FORBIDDEN_METHODS = [:send,:call]
		
		def process_request(sess,func)
			puts sess.client.public_methods
			obj = sess.client
			meth_arr = func.method.split("/")
			begin
				while meth_arr.length > 1
					m_name = meth_arr.shift.intern
					if obj.respond_to? m_name and not FORBIDDEN_METHODS.include? m_name
						obj = obj.send(m_name)
					else
						raise StandardError, "property not found"
					end
					
				end
				m_name = meth_arr.shift.intern
				if obj.respond_to? m_name and not FORBIDDEN_METHODS.include? m_name
					res = obj.send(m_name,*func.args)
				else
					raise StandardError, "method not found"
				end
			rescue Exception => err
				t_func = FunctionCall.new("_error",func.request_id,[{'level'=>"error",'code'=>"NetConnection.Call.Failed",'description'=>err.to_s}])
			else
				t_func = FunctionCall.new("_result",func.request_id,[res])
			end
			sess.send_function(t_func)
		end
		
		def serve
			while @running
				message,sess,obj = @queue.pop
				begin
					case message
					
					when :connect
						ret_val = on_connect(sess)
						if ret_val == true
							accept_connection(sess)
						elsif ret_val == false
							refuse_connection(sess)
						end
					when :shutdown
						@running = false
						@app.onAppStop
					when :disconnect
						if @pre_clients.include?(sess)
							@pre_clients.delete(sess) 
						else
							@connected_clients.delete(sess.client)
							@connected_sessions.delete(sess)
							
						end
						puts "session disconnected"
						if not obj.nil?
							puts "reason : "<< obj
							#puts obj.backtrace
						end
						@app.onDisconnect(sess.client)
					when :packet
						t_func = obj.parsed_data
						t_func.request_id = t_func.request_id.to_i
						case t_func.method
						when "_result"
							on_returned_result(sess,t_func)
						when "onStatus"
							on_returned_error(sess,t_func)
						when "_error"
							on_returned_error(sess,t_func)
						else
 							process_request(sess,t_func)
						end
					else
					end
				rescue Exception => obj
					puts obj.class
					puts obj
					puts obj.backtrace
					shutdown(true)
				else
				
				end
			end
		end
	end
	
	class AppliRootRegistar
		
		def initialize(id_obj,root_path)
			@id_obj = id_obj
			@root_path = root_path
			@application_class = {}
		end

		
		def process_session(sess)
			t_path = sess.first_packet.parsed_data.first_arg['app']
			
			f_path = t_path.split("/")
			full_path = @root_path+"/"+f_path[0]+"/main.rb"
			if f_path.length > 0
				if !@application_class.has_key? f_path[0]
					if File.exist? full_path
						if File.readable? full_path
							class_str = File.open(full_path,"r").read
							@application_class[f_path[0]] = AppliClassContainer.new(class_str)
						else
							raise StandardError,"path not readable"
						end
					else
						raise StandardError,"wrong path"
					end
				end
				if f_path.length == 1
					f_path[1] = "__default__"
				end
				if ! @application_class[f_path[0]].instanced? f_path[1]
					@application_class[f_path[0]].init_instance(f_path[1],t_path)
				end
				@application_class[f_path[0]][f_path[1]].connect_session sess
			else
				raise StandardError,"empty path"
			end
		end
		
		def correct?(t_name)
			if t_name == @id_obj
				return true
			else
				return false
			end
		end
	end

	class Registar
	
		def initialize()
			@virtual_hosts = []
			@default_virtual_host = nil
		end
		
		def append_virtual_host(virtual_host,default=false)
			if default
				@default_virtual_host = virtual_host
			else
				@virtual_hosts.push virtual_host
			end
		end
		
		def process_session(sess)
			t_host = URI.parse(sess.first_packet.parsed_data.first_arg['tcUrl']).host
			@virtual_hosts.each do |obj|
				obj.correct? t_host
				obj.process_session(sess)
				return nil
			end
			if @default_virtual_host.nil?
				raise StandardError, "no default virtual host defined"
			else
				@default_virtual_host.process_session(sess)
			end
		end
	end

end
