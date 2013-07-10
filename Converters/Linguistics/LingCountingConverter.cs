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
    [ValueConversion(typeof(Double), typeof(String))]
    public class LingCountingConverter : ConverterBase<LingCountingConverter>
    {
        public static String Convert(int Value, String Parameters)
        {
            return Convert(Value, Parameters.Split(new char[] { '|' }));
        }
        public static String Convert(int Value, IList<String> Parameters)
        {
            var Forms = Parameters.Select(f => f.Trim()).ToList();

            if (Forms.Contains("~hidezero") && (int)Value == 0) return null;

            String NumberFormat = Value.ToString() + " ";

            if (Forms.Contains("~nonum")) NumberFormat = "";
            String zf = Forms.SingleOrDefault(f => f.StartsWith("~litzero:"));
            if (zf != null && (int)Value == 0) NumberFormat = zf.Substring(zf.IndexOf(":")+1) + " ";

            Forms = Forms.Where(f => !f.StartsWith("~")).ToList();

            int v = (int)Value;
            int ost = (int)Value % 10;
            if (v >= 11 && v <= 14) return NumberFormat + Forms[2];
            else if (ost == 1) return NumberFormat + Forms[0];
            else if (ost >= 2 && ost <= 4) return NumberFormat + Forms[1];
            else return NumberFormat + Forms[2];
        }

        // Пример строки параметра
        // Объект|Объекта|Объектов
        public override object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            if (Value == null) return "";
            if (Value is String) Value = Int32.Parse(Value as string);

            IList<String> Forms;
            if (Parameter is IEnumerable<String>) Forms = (Parameter as IEnumerable<String>).ToList();
            else if (Parameter is String) Forms = (Parameter as String).Split(new Char[] { '|' });
            else return Value.ToString();

            return LingCountingConverter.Convert((int)Value, Forms);
        }
    }
}
