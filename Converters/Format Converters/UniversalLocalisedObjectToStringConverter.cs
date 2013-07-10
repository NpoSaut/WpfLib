﻿using System;
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
        private static EnumDescriptionToStringConverter descriptedEnumConverter = new EnumDescriptionToStringConverter();
        private static UniversalLocalisedObjectToStringConverter DefaultConverter = new UniversalLocalisedObjectToStringConverter();

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
                return (Value ?? "null").ToString();
            }
        }

        public static String GetString(Object value)
        {
            return (String)DefaultConverter.Convert(value, typeof(String), null, CultureInfo.CurrentUICulture);
        }
    }
}
