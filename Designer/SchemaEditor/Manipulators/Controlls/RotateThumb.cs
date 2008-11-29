using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;


namespace FreeSCADA.Designer.SchemaEditor.Manipulators.Controlls
{
    /// <summary>
    /// Rotating controll for DragResizeRotateManipulator
    /// </summary>
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
                designerItem = this.DataContext as FrameworkElement;;
                
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

		/// <summary>
		/// Constructor
		/// </summary>
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
            
        }

        void RotateThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            Canvas canvas = VisualTreeHelper.GetParent(DesignerItem) as Canvas;
            if (DesignerItem != null && canvas != null)
            {
                // the RenderTransformOrigin property of DesignerItem defines
                // transformation center relative to its bounds
              
                centerPoint = DesignerItem.TranslatePoint(
                    new Point(DesignerItem.DesiredSize.Width * DesignerItem.RenderTransformOrigin.X,
                              DesignerItem.DesiredSize.Height * DesignerItem.RenderTransformOrigin.Y),
                               canvas);
            
            
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
                ItemRotateTransform.Angle = initialAngle + Math.Round(angle, 0);
           }
        }
    }
}
