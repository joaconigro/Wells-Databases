using System;
using System.Globalization;
using System.Windows.Data;

namespace Wells.BaseView.Converters
{
    public class InverseBooleanConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() == typeof(bool)) { return !(bool)value; }
            return false;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() == typeof(bool)) { return !(bool)value; }
            return false;
        }
    }
}
