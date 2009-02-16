using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace FreeSCADA.Common.Schema
{
    /// <summary>
    /// Converter for brush bindung
    /// SoldBrush
    /// </summary>
    public class SolidBrushConverter : IValueConverter
    {
        public Color StartColor
        {
            get;
            set;
        }
        public Color EndColor
        {
            get;
            set;
        }
        public Double MinValue
        {
            get;
            set;
        }
        public Double MaxValue
        {
            get;
            set;
        }
        public SolidBrushConverter()
        {
        }
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val;

            if (Double.TryParse(value.ToString(), out val) && targetType == typeof(Brush))
            {

                Color color = new Color();
                double curPos = (val - MinValue) / (MaxValue - MinValue);
                color.A = (byte)(Math.Abs(StartColor.A - Math.Abs(EndColor.A - StartColor.A) * curPos));
                color.R = (byte)(Math.Abs(StartColor.R - Math.Abs(EndColor.R - StartColor.R) * curPos));
                color.G = (byte)(Math.Abs(StartColor.G - Math.Abs(EndColor.G - StartColor.G) * curPos));
                color.B = (byte)(Math.Abs(StartColor.B - Math.Abs(EndColor.B - StartColor.B) * curPos));
                return new SolidColorBrush(color);
            }
            return null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("Can convert back");
        }

        #endregion
    }
}
