using Microsoft.VisualBasic;
using System;
using System.Globalization;
using System.Windows.Data;
using Wells.Base;

namespace Wells.BaseView.Converters
{
    public class IntegerStringConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Common.IsIntegerNumericType(value.GetType()))
            {
                var integer = Convert.ToInt32(value, culture);
                if (parameter != null) { return integer.ToString(parameter.ToString(), culture); }
                return integer.ToString(culture);
            }
            return "0";
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Information.IsNumeric(value))
            {
                var str = (string)value;
                bool ok = int.TryParse(str, NumberStyles.Any, culture, out int integer);
                if (ok) { return integer; }
            }
            return 0;
        }
    }
}
