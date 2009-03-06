using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FreeSCADA.Designer.SchemaEditor.PropertiesUtils
{
    /// <summary>
    /// 
    /// </summary>
    public class PropProxy : ICustomTypeDescriptor
    {
        /// <summary>
        /// 
        /// </summary>
        protected object controlledObject;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controlledObject"></param>
        public PropProxy(object controlledObject)
        {
            this.controlledObject = controlledObject;
        }

        #region ICustomTypeDescriptor methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="editorBaseType"></param>
        /// <returns></returns>
        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public virtual PropertyDescriptorCollection GetProperties(Attribute[] attributes)
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PropertyDescriptorCollection GetProperties()
        {

            return GetProperties(new Attribute[] { });
        } 
        #endregion
    }
}
