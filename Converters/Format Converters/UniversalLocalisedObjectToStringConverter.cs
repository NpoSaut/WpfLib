using System;
using System.Globalization;
using System.Windows.Data;

namespace Converters
{
    [ValueConversion(typeof (Enum), typeof (String))]
    public class UniversalLocalisedObjectToStringConverter : ConverterBase<UniversalLocalisedObjectToStringConverter>
    {
        private static readonly EnumDescriptionToStringConverter DescriptedEnumConverter = new EnumDescriptionToStringConverter();

        public UniversalLocalisedObjectToStringConverter() { NullString = "null"; }
        public String StringFormat { get; set; }
        public String NullString { get; set; }

        public override object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            if (Value == null) return NullString;
            if (Value is Boolean) return (Boolean)Value ? "Да" : "Нет";
            if (Value is Enum) return DescriptedEnumConverter.Convert(Value, TargetType, Parameter, Culture);

            string format = StringFormat != null ? ("{0:" + StringFormat + "}") : "{0}";
            return string.Format(format, Value);
        }

        public string Convert(object value) { return (String)Convert(value, typeof (String), null, CultureInfo.CurrentUICulture); }

        public static String GetString(Object value, String StringFormat = null, String NullString = "---")
        {
            return (new UniversalLocalisedObjectToStringConverter { StringFormat = StringFormat, NullString = NullString }).Convert(value);
        }
    }
}
