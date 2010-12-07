using System.Windows;
using System;
using System.Windows.Input;
using FreeSCADA.Designer.SchemaEditor.Manipulators;
using FreeSCADA.Common;

namespace FreeSCADA.Designer.SchemaEditor.Tools
{
    class ActionEditTool:BaseTool
    {
        
        public ActionEditTool(IDocumentView view)
            : base(view)
        {
            
        }
        public override Type GetToolManipulator()
        {
            return typeof(ActionsEditManipulator);
            
        }
        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            //ReleaseMouseCapture();
            //base.OnPreviewMouseLeftButtonUp(e);
        }
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            e.Handled = false;
        }
       
    }
}
