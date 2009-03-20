using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Documents;
using FreeSCADA.Designer.SchemaEditor.Manipulators.Controls;

namespace FreeSCADA.Designer.SchemaEditor.Manipulators
{
    class PolylineEditManipulantor : BaseManipulator
    {
        
        public PolylineEditManipulantor(UIElement el)
            : base(el)
        {

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
            
            for (int i = 0; i < poly.Points.Count; i++)
            {
                Matrix m = poly.GeometryTransform.Value;
               
                Point p = m.Transform(poly.Points[i]);
                p = poly.TranslatePoint(p,mainCanvas);
                poly.Points[i] = p;
            }
            poly.Stretch = Stretch.None;
            EditorHelper.SetDependencyProperty(poly, Canvas.LeftProperty, DependencyProperty.UnsetValue);
            EditorHelper.SetDependencyProperty(poly, Canvas.TopProperty, DependencyProperty.UnsetValue);
            EditorHelper.SetDependencyProperty(poly, FrameworkElement.WidthProperty, DependencyProperty.UnsetValue);
            EditorHelper.SetDependencyProperty(poly, FrameworkElement.HeightProperty, DependencyProperty.UnsetValue);

            poly.RenderTransform = null;
            AdornerLayer.GetAdornerLayer(this).PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(PolylineEditManipulantor_PreviewMouseLeftButtonDown);
          
            poly.UpdateLayout();
            base.Activate();
        }

        void PolylineEditManipulantor_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if ((System.Windows.Forms.Control.ModifierKeys & System.Windows.Forms.Keys.Control) == 0)
                return;
            Polyline poly = AdornedElement as Polyline;
            GridManager gridMan=GridManager.GetGridManagerFor(AdornedElement);
            for (int i = 0; i < poly.Points.Count - 1; i++)
            {
                // Hit test
                LineGeometry lg = new LineGeometry(poly.Points[i], poly.Points[i + 1]);
                EllipseGeometry eg = new EllipseGeometry(gridMan.GetMousePos(), gridMan.GridDelta, gridMan.GridDelta);
                IntersectionDetail id = eg.FillContainsWithDetail(lg);
                if (id == IntersectionDetail.Intersects)
                {
                    // Insert point to the polyline
                    poly.Points.Insert(i + 1, gridMan.GetMousePos());
                    // Rendering (new thumbs)
                    AddThumb(gridMan.GetMousePos());
                    e.Handled = true;
                    break;
                }
            }
            InvalidateVisual();
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
            AdornerLayer.GetAdornerLayer(this).PreviewMouseLeftButtonDown -= new System.Windows.Input.MouseButtonEventHandler(PolylineEditManipulantor_PreviewMouseLeftButtonDown);
            visualChildren.Clear(); ;
            poly.UpdateLayout();
            base.Deactivate();
        }
        void pointDragDelta(object sender, DragDeltaEventArgs e)
        {
            Polyline poly = AdornedElement as Polyline;
            
            Point p = poly.Points[visualChildren.IndexOf(sender as PointDragThumb)];
            
            Point dragDelta = new Point(e.HorizontalChange, e.VerticalChange);
            System.Diagnostics.Trace.WriteLine("Point " + p.ToString() + "Drag delta " + dragDelta.ToString());
            p= poly.TranslatePoint(p,this );
            System.Diagnostics.Trace.WriteLine("Point " + p.ToString() + "Drag delta " + dragDelta.ToString());
            p.X += dragDelta.X;
            p.Y += dragDelta.Y;
            p = this.TranslatePoint(p, poly);
            GridManager.GetGridManagerFor(AdornedElement).AdjustPointToGrid(ref p);
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
                p = poly.TranslatePoint(p, this);
                
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
