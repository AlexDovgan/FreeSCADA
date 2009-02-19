using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FreeSCADA.Designer.SchemaEditor.Tools
{
    /// <summary>
    /// tool for rectangle creation
    /// </summary>
    class RectangleTool : BaseTool
    {
        Rect rect;

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
                Point rectStart;
                DrawingContext drawingContext = objectPrview.RenderOpen();
                if ((System.Windows.Forms.Control.ModifierKeys & System.Windows.Forms.Keys.Control) != 0)
                    v = new Vector(System.Math.Max(v.X, v.Y), System.Math.Max(v.X, v.Y));

                if ((System.Windows.Forms.Control.ModifierKeys & System.Windows.Forms.Keys.Shift) != 0)
                {
                    rectStart = new Point(startPos.X - v.X / 2, startPos.Y - v.Y / 2);
                }
                else
                    rectStart = startPos;
 
                rect = new Rect(rectStart, v);
                drawingContext.DrawRectangle(Brushes.Gray, new Pen(Brushes.Black, 1), rect);
                drawingContext.Close();
            }
            base.OnPreviewMouseMove(e);
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
             if (isDragged)
             {
                //Rect b = VisualTreeHelper.GetContentBounds(objectPrview);
                if (!rect.IsEmpty)
                {
                    Rectangle r = new Rectangle();
                    Canvas.SetLeft(r, rect.X);
                    Canvas.SetTop(r, rect.Y);
                    r.Width = rect.Width;
                    r.Height = rect.Height;
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
