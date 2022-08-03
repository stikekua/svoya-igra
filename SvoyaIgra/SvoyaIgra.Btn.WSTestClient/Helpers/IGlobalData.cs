using SvoyaIgra.WebSocketProvider.Client;
using System.Threading;

namespace SvoyaIgra.Btn.WSTestClient.Helpers
{
    public interface IGlobalData
    {
        CancellationTokenSource CancellationToken { get; set; }

        WebSocketClientProvider WebSocketClient { get; set; }
    }
}
