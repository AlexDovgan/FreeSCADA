using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FreeSCADA.Designer.SchemaEditor.PropertiesUtils
{
    class PropProxy : ICustomTypeDescriptor
    {
        object controlledObject;

        public PropProxy(object controlledObject)
        {
            this.controlledObject = controlledObject;
        }

        #region ICustomTypeDescriptor methods

        public String GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public String GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
			List<PropertyWrapper> result = new List<PropertyWrapper>();
			List<PropertyInfo> properties = PropertiesMap.GetProperties(controlledObject.GetType());
			foreach (PropertyInfo propertyInfo in properties)
			{
				if(PropertyWrapper.CheckIfApplicable(controlledObject, propertyInfo))
					result.Add(new PropertyWrapper(controlledObject, propertyInfo));
			}
			return new PropertyDescriptorCollection(result.ToArray());
        }

        public PropertyDescriptorCollection GetProperties()
        {

            return GetProperties(new Attribute[] { });
        }
        #endregion
    }
}
