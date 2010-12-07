using System.Windows.Controls.Primitives;
using FreeSCADA.Common;
using System.Windows;

namespace FreeSCADA.Designer.SchemaEditor.Manipulators.Controls
{
    /// <summary>
    /// Drag controll for DragResizeRotateManipulator
    /// </summary>
    class PointDragThumb: BaseControl
    {
        public PointDragThumb(IDocumentView view, FrameworkElement el)
            : base(view, el)
        {
        }
    }
}


