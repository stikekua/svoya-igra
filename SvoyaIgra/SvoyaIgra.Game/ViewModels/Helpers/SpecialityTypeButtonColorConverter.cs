using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using SvoyaIgra.Game.Enums;

namespace SvoyaIgra.Game.ViewModels.Helpers
{
    public class SpecialityTypeButtonColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {         
            switch (value)
            {                    
                case SpecialityTypesEnum.Cat:
                    return "#0000FF";
                case SpecialityTypesEnum.Auction:
                    return "#FF0000";
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
