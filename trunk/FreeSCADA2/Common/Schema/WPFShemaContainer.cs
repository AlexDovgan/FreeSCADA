﻿using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows;

namespace FreeSCADA.Common.Schema
{
	public class WPFShemaContainer : System.Windows.Forms.Integration.ElementHost
	{
		SchemaDocument document;
        ScrollViewer scroll;
        System.Windows.Point origPanPoint;
        public delegate void ZoomDelegate(Point pt);
        public event ZoomDelegate ZoomInEvent;
        public event ZoomDelegate ZoomOutEvent;

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

       
        private void Initialize()
        {
            //Child =scroll= new ZoomViewer(this);
            Child = scroll = new ScrollViewer();
			//Child.Focusable = false;
			
			Child.SnapsToDevicePixels = true;
            scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            Child.PreviewMouseWheel += new MouseWheelEventHandler(Child_MouseWheel);
            Child.PreviewKeyDown += new KeyEventHandler(Child_KeyDown);
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

        void Child_KeyDown(object sender,KeyEventArgs e)
        {
            if ((e.Key == System.Windows.Input.Key.Left || e.Key == System.Windows.Input.Key.Right || e.Key == System.Windows.Input.Key.Up || e.Key == System.Windows.Input.Key.Down) &&
                (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) == System.Windows.Input.ModifierKeys.None)
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
