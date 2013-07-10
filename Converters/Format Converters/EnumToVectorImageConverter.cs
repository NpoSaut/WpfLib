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
    [ValueConversion(typeof(Enum), typeof(ImageSource))]
    public class EnumToVectorImageConverter : ConverterBase<EnumToVectorImageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            return (ImageSource)Application.Current.TryFindResource(value.GetType().Name + "-" + value.ToString());
        }

        //public static Brush GetBrush(Enum value)
        //{
        //    return (Brush)DefaultConverter.Convert(value, typeof(String), null, CultureInfo.CurrentUICulture);
        //}
    }
}