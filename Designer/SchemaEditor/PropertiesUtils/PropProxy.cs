using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

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
            List<string> props = new List<string>(
                    new string[]{
                    "Width",
                    "Height",
                    "Background",
                    "Foreground",
                    "Opacity",
                    "Canvas.Top",
                    "Canvas.Left",
                    "Canvas.ZIndex",
                    "Value",
                    "Maximum",
                    "Mimimum",
                    "Content",
                    "Style",
                    "StrokeThickness",
                    "Stroke",
                    "Fill",
                    "RenderTransform",
                    "RenderTransformOrigin",
                    "Name",
                    "ContentTemplate",
                    "Orientation",
                    "Text",
                    "ActionChannelName",
                    "MinChannelValue",
                    "MaxChannelValue",
                    "MinAngle",
                    "MaxAngle",
                    "IsChecked",
					"FontFamily",
					"FontSize",
					"FontStretch",
					"FontStyle",
					"FontWeight",
					"TextAlignment"
					}
                        );
            IEnumerable<PropertyWrapper> ie;
            if(controlledObject is System.Windows.DependencyObject)
                ie = TypeDescriptor.GetProperties(controlledObject, null).Cast<PropertyDescriptor>()
                     .Where(x => DependencyPropertyDescriptor.FromProperty(x) != null
                         && props.Contains(x.Name) == true).Select<PropertyDescriptor,PropertyWrapper>(x=>new PropertyWrapper(controlledObject,x));
            else
                ie = TypeDescriptor.GetProperties(controlledObject, attributes).Cast<PropertyDescriptor>()
                    .Select<PropertyDescriptor, PropertyWrapper>(x => new PropertyWrapper(controlledObject, x));

            return new PropertyDescriptorCollection(ie.ToArray());
        }

        public PropertyDescriptorCollection GetProperties()
        {

            return GetProperties(new Attribute[] { });
        }
        #endregion
    }
}
