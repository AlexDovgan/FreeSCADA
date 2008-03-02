using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System;


namespace FreeSCADA.Schema.Manipulators
{
    public class RotateThumb : Thumb
    {
        double initialAngle;
        private Vector startVector;
        private Point centerPoint;

        private FrameworkElement designerItem;
        private FrameworkElement DesignerItem
        {
            get
            {
                if (designerItem == null)
                {
                    designerItem = this.DataContext as FrameworkElement;;
                }
                return designerItem;
            }
        }
        private RotateTransform rotateTransform;
        private MatrixTransform matrixTransform;

        private RotateTransform ItemRotateTransform
        {
            get
            {
                return (rotateTransform = (DesignerItem.RenderTransform as TransformGroup).Children[1] as RotateTransform);
         
            }
        }
        private MatrixTransform ItemMatrixTransform
        {
            get
            {
                return (matrixTransform = (DesignerItem.RenderTransform as TransformGroup).Children[0] as MatrixTransform);

            }
        }


        public RotateThumb()
        {
            base.DragDelta += new DragDeltaEventHandler(RotateThumb_DragDelta);
            base.DragStarted += new DragStartedEventHandler(RotateThumb_DragStarted);
            base.DragCompleted += new DragCompletedEventHandler(RotateThumb_DragCompleted);
            Width = 50;
            Height = 50;

        }
        
        void RotateThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
           Canvas.SetLeft(DesignerItem, Canvas.GetLeft(DesignerItem) + rotateTransform.Value.OffsetX);
            Canvas.SetTop(DesignerItem, Canvas.GetTop(DesignerItem) + rotateTransform.Value.OffsetY);
            rotateTransform.CenterX = 0;
            rotateTransform.CenterY = 0;
            
        }

        void RotateThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            Canvas canvas = VisualTreeHelper.GetParent(DesignerItem) as Canvas;
            if (DesignerItem != null && canvas != null)
            {
                // the RenderTransformOrigin property of DesignerItem defines
                // transformation center relative to its bounds
                centerPoint = DesignerItem.TranslatePoint(
                    new Point(DesignerItem.Width /2,
                              DesignerItem.Height/2),
                               canvas);
                Point p = ItemMatrixTransform.Transform(new Point(DesignerItem.Width / 2, DesignerItem.Height / 2));
                ItemRotateTransform.CenterX = p.X;
                ItemRotateTransform.CenterY = p.Y;
                
                Canvas.SetLeft(DesignerItem, Canvas.GetLeft(DesignerItem) - ItemRotateTransform.Value.OffsetX);
                Canvas.SetTop(DesignerItem, Canvas.GetTop(DesignerItem) - ItemRotateTransform.Value.OffsetY);
            
                // calculate startVector, that is the vector from centerPoint to startPoint
                Point startPoint = Mouse.GetPosition(canvas);
                startVector = Point.Subtract(startPoint, centerPoint);

                // check if the DesignerItem already has a RotateTransform set ...
                initialAngle = ItemRotateTransform.Angle;
              
            }
        }

        void RotateThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Canvas canvas = VisualTreeHelper.GetParent(DesignerItem) as Canvas;

            if (DesignerItem != null && canvas != null)
            {
                // calculate deltaVector, that is the vector from centerPoint to current mouse position                
                Point currentPoint = Mouse.GetPosition(canvas);
                Vector deltaVector = Point.Subtract(currentPoint, centerPoint);

                //calculate the angle between startVector and dragVector
                double angle = Vector.AngleBetween(startVector, deltaVector);

                // and update the transformation
                rotateTransform.Angle = initialAngle + Math.Round(angle, 0);
           }
        }
    }
}
