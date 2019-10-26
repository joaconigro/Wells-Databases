using Wells.Base;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Wells.CoreView.Converters
{
    public class EnumValueConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var anEnum = (Enum)value;
            return Common.GetEnumDescription(anEnum);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = value.ToString();
            var values = Common.EnumDescriptionsToList(targetType);
            if (values.Contains(text))
                return Enum.ToObject(targetType, values.IndexOf(text));
            return 0;
        }
    }
}
