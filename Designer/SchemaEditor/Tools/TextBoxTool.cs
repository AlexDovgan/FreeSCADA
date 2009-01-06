using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
namespace FreeSCADA.Designer.SchemaEditor.Tools
{
    class TextBoxTool:BaseTool
    {
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
                    TextBlock text = new TextBlock();
                    Canvas.SetLeft(text, b.X);
                    Canvas.SetTop(text, b.Y);
          
                    text.Width = b.Width;
                    text.Height = b.Height;
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
                startPos = e.GetPosition(this);
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
