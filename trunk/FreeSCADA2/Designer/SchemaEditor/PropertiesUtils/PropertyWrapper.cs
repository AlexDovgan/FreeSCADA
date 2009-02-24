﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FreeSCADA.Designer.SchemaEditor.PropertiesUtils
{
	/// <summary>
	/// This class wraps given property. Almost all calls go directly to original property, but some of 
	/// them (customization of user visible strings) are handled by this class.
	/// </summary>
	class PropertyWrapper : PropertyDescriptor
	{
		object controlledObject;
		PropertyDescriptor controlledProperty;
		PropertyInfo propertyInfo;

		public PropertyWrapper(object controlledObject, PropertyInfo propertyInfo)
			: base(propertyInfo.GetTargetPropertyName(), null)
		{
			this.controlledObject = controlledObject;
			this.propertyInfo = propertyInfo;

			foreach(PropertyDescriptor pd in TypeDescriptor.GetProperties(controlledObject))
			{
				if(pd.Name == propertyInfo.SourceProperty)
				{
					this.controlledProperty = pd;
					break;
				}
			}
			if (this.controlledProperty == null)
				throw new System.InvalidOperationException("sourceProperty is not found in controlled object");
		}

		public static bool CheckIfApplicable(object controlledObject, PropertyInfo propertyInfo)
		{
			foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(controlledObject))
			{
				if (pd.Name == propertyInfo.SourceProperty)
					return true;
			}

			return false;
		}

        public object ControlledObject
        {
            get { return controlledObject; }
        }

        public PropertyDescriptor ControlledProperty
        {
            get { return controlledProperty; }
        }

		public override AttributeCollection Attributes
		{
			get
			{
				List<Attribute> attrs = new List<Attribute>();
                if (propertyInfo.Editor != null && propertyInfo.Editor.IsSubclassOf(typeof(System.Drawing.Design.UITypeEditor)))
					attrs.Add(new EditorAttribute(propertyInfo.Editor, typeof(System.Drawing.Design.UITypeEditor)));
				if (string.IsNullOrEmpty(propertyInfo.Group) == false)
					attrs.Add(new CategoryAttribute(propertyInfo.Group));

                return new AttributeCollection(attrs.ToArray());
			}
		}

		public override bool CanResetValue(object component)
		{
			return true;
		}

		public override Type ComponentType
		{
			get
			{
				return controlledProperty.ComponentType;
				
			}
		}

		public override string DisplayName
		{
			get
			{
				return propertyInfo.GetTargetPropertyDisplayName();
			}
		}

		public override string Description
		{
			get
			{
				if (string.IsNullOrEmpty(propertyInfo.Description))
					return controlledProperty.Description;
				else
					return propertyInfo.Description;
			}
		}

		internal PropertyInfo PropertyInfo
		{
			get { return propertyInfo; }
		}

		public override object GetValue(object component)
		{
			//TODO: probably we need some type convertor here for "compound" properties
            System.Windows.Data.BindingBase binding;
            if ((binding = GetBinding()) != null)
                return "Binding";
			return controlledProperty.GetValue(controlledObject);
		}

		public override void SetValue(object component, object value)
		{
			//TODO: probably we need some type convertor here for "compound" properties
            if (controlledObject is System.Windows.DependencyObject && DependencyPropertyDescriptor.FromProperty(controlledProperty) != null)
                EditorHelper.SetDependencyProperty(controlledObject as System.Windows.DependencyObject, DependencyPropertyDescriptor.FromProperty(controlledProperty).DependencyProperty, value);
            else		
                controlledProperty.SetValue(controlledObject, value);
            
		}

		public override bool IsReadOnly
		{
			get { return controlledProperty.IsReadOnly; }
		}

		public override string Name
		{
            get 
			{
				return propertyInfo.GetTargetPropertyName();
			}
		}

		public override Type PropertyType
		{
			get { return controlledProperty.PropertyType; }
		}

		public override void ResetValue(object component) { }

		public override bool ShouldSerializeValue(object component)
		{
			return true;
		}
        public System.Windows.Data.BindingBase GetBinding()
        {
            if (controlledObject is System.Windows.DependencyObject
                && DependencyPropertyDescriptor.FromProperty(controlledProperty)!=null)
                return System.Windows.Data.BindingOperations.GetBinding(
                    (controlledObject as System.Windows.DependencyObject),
                    DependencyPropertyDescriptor.FromProperty(controlledProperty).DependencyProperty);
            return null;

        }
        internal bool GetWpfObjects(
            out System.Windows.DependencyObject depObj,
            out System.Windows.DependencyProperty depProp)
        {
            if (controlledObject is System.Windows.DependencyObject
                && DependencyPropertyDescriptor.FromProperty(controlledProperty) != null)
            {
                depObj = controlledObject as System.Windows.DependencyObject;
                depProp = DependencyPropertyDescriptor.FromProperty(controlledProperty).DependencyProperty;
                return true;
            }
            depObj = null;
            depProp = null;
            return false;
        }
        public override string ToString()
        {
            return DisplayName;
        }

	}
}
