using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Reflection;
using System.ComponentModel;

namespace Converters
{
    [ValueConversion(typeof(Double), typeof(Double))]
    public class DoubleIntoIntervalConverter : IValueConverter
    {
        public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            Double val = (Double)Value;
            String par = (String)Parameter;
            var pars = par.Split(new Char[] { ' ' }).Select(s => s.Trim().Split(new Char[] { ':' }).Select(ss => Double.Parse(ss)).ToArray()).Select(s => new { from = s[0], to = s[1]}).ToArray();
            if (val < pars[0].from) return pars[1].from;
            if (val > pars[0].to) return pars[1].to;
            return pars[1].from + (pars[1].to - pars[1].from) * (val - pars[0].from) / (pars[0].to - pars[0].from);
        }

        public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            return null;
        }
    }
}
