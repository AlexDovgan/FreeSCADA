using System;
using System.ComponentModel;
using System.Windows;

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

		public PropertyWrapper(object controlledObject, PropertyDescriptor controlledProperty)
			: base(controlledProperty.Name, null)
		{
			this.controlledObject = controlledObject;
			this.controlledProperty = controlledProperty;
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
				//List<Attribute> attrs= new List<Attribute>();
                //attrs.Add(new EditorAttribute(typeof(FreeSCADA.Designer.SchemaEditor.PropertyGridTypeEditors.DoubleEditor), typeof(System.Drawing.Design.UITypeEditor)));
              //  AttributeCollection at = new AttributeCollection(attrs.ToArray());
               // return at;
                return new AttributeCollection();
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
                return controlledProperty.DisplayName;
			}
		}

		public override string Description
		{
			get
			{
                return controlledProperty.Description;
			}
		}

		public override object GetValue(object component)
		{
			return controlledProperty.GetValue(controlledObject);
			//return "value";
		}

		public override void SetValue(object component, object value)
		{
            if(controlledObject is DependencyObject && DependencyPropertyDescriptor.FromProperty(controlledProperty)!=null)
                EditorHelper.SetDependencyProperty(controlledObject as DependencyObject, DependencyPropertyDescriptor.FromProperty(controlledProperty).DependencyProperty, value);
            else		
                controlledProperty.SetValue(controlledObject, value);
            
		}

		public override bool IsReadOnly
		{
			get { return controlledProperty.IsReadOnly; }
		}

		public override string Name
		{
            get { return controlledProperty.Name; }
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
	}
}
