using System;
using System.ComponentModel;

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

        object CreateControl();

        object Tag
        {
            get;
            set;
        }

        ICustomTypeDescriptor getPropProxy(object o);

    }
}
