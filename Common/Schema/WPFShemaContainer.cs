using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows;


namespace FreeSCADA.Common.Schema
{
    public class myHelpScrollViewer : ScrollViewer
    {
        // The only solution I know for enablig the positioning of elements with arrows in Canvas inside ScrollViewer
        // ScrollViewer normally consumes Arrow keys for scrolling
        // So I need to derive own class and override OnKeyDown
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if ((e.Key == System.Windows.Input.Key.Left || e.Key == System.Windows.Input.Key.Right || e.Key == System.Windows.Input.Key.Up || e.Key == System.Windows.Input.Key.Down) &&
                ((System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) == System.Windows.Input.ModifierKeys.None) &&
                Env.Current.Mode == FreeSCADA.ShellInterfaces.EnvironmentMode.Designer)
            {
                // the Keydown event for Arrows is not catched by the ScrollViewer, but is used to position the selected element inside Canvas
                // Arrows = positioning
                e.Handled = false;
            }
            else
                // Ctrl/Arrows = scrolling
                // And all other keys
                base.OnKeyDown(e);  // normal behavior
        }
    }

    public class WPFShemaContainer : System.Windows.Forms.Integration.ElementHost
	{
		//SchemaDocument document;
        FrameworkElement view;
        myHelpScrollViewer scroll;
        System.Windows.Point origPanPoint;
        public delegate void ZoomDelegate(Point pt);
        public event ZoomDelegate ZoomInEvent;
        public event ZoomDelegate ZoomOutEvent;

        public FrameworkElement View
		{
			get { return (Child as ScrollViewer).Content as FrameworkElement ; }
			set
			{
				
				(Child as ScrollViewer).Content =view= value;
				view.Focusable= false;
                if (!(view.RenderTransform is System.Windows.Media.ScaleTransform))
                    view.RenderTransform = new System.Windows.Media.ScaleTransform();

                if (!view.Resources.Contains("DesignerSettings_GridOn"))
                {
                    view.Resources.Add("DesignerSettings_GridOn", true);
                    view.Resources.Add("DesignerSettings_GridDelta", 10.0);
                }
				//document.MainCanvas.Background = resources["GridBackgroundBrush"] as DrawingBrush;

                if ((bool)view.FindResource("DesignerSettings_GridOn") == true && Env.Current.Mode == FreeSCADA.ShellInterfaces.EnvironmentMode.Designer)
                {
                    ViewGrid(view as Canvas, true);
                }

				//document.MainCanvas.Background = resources["GridBackgroundBrush"] as DrawingBrush;

			}

		}

		public WPFShemaContainer()
		{
            this.Initialize();
		}

       
        private void Initialize()
        {
            Child = scroll = new myHelpScrollViewer();
			
			Child.SnapsToDevicePixels = true;
            scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            Child.PreviewMouseWheel += new MouseWheelEventHandler(Child_MouseWheel);
            Child.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(Child_MouseDown);
            Child.PreviewMouseUp += new System.Windows.Input.MouseButtonEventHandler(Child_MouseUp);
            Child.PreviewMouseMove += new System.Windows.Input.MouseEventHandler(Child_MouseMove);
        }

        
        void Child_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Point pt = e.GetPosition(Child);
                if (e.Delta > 0)
                    NotifyZoomInEvent(pt);
                else
                    NotifyZoomOutEvent(pt);
                e.Handled = true;   
            }
            
        }

        void Child_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.MiddleButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                origPanPoint = e.GetPosition(Child);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Hand;
            }
            else
            {
                e.Handled = false;
            }
        }

        void Child_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.MiddleButton == System.Windows.Input.MouseButtonState.Released)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            else
            {
                e.Handled = false;
            }
        }

        void Child_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.MiddleButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                System.Windows.Point currPanPoint = e.GetPosition(Child);

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Hand;
                scroll.ScrollToVerticalOffset(scroll.VerticalOffset + origPanPoint.Y - currPanPoint.Y);
                scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset + origPanPoint.X - currPanPoint.X);
                origPanPoint.X = currPanPoint.X;
                origPanPoint.Y = currPanPoint.Y;
            }
            else
            {
                e.Handled = false;
            }
        }

        protected void NotifyZoomInEvent(Point pt)
        {
            if (ZoomInEvent != null)
                ZoomInEvent(pt);
        }
        protected void NotifyZoomOutEvent(Point pt)
        {
            if (ZoomOutEvent != null)
                ZoomOutEvent(pt);
        }
        


        public static void ViewGrid(Canvas view, bool activate)
        {
            if (activate)
            {
                double grid = (double)view.FindResource("DesignerSettings_GridDelta");
                Color c = ((view as Canvas).Background as SolidColorBrush).Color;
                c.R = (byte)(255 - c.R);
                c.G = (byte)(255 - c.G);
                c.B = (byte)(255 - c.B);

                GeometryDrawing aDrawing = new GeometryDrawing();

                // Use geometries to describe two overlapping ellipses.
                //EllipseGeometry ellipse1 = new EllipseGeometry();
                //ellipse1.RadiusX = 1;
                //ellipse1.RadiusY = 1;
                //ellipse1.Center = new Point(0, 0);

                Rect r = new Rect(0, 0, grid, grid);
                RectangleGeometry rect1 = new RectangleGeometry(r);
                //GeometryGroup ellipses = new GeometryGroup();
                //ellipses.Children.Add(ellipse1);
                //ellipses.Children.Add(rect1);

                // Add the geometry to the drawing.
                aDrawing.Geometry = rect1;

                // Specify the drawing's fill.

                aDrawing.Brush = ((view as Canvas).Background as SolidColorBrush);

                // Specify the drawing's stroke.
                Pen stroke = new Pen();
                stroke.Thickness = 0.5;
                stroke.Brush = new SolidColorBrush(c);
                aDrawing.Pen = stroke;

                // Create a DrawingBrush
                DrawingBrush myDrawingBrush = new DrawingBrush();
                myDrawingBrush.Drawing = aDrawing;
                myDrawingBrush.Stretch = Stretch.None;
                myDrawingBrush.TileMode = TileMode.Tile;
                myDrawingBrush.Viewport = new Rect(0, 0, grid, grid);
                myDrawingBrush.ViewportUnits = BrushMappingMode.Absolute;
                view.Background = myDrawingBrush;
            }
            else
            {
                if (view.Background is DrawingBrush)
                {
                    view.Background = ((view.Background as DrawingBrush).Drawing as GeometryDrawing).Brush;
                }
            }
        }
    }
}
