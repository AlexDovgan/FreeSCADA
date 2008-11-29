using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
namespace FreeSCADA.Designer.SchemaEditor.Manipulators
{
    class GeometryHilightManipulator:BaseManipulator
    {
        Rectangle hilightRect = new Rectangle();
        Path path = new Path();
        public GeometryHilightManipulator(UIElement el)
            : base(el)
        {
            
            VisualBrush brush = new VisualBrush(AdornedElement);
            hilightRect.Opacity = 0.5;
            hilightRect.Fill = brush;
            
            path.Data = LayoutInformation.GetLayoutClip(AdornedElement as FrameworkElement);
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 1;

            visualChildren.Add(hilightRect);
            visualChildren.Add(path);
             
        }
        
        protected override Size ArrangeOverride(Size finalSize)
        {


            MatrixTransform m = (MatrixTransform)AdornedElement.TransformToVisual(this);
            
            Rect r= m.TransformBounds(new Rect(new Point(0, 0), AdornedElement.DesiredSize));
            hilightRect.Arrange(r);
            
            return finalSize;
        }
          
    }
}
