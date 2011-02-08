using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using FreeSCADA.Common;
using FreeSCADA.Designer.SchemaEditor.Manipulators;
using FreeSCADA.CommonUI.Interfaces;

namespace FreeSCADA.Designer.SchemaEditor.Tools
{
    /// <summary>
    /// tool for rectangle creation
    /// </summary>
    class RectangleTool : DrawTool
    {
        public RectangleTool(IDocumentView view)
            : base(view)
        {

        }

        protected override void DrawPreview(DrawingContext context, Rect rect)
        {
            context.DrawRectangle(Brushes.Gray, new Pen(Brushes.Black, 1), rect);
        }
        protected override FrameworkElement DrawEnded(Rect rect)
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
        public override Type ToolEditingType()
        {
            return typeof(Rectangle);
        }
        public override Type GetToolManipulator()
        {
            return typeof(DragResizeRotateManipulator);

        }
    }

}
