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

int main(int argc, char* argv[])
{
	try
	{
		//TODO: ver de hacer esto con una clase que maneje los parametros del programa
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

		const std::string address = argv[1];
		const std::string port = argv[2];
		const std::string docRoot = argv[3];

		// Initialise server.
		http::server::server s(address, port, docRoot);

		// Set console control handler to allow server to be stopped.
		console_ctrl_function = boost::bind(&http::server::server::stop, &s);
		SetConsoleCtrlHandler(console_ctrl_handler, TRUE);

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
