using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace FreeSCADA.Designer.SchemaEditor.ShortProperties
{
    class RangeBaseShortProp : ControlShortProp
    {
        public RangeBaseShortProp(RangeBase rb)
            : base(rb)
        {
            rangebase = rb;
        }
        [Description("Minimum Scale Value"), Category("Appearence")]
        public double MinValue
        {
            get { return rangebase.Minimum; }
            set
            {
                RaisePropertiesBrowserChanged((UIElement)rangebase);
                rangebase.Minimum = value;
            }
        }
        [Description("Maximum Scale Value"), Category("Appearence")]
        public double MaxValue
        {
            get { return rangebase.Maximum; }
            set
            {
                RaisePropertiesBrowserChanged((UIElement)rangebase);
                rangebase.Maximum = value;
            }
        }
        public double Value
        {
            get { return rangebase.Value; }
            set { rangebase.Value = value; }
        }
        RangeBase rangebase;

    }
   
}
