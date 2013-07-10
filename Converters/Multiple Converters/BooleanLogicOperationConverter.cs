using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace Converters
{
    /// <summary>
    /// Производит логическую операцию с элементами (по-умолчанию, операция AND. Операцию OR можно задать, передав "or" в качестве параметра)
    /// </summary>
    public class BooleanLogicOperationConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Boolean res;
            if (parameter.ToString().ToLower() == "or")
                res = values.Any(v => (v as Boolean?) == true);
            else
                res = values.All(v => (v as Boolean?) == true);
            return res;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// Отрицает конвертируемое значение
    /// </summary>
    [ValueConversion(typeof(Boolean), typeof(Boolean))]
    public class BooleanNotConverter : IValueConverter
    {
        public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            return !(Boolean)Value;
        }

        public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            return !(Boolean)Value;
        }
    }
}
