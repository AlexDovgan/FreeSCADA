using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
namespace FreeSCADA.Designer.SchemaEditor.Tools
{
    class TextBoxTool:BaseTool
    {
        Rect rect;
        Point startPos;
        bool isDragged;
        DrawingVisual objectPrview = new DrawingVisual();

        public TextBoxTool(UIElement element)
            : base(element)
        {

            objectPrview.Opacity = 0.5;
            visualChildren.Add(objectPrview);
            
        }

      
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {

            if (isDragged)
            {
                Vector v = gridManager.GetMousePos() - startPos;
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

                rect= new Rect(rectStart, v);
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
                    TextBlock text = new TextBlock();
                    Canvas.SetLeft(text, rect.X);
                    Canvas.SetTop(text, rect.Y);
          
                    text.Width = rect.Width;
                    text.Height = rect.Height;
                    text.Text = "You can write text here"; 
                    text.TextWrapping = TextWrapping.Wrap;
                    NotifyObjectCreated(text);
                    SelectedObject = text;

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
                startPos = gridManager.GetMousePos();
                isDragged = true;
            }

            e.Handled = false;
        }
        /*protected override BaseManipulator CreateToolManipulator(UIElement obj)
        {
            return new TextBoxManipulator(obj);
        }*/

    }
}
