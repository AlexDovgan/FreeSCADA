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
    public enum ToolTypes
    {
        Select,
        VertexSelect,
        Rectangle,
        Ellipse,
        Max
    }

    public abstract class Tool : Adorner
    {
        public Manipulator manipulator;
        public VisualCollection visualChildren;
        
        public Tool(UIElement adornedElement)
            : base(adornedElement)
        {
            visualChildren = new VisualCollection(this);
            adornedElement.Focus();
        }
        
        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }

        public virtual void OnCanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        { }
        public virtual void OnCanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        { }
        public virtual void OnCanvasMouseMove(object sender, MouseEventArgs e)
        { }
        public virtual void OnCanvasKeyDown(object sender, KeyEventArgs e)
        { }
        public virtual void OnCanvasKeyUp(object sender, KeyEventArgs e)
        { }
        public void Activate()
        {
            AdornedElement.MouseLeftButtonDown += new MouseButtonEventHandler(OnCanvasMouseLeftButtonDown);
            AdornedElement.MouseLeftButtonUp += new MouseButtonEventHandler(OnCanvasMouseLeftButtonUp);
            AdornedElement.MouseMove += new MouseEventHandler(OnCanvasMouseMove);
            AdornedElement.KeyDown += new KeyEventHandler(OnCanvasKeyDown);
            AdornedElement.KeyUp += new KeyEventHandler(OnCanvasKeyUp);
        }
        public void Deactivate()
        {
            AdornedElement.MouseLeftButtonDown -= OnCanvasMouseLeftButtonDown;
            AdornedElement.MouseLeftButtonUp -= OnCanvasMouseLeftButtonUp;
            AdornedElement.MouseMove -= OnCanvasMouseMove;
            AdornedElement.KeyDown -= OnCanvasKeyDown;
            AdornedElement.KeyUp -= OnCanvasKeyUp;
            if (manipulator != null)
                AdornerLayer.GetAdornerLayer(AdornedElement).Remove(manipulator);

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

    public class SelectionTool : Tool, ITool 
    {

        Point startPos;
        Canvas workCanvas;
        bool ShiftDown;
        public SelectionTool(Canvas adornedElement)
            : base(adornedElement)
        {
            workCanvas = adornedElement;
            
        }

        public String  ToolName
        {
            get{ return "Selection Tool";}
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
            {

                ShiftDown = false;
            }
        }

        public override void OnCanvasKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift && ShiftDown == false)
            {


                ShiftDown = true;
            }


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

        public  override void OnCanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (visualChildren.Count > 0)
            {
                visualChildren.Remove(visualChildren[0]);

            }
            workCanvas.ReleaseMouseCapture();
        }

        public override void OnCanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

    public class RectangleTool : Tool, ITool 
    {
        
        Point startPos;
        Canvas workCanvas;
        public RectangleTool(Canvas adornedElement)
            : base(adornedElement)
        {
            workCanvas = adornedElement;
        
        }
        public String ToolName
        {
            get { return "Rectangle Tool"; }
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
        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }

        public override void OnCanvasMouseMove(object sender, MouseEventArgs e)
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

        public override void OnCanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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
                    UndoRedoManager.GetUndoBuffer(workCanvas).AddCommand(new AddObject(r, workCanvas));

                    workCanvas.Children.Add(r);
                    //AdornerLayer.GetAdornerLayer(AdornedElement).Add(new GeometryEditManipulator(r));
                }
                visualChildren.Remove(visualChildren[0]);


            }
            workCanvas.ReleaseMouseCapture();
        }

        public override void OnCanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            workCanvas.CaptureMouse();
            //    RaisToolStarted(e);
            startPos = e.GetPosition(this);

            DrawingVisual drawingVisual = new DrawingVisual();
            if (visualChildren.Count == 0)
                visualChildren.Add(drawingVisual);


        }

    }
    public class EllipseTool : Tool, ITool 
    {

        Point startPos;
        Canvas workCanvas;
        public EllipseTool(Canvas adornedElement)
            : base(adornedElement)
        {
            workCanvas = adornedElement;
        }

        public String ToolName
        {
            get { return "Ellipse Tool"; }
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
        public override void OnCanvasMouseMove(object sender, MouseEventArgs e)
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

        public override void OnCanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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

                    UndoRedoManager.GetUndoBuffer(workCanvas).AddCommand(new AddObject(el, workCanvas));
                    workCanvas.Children.Add(el);
                    //AdornerLayer.GetAdornerLayer(AdornedElement).Add(new GeometryEditManipulator(r));
                }
                visualChildren.Remove(visualChildren[0]);


            }
            workCanvas.ReleaseMouseCapture();

        }

        public override void OnCanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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