using Microsoft.VisualBasic;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Wells.BaseView.Converters
{
    public class DoubleStringConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Information.IsNumeric(value))
            {
                var dbl = Convert.ToDouble(value, culture);
                if (parameter != null) { return dbl.ToString(parameter.ToString(), culture); }
                return dbl.ToString(culture);
            }
            return "0";
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Information.IsNumeric(value))
            {
                var str = (string)value;
                bool ok = double.TryParse(str, NumberStyles.Any, culture, out double doubleValue);
                if (ok) { return doubleValue; }
            }
            return 0.0;
        }
    }
}
