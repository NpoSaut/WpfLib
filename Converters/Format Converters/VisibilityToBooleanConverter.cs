using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Reflection;
using System.ComponentModel;
using System.Windows;

namespace Converters
{
    [ValueConversion(typeof(Visibility), typeof(Boolean))]
    public class VisibilityToBooleanConverter : ConverterBase<VisibilityToBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility)value == Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Boolean)value ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}