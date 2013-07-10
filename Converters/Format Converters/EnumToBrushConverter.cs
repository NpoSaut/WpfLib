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

namespace Converters
{
    [ValueConversion(typeof(Enum), typeof(Brush))]
    public class EnumToBrushConverter : ConverterBase<EnumToBrushConverter>
    {
        private static EnumToBrushConverter DefaultConverter = new EnumToBrushConverter();

        public override object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            if (Value == null) return null;
            return (Brush)Application.Current.FindResource(Value.GetType().Name + "-" + Value.ToString());
        }

        public static Brush GetBrush(Enum value)
        {
            return (Brush)DefaultConverter.Convert(value, typeof(String), null, CultureInfo.CurrentUICulture);
        }
    }
}