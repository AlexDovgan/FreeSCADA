using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;

namespace FreeSCADA.Designer.SchemaEditor
{
    class GridManager : Adorner
    {
        #region AttachedProperties
        public static readonly DependencyProperty GridOnProperty = DependencyProperty.RegisterAttached(
            "GridOn",
            typeof(Boolean),
            typeof(GridManager),
            new FrameworkPropertyMetadata());
        public static readonly DependencyProperty ShowGridProperty = DependencyProperty.RegisterAttached(
            "ShowGrid",
            typeof(Boolean),
            typeof(GridManager),
            new FrameworkPropertyMetadata());

        public static readonly DependencyProperty GridDeltaProperty = DependencyProperty.RegisterAttached(
            "GridDelta",
            typeof(Double),
            typeof(GridManager),
            new FrameworkPropertyMetadata());

        public static void SetGridDelta(Canvas c, Double value)
        {
            c.SetValue(GridDeltaProperty, value);
        }
        public static Double GetGridDelta(Canvas c)
        {
            return (Double)c.GetValue(GridDeltaProperty);
        }

        public static void SetGridOn(Canvas c, Boolean value)
        {
            c.SetValue(GridOnProperty, value);
        }
        public static Boolean GetGridOn(Canvas c)
        {
            return (Boolean)c.GetValue(GridOnProperty);
        }
        public static void SetShowGrid(Canvas c, Boolean value)
        {
            c.SetValue(ShowGridProperty, value);
        }
        public static Boolean GetShowGrid(Canvas c)
        {
            return (Boolean)c.GetValue(ShowGridProperty);
        }
        
        #endregion AttachedProperties
        #region Properties
        public Boolean GridOn
        {
            get
            {
               return GetGridOn(AdornedElement as Canvas);
            }
            set
            {
                SetGridOn(AdornedElement as Canvas,value);
          
            }
        }
        public double GridDelta
        {
            get
            {
                return GetGridDelta(AdornedElement as Canvas);
            }
            set
            {
                SetGridDelta(AdornedElement as Canvas, value);
                
            }
        }
        public Boolean ShowGrid
        {
            get
            {
                return GetShowGrid(AdornedElement as Canvas);
            }
            set
            {
                SetShowGrid(AdornedElement as Canvas, value);
          
            }
        }
        #endregion Properties

        protected Vector delta = new Vector();
        public static GridManager GetGridManagerFor(UIElement el)
        {
            AdornerLayer al=AdornerLayer.GetAdornerLayer(el);
            Canvas c= EditorHelper.FindTopParent(el);
            if (al.GetAdorners(c) != null)
                foreach (Adorner ad in al.GetAdorners(c))
                    if (ad is GridManager)
                        return ad as GridManager;
            GridManager gm = new GridManager(c);
            al.Add(gm);
            return gm;
            
        }
        protected Brush CreateGridBrush()
        {

            Color c = ((AdornedElement as Canvas).Background as SolidColorBrush).Color;
            c.R = (byte)(255 - c.R);
            c.G = (byte)(255 - c.G);
            c.B = (byte)(255 - c.B);

            GeometryDrawing aDrawing = new GeometryDrawing();

            Rect r = new Rect(0, 0, GridDelta, GridDelta);
            RectangleGeometry rect1 = new RectangleGeometry(r);

            // Add the geometry to the drawing.
            aDrawing.Geometry = rect1;
            
            // Specify the drawing's fill.

            aDrawing.Brush = Brushes.Transparent;
            //((AdornedElement as Canvas).Background as SolidColorBrush);

            // Specify the drawing's stroke.
            Pen stroke = new Pen();
            stroke.Thickness = 0.1;
            stroke.Brush = new SolidColorBrush(c);
            aDrawing.Pen = stroke;
           
            // Create a DrawingBrush
            DrawingBrush myDrawingBrush = new DrawingBrush();
            myDrawingBrush.Drawing = aDrawing;
            myDrawingBrush.Stretch = Stretch.None;
            myDrawingBrush.TileMode = TileMode.Tile;
            myDrawingBrush.Viewport = new Rect(0, 0, GridDelta, GridDelta);
            myDrawingBrush.ViewportUnits = BrushMappingMode.Absolute;
            
            myDrawingBrush.Opacity = 0.5;
            return  myDrawingBrush;
        }
        protected override void OnRender(DrawingContext dc)
        {
            if(ShowGrid)
                dc.DrawRectangle(CreateGridBrush(), null, new Rect(AdornedElement.RenderSize));
        }
        protected GridManager(Canvas el):base(el)
        {
            if (el.ReadLocalValue(GridOnProperty) == DependencyProperty.UnsetValue)
                SetGridOn(el, true);
            if (el.ReadLocalValue(GridDeltaProperty) == DependencyProperty.UnsetValue)
                SetGridDelta(el, 10.0);
            if (el.ReadLocalValue(ShowGridProperty) == DependencyProperty.UnsetValue)
                SetShowGrid(el,true);
            DependencyPropertyDescriptor.FromProperty(ShowGridProperty,typeof(Canvas)).AddValueChanged(el, PropertyChanged);
            DependencyPropertyDescriptor.FromProperty(GridDeltaProperty, typeof(Canvas)).AddValueChanged(el, PropertyChanged);

        }
        protected void PropertyChanged(object ogj, EventArgs e)
        {
            InvalidateVisual();
        }
        //try to make common point for grid snaping
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            
          /* Point pos =  e.GetPosition(AdornedElement);
            Point oldPos = pos;
            GridManager.GetGridManagerFor(AdornedElement).AdjustPointToGrid(ref pos);
            delta += (oldPos - pos);
            if (Math.Abs(delta.X) > GridDelta / 2 || Math.Abs(delta.Y) > GridDelta / 2)
            {
                pos.X += delta.X;
                pos.Y += delta.Y;
                delta.X = 0; delta.Y = 0;
                GridManager.GetGridManagerFor(AdornedElement).AdjustPointToGrid(ref pos);
            }
            pos = PointToScreen(pos);
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)pos.X, (int)pos.Y); ;
                
            */
            
            base.OnPreviewMouseMove(e);
        }
        public Point GetMousePos()
        {
            Point pos = Mouse.GetPosition(AdornedElement);
            AdjustPointToGrid(ref pos);
            return pos;
        }
        public void AdjustPointToGrid(ref Point point)
        {
            if (!GridOn)
                return;
            point.X = Math.Round(point.X / GridDelta, 0) * GridDelta;
            point.Y = Math.Round(point.Y / GridDelta, 0) * GridDelta;
        }
        public void AdjustSizeToGrid(ref Size size)
        {
            if (!GridOn)
                return;
            
            size.Height = Math.Round(size.Height / GridDelta, 0) * GridDelta;
            size.Width = Math.Round(size.Width / GridDelta, 0) * GridDelta;
        }

        public void AdjustRectToGrid(ref Rect rect)
        {
            if (!GridOn)
                return;
            Point p1 = new Point(rect.X, rect.Y);
            Point p2 = new Point(rect.X + rect.Width, rect.Y + rect.Height);
            //Point p3 = p1;

            AdjustPointToGrid(ref p1);

            //Vector v = p3 - p1;
            //p2 = p2 - v;

            AdjustPointToGrid(ref p2);

            rect = new Rect(p1, p2);
        }
       

    }
}
