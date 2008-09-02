/*
def server_loop(path, port)
  pool = IZUMI::StreamPool.new(path)

  case path.type
  when :directory
    IzumiLogger.info "Document Root:#{path.path}"
  when :file
    IzumiLogger.info "Target File:#{path.path}"
  end

  # disable DNS reverse lookup
  TCPSocket.do_not_reverse_lookup = true

  begin
    gs = TCPServer.open(port)
    IzumiLogger.info "Server started. Ver:#{RTMP::FmsVer} Pid:#{$$} Port:#{port}"
    loop do
      Thread.start(gs.accept) do |s|
        begin
          session = RTMP::Session.new(s, pool)
          session.do_session
        rescue => e
          puts "Exception caught: #{e}"
        ensure
          s.close
        end
      end
    end
  rescue Interrupt
  ensure
    gs.close
    IzumiLogger.info "Exit."
  end
end

if $0 == __FILE__
  path, options = parse_argv
  
  # setup logger 
  IzumiLogger = Logger.new(if options[:l] then options[:l] else STDERR end)
  IzumiLogger.level = if options[:v] == 1 then Logger::INFO else Logger::DEBUG end  
  
  if options[:d]
    puts "Warnning: Please specify log file in daemon mode." unless options[:l]
    daemon do
      server_loop(path, options[:p])
    end
  else
    server_loop(path, options[:p])
  end
end
*/



#include <iostream>
#include <string>
#include <boost/asio.hpp>
#include <boost/bind.hpp>
#include <boost/function.hpp>
#include "server.hpp"

#if defined(_WIN32)

boost::function0<void> console_ctrl_function;

BOOL WINAPI console_ctrl_handler(DWORD ctrl_type)
{
	switch (ctrl_type)
	{
		case CTRL_C_EVENT:
		case CTRL_BREAK_EVENT:
		case CTRL_CLOSE_EVENT:
		case CTRL_SHUTDOWN_EVENT:
			console_ctrl_function();
			return TRUE;
		default:
			return FALSE;
	}
}


void greeting()
{
	//if(zConf::def.verbose)  //TODO: Buscar un buen Manager de configuraciones
	//{
		std::cout << "STGS server v" << VERSION << " starting..." << std::endl;
		//std::cout << "Root Dir: " << zConf::def.root_dir << std::endl;
		std::cout << "Report bugs to fpelliccioni@gmail.com" << std::endl;
	//}
}


int main(int argc, char* argv[])
{
	try
	{
		//TODO: ver de hacer esto con una clase que maneje los parametros del programa.
		// Buscar un buen Manager de configuraciones
		// Check command line arguments.
		if (argc != 4)
		{
			std::cerr << "Usage: http_server <address> <port> <doc_root>\n";
			std::cerr << "  For IPv4, try:\n";
			std::cerr << "    http_server 0.0.0.0 80 .\n";
			std::cerr << "  For IPv6, try:\n";
			std::cerr << "    http_server 0::0 80 .\n";
			return 1;
		}



		// Initialise server.
		http::server::server s(argv[1], argv[2], argv[3]);

		// Set console control handler to allow server to be stopped.
		console_ctrl_function = boost::bind(&http::server::server::stop, &s);
		SetConsoleCtrlHandler(console_ctrl_handler, TRUE);


		greeting();

		// Run the server until stopped.
		s.run();
	}
	catch (std::exception& e)
	{
		std::cerr << "exception: " << e.what() << "\n";
	}

	return 0;
}

#endif // defined(_WIN32)
