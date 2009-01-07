using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using FreeSCADA.Designer.SchemaEditor.Manipulators.Controls;

namespace FreeSCADA.Designer.SchemaEditor.Manipulators
{
    class PolylineEditManipulantor:BaseManipulator
    {
        //List<PointDragThumb> pointsDrags =new List<PointDragThumb>();
        public PolylineEditManipulantor(UIElement el)
            : base(el)
        {

            Activate();
        }
        public override void Activate()
        {
           Polyline poly = AdornedElement as Polyline;
            
            foreach (Point p in poly.Points)
            {
                PointDragThumb pd = new PointDragThumb();
                pd.DragDelta += pointDragDelta;
                visualChildren.Add(pd);
            }
            poly.UpdateLayout();
            for (int i = 0; i < poly.Points.Count; i++)
            {
                Matrix m= poly.GeometryTransform.Value;
                
                Point p = m.Transform(poly.Points[i]);
                p = poly.TranslatePoint(p, (UIElement)poly.Parent);
                poly.Points[i] = p;
            }
            poly.Stretch = Stretch.None;
            //poly.SetValue(Canvas.LeftProperty, DependencyProperty.UnsetValue);
            //poly.SetValue(Canvas.TopProperty, DependencyProperty.UnsetValue);
            //poly.SetValue(FrameworkElement.WidthProperty, DependencyProperty.UnsetValue);
            //poly.SetValue(FrameworkElement.HeightProperty, DependencyProperty.UnsetValue);
            EditorHelper.SetDependencyProperty(poly, Canvas.LeftProperty, DependencyProperty.UnsetValue);
            EditorHelper.SetDependencyProperty(poly, Canvas.TopProperty, DependencyProperty.UnsetValue);
            EditorHelper.SetDependencyProperty(poly, FrameworkElement.WidthProperty, DependencyProperty.UnsetValue);
            EditorHelper.SetDependencyProperty(poly, FrameworkElement.HeightProperty, DependencyProperty.UnsetValue);


            //poly.SetValue(UIElement.RenderTransformProperty, DependencyProperty.UnsetValue);
            TransformGroup t = new TransformGroup();
            t.Children.Add(new MatrixTransform());
            t.Children.Add(new RotateTransform());
            poly.RenderTransform = t;
            poly.UpdateLayout();
            base.Activate();
        } 
        public override void Deactivate()
        {
            
            Polyline poly = AdornedElement as Polyline;
            Rect b = VisualTreeHelper.GetContentBounds(poly);
            poly.Stretch = Stretch.Fill;
            //poly.SetValue(Canvas.LeftProperty, b.X);
            //poly.SetValue(Canvas.TopProperty, b.Y);
            //poly.SetValue(FrameworkElement.WidthProperty, b.Width);
            //poly.SetValue(FrameworkElement.HeightProperty, b.Height);
            EditorHelper.SetDependencyProperty(poly, Canvas.LeftProperty, b.X);
            EditorHelper.SetDependencyProperty(poly, Canvas.TopProperty, b.Y);
            EditorHelper.SetDependencyProperty(poly, FrameworkElement.WidthProperty, b.Width);
            EditorHelper.SetDependencyProperty(poly, FrameworkElement.HeightProperty, b.Height);

            for (int i = 0; i < poly.Points.Count; i++)
            {
                Point p = poly.Points[i];
                p.X -= b.X;
                p.Y -= b.Y;
                poly.Points[i] = p;
            }
            foreach (PointDragThumb pdt in visualChildren)
            {
                pdt.DragDelta -= pointDragDelta;
                
            }
            visualChildren.Clear(); ;
            poly.UpdateLayout();
            base.Deactivate();
        }
        void pointDragDelta(object sender, DragDeltaEventArgs e)
        {
            Polyline poly = AdornedElement as Polyline;
            double gridDelta = (double)poly.FindResource("DesignerSettings_GridDelta");
            bool gridOn = (bool)poly.FindResource("DesignerSettings_GridOn");
            
            Point p=poly.Points[visualChildren.IndexOf(sender as PointDragThumb)];
            p.X+=e.HorizontalChange;
            p.Y+= e.VerticalChange;
            if (gridOn)
            {
                p.X -= p.X % gridDelta;
                p.X -= p.X % gridDelta;
            }
            poly.Points[visualChildren.IndexOf(sender as PointDragThumb)] = p;
            
            InvalidateArrange();

        }
        protected override Size MeasureOverride(Size finalSize)
        {
            foreach (PointDragThumb pdt in visualChildren)
            {
                pdt.Measure(finalSize);
            }
            return finalSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Polyline poly = AdornedElement as Polyline;            

            foreach (PointDragThumb pdt in visualChildren)
            {
                Point p =/*poly.TranslatePoint(*/poly.Points[visualChildren.IndexOf(pdt)];//,this);
                p.X-=pdt.DesiredSize.Width/2;
                p.Y-=pdt.DesiredSize.Height/2;
                pdt.Arrange(new Rect(p, pdt.DesiredSize));
            }
            return finalSize;
        }

        public override  bool IsSelactable(UIElement el)
        {
            if (el is Polyline)
                return true;
            else return false;
        }
    }
}
