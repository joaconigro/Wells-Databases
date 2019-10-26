using Microsoft.VisualBasic;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Wells.CoreView.Converters
{
    public class DoubleStringConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Information.IsNumeric(value))
            {
                var dbl = Convert.ToDouble(value);
                if (parameter != null)
                    return dbl.ToString(parameter.ToString());
                return dbl.ToString();
            }
            return "0";
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Information.IsNumeric(value))
            {
                var str = (string)value;
                double doubleValue;
                bool ok = double.TryParse(str, NumberStyles.Any, CultureInfo.CurrentCulture, out doubleValue);
                if (ok)
                    return doubleValue;
            }
            return 0.0;
        }
    }
}
