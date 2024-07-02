using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SvoyaIgra.Dal.Bo;

namespace SvoyaIgra.Dal.Helpers
{
    public static class ParametersConfigParser
    {
        public static string? ToString(ParametersConfig? parameters)
        {
            if (parameters == null) return null;
            return JsonSerializer.Serialize(parameters);
        }

        public static ParametersConfig? ToObject(string? parameters)
        {
            if(parameters == null) return null;
            return JsonSerializer.Deserialize<ParametersConfig>(parameters);
        }
    }
}
