using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace Converters
{
    /// <summary>
    /// Определяет, равны ли все элементы коллекции
    /// </summary>
    public class EqualityConverter : MultiConverterBase<EqualityConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var o1 = values.First();
            return values.Skip(1).All(o => o1.Equals(o));
        }
    }
}
