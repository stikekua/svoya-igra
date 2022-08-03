using log4net;
using System;
using WebSocketSharp;

namespace SvoyaIgra.WebSocketProvider.Client
{
    public delegate void ClosedEventHandler();
    public delegate void OpenedEventHandler();
    public delegate void ErrorEventHandler(string message);
    public delegate void MessageReceivedEventHandler(string message);

    public class WebSocketClientProvider : IDisposable
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(WebSocketClientProvider));

        public event ClosedEventHandler? Closed;
        public event OpenedEventHandler? Opened;
        public event ErrorEventHandler? Error;
        public event MessageReceivedEventHandler? Received;

        private readonly WebSocket _ws;
        private string _wsUri = "ws://localhost:81";

        public WebSocketClientProvider(string uri) : this()
        {
            _wsUri = uri;
        }

        public WebSocketClientProvider()
        {
            _ws = new WebSocket(_wsUri);

            _ws.OnOpen += OnWebsocketOpen;
            _ws.OnClose += OnWebsocketClose;
            _ws.OnError += OnWebsocketError;
            _ws.OnMessage += OnWebsocketReceived;
        }

        public bool Connect()
        {
            try
            {
                _log.Debug($"Try to connect to the server {_wsUri}");
                _ws.Connect();
                _log.Debug($"Conneected to the server {_wsUri}");
                return true;
            }
            catch (Exception e)
            {
                _log.Error($"Error during connect to the server", e);
                return false;
            }
        }

        public bool Send(string message)
        {
            try
            {
                _log.Debug($"Try to send message: {message}");
                _ws.Send(message);
                return true;
            }
            catch (Exception e)
            {
                _log.Error($"Error during send to the server", e);
                Error?.Invoke(e.Message);
                return false;
            }

        }

        private void OnWebsocketOpen(object sender, EventArgs e)
        {
            _log.Info($"Websocket opened");
            Opened?.Invoke();
        }

        private void OnWebsocketClose(object sender, CloseEventArgs e)
        {
            _log.Info($"Websocket closed");
            Closed?.Invoke();
        }

        private void OnWebsocketError(object sender, WebSocketSharp.ErrorEventArgs e)
        {
            _log.Error($"Websocket error", e.Exception);
            Error?.Invoke(e.Message);
        }

        private void OnWebsocketReceived(object sender, MessageEventArgs e)
        {
            _log.Debug($"Websocket received: {e.Data}");
            Received?.Invoke(e.Data);
        }

        public void Dispose()
        {
            _log.Debug("Closing WS connection...");
            _ws.Close();
            _log.Debug("Closed WS connection.");
        }
    }
}
