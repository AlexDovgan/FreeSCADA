using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Globalization;

namespace FreeSCADA.Schema
{
    public class DoubleValidation : ValidationRule
    {
        double minValue;
        double maxValue;

        public double MinMargin
        {
            get { return this.minValue; }
            set { this.minValue = value; }
        }

        public double MaxMargin
        {
            get { return this.maxValue; }
            set { this.maxValue = value; }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double val;

            // Is a number?
            if (!double.TryParse((string)value, out val))
            {
                return new ValidationResult(false, "Not a number.");
            }

            // Is in range?
            if ((val < this.minValue) || (val > this.maxValue))
            {
                string msg = string.Format("Margin must be between {0} and {1}.", this.minValue, this.maxValue);
                return new ValidationResult(false, msg);
            }

            // Number is valid
            return new ValidationResult(true, null);
        }
    }

}
