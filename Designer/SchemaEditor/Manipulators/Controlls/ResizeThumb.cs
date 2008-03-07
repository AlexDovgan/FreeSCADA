using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows;

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
        private FrameworkElement parentItem;
        private FrameworkElement ParentItem
        {
            get
            {
                if (parentItem == null)
                {
                    parentItem = this.DataContext as FrameworkElement;
                }
                return parentItem;
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
            if (ParentItem != null)
            {
                transformOrigin = ParentItem.RenderTransformOrigin;

                RotateTransform rotateTransform = (ParentItem.RenderTransform as TransformGroup).Children[1] as RotateTransform;
                if (rotateTransform != null)
                    angle = rotateTransform.Angle * Math.PI / 180.0;   //convert degrees to radians
                else
                    angle = 0.0d;
            }

        }

        void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (ParentItem != null)
            {
                double deltaVertical=0, deltaHorizontal=0;
                
           
                Point dragDelta= ParentItem.RenderTransform.Inverse.Transform(new Point(e.HorizontalChange, e.VerticalChange ));
                double w, h;
                w = ParentItem.Width;
                h = ParentItem.Height;
                switch (base.VerticalAlignment)
                {
                    case System.Windows.VerticalAlignment.Bottom:
                        deltaVertical = Math.Min(-dragDelta.Y, ParentItem.ActualHeight - Height);
                        ParentItem.Height -= deltaVertical;
                        break;
                    case System.Windows.VerticalAlignment.Top:
                        deltaVertical = Math.Min(dragDelta.Y, ParentItem.ActualHeight - Height);
                        Point p = ParentItem.RenderTransform.Transform(new Point(0, deltaVertical));
                        Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + p.Y);
                        Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + p.X);
                        ParentItem.Height -= deltaVertical;
                        break;
                    default:
                        break;
                }

                switch (base.HorizontalAlignment)
                {
                    case System.Windows.HorizontalAlignment.Left:
                        deltaHorizontal = Math.Min(dragDelta.X, ParentItem.ActualWidth - Width);
                        Point p = ParentItem.RenderTransform.Transform(new Point(deltaHorizontal, 0));
                        Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + p.Y);
                        Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + p.X);
                        ParentItem.Width -= deltaHorizontal;
                        break;
                    case System.Windows.HorizontalAlignment.Right:
                        deltaHorizontal = Math.Min(-dragDelta.X, ParentItem.ActualWidth - Width);
                         ParentItem.Width -= deltaHorizontal;
                        break;
                    default:
                        break;
                }
                Point sizeDelta = new Point(deltaHorizontal, deltaVertical);
                Point sizeDeltaTrans = ParentItem.RenderTransform.Transform(sizeDelta);
                Vector v=sizeDelta-sizeDeltaTrans;

                Canvas.SetTop(ParentItem, Canvas.GetTop(ParentItem) + v.Y * parentItem.RenderTransformOrigin.Y);
                Canvas.SetLeft(ParentItem, Canvas.GetLeft(ParentItem) + v.X * parentItem.RenderTransformOrigin.X);
            }
            //e.Handled = true;
        }
    }
}
