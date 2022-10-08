using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvoyaIgra.WebSocketProvider.Entities
{
    public class Message
    {
        public string ClientId { get; set; }
        public string Data { get; set; }
        public string ForLog { get; set; }

    }
}
