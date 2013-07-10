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
    [ValueConversion(typeof(Enum), typeof(Uri))]
    public class EnumToImageConverter : ConverterBase<EnumToImageConverter>
    {
        public String AssemblyName { get; set; }

        public override object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            Enum v = (Value as Enum);
            if (v == null) return null;

            Uri u = new Uri(String.Format("/{0};component/Images/{1}/{2}.png", AssemblyName, v.GetType().Name, v), UriKind.Relative);

            //var im = new BitmapImage(u);

            return u;
        }
    }
}