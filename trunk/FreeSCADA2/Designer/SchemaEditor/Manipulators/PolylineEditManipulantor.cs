using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using FreeSCADA.Designer.SchemaEditor.Manipulators.Controls;

namespace FreeSCADA.Designer.SchemaEditor.Manipulators
{
    class PolylineEditManipulantor : BaseManipulator
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
                pd.PreviewMouseLeftButtonUp += pd_PreviewMouseLeftButtonUp;
                visualChildren.Add(pd);
            }
            
           // double tmpThnk = poly.StrokeThickness;
           // poly.StrokeThickness = 0;
            //poly.UpdateLayout();
            for (int i = 0; i < poly.Points.Count; i++)
            {
                Matrix m = poly.GeometryTransform.Value;

                Point p = m.Transform(poly.Points[i]);
                p = poly.TranslatePoint(p, (UIElement)poly.Parent);
                poly.Points[i] = p;
            }
           // poly.StrokeThickness = tmpThnk;
           // poly.UpdateLayout();
            poly.Stretch = Stretch.None;
            EditorHelper.SetDependencyProperty(poly, Canvas.LeftProperty, DependencyProperty.UnsetValue);
            EditorHelper.SetDependencyProperty(poly, Canvas.TopProperty, DependencyProperty.UnsetValue);
            EditorHelper.SetDependencyProperty(poly, FrameworkElement.WidthProperty, DependencyProperty.UnsetValue);
            EditorHelper.SetDependencyProperty(poly, FrameworkElement.HeightProperty, DependencyProperty.UnsetValue);

            TransformGroup t = new TransformGroup();
            t.Children.Add(new MatrixTransform());
            t.Children.Add(new RotateTransform());
            poly.RenderTransform = t;
            poly.UpdateLayout();
            base.Activate();
        }

        void pd_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Polyline poly = AdornedElement as Polyline;
            if (poly.Points.Count > 2 && (System.Windows.Forms.Control.ModifierKeys & System.Windows.Forms.Keys.Shift) != 0)
            {
                Point p = poly.Points[visualChildren.IndexOf(sender as PointDragThumb)];

                (sender as PointDragThumb).DragDelta -= pointDragDelta;
                (sender as PointDragThumb).PreviewMouseLeftButtonUp -= pd_PreviewMouseLeftButtonUp;
                visualChildren.Remove((Visual)sender);
                poly.Points.Remove(p);
                poly.UpdateLayout();
            }
        }

        public override void Deactivate()
        {

            Polyline poly = AdornedElement as Polyline;
            Rect b = new Rect();
            b.Y = b.X = System.Double.MaxValue;
            b.Width = b.Height = 0;
            foreach (Point p in poly.Points)
            {
                if (p.X < b.X)
                    b.X = p.X;
                if (p.Y < b.Y)
                    b.Y = p.Y;
                if (p.X > b.Width)
                    b.Width = p.X;
                if (p.Y > b.Height)
                    b.Height = p.Y;
            }
            b.Width -= b.X;
            b.Height -= b.Y;

            poly.Stretch = Stretch.Fill;

            /*for (int i = 0; i < poly.Points.Count; i++)
            {
                Point pp = new Point();
                pp.X = poly.Points[i].X - b.X;
                pp.Y = poly.Points[i].Y - b.Y;
                poly.Points[i] = pp; ;
            }*/
            EditorHelper.SetDependencyProperty(poly, Canvas.LeftProperty, b.X);
            EditorHelper.SetDependencyProperty(poly, Canvas.TopProperty, b.Y);
            EditorHelper.SetDependencyProperty(poly, FrameworkElement.WidthProperty, b.Width);
            EditorHelper.SetDependencyProperty(poly, FrameworkElement.HeightProperty, b.Height);

            foreach (PointDragThumb pdt in visualChildren)
            {
                pdt.DragDelta -= pointDragDelta;
                pdt.PreviewMouseLeftButtonUp -= pd_PreviewMouseLeftButtonUp;
            }
            visualChildren.Clear(); ;
            poly.UpdateLayout();
            base.Deactivate();
        }
        void pointDragDelta(object sender, DragDeltaEventArgs e)
        {
            Polyline poly = AdornedElement as Polyline;
            
            Point p = poly.Points[visualChildren.IndexOf(sender as PointDragThumb)];
            p.X += e.HorizontalChange;
            p.Y += e.VerticalChange;
            GridManager.GetGridManagerFor(adornedElement).AdjustPointToGrid(ref p);
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
                Point p =poly.Points[visualChildren.IndexOf(pdt)];
                
                p.X -= pdt.DesiredSize.Width / 2;
                p.Y -= pdt.DesiredSize.Height / 2;
                
                
                pdt.Arrange(new Rect(p, pdt.DesiredSize));
            }
            return finalSize;
        }

        public override bool IsSelactable(UIElement el)
        {
            if (el is Polyline)
                return true;
            else return false;
        }

        public void AddThumb(Point p)
        {
            Polyline poly = AdornedElement as Polyline;

            PointDragThumb pd = new PointDragThumb();
            pd.DragDelta += pointDragDelta;
            pd.PreviewMouseLeftButtonUp += pd_PreviewMouseLeftButtonUp;
            visualChildren.Add(pd);
            poly.UpdateLayout();
        }
    }
}
