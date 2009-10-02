using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


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
                Env.Current.Mode == FreeSCADA.Interfaces.EnvironmentMode.Designer)
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
				if (!(view.RenderTransform is System.Windows.Media.ScaleTransform))
                    view.RenderTransform = new System.Windows.Media.ScaleTransform();
            
    		}

		}

		public WPFShemaContainer()
		{
            this.Initialize();
		}

       
        private void Initialize()
        {
            Child = scroll = new myHelpScrollViewer();
			System.Windows.Automation.AutomationProperties.SetAutomationId(Child, "SchemaCanvas");
			
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
        

       
    }
}
