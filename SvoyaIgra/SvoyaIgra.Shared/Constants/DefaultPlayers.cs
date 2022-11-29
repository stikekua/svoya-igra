using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvoyaIgra.Shared.Constants
{
    public static class DefaultPlayers
    {
        public static class Red
        {
            public const string Color = "#FF0000";
            public const string Name = "Red";

        }
        public static class Green
        {
            public const string Color = "#00FF00";
            public const string Name = "Green";

        }
        public static class Blue
        {
            public const string Color = "#0000FF";
            public const string Name = "Blue";

        }
        public static class Yellow
        {
            public const string Color = "#FFFF00";
            public const string Name = "Yellow";

        }

        public static string[] GetColors()
        {
            return new[] { Red.Color, Green.Color, Blue.Color, Yellow.Color };
        }

        public static string[] GetNames()
        {
            return new[] { Red.Name, Green.Name, Blue.Name, Yellow.Name };
        }

    }
}
