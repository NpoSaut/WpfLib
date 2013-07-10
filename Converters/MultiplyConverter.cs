using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Converters
{
    public class MultiplyConverter : ConverterBase<MultiplyConverter>
    {
        public double Factor { get; set; }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is Double)) throw new ArgumentException();
            Double val = (double)value;
            return val * Factor;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is Double)) throw new ArgumentException();
            Double val = (double)value;
            return val / Factor;
        }
    }
}
