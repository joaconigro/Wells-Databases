using Wells.Base;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Wells.BaseView.Converters
{
    public class EnumDescriptionsConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Common.EnumDescriptionsToList(value.GetType());
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
