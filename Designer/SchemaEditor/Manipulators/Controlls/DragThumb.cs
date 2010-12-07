using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using FreeSCADA.Common;

namespace FreeSCADA.Designer.SchemaEditor.Manipulators.Controls
{
    /// <summary>
    /// Drag controll for DragResizeRotateManipulator
    /// </summary>
    class DragThumb :BaseControl 
    {

        public DragThumb(IDocumentView view, FrameworkElement el)
            : base(view, el)
        {
        }
        void DragThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            FrameworkElement item = _controlledItem;
            if (item != null)
            {
           
                Point dragDelta = new Point(e.HorizontalChange, e.VerticalChange);

                dragDelta = RenderTransform.Transform(dragDelta);

                double left = Canvas.GetLeft(item);
                double top = Canvas.GetTop(item);

                left = double.IsNaN(left) ? 0 : left;
                top = double.IsNaN(top) ? 0 : top;

                double x = left + dragDelta.X;
                double y = top + dragDelta.Y;
             
                EditorHelper.SetDependencyProperty(item, Canvas.LeftProperty, x);
                EditorHelper.SetDependencyProperty(item, Canvas.TopProperty, y);
            }
            e.Handled = false;
        }
    }
}
