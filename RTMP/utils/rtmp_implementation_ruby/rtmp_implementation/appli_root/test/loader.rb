require 'thread'

class BaseApplication
	def toto
		return "hey !"
	end
end
class BaseClient
	def toto
		return "yo !"
	end
end

A_array = {}

A_array["zuzu"] = Module.new() do
	Active_Instances = {}
	class Application < BaseApplication
	
	end
	class Client < BaseClient
	
	end
	module_eval File.open("main.rb","r").read
end



def startInstance(app_class,inst_name)
	app_class = A_array[app_class]
	app  = app_class.const_get(:Application).new
	app_class.const_get(:Active_Instances)[inst_name] = Thread.new(app) do |application|
		begin
			application.onAppStart
			#application.serve
		rescue Exception => exp
			puts exp.class
			puts exp
		end
	end
end

startInstance("zuzu","aaaa")