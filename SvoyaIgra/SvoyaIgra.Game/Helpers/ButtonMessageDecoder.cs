using System;
using System.Linq;
using SvoyaIgra.Game.Enums;
using SvoyaIgra.Shared.Entities;

namespace SvoyaIgra.Game.Helpers;

public static class ButtonMessageDecoder
{
    public const string EmptyMessage = "1;0;0;0;0";

    public static PlayerIndexEnum GetSelectedPlayerIndex(string message)
    {
        const char separator = ';';
        var buttonsIndexes = message.Split(separator);

        var qIndex = Convert.ToInt32(buttonsIndexes[0]);

        var queue = buttonsIndexes
            .Skip(1).Take(4)
            .Select(Enum.Parse<ButtonEnum>).ToArray();
        
        switch (queue[qIndex - 1])
        {
            case ButtonEnum.Red:
                return PlayerIndexEnum.Red;
            case ButtonEnum.Green:
                return PlayerIndexEnum.Green;
            case ButtonEnum.Blue:
                return PlayerIndexEnum.Blue;
            case ButtonEnum.Yellow:
                return PlayerIndexEnum.Yellow;

            default:
                return PlayerIndexEnum.Nobody;
        }
        
    }
}