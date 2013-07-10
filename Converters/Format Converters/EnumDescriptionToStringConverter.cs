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
    [ValueConversion(typeof(Enum), typeof(String))]
    public class EnumDescriptionToStringConverter : ConverterBase<EnumDescriptionToStringConverter>
    {
        //private static EnumDescriptionToStringConverter descriptedEnumConverter = new EnumDescriptionToStringConverter();

        public override object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            var v = Value as Enum;
            if (v == null) return "{null}";
            FieldInfo fi = Value.GetType().GetField(Value.ToString());
            DescriptionAttribute d = (DescriptionAttribute)fi.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault();
            if (d != null)
                return d.Description;
            else
                return Value.ToString();
        }

        public override object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            if ((Value as string) == null) return TargetType.GetEnumValues().GetValue(0);
            return TargetType.GetEnumValues().OfType<Enum>().Single(e => GetString(e).Equals(Value) || e.ToString().Equals(Value));
        }

        public static String GetString(Enum value)
        {
            return (String)_converter.Convert(value, typeof(String), null, CultureInfo.CurrentUICulture);
        }
    }

    [ValueConversion(typeof(IEnumerable<Enum>), typeof(IEnumerable<String>))]
    public class EnumDescriptionToStringMassConverter : IValueConverter
    {
        private EnumDescriptionToStringConverter descriptedEnumConverter = new EnumDescriptionToStringConverter();

        public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            var en = (Value as Array);
            if (en == null) return null;
            return en.OfType<Enum>().Select(e => descriptedEnumConverter.Convert(e, typeof(String), Parameter, Culture)).AsEnumerable();
        }

        public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            var en = (Value as Array);
            if (en == null) return null;
            return en.OfType<String>().Select(e => descriptedEnumConverter.ConvertBack(e, typeof(Enum), Parameter, Culture)).AsEnumerable();
        }
    }
}
