using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace Converters
{
    [ValueConversion(typeof(Enum), typeof(String))]
    public class UniversalLocalisedObjectToStringConverter : ConverterBase<UniversalLocalisedObjectToStringConverter>
    {
        public String NullString { get; set; }
        private static EnumDescriptionToStringConverter descriptedEnumConverter = new EnumDescriptionToStringConverter();
        private static UniversalLocalisedObjectToStringConverter DefaultConverter = new UniversalLocalisedObjectToStringConverter();

        public UniversalLocalisedObjectToStringConverter()
        {
            NullString = "null";
        }

        public override object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            if (Value is Boolean)
            {
                return (Boolean)Value ? "Да" : "Нет";
            }
            else if (Value is Enum)
            {
                return descriptedEnumConverter.Convert(Value, TargetType, Parameter, Culture);
            }
            else
            {
                return (Value ?? NullString).ToString();
            }
        }
        public String Convert(Object value)
        {
            return (String)this.Convert(value, typeof(String), null, CultureInfo.CurrentUICulture);
        }
        public static String GetString(Object value)
        {
            return DefaultConverter.Convert(value);
        }
    }
}
