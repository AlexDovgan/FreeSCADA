using System.Collections.Generic;
using System.Windows;

namespace FreeSCADA.Designer.SchemaEditor
{
    class SelectionManager
    {
        Tools.BaseTool _tool;
        protected List<UIElement> _selected=new List<UIElement>();
        SelectionManager(Tools.BaseTool tool)
        {
            _tool = tool;
            
        }
        public List<UIElement>  Selected
        {
            get {return _selected;}
        }
        public Rect GetSelectedBounds()
        {
            EditorHelper.CalculateBounds(Selected, _tool.AdornedElement);

            return new Rect();
        }
        public List<UIElement> Clone()
        {
            return new List<UIElement>();
        }


    }
}
