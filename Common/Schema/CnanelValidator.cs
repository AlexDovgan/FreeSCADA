using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Globalization;

namespace FreeSCADA.Common.Schema
{
    public class ChannelValidator : ValidationRule
    {
        private double _min;
        private double _max;

        public ChannelValidator()
        {
        }

        public double Min
        {
            get { return _min; }
            set { _min = value; }
        }

        public double Max
        {
            get { return _max; }
            set { _max = value; }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double val = (Double)value;

           
            if ((val < Min) || (val > Max))
            {
                return new ValidationResult(false,
                  "Channel's value out of range: " + Min + " - " + Max + ".");
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
        
    }


}
