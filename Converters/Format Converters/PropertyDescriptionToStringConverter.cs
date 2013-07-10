using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;
using System.Windows;
using System.Reflection;
using System.ComponentModel;

namespace Converters
{
    [ValueConversion(typeof(PropertyInfo), typeof(String))]
    public class PropertyDescriptionToStringConverter : IValueConverter
    {
        private static PropertyDescriptionToStringConverter StaticConverter = new PropertyDescriptionToStringConverter();

        public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            PropertyInfo pi = (PropertyInfo)Value;
            if (pi == null) return Value.ToString() + " - не является объектом PropertyInfo";
            var ats = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
            if (ats.Length == 0) return pi.Name;
            return (ats.First() as DisplayNameAttribute).DisplayName;
        }

        public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            return null;
        }

        public static String GetString(PropertyInfo p)
        {
            return (String)StaticConverter.Convert(p, typeof(String), null, CultureInfo.CurrentUICulture);
        }
    }
}
