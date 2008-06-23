using System.Windows.Controls;
using System.Windows.Media;
using FreeSCADA.Common.Schema;
using FreeSCADA.Designer.Views;
using System.Windows.Input;
using System.Windows;

namespace FreeSCADA.Designer.SchemaEditor
{
    class ZoomViewer : ScrollViewer
    {
        WPFShemaContainer cnt;
        System.Windows.Point origPanPoint;

        public ZoomViewer()
        {
        }

        public ZoomViewer(WPFShemaContainer Cnt)
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
}