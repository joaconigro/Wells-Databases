﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Microsoft.VisualBasic;

namespace Wells.CoreView.Converters
{
    public class InverseBooleanConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() == typeof(bool))
                return !(bool)value;
            return false;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() == typeof(bool))
                return !(bool)value;
            return false;
        }
    }
}
