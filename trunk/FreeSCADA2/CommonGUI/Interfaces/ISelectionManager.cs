using System;

namespace FreeSCADA.Common
{
    public delegate void ObjectSelectedDelegate(object sender);
    // may be need to getrid manipulator from here

    public interface ISelectionManager
    {
        Type ManipulatorType
        {
            get;
            set;
        }
        void AddObject(Object el);
        void DeleteObject(Object el);
        System.Collections.Generic.List<Object> SelectedObjects { get; }
        event ObjectSelectedDelegate SelectionChanged;
        void SelectObject(Object el);
        void UpdateManipulator();
    }
}
