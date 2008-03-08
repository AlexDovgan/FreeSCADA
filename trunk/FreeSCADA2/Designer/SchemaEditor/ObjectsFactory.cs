using System;
using System.Windows;
using System.Collections.Generic;
using System.Text;
using FreeSCADA.Designer.SchemaEditor.Manipulators;
using FreeSCADA.Designer.SchemaEditor.ShortProperties;


namespace FreeSCADA.Designer.SchemaEditor
{
    /// <summary>
    /// Descriptor for objetcs
    /// </summary>
    public class  ObjectDescriptor
    {
        public Type DefaultManipulatorType;
        public Type DefaultShortPropType;
    }

    static class ObjectsFactory
    {
        public static BaseManipulator CreateDefaultManipulator( UIElement element)
        {
            FrameworkElement frameworkElement=element as FrameworkElement;
            ObjectDescriptor desctiptor;
            if (frameworkElement!=null && frameworkElement.Tag is ObjectDescriptor)
            {
                desctiptor = frameworkElement.Tag as ObjectDescriptor;

                if (desctiptor.DefaultManipulatorType.IsSubclassOf(typeof(BaseManipulator)))
                {
                    object[] a = new object[1];
                    a[0] = element;
                    return (BaseManipulator)System.Activator.CreateInstance(desctiptor.DefaultManipulatorType, a);
                }

            }
            return null;
        }
        public static CommonShortProp CreateShortProp(UIElement element)
        {
            FrameworkElement frameworkElement = element as FrameworkElement;
            ObjectDescriptor desctiptor;
            if (frameworkElement!=null && frameworkElement.Tag is ObjectDescriptor)
            {
                desctiptor = frameworkElement.Tag as ObjectDescriptor;

                if (desctiptor.DefaultShortPropType.IsSubclassOf(typeof(CommonShortProp)))
                {
                    object[] a = new object[1];
                    a[0] = element;
                    return (CommonShortProp)System.Activator.CreateInstance(desctiptor.DefaultShortPropType, a);
                }
                
            }
            return null;
        }
    }
}
