using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace FreeSCADA.Designer.SchemaEditor
{
    class SelectionManager
    {
        Canvas mainCanvas;
        protected List<UIElement> _selected=new List<UIElement>();
        SelectionManager(Canvas canvas)
        {
            mainCanvas = canvas;
            
        }
        public List<UIElement>  Selected
        {
            get {return _selected;}
        }
        public Rect GetSelectedBounds()
        {
            EditorHelper.CalculateBoundce(Selected, mainCanvas);

            return new Rect();
        }
        public List<UIElement> Clone()
        {
            return new List<UIElement>();
        }


    }
}
