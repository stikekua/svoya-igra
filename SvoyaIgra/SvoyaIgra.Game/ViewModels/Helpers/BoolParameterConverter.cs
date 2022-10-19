using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SvoyaIgra.Game.ViewModels.Helpers
{
    public class BoolParameterConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int intQuestionValue = (int)value;
            int intParameterValue = (int)parameter;

            if (intQuestionValue == intParameterValue) return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
            //throw new NotImplementedException();
        }
    }
}
