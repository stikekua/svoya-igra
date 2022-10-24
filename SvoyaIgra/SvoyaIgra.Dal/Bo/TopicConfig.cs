using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvoyaIgra.Dal.Bo
{
    public class TopicConfig
    {
        public IEnumerable<int> RoundTopicIds { get; set; }
        public IEnumerable<int> FinalTopicIds { get; set; }

    }
}
