using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Converters
{
    [ValueConversion(typeof(Boolean), typeof(Boolean))]
    public class NotConverter : ConverterBase<NotConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is Boolean)) throw new ArgumentException("Объект должен иметь тип Boolean", "value");
            return !(Boolean)value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
