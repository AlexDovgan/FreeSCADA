using System.Windows;
using System.Windows.Input;
using FreeSCADA.Designer.SchemaEditor.Manipulators;

namespace FreeSCADA.Designer.SchemaEditor.Tools
{
    class ActionEditTool:BaseTool
    {
        
        public ActionEditTool(UIElement elemnet)
            : base(elemnet)
        {
            
        }
        public  override BaseManipulator CreateToolManipulator(UIElement obj)
        {
            return new ActionsEditManipulator(obj);
            
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
