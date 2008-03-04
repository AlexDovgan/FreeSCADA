using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Input;
using FreeSCADA.Common.Schema;
using System.Windows.Controls.Primitives;
namespace FreeSCADA.Designer.SchemaEditor.Manipulators
{
    class GeometryHilightManipulator:BaseManipulator
    {
        Rectangle hilightRect = new Rectangle();
        Path path = new Path();
        public GeometryHilightManipulator(UIElement element, SchemaDocument doc)
            : base(element, doc)
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

            Rect r=AdornedElement.TransformToVisual(this).TransformBounds(new Rect(new Point(0, 0), AdornedElement.DesiredSize));
            path.Arrange(r);
            r.X = 0;
            r.Y = 0;
            hilightRect.Arrange(r);
            
            return finalSize;
        }
          
    }
}
