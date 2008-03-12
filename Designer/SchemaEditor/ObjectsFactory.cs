using System;
using System.Windows;
using System.Collections.Generic;
using System.Text;
using FreeSCADA.Designer.SchemaEditor.Manipulators;
using FreeSCADA.Designer.SchemaEditor.ShortProperties;
using System.Windows.Controls;
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

        public static BaseManipulator CreateDefaultManipulator( UIElement element)
        {
            FrameworkElement frameworkElement=element as FrameworkElement;
            ObjectDescriptor desctiptor;
            if (frameworkElement!=null)
            {
                /*
                if(descriptorsDictionary.ContainsKey(frameworkElement.GetType()))
                    desctiptor = descriptorsDictionary[frameworkElement.GetType()];
                else   
                    desctiptor = descriptorsDictionary[typeof(FrameworkElement)];
                */
                desctiptor =FindDescriptor(frameworkElement.GetType());
                if (desctiptor.ObjectManipulatorType.IsSubclassOf(typeof(BaseManipulator)))
                {
                    object[] a = new object[1];
                    a[0] = element;
                    return (BaseManipulator)System.Activator.CreateInstance(desctiptor.ObjectManipulatorType, a);
                }

            }
            return null;
        }
        public static CommonShortProp CreateShortProp(UIElement element)
        {
            FrameworkElement frameworkElement = element as FrameworkElement;
            ObjectDescriptor desctiptor;
            if (frameworkElement != null)
            {
                /*
                if (descriptorsDictionary.ContainsKey(frameworkElement.GetType()))
                    desctiptor = descriptorsDictionary[frameworkElement.GetType()];
                else 
                    desctiptor = descriptorsDictionary[typeof(FrameworkElement)];
                */
                desctiptor = FindDescriptor(frameworkElement.GetType());

                if (desctiptor.ObjectShortPropType.IsSubclassOf(typeof(CommonShortProp)))
                {
                    object[] a = new object[1];
                    a[0] = element;
                    return (CommonShortProp)System.Activator.CreateInstance(desctiptor.ObjectShortPropType, a);
                }
                
            }
            return null;
        }
    }
}
