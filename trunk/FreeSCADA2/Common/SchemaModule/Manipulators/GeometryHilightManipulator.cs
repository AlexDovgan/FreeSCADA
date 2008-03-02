using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Input;

namespace FreeSCADA.Schema.Manipulators
{
    class GeometryHilightManipulator:BaseManipulator
    {
        Rectangle hilightRect = new Rectangle();
        public GeometryHilightManipulator(UIElement element, SchemaDocument doc)
            : base(element, doc)
        {
            VisualBrush brush = new VisualBrush(AdornedElement);
            //hilightRect.Opacity = 0.5;
          //  hilightRect.Fill = brush;
            
            visualChildren.Add(hilightRect);
        }
        
        protected override Size ArrangeOverride(Size finalSize)
        {

            Rect r=AdornedElement.TransformToVisual(this).TransformBounds(new Rect(new Point(0, 0), AdornedElement.DesiredSize));
            r.X = 0;
            r.Y = 0;
            hilightRect.Arrange(r);
            return finalSize;
        }
          
    }
}
