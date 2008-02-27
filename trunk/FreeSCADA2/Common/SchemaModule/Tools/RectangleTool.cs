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
using FreeSCADA.Schema.Commands;
using FreeSCADA.Schema.Manipulators;
using FreeSCADA.Schema.UndoRedo;
using FreeSCADA.ShellInterfaces;

namespace FreeSCADA.Schema.Tools
{
    public class RectangleTool : BasicTool, ITool
    {

        Point startPos;
        public RectangleTool(SchemaDocument schema)
            : base(schema)
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
            base.OnCanvasMouseMove(sender, e);
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
                    UndoRedoManager.GetUndoBuffer(workedSchema.MainCanvas).AddCommand(new AddObject(r, workedSchema.MainCanvas));
                    workedSchema.MainCanvas.Children.Add(r);
                    manipulator = new DragResizeRotate(r, workedSchema);
                    AdornerLayer.GetAdornerLayer(workedSchema.MainCanvas).Add(manipulator);

                }
                visualChildren.Remove(visualChildren[0]);
            }
            workedSchema.MainCanvas.ReleaseMouseCapture();
            base.OnCanvasMouseLeftButtonUp(sender, e);
        }

        public override void OnCanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            workedSchema.MainCanvas.CaptureMouse();
            startPos = e.GetPosition(this);

            DrawingVisual drawingVisual = new DrawingVisual();
            if (visualChildren.Count == 0)
                visualChildren.Add(drawingVisual);
            base.OnCanvasMouseLeftButtonDown(sender, e);

        }

    }

}
