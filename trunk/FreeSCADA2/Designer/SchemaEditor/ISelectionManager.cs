using System;

namespace FreeSCADA.Designer
{
    public delegate void ObjectSelectedDelegate(object sender);
    interface ISelectionManager
    {
        
        void AddObject(Object el);
        void DeleteObject(Object el);
        System.Collections.Generic.List<Object> SelectedObjects { get; }
        event ObjectSelectedDelegate SelectionChanged;
        void SelectObject(Object el);
        void UpdateManipulator();
    }
}
