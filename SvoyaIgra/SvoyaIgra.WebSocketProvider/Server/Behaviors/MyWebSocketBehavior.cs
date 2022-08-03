using log4net;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace SvoyaIgra.WebSocketProvider.Server.Behaviors
{
    public class MyWebSocketBehavior : WebSocketBehavior
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(MyWebSocketBehavior));

        private WebSocketServerProvider _instance;
        public MyWebSocketBehavior(WebSocketServerProvider instance)
        {
            _instance = instance;
        }       

        protected override void OnOpen()
        {
            var clientAddress = Context.UserEndPoint.Address;
            var msg = $"{ID}: Opened connection. Address: {clientAddress}";
            _instance.FireEvent("Opened", msg);
            _log.Debug(msg);            
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            var msg = $"{ID}: {e.Data}";
            _instance.FireEvent("Received", msg);
            _log.Debug(msg);
            Sessions.Broadcast(e.Data);
        }

        protected override void OnError(WebSocketSharp.ErrorEventArgs e)
        {
            var msg = $"{ID}: Error - {e.Message}";
            _instance.FireEvent("ErrorReceived", msg);
            _log.Debug(msg);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            var msg = $"{ID}: Closed connection - {e.Reason}";
            _instance.FireEvent("Closed", msg);
            _log.Debug(msg);
        }
    }
}
