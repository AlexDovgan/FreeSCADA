using System;
using System.Windows;

namespace FreeSCADA.Designer.SchemaEditor.ShortProperties
{
    abstract class CommonShortProp
    {
        public delegate void PropertiesChangedDelegate();
        public event PropertiesChangedDelegate PropertiesChanged;

        public delegate void PropertiesBrowserChangedDelegate(UIElement el);
        public event PropertiesBrowserChangedDelegate PropertiesBrowserChanged;

        public CommonShortProp(object obj)
        {
            wrapedObject = obj;
        }
        public string ObjectType
        {
            get { return wrapedObject.GetType().ToString(); }
        }

        public void RaisePropertiesChanged()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(NotifyPropertyChangedAsync), this);
        }

        public void RaisePropertiesBrowserChanged(UIElement el)
        {
            if (PropertiesBrowserChanged != null)
                PropertiesBrowserChanged(el);
        }

        protected static void NotifyPropertyChangedAsync(Object info)
        {
            CommonShortProp obj = (CommonShortProp)info;
            if (obj.PropertiesChanged != null)
                obj.PropertiesChanged();
        }
        public object WrapedObject
        {
            get { return wrapedObject; }
        }
        object wrapedObject;

    }
}
