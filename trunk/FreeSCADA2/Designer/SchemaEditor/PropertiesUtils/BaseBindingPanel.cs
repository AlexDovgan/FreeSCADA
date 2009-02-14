﻿using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Designer.SchemaEditor.PropertiesUtils
{
	internal abstract class BaseBindingPanelFactory
	{
		abstract public bool CheckApplicability(object element, PropertyInfo property);
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
		protected PropertyInfo property;
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

		virtual public void Initialize(object element, PropertyInfo property, System.Windows.Data.BindingBase binding)
		{
			this.element = element;
			this.property = property;
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

		virtual public System.Windows.Data.BindingBase GetBinding()
		{
        
            return null;
		}

		internal static Type GetPropertyType(object element, PropertyInfo property)
		{
			PropertyDescriptor pd = TypeDescriptor.GetProperties(element).Find(property.SourceProperty, true);
			if (pd != null)
				return pd.PropertyType;
			else
				return null;
		}

		public static void GetPropertyObjects(object element, PropertyInfo property, out DependencyObject depObj, out DependencyProperty depProp)
		{
			depObj = null;
			depProp = null;

			PropertyDescriptor pd = TypeDescriptor.GetProperties(element).Find(property.SourceProperty, true);
			if (pd == null || !(pd is PropertiesUtils.PropertyWrapper))
				return;

			DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty((pd as PropertiesUtils.PropertyWrapper).ControlledProperty);
			if (dpd == null)
				return;

			depObj = (pd as PropertiesUtils.PropertyWrapper).ControlledObject as DependencyObject;
			depProp = dpd.DependencyProperty;
		}

		public void Close()
		{
			if (element != null && property != null)
				GetBinding();
			Dispose();
		}
	}
}
