using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;


namespace FreeSCADA.Designer.SchemaEditor.Manipulators.Controls
{
    /// <summary>
    /// Rotating controll for DragResizeRotateManipulator
    /// </summary>
    public class RotateThumb : Thumb
    {
        double initialAngle;
        private Vector startVector;
        private Point centerPoint;

        private FrameworkElement controlledItem;
        private FrameworkElement Controlledtem
        {
            get
            {
                controlledItem = this.DataContext as FrameworkElement;;
                
                return controlledItem;
            }
        }
        private RotateTransform rotateTransform;
        private MatrixTransform matrixTransform;

        private RotateTransform ItemRotateTransform
        {
            get
            {
                return (rotateTransform = (Controlledtem.RenderTransform as TransformGroup).Children[1] as RotateTransform);
         
            }
        }
        private MatrixTransform ItemMatrixTransform
        {
            get
            {
                return (matrixTransform = (Controlledtem.RenderTransform as TransformGroup).Children[0] as MatrixTransform);

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
            Canvas canvas = VisualTreeHelper.GetParent(Controlledtem) as Canvas;
            if (Controlledtem != null && canvas != null)
            {
                // the RenderTransformOrigin property of DesignerItem defines
                // transformation center relative to its bounds
              
                centerPoint = Controlledtem.TranslatePoint(
                    new Point(Controlledtem.DesiredSize.Width * Controlledtem.RenderTransformOrigin.X,
                              Controlledtem.DesiredSize.Height * Controlledtem.RenderTransformOrigin.Y),
                               canvas);
            
            
                // calculate startVector, that is the vector from centerPoint to startPoint
                Point startPoint = GridManager.GetGridManagerFor(controlledItem).GetMousePos();
                startVector = Point.Subtract(startPoint, centerPoint);

                // check if the DesignerItem already has a RotateTransform set ...
                initialAngle = ItemRotateTransform.Angle;
              
            }
        }

        void RotateThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Canvas canvas = VisualTreeHelper.GetParent(Controlledtem) as Canvas;

            if (Controlledtem != null && canvas != null)
            {
                // calculate deltaVector, that is the vector from centerPoint to current mouse position                
                Point currentPoint = GridManager.GetGridManagerFor(controlledItem).GetMousePos();
                Vector deltaVector = Point.Subtract(currentPoint, centerPoint);

                //calculate the angle between startVector and dragVector
                double angle = Vector.AngleBetween(startVector, deltaVector);

                // and update the transformation
                EditorHelper.SetDependencyProperty(ItemRotateTransform, RotateTransform.AngleProperty, initialAngle + Math.Round(angle, 0));

                //ItemRotateTransform.Angle = initialAngle + Math.Round(angle, 0);
           }
        }
    }
}
