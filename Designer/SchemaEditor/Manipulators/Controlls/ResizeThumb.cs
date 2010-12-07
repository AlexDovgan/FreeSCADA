using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using FreeSCADA.Common;

namespace FreeSCADA.Designer.SchemaEditor.Manipulators.Controls
{
    /// <summary>
    /// resize controll dor DragResizeRotateManipulator
    /// </summary>
    /// 
    class ResizeThumb : BaseControl
    {
        private double angle;
        private Point transformOrigin;
        

        public ResizeThumb(IDocumentView view,FrameworkElement el):base(view,el)
        {
            Width = 10;
            Height = 10;
                  
            base.DragStarted += new DragStartedEventHandler(ResizeThumb_DragStarted);
            base.DragDelta += new DragDeltaEventHandler(ResizeThumb_DragDelta);
        }

        void ResizeThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            if (_controlledItem != null)
            {
                transformOrigin = _controlledItem.RenderTransformOrigin;

                RotateTransform rotateTransform = (_controlledItem.RenderTransform as TransformGroup).Children[1] as RotateTransform;
                if (rotateTransform != null)
                    angle = rotateTransform.Angle * Math.PI / 180.0;   //convert degrees to radians
                else
                    angle = 0.0d;
            }
            
        }

        void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (_controlledItem != null)
            {
                double deltaVertical = 0, deltaHorizontal = 0;
                Point dragDelta = new Point(e.HorizontalChange, e.VerticalChange);
                dragDelta =GridManager.AdjustPointToGrid(dragDelta);

                Matrix m =((Transform)this.TransformToVisual(_controlledItem)).Value;
                m.OffsetX = 0;
                m.OffsetY = 0;
                System.Windows.Media.Transform t = new MatrixTransform(m);
                dragDelta = t.Transform(dragDelta);

                Rect r = new Rect(Canvas.GetLeft(_controlledItem), Canvas.GetTop(_controlledItem),
                                _controlledItem.Width, _controlledItem.Height);
                //r = GridManager.AdjustRectToGrid(r);                

                switch (base.VerticalAlignment)
                {
                    case System.Windows.VerticalAlignment.Bottom:
                        deltaVertical = Math.Min(-dragDelta.Y, _controlledItem.ActualHeight - Height);

                        r.Height -= deltaVertical;
                        break;
                    case System.Windows.VerticalAlignment.Top:
                        deltaVertical = Math.Min(dragDelta.Y, _controlledItem.ActualHeight - Height);
                        Point p = _controlledItem.RenderTransform.Transform(new Point(0, deltaVertical));
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
                        deltaHorizontal = Math.Min(dragDelta.X, _controlledItem.ActualWidth - Width);
                        Point p = _controlledItem.RenderTransform.Transform(new Point(deltaHorizontal, 0));
                        r.Y += p.Y;
                        r.X += p.X;
                        r.Width -= deltaHorizontal;
                        break;
                    case System.Windows.HorizontalAlignment.Right:
                        deltaHorizontal = Math.Min(-dragDelta.X, _controlledItem.ActualWidth - Width);
                        r.Width -= deltaHorizontal;
                        break;
                    default:
                        break;
                }
                Point sizeDelta = new Point(deltaHorizontal, deltaVertical);
                Point sizeDeltaTrans = _controlledItem.RenderTransform.Transform(sizeDelta);
                Vector v = sizeDelta - sizeDeltaTrans;

                r.X = r.X + v.X * _controlledItem.RenderTransformOrigin.X;
                r.Y = r.Y + v.Y * _controlledItem.RenderTransformOrigin.Y;


                EditorHelper.SetDependencyProperty(_controlledItem, Canvas.LeftProperty, r.X);
                EditorHelper.SetDependencyProperty(_controlledItem, Canvas.TopProperty, r.Y);
                EditorHelper.SetDependencyProperty(_controlledItem, FrameworkElement.WidthProperty, r.Width);
                EditorHelper.SetDependencyProperty(_controlledItem, FrameworkElement.HeightProperty, r.Height);
               

            }
            //e.Handled = true;
        }
    }
}
