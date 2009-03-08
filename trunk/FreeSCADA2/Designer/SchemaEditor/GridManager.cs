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
    /// <summary>
    /// Grid manager class
    /// implemented threу Attached properties for grid settings
    /// </summary>
    public class GridManager : Adorner
    {
        #region AttachedProperties
        /// <summary>
        /// Grid snapling on attached€ property
        /// </summary>
        public static readonly DependencyProperty GridOnProperty = DependencyProperty.RegisterAttached(
            "GridOn",
            typeof(Boolean),
            typeof(GridManager),
            new FrameworkPropertyMetadata());

        /// <summary>
        /// Gred showing on/off attached property
        /// </summary>
        public static readonly DependencyProperty ShowGridProperty = DependencyProperty.RegisterAttached(
            "ShowGrid",
            typeof(Boolean),
            typeof(GridManager),
            new FrameworkPropertyMetadata());
        /// <summary>
        /// Grid delta value
        /// </summary>
        public static readonly DependencyProperty GridDeltaProperty = DependencyProperty.RegisterAttached(
            "GridDelta",
            typeof(Double),
            typeof(GridManager),
            new FrameworkPropertyMetadata());
        /// <summary>
        /// delta atached property setter
        /// </summary>
        /// <param name="c"> object for setting</param>
        /// <param name="value">value</param>
        public static void SetGridDelta(Canvas c, Double value)
        {
            c.SetValue(GridDeltaProperty, value);
        }
        /// <summary>
        /// delta attached property getter
        /// </summary>
        /// <param name="c"> object is asked on property</param>
        /// <returns>value </returns>
        public static Double GetGridDelta(Canvas c)
        {
            return (Double)c.GetValue(GridDeltaProperty);
        }
        /// <summary>
        ///  grid snapping mode attached property setter 
        /// </summary>
        /// <param name="c">object </param>
        /// <param name="value"> value </param>
        public static void SetGridOn(Canvas c, Boolean value)
        {
            c.SetValue(GridOnProperty, value);
        }
        /// <summary>
        /// grid snapping mode attached property getter
        /// </summary>
        /// <param name="c"> object</param>
        /// <returns>value </returns>
        public static Boolean GetGridOn(Canvas c)
        {
            return (Boolean)c.GetValue(GridOnProperty);
        }
        /// <summary>
        /// grid showing attached property setter 
        /// </summary>
        /// <param name="c"> object</param>
        /// <param name="value">value</param>
        public static void SetShowGrid(Canvas c, Boolean value)
        {
            c.SetValue(ShowGridProperty, value);
        }
        /// <summary>
        /// grid shoing attached property getter
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static Boolean GetShowGrid(Canvas c)
        {
            return (Boolean)c.GetValue(ShowGridProperty);
        }

        #endregion AttachedProperties
        #region Properties
        /// <summary>
        /// CLR stumb property for GridOn attached Property
        /// </summary>
        public Boolean GridOn
        {
            get
            {
                return GetGridOn(AdornedElement as Canvas);
            }
            set
            {
                SetGridOn(AdornedElement as Canvas, value);

            }
        }
        /// <summary>
        /// CLR stumb property for GridDelta attached Property
        /// </summary>
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
        /// <summary>
        /// CLR stumb property for ShowGrid attached property
        /// </summary>
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

        private Vector delta = new Vector();
        bool snapActive = false;
        /// <summary>
        /// return grid manager for any object belong to canvas
        /// </summary>
        /// <param name="el">any uielement belonged to canvas</param>
        /// <returns>GridManager instance registerd as adorner</returns>

        public static GridManager GetGridManagerFor(UIElement el)
        {
            AdornerLayer al = AdornerLayer.GetAdornerLayer(el);
            Canvas c = EditorHelper.FindTopParent(el);
            if (al.GetAdorners(c) != null)
                foreach (Adorner ad in al.GetAdorners(c))
                    if (ad is GridManager)
                        return ad as GridManager;
            GridManager gm = new GridManager(c);
            al.Add(gm);
            return gm;

        }
        /// <summary>
        /// object can be constructed only with calling  GetGridManagerFor
        /// </summary>
        /// <param name="el"></param>
        protected GridManager(Canvas el)
            : base(el)
        {
            if (el.ReadLocalValue(GridOnProperty) == DependencyProperty.UnsetValue)
                SetGridOn(el, true);
            if (el.ReadLocalValue(GridDeltaProperty) == DependencyProperty.UnsetValue)
                SetGridDelta(el, 10.0);
            if (el.ReadLocalValue(ShowGridProperty) == DependencyProperty.UnsetValue)
                SetShowGrid(el, true);
            DependencyPropertyDescriptor.FromProperty(ShowGridProperty, typeof(Canvas)).AddValueChanged(el, PropertyChanged);
            DependencyPropertyDescriptor.FromProperty(GridDeltaProperty, typeof(Canvas)).AddValueChanged(el, PropertyChanged);
            //AdornerLayer.GetAdornerLayer(AdornedElement).MouseMove += new MouseEventHandler(GridManager_MouseMove);
        }
        /// <summary>
        /// try to implementing grid snapping throug mouse snapping
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GridManager_MouseMove(object sender, MouseEventArgs e)
        {
            
            Point pos = e.GetPosition(AdornedElement);
            Point oldPos = pos;
            GridManager.GetGridManagerFor(AdornedElement).AdjustPointToGrid(ref pos);
            delta = (oldPos - pos);
            if (!snapActive&& (Math.Abs(delta.X) > GridDelta / 3 || Math.Abs(delta.Y) > GridDelta / 3 ))
            {
                snapActive = true;
            }
            else if (snapActive && (Math.Abs(delta.X) < GridDelta / 3 || Math.Abs(delta.Y) < GridDelta / 3))
            {
                GridManager.GetGridManagerFor(AdornedElement).AdjustPointToGrid(ref pos);
                pos = PointToScreen(pos);
                System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)pos.X, (int)pos.Y);
                snapActive = false;

            }


        }

        private Brush CreateGridBrush()
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
            return myDrawingBrush;
        }
        /// <summary>
        /// OnRender overriding 
        /// </summary>
        /// <param name="dc"></param>
        protected override void OnRender(DrawingContext dc)
        {
            if (ShowGrid)
                dc.DrawRectangle(CreateGridBrush(), null, new Rect(AdornedElement.RenderSize));
        }
        /// <summary>
        /// property changing hendler for repainting intiation
        /// </summary>
        /// <param name="ogj"></param>
        /// <param name="e"></param>
        protected void PropertyChanged(object ogj, EventArgs e)
        {
            InvalidateVisual();
        }

        /// <summary>
        /// return curent mouse pos snapped to grid
        /// </summary>
        /// <returns></returns>
        public Point GetMousePos()
        {
            Point pos = Mouse.GetPosition(AdornedElement);
            AdjustPointToGrid(ref pos);
            return pos;
        }
        /// <summary>
        /// Adjusting point to grid 
        /// </summary>
        /// <param name="point">Point reference</param>

        public void AdjustPointToGrid(ref Point point)
        {
            if (!GridOn)
                return;
            point.X = Math.Round(point.X / GridDelta, 0) * GridDelta;
            point.Y = Math.Round(point.Y / GridDelta, 0) * GridDelta;
        }
        /// <summary>
        /// adjucting size to grid 
        /// </summary>
        /// <param name="size">Size reference</param>
        public void AdjustSizeToGrid(ref Size size)
        {
            if (!GridOn)
                return;

            size.Height = Math.Round(size.Height / GridDelta, 0) * GridDelta;
            size.Width = Math.Round(size.Width / GridDelta, 0) * GridDelta;
        }
        /// <summary>
        /// adjusting rectangle to grid
        /// </summary>
        /// <param name="rect">Rect reference</param>
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
