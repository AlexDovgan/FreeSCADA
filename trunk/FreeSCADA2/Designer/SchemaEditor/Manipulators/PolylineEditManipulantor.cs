using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using FreeSCADA.Designer.SchemaEditor.Manipulators.Controls;
using FreeSCADA.Common.Schema;


namespace FreeSCADA.Designer.SchemaEditor.Manipulators
{
    class PolylineEditManipulantor : BaseManipulator
    {
        private Polyline _poly;
        public PolylineEditManipulantor(UIElement el)
            : base(el)
        {
            _poly = AdornedElement as Polyline;
            if (_poly!=null)
                throw new ArgumentException();
        }
        public override void Activate()
        {
            
            foreach (var p in _poly.Points)
            {
                var pd = new PointDragThumb();
                pd.DragStarted += PointDragStarted;
                pd.DragDelta += PointDragDelta;
                pd.PreviewMouseLeftButtonUp += OnPreviewMouseLeftButtonUp;
                visualChildren.Add(pd);
            }
            
            for (int i = 0; i < _poly.Points.Count; i++)
            {
                var m = _poly.GeometryTransform.Value;
               
                var p = m.Transform(_poly.Points[i]);
                p = _poly.TranslatePoint(p,mainCanvas);
                _poly.Points[i] = p;
            }
            _poly.Stretch = Stretch.None;
            EditorHelper.SetDependencyProperty(_poly, Canvas.LeftProperty, DependencyProperty.UnsetValue);
            EditorHelper.SetDependencyProperty(_poly, Canvas.TopProperty, DependencyProperty.UnsetValue);
            EditorHelper.SetDependencyProperty(_poly, FrameworkElement.WidthProperty, DependencyProperty.UnsetValue);
            EditorHelper.SetDependencyProperty(_poly, FrameworkElement.HeightProperty, DependencyProperty.UnsetValue);

            _poly.RenderTransform = null;
            AdornerLayer.GetAdornerLayer(this).PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(PolylineEditManipulantor_PreviewMouseLeftButtonDown);
          
            _poly.UpdateLayout();
            base.Activate();
        }

        void PolylineEditManipulantor_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if ((System.Windows.Forms.Control.ModifierKeys & System.Windows.Forms.Keys.Control) == 0)
                return;
            
            if(_poly==null)
                return;
            var gridMan=GridManager.GetGridManagerFor(AdornedElement);
            for (int i = 0; i < _poly.Points.Count - 1; i++)
            {
                // Hit test
                var lg = new LineGeometry(_poly.Points[i], _poly.Points[i + 1]);
                var eg = new EllipseGeometry(gridMan.GetMousePos(), gridMan.GridDelta, gridMan.GridDelta);
                var id = eg.FillContainsWithDetail(lg);
                if (id == IntersectionDetail.Intersects)
                {
                    // Insert point to the polyline
                    _poly.Points.Insert(i + 1, gridMan.GetMousePos());
                    // Rendering (new thumbs)
                    AddThumb(gridMan.GetMousePos());
                    e.Handled = true;
                    break;
                }
            }
            InvalidateVisual();
        }

        

        void OnPreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
            if (_poly.Points.Count > 2 && (System.Windows.Forms.Control.ModifierKeys & System.Windows.Forms.Keys.Shift) != 0)
            {
                var  p = _poly.Points[visualChildren.IndexOf(sender as PointDragThumb)];
                (sender as PointDragThumb).DragStarted -= PointDragStarted;
                (sender as PointDragThumb).DragDelta -= PointDragDelta;
                (sender as PointDragThumb).PreviewMouseLeftButtonUp -= OnPreviewMouseLeftButtonUp;
                visualChildren.Remove((Visual)sender);
                _poly.Points.Remove(p);
                _poly.UpdateLayout();
            }
        }

        public override void Deactivate()
        {

            
            var b = new Rect();
            b.Y = b.X = System.Double.MaxValue;
            b.Width = b.Height = 0;
            foreach (var p in _poly.Points)
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

            _poly.Stretch = Stretch.Fill;

            /*for (int i = 0; i < poly.Points.Count; i++)
            {
                Point pp = new Point();
                pp.X = poly.Points[i].X - b.X;
                pp.Y = poly.Points[i].Y - b.Y;
                poly.Points[i] = pp; ;
            }*/
            EditorHelper.SetDependencyProperty(_poly, Canvas.LeftProperty, b.X);
            EditorHelper.SetDependencyProperty(_poly, Canvas.TopProperty, b.Y);
            EditorHelper.SetDependencyProperty(_poly, FrameworkElement.WidthProperty, b.Width);
            EditorHelper.SetDependencyProperty(_poly, FrameworkElement.HeightProperty, b.Height);

            foreach (PointDragThumb pdt in visualChildren)
            {
                pdt.DragStarted -= PointDragStarted;
                pdt.DragDelta -= PointDragDelta;
                pdt.PreviewMouseLeftButtonUp -= OnPreviewMouseLeftButtonUp;
            }
            AdornerLayer.GetAdornerLayer(this).PreviewMouseLeftButtonDown -= new System.Windows.Input.MouseButtonEventHandler(PolylineEditManipulantor_PreviewMouseLeftButtonDown);
            visualChildren.Clear(); ;
            _poly.UpdateLayout();
            base.Deactivate();
        }
        void PointDragStarted(object sender, DragStartedEventArgs e)
        {
            var ub = UndoRedoManager.GetUndoBufferFor(AdornedElement);
            ub.AddCommand(new ModifyGraphicsObject(AdornedElement));
        }   
        void PointDragDelta(object sender, DragDeltaEventArgs e)
        {
            
            
            var p = _poly.Points[visualChildren.IndexOf(sender as PointDragThumb)];
            
            var dragDelta = new Point(e.HorizontalChange, e.VerticalChange);
            System.Diagnostics.Trace.WriteLine("Point " + p.ToString() + "Drag delta " + dragDelta.ToString());
            p= _poly.TranslatePoint(p,this );
            System.Diagnostics.Trace.WriteLine("Point " + p.ToString() + "Drag delta " + dragDelta.ToString());
            p.X += dragDelta.X;
            p.Y += dragDelta.Y;
            p = this.TranslatePoint(p, _poly);
            GridManager.GetGridManagerFor(AdornedElement).AdjustPointToGrid(ref p);
            _poly.Points[visualChildren.IndexOf(sender as PointDragThumb)] = p;
           
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
            

            foreach (PointDragThumb pdt in visualChildren)
            {
                var p =_poly.Points[visualChildren.IndexOf(pdt)];
                p = _poly.TranslatePoint(p, this);
                
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
            return false;
        }

        private void AddThumb(Point p)
        {
            
            var pd = new PointDragThumb();
            pd.DragDelta += PointDragDelta;
            pd.PreviewMouseLeftButtonUp += OnPreviewMouseLeftButtonUp;
            visualChildren.Add(pd);
            _poly.UpdateLayout();
        }

        
    }
}
