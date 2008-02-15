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

namespace FreeSCADA.Scheme.Tools
{
    public enum ToolTypes
    {
        Select,
        VertexSelect,
        Rectangle,
        Ellipse,
        Max
    }

    public class Tool : Adorner
    {
        public Manipulator manipulator;
        public Tool(UIElement adornedElement)
            : base(adornedElement)
        {

        }
        public delegate void ToolStarted(MouseEventArgs e);
        public delegate void ToolFinished(Manipulator m);
        public event ToolFinished Finished;
        public event ToolStarted Started;

        protected void RaisToolStarted(MouseEventArgs e)
        {
            Started(e);
        }
        protected void RaisToolFinished(Manipulator m)
        {
            if (Finished != null)
                Finished(m);

        }


    }

    public class SelectionTool : Tool
    {
        VisualCollection visualChildren;
        Point startPos;
        Canvas workCanvas;
        bool ShiftDown;
        public SelectionTool(Canvas adornedElement)
            : base(adornedElement)
        {
            workCanvas = adornedElement;
            workCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(SelectionTool_MouseLeftButtonDown);
            workCanvas.MouseLeftButtonUp += new MouseButtonEventHandler(SelectionTool_MouseLeftButtonUp);
            workCanvas.MouseMove += new MouseEventHandler(SelectionTool_MouseMove);
            workCanvas.KeyDown += new KeyEventHandler(workCanvas_KeyDown);
            workCanvas.KeyUp += new KeyEventHandler(workCanvas_KeyUp);
            visualChildren = new VisualCollection(this);

        }

        void workCanvas_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift)
            {

                ShiftDown = false;
            }
        }

        void workCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift && ShiftDown == false)
            {


                ShiftDown = true;
            }


        }
        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }

        void SelectionTool_MouseMove(object sender, MouseEventArgs e)
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

        void SelectionTool_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (visualChildren.Count > 0)
            {
                visualChildren.Remove(visualChildren[0]);

            }
            workCanvas.ReleaseMouseCapture();
        }

        void SelectionTool_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pt = e.GetPosition((UIElement)sender);
            workCanvas.CaptureMouse();
            //RaisToolStarted(e);


            // Perform the hit test against a given portion of the visual object tree.
            HitTestResult result = VisualTreeHelper.HitTest(workCanvas, pt);
            if (manipulator != null && ShiftDown != true)
            {
                AdornerLayer.GetAdornerLayer(AdornedElement).Remove(manipulator);
                manipulator = null;
            }

            if (result.VisualHit == workCanvas && ShiftDown != true)
            {

                startPos = e.GetPosition(this);

                DrawingVisual drawingVisual = new DrawingVisual();
                if (visualChildren.Count == 0)
                    visualChildren.Add(drawingVisual);

            }
            else if (result.VisualHit != workCanvas)
            {
                FrameworkElement el = (FrameworkElement)EditorHelper.FindTopParentUnder(workCanvas, (FrameworkElement)result.VisualHit);
                if (ShiftDown != true)
                {
                    RaisToolFinished(manipulator = new GeometryEditManipulator(el));
                    AdornerLayer.GetAdornerLayer(AdornedElement).Add(manipulator);
                }
                else
                {
                    if (!(manipulator is GroupEditManipulator))
                    {
                        GroupEditManipulator m = new GroupEditManipulator(workCanvas);
                        AdornerLayer al = AdornerLayer.GetAdornerLayer(AdornedElement);
                        if (manipulator != null)
                        {
                            al.Remove(manipulator);
                            m.Add(manipulator.AdornedElement);

                        }
                        m.Add(el);
                        al.Add(m);
                        RaisToolFinished(m);
                        manipulator = m;
                    }
                    else

                        ((GroupEditManipulator)manipulator).Add(el);

                }

            }
        }

    }

    public class RectangleTool : Tool
    {
        VisualCollection visualChildren;
        Point startPos;
        Canvas workCanvas;
        bool ShiftDown;
        public RectangleTool(Canvas adornedElement)
            : base(adornedElement)
        {
            workCanvas = adornedElement;
            workCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(RectangleTool_MouseLeftButtonDown);
            workCanvas.MouseLeftButtonUp += new MouseButtonEventHandler(RectangleTool_MouseLeftButtonUp);
            workCanvas.MouseMove += new MouseEventHandler(RectangleTool_MouseMove);
            visualChildren = new VisualCollection(this);

        }
        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }

        void RectangleTool_MouseMove(object sender, MouseEventArgs e)
        {
            if (visualChildren.Count > 0)
            {
                Vector v = e.GetPosition(this) - startPos;

                DrawingVisual vis = (DrawingVisual)visualChildren[0];
                DrawingContext drawingContext = vis.RenderOpen();

                // Create a rectangle and draw it in the DrawingContext.
                Rect rect = new Rect(startPos, v);
                drawingContext.DrawRectangle(Brushes.Gray, new Pen(Brushes.Black, 0.2), rect);

                // Persist the drawing content.
                drawingContext.Close();
                vis.Opacity = 0.5;



            }
        }

        void RectangleTool_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (visualChildren.Count > 0)
            {
                Rect b = VisualTreeHelper.GetContentBounds(visualChildren[0]);
                if (!b.IsEmpty)
                {
                    Rectangle r = new Rectangle();
                    Canvas.SetLeft(r, b.X);
                    Canvas.SetTop(r, b.Y);
                    r.Width = b.Width;
                    r.Height = b.Height;
                    r.Stroke = Brushes.Black;
                    r.Fill = Brushes.Red;
                    workCanvas.Children.Add(r);
                    //AdornerLayer.GetAdornerLayer(AdornedElement).Add(new GeometryEditManipulator(r));
                }
                visualChildren.Remove(visualChildren[0]);


            }
            workCanvas.ReleaseMouseCapture();
        }

        void RectangleTool_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            workCanvas.CaptureMouse();
            //    RaisToolStarted(e);
            startPos = e.GetPosition(this);

            DrawingVisual drawingVisual = new DrawingVisual();
            if (visualChildren.Count == 0)
                visualChildren.Add(drawingVisual);


        }

    }
    public class EllipseTool : Tool
    {
        VisualCollection visualChildren;
        Point startPos;
        Canvas workCanvas;
        bool ShiftDown;
        public EllipseTool(Canvas adornedElement)
            : base(adornedElement)
        {
            workCanvas = adornedElement;
            workCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(EllipseTool_MouseLeftButtonDown);
            workCanvas.MouseLeftButtonUp += new MouseButtonEventHandler(EllipseTool_MouseLeftButtonUp);
            workCanvas.MouseMove += new MouseEventHandler(EllipseTool_MouseMove);
            visualChildren = new VisualCollection(this);

        }
        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }

        void EllipseTool_MouseMove(object sender, MouseEventArgs e)
        {
            if (visualChildren.Count > 0)
            {
                Vector v = e.GetPosition(this) - startPos;

                DrawingVisual vis = (DrawingVisual)visualChildren[0];
                DrawingContext drawingContext = vis.RenderOpen();

                // Create a rectangle and draw it in the DrawingContext.
                Rect rect = new Rect(startPos, v);
                drawingContext.DrawEllipse(Brushes.Gray, new Pen(Brushes.Black, 0.2),startPos,v.X,v.Y);

                // Persist the drawing content.
                drawingContext.Close();
                vis.Opacity = 0.5;



            }
        }

        void EllipseTool_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (visualChildren.Count > 0)
            {
                Rect b = VisualTreeHelper.GetContentBounds(visualChildren[0]);
                if (!b.IsEmpty)
                {
                    Ellipse el= new Ellipse();
                    Canvas.SetLeft(el, b.X);
                    Canvas.SetTop(el, b.Y);
                    el.Width = b.Width;
                    el.Height = b.Height;
                    el.Stroke = Brushes.Black;
                    el.Fill = Brushes.Red; 
                    workCanvas.Children.Add(el);
                    //AdornerLayer.GetAdornerLayer(AdornedElement).Add(new GeometryEditManipulator(r));
                }
                visualChildren.Remove(visualChildren[0]);


            }
            workCanvas.ReleaseMouseCapture();
        }

        void EllipseTool_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            workCanvas.CaptureMouse();
            //    RaisToolStarted(e);
            startPos = e.GetPosition(this);

            DrawingVisual drawingVisual = new DrawingVisual();
            if (visualChildren.Count == 0)
                visualChildren.Add(drawingVisual);


        }

    }
}