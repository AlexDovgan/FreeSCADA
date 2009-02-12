using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FreeSCADA.Designer.SchemaEditor.Tools
{
    /// <summary>
    /// Ellipse object creation tool
    /// </summary>
    /// 
    class EllipseTool : BaseTool
    {

        Point startPos;
        bool isDragged;
        DrawingVisual objectPrview = new DrawingVisual();
        public EllipseTool(UIElement element)
            : base(element)
        {
            objectPrview.Opacity = 0.5;
            visualChildren.Add(objectPrview);
        }
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (isDragged)
            {
                Vector v = e.GetPosition(this) - startPos;
                DrawingContext drawingContext = objectPrview.RenderOpen();
                if ((System.Windows.Forms.Control.ModifierKeys & System.Windows.Forms.Keys.Control) != 0)
                    v = new Vector(System.Math.Max(v.X, v.Y), System.Math.Max(v.X, v.Y));
                drawingContext.DrawEllipse(Brushes.Gray, new Pen(Brushes.Black, 1), startPos, v.X, v.Y);
                drawingContext.Close();
           }

        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (isDragged)
            {
                Rect b = VisualTreeHelper.GetContentBounds(objectPrview);
                if (!b.IsEmpty)
                {
                    Ellipse el = new Ellipse();
                    Canvas.SetLeft(el, b.X);
                    Canvas.SetTop(el, b.Y);
                    el.Width = b.Width;
                    el.Height = b.Height;
                    el.Stroke = Brushes.Black;
                    el.Fill = Brushes.Gray;
                    NotifyObjectCreated(el);
                    SelectedObject = el;
                    
                }
                isDragged = false;
                objectPrview.RenderOpen().Close();
            }
            ReleaseMouseCapture();
            base.OnPreviewMouseLeftButtonUp(e);
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
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
