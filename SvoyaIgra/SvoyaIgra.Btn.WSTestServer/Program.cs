using System.Text.Json;
using log4net;
using log4net.Config;
using SvoyaIgra.Shared.Constants;
using SvoyaIgra.WebSocketProvider.Entities;
using SvoyaIgra.WebSocketProvider.Server;

namespace SvoyaIgra.Btn.WSTestServer
{
    public class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Program));

        private static WebSocketServerProvider server;

        public static void Main(string[] args)
        {
            // Configure file logging log4net 
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs/log4net.config");
            XmlConfigurator.ConfigureAndWatch(new FileInfo(path));

            _log.Info("Starting application...");

            server = new WebSocketServerProvider();
            server.Started += Server_Started;
            server.Stopped += Server_Stopped;
            server.EchoMade += Server_EchoMade;
            server.Opened += Server_Opened;
            server.Closed += Server_Closed;
            server.ErrorReceived += Server_ErrorReceived;
            server.Received += Server_Received;

            server.Start();

            Console.WriteLine("Press 'q' key to stop server.\n");
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
            var msg = JsonSerializer.Deserialize<Message>(message);

            Console.WriteLine($"Received: {msg.ForLog}");
            
            QueueOperator.ProcessMessage(msg.Data);

            //send actulal status
            var wsMsg = QueueOperator.MakeWsMessage();
            Console.WriteLine(wsMsg);
            server.Brodcast(wsMsg);

            Console.WriteLine();
        }

        private static void Server_ErrorReceived(string message)
        {
            var msg = JsonSerializer.Deserialize<Message>(message);

            Console.WriteLine($"ErrorReceived: {msg?.ForLog}");
        }

        private static void Server_Closed(string message)
        {
            var msg = JsonSerializer.Deserialize<Message>(message);

            Console.WriteLine($"Closed: {msg?.ForLog}");
        }

        private static void Server_Opened(string message)
        {
            var msg = JsonSerializer.Deserialize<Message>(message);

            Console.WriteLine($"Opened: {msg?.ForLog}");
        }

        private static void Server_EchoMade(string message)
        {
            var msg = JsonSerializer.Deserialize<Message>(message);

            Console.WriteLine($"Echo: {msg?.ForLog}");
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