using System;
using System.ComponentModel;
using System.Windows;

namespace FreeSCADA.Interfaces
{
    public enum ManipulatorKind
    {
        DragResizeRotateManipulator,
        PolylineManipulator
    }

    public interface IVisualControlDescriptor
    {
        string Name
        {
            get;
        }

        string PluginId
        {
            get;
        }

        Type Type
        {
            get;
        }

        ManipulatorKind ManipulatorKind
        {
            get;
        }

        UIElement CreateControl();

        object Tag
        {
            get;
            set;
        }

        ICustomTypeDescriptor getPropProxy(object o);

    }
}
