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
    public class RectangleTool : BasicTool, ITool
    {

        Point startPos;
        public RectangleTool(FSScheme scheme)
            : base(scheme)
        {
            

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
            base.OnCanvasMouseMove(sender, e);
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
            base.OnCanvasMouseLeftButtonUp(sender, e);
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
                    UndoRedoManager.GetUndoBuffer(workedScheme.MainCanvas).AddCommand(new AddObject(r, workedScheme.MainCanvas));
                    workedScheme.MainCanvas.Children.Add(r);
                    manipulator = new MoveResizeRotateManipulator(r);
                    AdornerLayer.GetAdornerLayer(workedScheme.MainCanvas).Add(manipulator);

                }
                visualChildren.Remove(visualChildren[0]);
            }
            workedScheme.MainCanvas.ReleaseMouseCapture();
        }

        public override void OnCanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnCanvasMouseLeftButtonDown(sender, e);
            workedScheme.MainCanvas.CaptureMouse();
            //    RaisToolStarted(e);
            startPos = e.GetPosition(this);

            DrawingVisual drawingVisual = new DrawingVisual();
            if (visualChildren.Count == 0)
                visualChildren.Add(drawingVisual);


        }

    }

}
