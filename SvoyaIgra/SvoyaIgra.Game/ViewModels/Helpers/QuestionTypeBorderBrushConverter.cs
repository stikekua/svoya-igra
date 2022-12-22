using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using SvoyaIgra.Game.Enums;

namespace SvoyaIgra.Game.ViewModels.Helpers
{
    public class QuestionTypeBorderBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case QuestionTypeEnum.Picture:
                    return "#964002";
                case QuestionTypeEnum.Musical:
                    return "#2dc200";
                case QuestionTypeEnum.Video:
                    return "#40164a";
                default:
                    return "#000000";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
            //throw new NotImplementedException();
        }
    }
}

