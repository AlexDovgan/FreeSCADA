using System.Windows.Controls;
using System.Windows.Media;
using FreeSCADA.Common.Schema;
using FreeSCADA.Designer.Views;
using System.Windows.Input;
using System.Windows;

namespace FreeSCADA.Designer.SchemaEditor
{
    class myScrollViewer: ScrollViewer
    {
        private WPFShemaContainer cnt;
        private System.Windows.Point origPanPoint;

        public myScrollViewer()
        {
        }

        public myScrollViewer(WPFShemaContainer Cnt)
        {
            cnt = Cnt;
            this.MouseDown += new System.Windows.Input.MouseButtonEventHandler(Child_MouseDown);
            this.MouseUp += new System.Windows.Input.MouseButtonEventHandler(Child_MouseUp);
            this.MouseMove += new System.Windows.Input.MouseEventHandler(Child_MouseMove);
        }

        protected override void OnMouseWheel(System.Windows.Input.MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Point pt = e.GetPosition(cnt.Child);
                if (e.Delta > 0)
                    cnt.ZoomIn(pt.X, pt.Y);
                else
                    cnt.ZoomOut(pt.X, pt.Y);
            }
            else
            {
                base.OnMouseWheel(e);
            }
        }
        void Child_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.MiddleButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                origPanPoint = e.GetPosition(this);
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
                System.Windows.Point currPanPoint = e.GetPosition(this);

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Hand;
                ScrollToVerticalOffset(VerticalOffset + origPanPoint.Y - currPanPoint.Y);
                ScrollToHorizontalOffset(HorizontalOffset + origPanPoint.X - currPanPoint.X);
                origPanPoint.X = currPanPoint.X;
                origPanPoint.Y = currPanPoint.Y;
            }
            else
            {
                e.Handled = false;
            }
        }
    }

	class WPFShemaContainer : System.Windows.Forms.Integration.ElementHost
	{
		SchemaDocument document;
        public SchemaView view;
		
		public SchemaDocument Document
		{
			get { return document; }
			set
			{
				document = value;
				(Child as ScrollViewer).Content = document.MainCanvas;
				document.MainCanvas.Focusable = false;
                if (!(document.MainCanvas.RenderTransform is System.Windows.Media.ScaleTransform))
                    document.MainCanvas.RenderTransform = new System.Windows.Media.ScaleTransform();
                    
                
				//document.MainCanvas.Background = resources["GridBackgroundBrush"] as DrawingBrush;

			}

		}

		public WPFShemaContainer()
		{
            this.Initialize();
		}

        public WPFShemaContainer(SchemaView View)
        {
            view = View;
            this.Initialize();
        }

        private void Initialize()
        {
            Child = new myScrollViewer(this);
			//Child.Focusable = false;
			
			Child.SnapsToDevicePixels = true;
            (Child as myScrollViewer).HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
        }

        public void ZoomIn(double x, double y)
        {
            view.ZoomIn(x, y);
        }
        public void ZoomOut(double x, double y)
        {
            view.ZoomOut(x, y);
        }
    }
}
