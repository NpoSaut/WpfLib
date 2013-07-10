using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace WpfTools.Validators
{
    /// <summary>
    /// Проверяет, лежит ли значение в указанном диапазоне
    /// </summary>
    public class NumericRangeValidator : ValidationRule
    {
        public Double Max { get; set; }
        public Double Min { get; set; }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string s = (string)value;
            Double v;
            if (!Double.TryParse(s, out v)) return new ValidationResult(false, String.Format("\"{0}\" не является числовым значением", value));
            if (v < Min || v > Max) return new ValidationResult(false, String.Format("Значение должно лежать в пределах от {0} до {1}", Min, Max));
            else return ValidationResult.ValidResult;
        }

        public NumericRangeValidator()
        {
            this.ValidationStep = System.Windows.Controls.ValidationStep.RawProposedValue;
            Min = Double.NegativeInfinity;
            Max = Double.PositiveInfinity;
        }
        public NumericRangeValidator(Double Min, Double Max)
        {
            this.ValidationStep = System.Windows.Controls.ValidationStep.RawProposedValue;
            this.Min = Min;
            this.Max = Max;
        }
    }
}
