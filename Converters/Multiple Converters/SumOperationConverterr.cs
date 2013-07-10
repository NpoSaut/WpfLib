using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace Converters
{
    /// <summary>
    /// Находит максимальное значение из предложенных
    /// <remarks>
    /// В параметрах конвертера необходимо для каждого слогаемого указать знак (+/-).
    /// Например, чтобы сложить первые два слогаемых и вычесть из них третье нужно указать параметр "++-"
    /// </remarks>
    /// </summary>
    [ValueConversion(typeof(Double), typeof(Double))]
    public class SumOperationConverter : MultiConverterBase<SumOperationConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is String) || ((parameter as String).Length < values.Length)) return null;
            var vals = values.Cast<Double>().ToList();
            string abs = parameter as string;

            double sup = abs.Contains('S') ? vals[abs.IndexOf('S')] : double.PositiveInfinity;
            double inf = abs.Contains('I') ? vals[abs.IndexOf('I')] : double.NegativeInfinity;

            vals = vals.Where((v, i) => abs[i] == '+' || abs[i] == '-').Select((v, i) => (abs[i] == '+') ? v : -v).ToList();
            return Math.Max(Math.Min(vals.Sum(), sup), inf);
        }
    }
}
