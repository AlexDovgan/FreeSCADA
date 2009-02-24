using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System;

namespace FreeSCADA.Designer.SchemaEditor.Tools
{
    /// <summary>
    /// Base tool implementation for tools that just draw a single object.
    /// </summary>
    abstract class DrawTool : BaseTool
    {
        Point startPos;
        Rect rect;
        bool isDragged;

        GridManager gridManager;

        DrawingVisual objectPrview = new DrawingVisual();

        public enum SnapOrgin {
            TopLeft,
            Center
        }
        
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="element"></param>
        public DrawTool(UIElement element)
            : base(element)
        {
            
            objectPrview.Opacity = 0.5;
            visualChildren.Add(objectPrview);

            gridManager = GridManager.GetGridManagerFor(element);
        }
        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
 
            if (isDragged)
            {
                rect = new Rect(
                    new Point(
                        startPos.X,
                        startPos.Y),
                    new Point(
                        gridManager.GetMousePos().X,
                        gridManager.GetMousePos().Y));

                if ((System.Windows.Forms.Control.ModifierKeys & System.Windows.Forms.Keys.Shift) != 0)
                {
                    rect.X = startPos.X - rect.Width / 2;
                    rect.Y = startPos.Y - rect.Height / 2;
                }


                if ((System.Windows.Forms.Control.ModifierKeys & System.Windows.Forms.Keys.Control) != 0)
                {
                    rect.Height = Math.Min(rect.Height, rect.Width);
                    rect.Width = rect.Height;
                }

                rect = new Rect(rect.Left, rect.Top, rect.Width, rect.Height);
                
                DrawingContext drawingContext = objectPrview.RenderOpen();
                DrawPreview(drawingContext, rect);
                drawingContext.Close();
            }
            base.OnPreviewMouseMove(e);
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
             if (isDragged)
             {
                if (Math.Max(rect.Width, rect.Height) > 2.0)
                {
                    UIElement uie = DrawEnded(rect);
                    NotifyObjectCreated(uie);
                    SelectedObject = uie;
                }
                isDragged = false;
                objectPrview.RenderOpen().Close();
                rect = Rect.Empty;
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
                startPos = gridManager.GetMousePos();
                isDragged = true;
            }

            e.Handled = false;
        }

        protected abstract void DrawPreview(DrawingContext context, Rect rect);
        protected abstract UIElement DrawEnded(Rect rect);
    }

}
