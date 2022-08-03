using log4net;
using SvoyaIgra.WebSocketProvider.Client;
using System.Threading;

namespace SvoyaIgra.Btn.WSTestClient.Helpers
{
    public class GlobalData : IGlobalData
    {
        public CancellationTokenSource CancellationToken { get; set; } = new CancellationTokenSource();

        public ILog _log { get; } = LogManager.GetLogger(typeof(GlobalData));

        public WebSocketClientProvider WebSocketClient { get; set; }
    }
}
