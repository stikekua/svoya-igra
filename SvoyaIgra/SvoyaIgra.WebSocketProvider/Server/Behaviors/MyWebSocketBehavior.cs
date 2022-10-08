using System.Text.Json;
using log4net;
using SvoyaIgra.WebSocketProvider.Entities;
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
            var msg = new Message
            {
                ClientId = ID,
                Data = clientAddress.ToString(),
                ForLog = $"{ID}: Opened connection. Address: {clientAddress}"
            };
            _instance.FireEvent("Opened", JsonSerializer.Serialize(msg));
            _log.Debug(msg);            
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            var msg = new Message
            {
                ClientId = ID,
                Data = e.Data,
                ForLog = $"{ID}: {e.Data}"
            };
            _instance.FireEvent("Received", JsonSerializer.Serialize(msg));
            _log.Debug(msg);
        }

        protected override void OnError(WebSocketSharp.ErrorEventArgs e)
        {
            var msg = new Message
            {
                ClientId = ID,
                Data = e.Message,
                ForLog = $"{ID}: Error - {e.Message}"
            };
            _instance.FireEvent("ErrorReceived", JsonSerializer.Serialize(msg));
            _log.Debug(msg);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            var msg = new Message
            {
                ClientId = ID,
                Data = e.Reason,
                ForLog = $"{ID}: Closed connection - {e.Reason}"
            };
            _instance.FireEvent("Closed", JsonSerializer.Serialize(msg));
            _log.Debug(msg);
        }
    }
}
