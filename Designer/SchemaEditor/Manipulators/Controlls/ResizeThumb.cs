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
    class ResizeThumb : Thumb
    {
        
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
            
        }

        void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (ParentItem != null)
            {
                double deltaVertical, deltaHorizontal;
                
           
                Point dragDelta= ParentItem.RenderTransform.Inverse.Transform(new Point(e.HorizontalChange, e.VerticalChange ));
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
            }
            //e.Handled = true;
        }
    }
}
