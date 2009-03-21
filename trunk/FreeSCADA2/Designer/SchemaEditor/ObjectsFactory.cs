using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;
using FreeSCADA.Common;
using FreeSCADA.Designer.SchemaEditor.Manipulators;
using FreeSCADA.Interfaces;
using FreeSCADA.Interfaces.Plugins;

namespace FreeSCADA.Designer.SchemaEditor
{
    /// <summary>
    /// Descriptor for objetcs
    /// </summary>
    class  ObjectDescriptor
    {
        public ObjectDescriptor(Type manType)
        {
            ObjectManipulatorType = manType;
         }
        public Type ObjectManipulatorType;
        
    }
    /// <summary>
    /// /
    /// </summary>
    static class ObjectsFactory
    {
        static Dictionary<Type, ObjectDescriptor> descriptorsDictionary = new Dictionary<Type, ObjectDescriptor>();

        static ObjectsFactory()
        {
            descriptorsDictionary[typeof(FrameworkElement)] = new ObjectDescriptor(typeof(DragResizeRotateManipulator));
            descriptorsDictionary[typeof(TextBlock)] = new ObjectDescriptor(typeof(DragResizeRotateManipulator));
            descriptorsDictionary[typeof(Shape)] = new ObjectDescriptor(typeof(DragResizeRotateManipulator));
            descriptorsDictionary[typeof(Polyline)] = new ObjectDescriptor(typeof(PolylineEditManipulantor));
            descriptorsDictionary[typeof(Control)] = new ObjectDescriptor(typeof(DragResizeRotateManipulator));
            descriptorsDictionary[typeof(ContentControl)] = new ObjectDescriptor(typeof(DragResizeRotateManipulator));
            descriptorsDictionary[typeof(RangeBase)] = new ObjectDescriptor(typeof(DragResizeRotateManipulator));
            descriptorsDictionary[typeof(Canvas)] = new ObjectDescriptor(typeof(DragResizeRotateManipulator));
            
            foreach (IVisualControlsPlug p in Env.Current.VisualPlugins.Plugins)
            {
                foreach (IVisualControlDescriptor d in p.Controls)
                {
                    if (d.ManipulatorKind == ManipulatorKind.DragResizeRotateManipulator)
                        descriptorsDictionary[d.Type] = new ObjectDescriptor(typeof(DragResizeRotateManipulator));
                    else if (d.ManipulatorKind == ManipulatorKind.PolylineManipulator)
                        descriptorsDictionary[d.Type] = new ObjectDescriptor(typeof(PolylineEditManipulantor));
                }
            }
        }
        private static ObjectDescriptor FindDescriptor(Type type)
        {

            do
            {
                if (descriptorsDictionary.ContainsKey(type))
                    return descriptorsDictionary[type];
            } while ((type = type.BaseType) != null);
            return null;


        }

        public static BaseManipulator CreateDefaultManipulator( Object obj )
        {
            FrameworkElement frameworkElement=obj as FrameworkElement;
            ObjectDescriptor desctiptor;
            if (frameworkElement!=null)
            {
                desctiptor =FindDescriptor(frameworkElement.GetType());
                if (desctiptor.ObjectManipulatorType.IsSubclassOf(typeof(BaseManipulator)))
                {
                    object[] a = new object[1];
                    a[0] = frameworkElement;
                    return (Manipulators.BaseManipulator)System.Activator.CreateInstance(desctiptor.ObjectManipulatorType,a);

                }

            }
            return null;
        }

    }
}
