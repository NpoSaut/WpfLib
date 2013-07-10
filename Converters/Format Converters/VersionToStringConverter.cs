using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Reflection;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Converters
{
    [ValueConversion(typeof(Version), typeof(String))]
    public class VersionToStringConverter : ConverterBase<VersionToStringConverter>
    {
        public override object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            var v = Value as Version;
            if (v == null) throw new ArgumentException();
            return v.ToString();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = new Version();
            Version.TryParse(value as String, out v);
            return v;
        }
    }
}