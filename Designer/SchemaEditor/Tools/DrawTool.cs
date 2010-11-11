using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using FreeSCADA.Common;

namespace FreeSCADA.Designer.SchemaEditor.Tools
{
    /// <summary>
    /// Base tool implementation for tools that just draw a single object.
    /// </summary>
    abstract public class DrawTool : BaseTool
    {
        Point startPos;
        Rect rect;
        bool isDragged;

        GridManager gridManager;

        DrawingVisual objectPrview = new DrawingVisual();
        /// <summary>
        /// 
        /// </summary>
        public enum SnapOrgin {
            /// <summary>
            /// 
            /// </summary>
            TopLeft,
            /// <summary>
            /// 
            /// </summary>
            Center
        }
        /// <summary>
        /// 
        /// </summary>
        public DrawTool()
            : base()
        {
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
        /// <summary>
        /// 
        /// </summary>
        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
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

                if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
                {
                    rect.X = startPos.X - rect.Width / 2;
                    rect.Y = startPos.Y - rect.Height / 2;
                }


                if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
             if (isDragged)
             {
                if (Math.Max(rect.Width, rect.Height) > 2.0)
                {
                    UIElement uie = DrawEnded(rect);
                    NotifyObjectCreated(uie);
              
                }
                isDragged = false;
                objectPrview.RenderOpen().Close();
                rect = Rect.Empty;
            }
            ReleaseMouseCapture();
            base.OnPreviewMouseLeftButtonUp(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="rect"></param>
        protected abstract void DrawPreview(DrawingContext context, Rect rect);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        protected abstract UIElement DrawEnded(Rect rect);
    }

}
