using System;
using System.Windows;
using System.Collections.Generic;
using System.Text;
using FreeSCADA.Designer.SchemaEditor.Manipulators;
using FreeSCADA.Designer.SchemaEditor.ShortProperties;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;

namespace FreeSCADA.Designer.SchemaEditor
{
    /// <summary>
    /// Descriptor for objetcs
    /// </summary>
    class  ObjectDescriptor
    {
        public ObjectDescriptor(Type manType, Type shPropType)
        {
            ObjectManipulatorType = manType;
            ObjectShortPropType = shPropType;
        }
        public Type ObjectManipulatorType;
        public Type ObjectShortPropType;
    }

    static class ObjectsFactory
    {
        static Dictionary<Type, ObjectDescriptor> descriptorsDictionary = new Dictionary<Type, ObjectDescriptor>();

        static ObjectsFactory()
        {
            descriptorsDictionary[typeof(FrameworkElement)]=new ObjectDescriptor(typeof(DragResizeRotateManipulator), typeof(FrameworkElementShortProp));
            descriptorsDictionary[typeof(TextBlock)] = new ObjectDescriptor(typeof(TextBoxManipulator), typeof(FrameworkElementShortProp));
            descriptorsDictionary[typeof(Shape)] = new ObjectDescriptor(typeof(DragResizeRotateManipulator), typeof(ShapeShortProp));
            descriptorsDictionary[typeof(Polyline)] = new ObjectDescriptor(typeof(PolylineEditManipulantor), typeof(ShapeShortProp));
            descriptorsDictionary[typeof(RangeBase)] = new ObjectDescriptor(typeof(DragResizeRotateManipulator), typeof(RangeBaseShortProp));
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
        public static CommonShortProp CreateShortProp(Object obj)
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
        }
    }
}
