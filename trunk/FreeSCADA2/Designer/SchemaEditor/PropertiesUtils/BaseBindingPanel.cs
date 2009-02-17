using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Designer.SchemaEditor.PropertiesUtils
{
	internal abstract class BaseBindingPanelFactory
	{
		abstract public bool CheckApplicability(object element, PropertyWrapper property);
		abstract public bool CanWorkWithBinding(System.Windows.Data.BindingBase binding);
		abstract public BaseBindingPanel CreateInstance();

		virtual public string Name
		{
			get { return this.GetType().FullName; }
		}

		public override string ToString()
		{
			return Name;
		}
	}

	internal partial class BaseBindingPanel : UserControl
	{
		protected object element;
		private PropertyWrapper property;

		private bool enableInDesigner = false;

		public BaseBindingPanel()
		{
			InitializeComponent();
		}

		public bool EnableInDesigner
		{
			get { return enableInDesigner; }
			set { enableInDesigner = value; }
		}

        public PropertyWrapper Property
		{
			get { return property; }
			set { property = value; }
		}

		virtual public void Initialize(object element, PropertyWrapper property, System.Windows.Data.BindingBase binding)
		{
			this.element = element;
			this.property = property;
		}

		virtual public bool CheckApplicability(IChannel channel)
		{
			if (element != null && property != null && channel != null)
			{
				Type type = property.PropertyType;
				if (type.Equals(channel.GetType()))
					return true;
			}

			return false;
		}

		virtual public void AddChannel(IChannel channel)
		{
		}

		virtual public System.Windows.Data.BindingBase Save()
		{
			return null;
		}

	
	}
}
