using log4net;
using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace SvoyaIgra.WebSocketProvider.Server.Behaviors
{
    public class Echo : WebSocketBehavior
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(MyWebSocketBehavior));

        private WebSocketServerProvider _instance;
        public Echo(WebSocketServerProvider instance)
        {
            _instance = instance;
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            Send(e.Data);
            _instance.FireEvent("EchoMade", $"{ID}: {e.Data}");
            _log.Debug($"{ID}: {e.Data}");
        }
    }
}
