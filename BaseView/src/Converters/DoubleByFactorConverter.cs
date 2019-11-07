using Microsoft.VisualBasic;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Wells.BaseView.Converters
{
    public class DoubleByFactorConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Information.IsNumeric(value))
            {
                var dbl = Convert.ToDouble(value, culture);
                double factor = 1.0;
                if (parameter != null) { factor = Convert.ToDouble(parameter, culture); }
                return dbl * factor;
            }
            return 0.0;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
