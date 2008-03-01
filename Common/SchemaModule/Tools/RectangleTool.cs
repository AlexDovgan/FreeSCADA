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
using FreeSCADA.Schema.Context_Menu;
using FreeSCADA.Schema.Manipulators;
using FreeSCADA.Schema.UndoRedo;
using FreeSCADA.ShellInterfaces;

namespace FreeSCADA.Schema.Tools
{
    public class RectangleTool : BasicTool, ITool
    {

        Point startPos;
        bool isDragged;
        DrawingVisual objectPrview = new DrawingVisual();
        public RectangleTool(SchemaDocument schema)
            : base(schema)
        {
            
            objectPrview.Opacity = 0.5;
            visualChildren.Add(objectPrview);    

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

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
 
            if (isDragged)
            {
                Vector v = e.GetPosition(this) - startPos;
                DrawingContext drawingContext = objectPrview.RenderOpen();
                Rect rect = new Rect(startPos, v);
                drawingContext.DrawRectangle(Brushes.Gray, new Pen(Brushes.Black, 0.2), rect);
           
                drawingContext.Close();
            }
            base.OnPreviewMouseMove(e);
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
             if (isDragged)
             {
                Rect b = VisualTreeHelper.GetContentBounds(objectPrview);
                if (!b.IsEmpty)
                {
                    Rectangle r = new Rectangle();
                    Canvas.SetLeft(r, b.X);
                    Canvas.SetTop(r, b.Y);
                    r.Width = b.Width;
                    r.Height = b.Height;
                    r.Stroke = Brushes.Black;
                    r.Fill = Brushes.Red;
                    UndoRedoManager.GetUndoBuffer(workedSchema).AddCommand(new AddGraphicsObject(r));
                    manipulator = new DragResizeRotate(r, workedSchema);
                    AdornerLayer.GetAdornerLayer(workedSchema.MainCanvas).Add(manipulator);

                }
                isDragged = false;
                objectPrview.RenderOpen().Close();

            }
            ReleaseMouseCapture();
            base.OnPreviewMouseLeftButtonUp(e);
        }

        protected override void OnPreviewMouseLeftButtonDown( MouseButtonEventArgs e)
        {
            
            CaptureMouse();
            startPos = e.GetPosition(this);
            isDragged = true;
            base.OnPreviewMouseLeftButtonDown(e);

        }
        
    }

}
