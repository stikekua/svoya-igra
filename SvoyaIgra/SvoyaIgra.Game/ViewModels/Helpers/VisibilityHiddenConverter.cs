﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SvoyaIgra.Game.ViewModels.Helpers
{
    public class VisibilityHiddenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = (bool)value;
            return boolValue ? Visibility.Visible : (parameter ?? Visibility.Hidden);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
            //throw new NotImplementedException();
        }
    }
}
