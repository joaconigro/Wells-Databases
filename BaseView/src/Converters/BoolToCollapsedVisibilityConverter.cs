using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Wells.BaseView.Converters
{
    public class BoolToCollapsedVisibilityConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) { return Visibility.Collapsed; };
            if (value.GetType() == typeof(bool)) { return (bool)value ? Visibility.Visible : Visibility.Collapsed; };
            return Visibility.Collapsed;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
