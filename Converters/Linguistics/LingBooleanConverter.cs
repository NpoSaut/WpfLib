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
    [ValueConversion(typeof(Boolean), typeof(String))]
    public class LingBooleanConverter : ConverterBase<LingBooleanConverter>
    {
        public String TrueText { get; set; }
        private String _ft;

        public String FalseText
        {
            get { return _ft; }
            set
            {
                _ft = value;
            }
        }
        

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Boolean)) throw new ArgumentException("Параметр должен быть типа Boolean");
            return (Boolean)value ? TrueText : FalseText;
        }

        public LingBooleanConverter()
        {
            TrueText = "Да";
            FalseText = "Нет";
        }
    }
}
