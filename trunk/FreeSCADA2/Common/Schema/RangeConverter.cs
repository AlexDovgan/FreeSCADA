using System;
using System.Globalization;
using System.Windows.Data;


namespace FreeSCADA.Common.Schema
{
    public class RangeConverter : IValueConverter
    {
        private double _min;
        private double _max;

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


        public object Convert(object o, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Double val = Double.Parse(o.ToString());
                if (val < _min)
                    return _min;
                else if (val > _max)
                    return _max;
                return val;
            }
            catch (System.Exception)
            {
                return o;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;

        }

    }
}
