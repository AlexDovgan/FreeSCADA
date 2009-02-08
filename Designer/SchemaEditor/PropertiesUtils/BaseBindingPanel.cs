using System;
using System.ComponentModel;
using System.Windows.Forms;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Designer.SchemaEditor.PropertiesUtils
{
	internal partial class BaseBindingPanel : UserControl
	{
		protected object element;
		protected PropertyInfo property;
		private bool enableInDesigner = false;

		public BaseBindingPanel()
		{
			InitializeComponent();
			this.Disposed += new EventHandler(OnDisposed);
		}

		public bool EnableInDesigner
		{
			get { return enableInDesigner; }
			set { enableInDesigner = value; }
		}

		virtual public void Initialize(object element, PropertyInfo property)
		{
			this.element = element;
			this.property = property;
		}

		virtual public bool CheckApplicability(object element, PropertyInfo property)
		{
			return false;
		}

		virtual public bool CheckApplicability(IChannel channel)
		{
			if (element != null && property != null && channel != null)
			{
				Type type = GetPropertyType(element, property);
				if (type.Equals(channel.GetType()))
					return true;
			}

			return false;
		}

		virtual public void AddChannel(IChannel channel)
		{
		}

		virtual protected void OnSave()
		{
		}

		protected Type GetPropertyType(object element, PropertyInfo property)
		{
			PropertyDescriptor pd = TypeDescriptor.GetProperties(element).Find(property.SourceProperty, true);
			if (pd != null)
				return pd.PropertyType;
			else
				return null;
		}

		void OnDisposed(object sender, EventArgs e)
		{
			this.Disposed -= new EventHandler(OnDisposed);

			if (element != null && property != null)
				OnSave();
		}
	}
}
