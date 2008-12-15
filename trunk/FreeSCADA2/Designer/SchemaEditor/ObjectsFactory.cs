using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;
using FreeSCADA.Designer.SchemaEditor.Manipulators;

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

    static class ObjectsFactory
    {
        static Dictionary<Type, ObjectDescriptor> descriptorsDictionary = new Dictionary<Type, ObjectDescriptor>();

        static ObjectsFactory()
        {
            descriptorsDictionary[typeof(FrameworkElement)]=new ObjectDescriptor(typeof(DragResizeRotateManipulator));
            descriptorsDictionary[typeof(TextBlock)] = new ObjectDescriptor(typeof(TextBoxManipulator));
            descriptorsDictionary[typeof(Shape)] = new ObjectDescriptor(typeof(DragResizeRotateManipulator));
            descriptorsDictionary[typeof(Polyline)] = new ObjectDescriptor(typeof(PolylineEditManipulantor));
            descriptorsDictionary[typeof(Control)] = new ObjectDescriptor(typeof(DragResizeRotateManipulator));
            descriptorsDictionary[typeof(ContentControl)] = new ObjectDescriptor(typeof(DragResizeRotateManipulator));
            descriptorsDictionary[typeof(RangeBase)] = new ObjectDescriptor(typeof(DragResizeRotateManipulator));
            descriptorsDictionary[typeof(Canvas)] = new ObjectDescriptor(typeof(DragResizeRotateManipulator));
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
                    return (BaseManipulator)System.Activator.CreateInstance(desctiptor.ObjectManipulatorType,a);
                }

            }
            return null;
        }
       /* public static CommonShortProp CreateShortProp(Object obj)
        {
            FrameworkElement frameworkElement = obj as FrameworkElement;
            ObjectDescriptor desctiptor;
            if (frameworkElement != null)
            {
                desctiptor = FindDescriptor(frameworkElement.GetType());

                if (desctiptor.ObjectShortPropType.IsSubclassOf(typeof(CommonShortProp)))
                {
                    object[] a = new object[1];
                    a[0] = frameworkElement;
                    return (CommonShortProp)System.Activator.CreateInstance(desctiptor.ObjectShortPropType, a);
                }
            }
            return null;
        }*/
    }
}
