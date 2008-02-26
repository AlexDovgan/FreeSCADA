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
    public class EllipseTool : BasicTool, ITool
    {

        Point startPos;
        
        public EllipseTool(SchemaDocument schema)
            : base(schema)
        {
           
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
                drawingContext.DrawEllipse(Brushes.Gray, new Pen(Brushes.Black, 0.2), startPos, v.X, v.Y);

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
                    Ellipse el = new Ellipse();
                    Canvas.SetLeft(el, b.X);
                    Canvas.SetTop(el, b.Y);
                    el.Width = b.Width;
                    el.Height = b.Height;
                    el.Stroke = Brushes.Black;
                    el.Fill = Brushes.Red;

                    UndoRedoManager.GetUndoBuffer(workedSchema.MainCanvas).AddCommand(new AddObject(el, workedSchema.MainCanvas));
                    workedSchema.MainCanvas.Children.Add(el);
                    manipulator = new MoveResizeRotateManipulator(el,workedSchema);
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
