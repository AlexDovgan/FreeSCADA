using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using  System.Windows.Markup;
using System.ComponentModel;

namespace FreeSCADA.Common.Schema
{
    [ContentProperty("Converters")]
    public class ComposingConverter : IValueConverter
    {
        private List<IValueConverter> converters = new List<IValueConverter>();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<IValueConverter> Converters
        {
            get { return this.converters; }
        }

        public object Convert(object o, Type targetType, object parameter, CultureInfo culture)
        {
            for (int i = 0; i < this.converters.Count; i++)
            {
                o = converters[i].Convert(o, targetType, parameter, culture);
            }
            return o;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            for (int i = this.converters.Count - 1; i >= 0; i--)
            {
                value = converters[i].ConvertBack(value, targetType, parameter, culture);
            }
            return value;
        }
    }
}

