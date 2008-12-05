using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FreeSCADA.Designer.SchemaEditor.ShortProperties
{


    class ShapeShortProp : FrameworkElementShortProp
    {

        public ShapeShortProp(Shape shp)
            : base(shp)
        {
            shape = shp;

        }
        [Description("Object's Fill color"), Category("Appearence")]
        public System.Drawing.Color FillColor
        {
            get
            {
                System.ComponentModel.TypeConverter ccv = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Drawing.Color));
                return (System.Drawing.Color)ccv.ConvertFromString((shape.Fill as SolidColorBrush).Color.ToString());

            }
            set
            {
                RaisePropertiesBrowserChanged((UIElement)shape);
                shape.Fill = shape.Fill.Clone();
                (shape.Fill as SolidColorBrush).Color = Color.FromArgb(value.A, value.R, value.G, value.B); ;

            }
        }
        [Description("Object's Stroke color"), Category("Appearence")]
        public System.Drawing.Color StrokeColor
        {
            get
            {
                System.ComponentModel.TypeConverter ccv = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Drawing.Color));
                return (System.Drawing.Color)ccv.ConvertFromString((shape.Stroke as SolidColorBrush).Color.ToString());

            }
            set
            {
                RaisePropertiesBrowserChanged((UIElement)shape);
                shape.Stroke = shape.Stroke.Clone();
                (shape.Stroke as SolidColorBrush).Color = Color.FromArgb(value.A, value.R, value.G, value.B); ;

            }
        }
        [Description("Object's Stroke thickness"), Category("Appearence")]
        public double StrokeThickness
        {
            get
            {

                return shape.StrokeThickness;

            }
            set
            {
                RaisePropertiesBrowserChanged((UIElement)shape);
                shape.StrokeThickness = value;

            }
        }
        Shape shape;
        //System.Drawing.Brush brush;
    }
}
