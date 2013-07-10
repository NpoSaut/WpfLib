using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace Converters
{
    [ValueConversion(typeof(Double), typeof(Double))]
    public class ConstantMathConverter : IValueConverter
    {
        public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            Double val = (Double)Value;
            String par = (String)Parameter;
            Double delta = Double.Parse(par, CultureInfo.InvariantCulture);
            return val + delta;
        }

        public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            Double val = (Double)Value;
            String par = (String)Parameter;
            Double delta = Double.Parse(par, CultureInfo.InvariantCulture);
            return val - delta;
        }
    }
}
