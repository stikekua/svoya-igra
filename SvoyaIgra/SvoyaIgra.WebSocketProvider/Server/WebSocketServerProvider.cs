using SvoyaIgra.WebSocketProvider.Server.Behaviors;
using log4net;
using WebSocketSharp.Server;

namespace SvoyaIgra.WebSocketProvider.Server
{
    public delegate void StartedEventHandler();
    public delegate void StoppedEventHandler();

    public delegate void EchoEventHandler(string message);
    public delegate void ClosedEventHandler(string message);
    public delegate void OpenedEventHandler(string message);
    public delegate void ErrorReceivedEventHandler(string message);
    public delegate void MessageReceivedEventHandler(string message);

    public class WebSocketServerProvider : IDisposable
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(WebSocketServerProvider));

        public event StartedEventHandler? Started;
        public event StoppedEventHandler? Stopped;

        public event EchoEventHandler? EchoMade;
        public event ClosedEventHandler? Closed;
        public event OpenedEventHandler? Opened;
        public event ErrorReceivedEventHandler? ErrorReceived;
        public event MessageReceivedEventHandler? Received;

        private readonly WebSocketServer _wssv;
        private string _wsUri = "ws://localhost:81";

        public WebSocketServerProvider(string uri) : this()
        {
            _wsUri = uri;
        }

        public WebSocketServerProvider()
        {
            _wssv = new WebSocketServer(_wsUri);
            _wssv.AddWebSocketService<MyWebSocketBehavior>("/", () => new MyWebSocketBehavior(this));
            _wssv.AddWebSocketService<Echo>("/Echo", () => new Echo(this));

        }

        public void Start()
        {
            _wssv.Start();
            if (_wssv.IsListening)
            {
                _log.Debug("The server started successfully");
                Started?.Invoke();
                foreach (var path in _wssv.WebSocketServices.Paths)
                {
                    _log.Debug($"\t{path}");
                }
            }
        }

        public void FireEvent(string eventType, string message)
        {
            switch (eventType)
            {
                case "EchoMade":
                    {
                        EchoMade?.Invoke(message);
                        break;
                    }
                case "Closed":
                    {
                        Closed?.Invoke(message);
                        break;
                    }
                case "Opened":
                    {
                        Opened?.Invoke(message);
                        break;
                    }
                case "ErrorReceived":
                    {
                        ErrorReceived?.Invoke(message);
                        break;
                    }
                case "Received":
                    {
                        Received?.Invoke(message);
                        break;
                    }
            }
        }

        public void Stop()
        {
            _wssv.Stop();
            Stopped?.Invoke();
            _log.Debug("The server stopped!");
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
