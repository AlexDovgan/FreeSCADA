using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using FreeSCADA.Designer.Views;

namespace FreeSCADA.Designer.SchemaEditor.Tools
{
    /// <summary>
    /// tool for rectangle creation
    /// </summary>
    class RectangleTool : DrawTool
    {
        public RectangleTool(UIElement element)
            : base(element)
        {

        }

        protected override void DrawPreview(DrawingContext context, Rect rect)
        {
            context.DrawRectangle(Brushes.Gray, new Pen(Brushes.Black, 1), rect);
        }
        protected override UIElement DrawEnded(Rect rect)
        {
            Rectangle recttangle = new Rectangle();
            Canvas.SetLeft(recttangle, rect.X);
            Canvas.SetTop(recttangle, rect.Y);
            recttangle.Width = rect.Width;
            recttangle.Height = rect.Height;
            recttangle.Stroke = Brushes.Black;
            recttangle.Fill = Brushes.Gray;

            return recttangle;
        }
    }

}
