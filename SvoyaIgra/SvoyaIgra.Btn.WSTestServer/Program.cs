using log4net;
using log4net.Config;
using SvoyaIgra.WebSocketProvider.Server;

namespace SvoyaIgra.Btn.WSTestServer
{
    public class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Program));

        public static void Main(string[] args)
        {
            // Configure file logging log4net 
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs/log4net.config");
            XmlConfigurator.ConfigureAndWatch(new FileInfo(path));

            _log.Info("Starting application...");

            var server = new WebSocketServerProvider();
            server.Started += Server_Started;
            server.Stopped += Server_Stopped;
            server.EchoMade += Server_EchoMade;
            server.Opened += Server_Opened;
            server.Closed += Server_Closed;
            server.ErrorReceived += Server_ErrorReceived;
            server.Received += Server_Received;

            server.Start();

            Console.WriteLine("Press 'q' key to stop server.");
            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }
            server.Stop();

            _log.Info("Closing application...");
        }

        private static void Server_Received(string message)
        {
            Console.WriteLine($"Received: {message}");
        }

        private static void Server_ErrorReceived(string message)
        {
            Console.WriteLine($"ErrorReceived: {message}");
        }

        private static void Server_Closed(string message)
        {
            Console.WriteLine($"Closed: {message}");
        }

        private static void Server_Opened(string message)
        {
            Console.WriteLine($"Opened: {message}");
        }

        private static void Server_EchoMade(string message)
        {
            Console.WriteLine($"Echo: {message}");
        }

        private static void Server_Stopped()
        {
            Console.WriteLine("The server stopped.");
        }

        private static void Server_Started()
        {
            Console.WriteLine("The server started successfully.");
        }
    }
}