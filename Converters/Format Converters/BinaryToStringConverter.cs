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
    [ValueConversion(typeof(Byte[]), typeof(String))]
    public class BinaryToStringConverter : IValueConverter
    {
        private static BinaryToStringConverter staticConverter = new BinaryToStringConverter();

        public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            Byte[] bin = Value as Byte[];
            if (bin == null) return "Объект не является массивом байт";
            return BitConverter.ToString(bin);
        }

        public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            return null;
        }

        public static String GetString(Enum value)
        {
            return (String)staticConverter.Convert(value, typeof(String), null, CultureInfo.CurrentUICulture);
        }
    }
}
