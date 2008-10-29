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
using FreeSCADA.Common.Schema;
using FreeSCADA.Designer.SchemaEditor.SchemaCommands;
using FreeSCADA.Designer.SchemaEditor.Manipulators;
using FreeSCADA.Designer.SchemaEditor.UndoRedo;
using FreeSCADA.ShellInterfaces;

namespace FreeSCADA.Designer.SchemaEditor.Tools
{
    /// <summary>
    /// tool for rectangle creation
    /// </summary>
    class RectangleTool : BaseTool
    {

        Point startPos;
        bool isDragged;
        DrawingVisual objectPrview = new DrawingVisual();
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="element"></param>
        public RectangleTool(UIElement element)
            : base(element)
        {
            
            objectPrview.Opacity = 0.5;
            visualChildren.Add(objectPrview);

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
                drawingContext.DrawRectangle(Brushes.Gray, new Pen(Brushes.Black, 1), rect);
           
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
                    r.Fill = Brushes.Gray;
                    NotifyObjectCreated(r);
                    SelectedObject = r;

                }
                isDragged = false;
                objectPrview.RenderOpen().Close();

            }
            ReleaseMouseCapture();
            base.OnPreviewMouseLeftButtonUp(e);
        }

        protected override void OnPreviewMouseLeftButtonDown( MouseButtonEventArgs e)
        {

            base.OnPreviewMouseLeftButtonDown(e);
            if (!e.Handled)
            {
                CaptureMouse();
                startPos = e.GetPosition(this);
                isDragged = true;
            }

            e.Handled = false;
        }
        
        
    }

}
