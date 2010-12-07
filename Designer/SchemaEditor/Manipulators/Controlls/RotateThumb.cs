using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using FreeSCADA.Common;

namespace FreeSCADA.Designer.SchemaEditor.Manipulators.Controls
{
    /// <summary>
    /// Rotating controll for DragResizeRotateManipulator
    /// </summary>
    public class RotateThumb : BaseControl
    {
        double initialAngle;
        private Vector startVector;
        private Point centerPoint;


        private RotateTransform rotateTransform;
        private MatrixTransform matrixTransform;

        private RotateTransform ItemRotateTransform
        {
            get
            {
                return (rotateTransform = (_controlledItem.RenderTransform as TransformGroup).Children[1] as RotateTransform);

            }
        }
        private MatrixTransform ItemMatrixTransform
        {
            get
            {
                return (matrixTransform = (_controlledItem.RenderTransform as TransformGroup).Children[0] as MatrixTransform);

            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RotateThumb(IDocumentView view, FrameworkElement el)
            : base(view,el)
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
            Canvas canvas = _view.MainPanel as Canvas;
            // the RenderTransformOrigin property of DesignerItem defines
            // transformation center relative to its bounds

            centerPoint = _controlledItem.TranslatePoint(
                new Point(_controlledItem.DesiredSize.Width * _controlledItem.RenderTransformOrigin.X,
                          _controlledItem.DesiredSize.Height * _controlledItem.RenderTransformOrigin.Y),
                           canvas);


            // calculate startVector, that is the vector from centerPoint to startPoint
            Point startPoint = GridManager.GetMousePos();
            startVector = Point.Subtract(startPoint, centerPoint);

            // check if the DesignerItem already has a RotateTransform set ...
            initialAngle = ItemRotateTransform.Angle;


        }

        void RotateThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Canvas canvas = _view.MainPanel as Canvas;

            // calculate deltaVector, that is the vector from centerPoint to current mouse position                
            Point currentPoint = GridManager.GetMousePos();
            Vector deltaVector = Point.Subtract(currentPoint, centerPoint);

            //calculate the angle between startVector and dragVector
            double angle = Vector.AngleBetween(startVector, deltaVector);

            // and update the transformation
            EditorHelper.SetDependencyProperty(ItemRotateTransform, RotateTransform.AngleProperty, initialAngle + Math.Round(angle, 0));

            //ItemRotateTransform.Angle = initialAngle + Math.Round(angle, 0);

        }
    }
}
