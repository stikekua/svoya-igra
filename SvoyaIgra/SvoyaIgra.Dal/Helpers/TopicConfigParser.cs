using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SvoyaIgra.Dal.Bo;

namespace SvoyaIgra.Dal.Helpers
{
    public static class TopicConfigParser
    {
        public static string? ToString(TopicConfig? tagConfig)
        {
            if (tagConfig == null) return null;
            return JsonSerializer.Serialize(tagConfig);
        }

        public static TopicConfig? ToObject(string? tagConfig)
        {
            if(tagConfig == null) return null;
            return JsonSerializer.Deserialize<TopicConfig>(tagConfig);
        }
    }
}
