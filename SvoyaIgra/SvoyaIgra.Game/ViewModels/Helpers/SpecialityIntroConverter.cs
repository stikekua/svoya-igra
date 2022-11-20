using SvoyaIgra.Game.Metadata;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SvoyaIgra.Game.ViewModels.Helpers
{
    public class SpecialityIntroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var q = (Question)value;
            return (q.SpecialityType>0 && q.SpecialIntroWasNotPlayed) ? Visibility.Visible :Visibility.Collapsed ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
            //throw new NotImplementedException();
        }
    }
}
