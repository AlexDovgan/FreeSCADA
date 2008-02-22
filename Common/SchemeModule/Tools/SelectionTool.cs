using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using FreeSCADA.Scheme.Commands;
using FreeSCADA.Scheme.Manipulators;
using FreeSCADA.Scheme.Helpers;
using FreeSCADA.Scheme.UndoRedo;
using FreeSCADA.ShellInterfaces;

namespace FreeSCADA.Scheme.Tools
{
    public class SelectionTool : BasicTool, ITool
    {

        Point startPos;
        bool ShiftDown;
        public SelectionTool(FSScheme scheme)
            : base(scheme)
        {


        }
        #region ITool implementation
        public String ToolName
        {
            get { return "Selection Tool"; }
        }

        public String ToolGroup
        {
            get { return "Graphics Tools"; }
        }
        public System.Drawing.Bitmap ToolIcon
        {
            get
            {
                return new System.Drawing.Bitmap(10, 10);
            }
        }

        public override void OnCanvasKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift)
                ShiftDown = false;

        }
        #endregion

        public override void OnCanvasKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift && ShiftDown == false)
                ShiftDown = true;
        }

        public override void OnCanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (visualChildren.Count > 0)
            {

                DrawingVisual vis = (DrawingVisual)visualChildren[0];
                DrawingContext drawingContext = vis.RenderOpen();

                // Create a rectangle and draw it in the DrawingContext.
                Vector v = e.GetPosition(this) - startPos;
                Rect rect = new Rect(startPos, v);

                drawingContext.DrawRectangle(Brushes.Gray, new Pen(Brushes.Black, 0.2), rect);

                // Persist the drawing content.
                drawingContext.Close();
                vis.Opacity = 0.5;

            }
        }

        public override void OnCanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (visualChildren.Count > 0)
            {
                visualChildren.Remove(visualChildren[0]);

            }
            workedScheme.MainCanvas.ReleaseMouseCapture();
        }

        public override void OnCanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pt = e.GetPosition((UIElement)sender);
            workedScheme.MainCanvas.CaptureMouse();
            //RaisToolStarted(e);


            // Perform the hit test against a given portion of the visual object tree.
            HitTestResult result = VisualTreeHelper.HitTest(workedScheme.MainCanvas, pt);
            if (manipulator != null && ShiftDown != true)
            {
                AdornerLayer.GetAdornerLayer(AdornedElement).Remove(manipulator);
                manipulator = null;
            }

            if (result.VisualHit == workedScheme.MainCanvas && ShiftDown != true)
            {

                startPos = e.GetPosition(this);

                DrawingVisual drawingVisual = new DrawingVisual();
                if (visualChildren.Count == 0)
                    visualChildren.Add(drawingVisual);

            }
            else if (result.VisualHit != workedScheme.MainCanvas)
            {
                FrameworkElement el = (FrameworkElement)EditorHelper.FindTopParentUnder(workedScheme.MainCanvas, (FrameworkElement)result.VisualHit);
                if (ShiftDown != true)
                {
                    RaiseToolFinished(this, e);
                    AdornerLayer.GetAdornerLayer(AdornedElement).Add(manipulator = new MoveResizeRotateManipulator(el));
                }
                else
                {
                    if (!(manipulator is GroupEditManipulator))
                    {
                        GroupEditManipulator m = new GroupEditManipulator(workedScheme.MainCanvas);
                        AdornerLayer al = AdornerLayer.GetAdornerLayer(AdornedElement);
                        if (manipulator != null)
                        {
                            al.Remove(manipulator);
                            m.Add(manipulator.AdornedElement);

                        }
                        m.Add(el);
                        al.Add(m);
                        RaiseToolFinished(this, e);
                        manipulator = m;
                    }
                    else

                        ((GroupEditManipulator)manipulator).Add(el);

                }

            }
        }

    }

}
