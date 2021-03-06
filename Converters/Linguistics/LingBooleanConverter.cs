﻿using System;
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
        public String FalseText { get; set; }
        public String NullText { get; set; }
        

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return NullText;
            if (!(value is Boolean)) throw new ArgumentException("Параметр должен быть типа Boolean");
            return (Boolean)value ? TrueText : FalseText;
        }

        public LingBooleanConverter()
        {
            TrueText = "Да";
            FalseText = "Нет";
            NullText = "--";
        }
    }
}
