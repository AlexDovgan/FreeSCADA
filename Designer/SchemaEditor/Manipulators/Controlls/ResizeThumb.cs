using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace FreeSCADA.Designer.SchemaEditor.Manipulators.Controlls
{
    /// <summary>
    /// resize controll dor DragResizeRotateManipulator
    /// </summary>
    /// 
    class ResizeThumb : Thumb
    {
        private double angle;
        private Point transformOrigin;
        private FrameworkElement controledItem;
        private FrameworkElement ControledItem
        {
            get
            {
                if (controledItem == null)
                {
                    controledItem = this.DataContext as FrameworkElement;
                }
                return controledItem;
            }
        }

        public ResizeThumb()
        {
            Width = 10;
            Height = 10;
            
            base.DragStarted += new DragStartedEventHandler(ResizeThumb_DragStarted);
            base.DragDelta += new DragDeltaEventHandler(ResizeThumb_DragDelta);
        }

        void ResizeThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            if (ControledItem != null)
            {
                transformOrigin = ControledItem.RenderTransformOrigin;

                RotateTransform rotateTransform = (ControledItem.RenderTransform as TransformGroup).Children[1] as RotateTransform;
                if (rotateTransform != null)
                    angle = rotateTransform.Angle * Math.PI / 180.0;   //convert degrees to radians
                else
                    angle = 0.0d;
            }

        }

        void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (ControledItem != null)
            {
                double deltaVertical=0, deltaHorizontal=0;
                double gridDelta = (double)ControledItem.FindResource("DesignerSettings_GridDelta");
                bool gridOn = (bool)ControledItem.FindResource("DesignerSettings_GridOn");
                
           
                Point dragDelta= ControledItem.RenderTransform.Inverse.Transform(new Point(e.HorizontalChange, e.VerticalChange ));
                double w, h,l,t;
                w = ControledItem.Width;
                h = ControledItem.Height;
                t = Canvas.GetTop(ControledItem);
                l = Canvas.GetLeft(ControledItem);

                switch (base.VerticalAlignment)
                {
                    case System.Windows.VerticalAlignment.Bottom:
                        deltaVertical = Math.Min(-dragDelta.Y, ControledItem.ActualHeight - Height);
                       
                        h-= deltaVertical;
                        break;
                    case System.Windows.VerticalAlignment.Top:
                        deltaVertical = Math.Min(dragDelta.Y, ControledItem.ActualHeight - Height);
                        Point p = ControledItem.RenderTransform.Transform(new Point(0, deltaVertical));
                         t += p.Y;
                        l += p.X;
                        h -= deltaVertical;
                        break;
                    default:
                        break;
                }

                switch (base.HorizontalAlignment)
                {
                    case System.Windows.HorizontalAlignment.Left:
                        deltaHorizontal = Math.Min(dragDelta.X, ControledItem.ActualWidth - Width);
                        Point p = ControledItem.RenderTransform.Transform(new Point(deltaHorizontal, 0));
                        t += p.Y;
                        l += p.X;
                        w -= deltaHorizontal;
                        break;
                    case System.Windows.HorizontalAlignment.Right:
                        deltaHorizontal = Math.Min(-dragDelta.X, ControledItem.ActualWidth - Width);
                        w -= deltaHorizontal;
                        break;
                    default:
                        break;
                }
                Point sizeDelta = new Point(deltaHorizontal, deltaVertical);
                Point sizeDeltaTrans = ControledItem.RenderTransform.Transform(sizeDelta);
                Vector v=sizeDelta-sizeDeltaTrans;
                
                double x = l + v.X * controledItem.RenderTransformOrigin.X;
                double y = t+ v.Y * controledItem.RenderTransformOrigin.Y;
                
                /*if (gridOn)
                {
                    x -= x % gridDelta;
                    y -= y % gridDelta;
                }*/
                EditorHelper.SetDependencyProperty(ControledItem, Canvas.LeftProperty, x);
                EditorHelper.SetDependencyProperty(ControledItem, Canvas.TopProperty, y);
                EditorHelper.SetDependencyProperty(ControledItem, FrameworkElement.WidthProperty, w);
                EditorHelper.SetDependencyProperty(ControledItem, FrameworkElement.HeightProperty, h);


            }
            //e.Handled = true;
        }
    }
}
