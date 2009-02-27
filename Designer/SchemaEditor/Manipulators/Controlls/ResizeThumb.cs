using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace FreeSCADA.Designer.SchemaEditor.Manipulators.Controls
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
                Point dragDelta = new Point(e.HorizontalChange, e.VerticalChange);
                GridManager.GetGridManagerFor(controledItem).AdjustPointToGrid(ref dragDelta);
                dragDelta= ControledItem.RenderTransform.Inverse.Transform(dragDelta);
                
                Rect r = new Rect(Canvas.GetLeft(ControledItem), Canvas.GetTop(ControledItem),
                                ControledItem.Width,
                                ControledItem.Height);
                GridManager.GetGridManagerFor(controledItem).AdjustRectToGrid(ref r);                
                
                switch (base.VerticalAlignment)
                {
                    case System.Windows.VerticalAlignment.Bottom:
                        deltaVertical = Math.Min(-dragDelta.Y, ControledItem.ActualHeight - Height);
                       
                        r.Height-= deltaVertical;
                        break;
                    case System.Windows.VerticalAlignment.Top:
                        deltaVertical = Math.Min(dragDelta.Y, ControledItem.ActualHeight - Height);
                        Point p = ControledItem.RenderTransform.Transform(new Point(0, deltaVertical));
                        r.Y += p.Y;
                        r.X += p.X;
                        r.Height -= deltaVertical;
                        break;
                    default:
                        break;
                }

                switch (base.HorizontalAlignment)
                {
                    case System.Windows.HorizontalAlignment.Left:
                        deltaHorizontal = Math.Min(dragDelta.X, ControledItem.ActualWidth - Width);
                        Point p = ControledItem.RenderTransform.Transform(new Point(deltaHorizontal, 0));
                        r.Y += p.Y;
                        r.X += p.X;
                        r.Width -= deltaHorizontal;
                        break;
                    case System.Windows.HorizontalAlignment.Right:
                        deltaHorizontal = Math.Min(-dragDelta.X, ControledItem.ActualWidth - Width);
                        r.Width -= deltaHorizontal;
                        break;
                    default:
                        break;
                }
                Point sizeDelta = new Point(deltaHorizontal, deltaVertical);
                Point sizeDeltaTrans = ControledItem.RenderTransform.Transform(sizeDelta);
                Vector v=sizeDelta-sizeDeltaTrans;
                
                r.X = r.X + v.X * controledItem.RenderTransformOrigin.X;
                r.Y = r.Y+ v.Y * controledItem.RenderTransformOrigin.Y;


                EditorHelper.SetDependencyProperty(ControledItem, Canvas.LeftProperty, r.X);
                EditorHelper.SetDependencyProperty(ControledItem, Canvas.TopProperty, r.Y);
                EditorHelper.SetDependencyProperty(ControledItem, FrameworkElement.WidthProperty, r.Width );
                EditorHelper.SetDependencyProperty(ControledItem, FrameworkElement.HeightProperty, r.Height );


            }
            //e.Handled = true;
        }
    }
}
