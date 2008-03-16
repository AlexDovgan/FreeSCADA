using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FreeSCADA.Designer.SchemaEditor.Manipulators.Controlls;
using System.Windows.Controls.Primitives;

namespace FreeSCADA.Designer.SchemaEditor.Manipulators
{
    class PolylineEditManipulantor:BaseManipulator,IDisposable
    {
        List<PointDragThumb> pointsDrags =new List<PointDragThumb>();
        public PolylineEditManipulantor(Polyline poly)
            : base(poly)
        {
            
            foreach (Point p in poly.Points)
            {
                PointDragThumb pd=new PointDragThumb();
                pointsDrags.Add(pd);
                pd.DragDelta += pointDragDelta;
                visualChildren.Add(pd);
            }
            poly.UpdateLayout();
            for (int i = 0; i < poly.Points.Count; i++)
            {
                Point p=poly.TranslatePoint(poly.Points[i],(UIElement)this.Parent);
                poly.Points[i] = p;
            }
            poly.Stretch = Stretch.None;
            poly.SetValue(Canvas.LeftProperty, DependencyProperty.UnsetValue);
            poly.SetValue(Canvas.TopProperty, DependencyProperty.UnsetValue);
            poly.SetValue(FrameworkElement.WidthProperty, DependencyProperty.UnsetValue);
            poly.SetValue(FrameworkElement.HeightProperty, DependencyProperty.UnsetValue);

            poly.SetValue(UIElement.RenderTransformProperty, DependencyProperty.UnsetValue);
           
        }
        public void Dispose()
        {
            Polyline poly = AdornedElement as Polyline;
            Rect b = VisualTreeHelper.GetContentBounds(poly);
            poly.Stretch = Stretch.Fill;
            poly.SetValue(Canvas.LeftProperty, b.X);
            poly.SetValue(Canvas.TopProperty, b.Y);
            poly.SetValue(FrameworkElement.WidthProperty, b.Width);
            poly.SetValue(FrameworkElement.HeightProperty, b.Height);
            for (int i = 0; i < poly.Points.Count; i++)
            {
                Point p = poly.Points[i];
                p.X -= b.X;
                p.Y -= b.Y;
                poly.Points[i] = p;
            }
                
        }
        void pointDragDelta(object sender, DragDeltaEventArgs e)
        {
            Polyline poly = AdornedElement as Polyline;
            
            Point p=poly.Points[pointsDrags.IndexOf(sender as PointDragThumb)];
            p.X+=e.HorizontalChange;
            p.Y+= e.VerticalChange;

            poly.Points[pointsDrags.IndexOf(sender as PointDragThumb)] = p;
            
            InvalidateArrange();

        }
        protected override Size MeasureOverride(Size finalSize)
        {
            foreach (PointDragThumb pdt in pointsDrags)
            {
                pdt.Measure(finalSize);
            }
            return finalSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Polyline poly = AdornedElement as Polyline;
            

            foreach (PointDragThumb pdt in pointsDrags)
            {
                Point p=/*poly.TranslatePoint(*/poly.Points[pointsDrags.IndexOf(pdt)];//,this);
                p.X-=pdt.DesiredSize.Width/2;
                p.Y-=pdt.DesiredSize.Height/2;
                pdt.Arrange(new Rect(p, pdt.DesiredSize));
            }
            return finalSize;
        }

    }
}
