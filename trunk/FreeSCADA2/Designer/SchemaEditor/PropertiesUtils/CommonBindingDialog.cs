﻿using System;
using System.ComponentModel;
using System.Reflection;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using FreeSCADA.Common;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Designer.SchemaEditor.PropertiesUtils
{
	/// <summary>
	/// Common dialog for bindings
	/// </summary>
	public partial class CommonBindingDialog : Form
	{
		object element;
		BaseBindingPanel bindingPanel;
		Dictionary<PropertyInfo, BindingBase> activeBindings = new Dictionary<PropertyInfo,BindingBase>();

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="element"></param>
		public CommonBindingDialog(object element)
		{
			this.element = element;
			InitializeComponent();

			FillChannels();
			FillProperties();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="element"></param>
		internal CommonBindingDialog(object element, PropertyInfo activeProperty)
		{
			this.element = element;
			InitializeComponent();

			FillChannels();
			FillProperties();

			for(int i=0;i<propertyList.Items.Count;i++)
			{
				if ((propertyList.Items[i] as PropertyInfo).SourceProperty == activeProperty.SourceProperty)
				{
					propertyList.SelectedIndex = i;
					break;
				}
			}
		}

		void FillProperties()
		{
			propertyList.Items.Clear();

			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(element);
			foreach (PropertyDescriptor property in properties)
			{
				if (property is PropertyWrapper)
					propertyList.Items.Add((property as PropertyWrapper).PropertyInfo);
			}

			if (propertyList.Items.Count > 0)
				propertyList.SelectedIndex = 0;
		}

		void FillChannels()
		{
			channelsTree.Nodes.Clear();
			foreach (string plugId in Env.Current.CommunicationPlugins.PluginIds)
			{
				TreeNode plugNode = channelsTree.Nodes.Add(Env.Current.CommunicationPlugins[plugId].Name);
				foreach (FreeSCADA.Interfaces.IChannel ch in Env.Current.CommunicationPlugins[plugId].Channels)
				{
					TreeNode chNode;
					chNode = plugNode.Nodes.Add(ch.Name);
					chNode.Tag = ch;
				}
				plugNode.Expand();
			}
		}

		void FillBindingTypes()
		{
			bindingTypes.Items.Clear();
			bindingTypes.Items.AddRange(GetAvailableBindingPanels().ToArray());

			if (bindingTypes.Items.Count > 0)
				bindingTypes.SelectedIndex = 0;
		}

		private void CreateAssociationButton_Click(object sender, EventArgs e)
		{
			SavePanelStateAndClose();

			if (propertyList.SelectedIndex >= 0 && bindingTypes.SelectedIndex >= 0)
			{
				BaseBindingPanelFactory factory = (BaseBindingPanelFactory)bindingTypes.SelectedItem;
				bindingPanel = factory.CreateInstance();
				bindingPanel.Initialize(element, propertyList.SelectedItem as PropertyInfo, null);
				bindingPanel.Parent = panel1;
				bindingPanel.Dock = DockStyle.Fill;
				CreateAssociationButton.Enabled = false;
				bindingTypes.Enabled = false;
			}
		}

		void ShowBindingPanel()
		{
			SavePanelStateAndClose();

			if (propertyList.SelectedIndex >= 0)
			{
				System.Windows.Data.BindingBase binding = GetExistingBinding(propertyList.SelectedItem as PropertyInfo);
				if (binding != null)
				{
					PropertyInfo propInfo = propertyList.Items[propertyList.SelectedIndex] as PropertyInfo;
					List<BaseBindingPanelFactory> panels = GetAvailableBindingPanels();
					foreach (BaseBindingPanelFactory panel in panels)
					{
						if (panel.CanWorkWithBinding(binding))
						{
							bindingPanel = panel.CreateInstance();
							break;
						}
					}

					if (bindingPanel != null)
					{
						bindingPanel.Initialize(element, propertyList.SelectedItem as PropertyInfo, binding);
						bindingPanel.Parent = panel1;
						bindingPanel.Dock = DockStyle.Fill;
						enableInDesignerCheckbox.Checked = bindingPanel.EnableInDesigner;
					}
					CreateAssociationButton.Enabled = false;
					bindingTypes.Enabled = false;
				}
			}
		}

		private void SavePanelStateAndClose()
		{
			if (bindingPanel != null)
			{
				BindingBase binding = bindingPanel.Save();
				if (binding != null)
					activeBindings[bindingPanel.Property] = binding;

				bindingPanel.Dispose();
				bindingPanel = null;
			}
		}

		System.Windows.Data.BindingBase GetExistingBinding(PropertyInfo property)
		{
			if (activeBindings.ContainsKey(property))
				return activeBindings[property];
			else
			{
				PropertyDescriptor pd = TypeDescriptor.GetProperties(element).Find(property.SourceProperty, true);
				if (pd == null || !(pd is PropertiesUtils.PropertyWrapper))
					return null;

				DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty((pd as PropertiesUtils.PropertyWrapper).ControlledProperty);
				if (dpd == null)
					return null;

				DependencyObject depObj = (pd as PropertiesUtils.PropertyWrapper).ControlledObject as DependencyObject;
				return BindingOperations.GetBindingBase(depObj, dpd.DependencyProperty);
			}
		}

		List<BaseBindingPanelFactory> GetAvailableBindingPanels()
		{
			PropertyInfo propInfo = null;
			List<BaseBindingPanelFactory> result = new List<BaseBindingPanelFactory>();

			if (propertyList.SelectedIndex >= 0)
				propInfo = propertyList.Items[propertyList.SelectedIndex] as PropertyInfo;

			if (propInfo == null)
				return result;

			Assembly archiverAssembly = this.GetType().Assembly;
			foreach (Type type in archiverAssembly.GetTypes())
			{
				if (type.IsSubclassOf(typeof(BaseBindingPanelFactory)))
				{
					BaseBindingPanelFactory factory = (BaseBindingPanelFactory)Activator.CreateInstance(type);
					if (factory != null && factory.CheckApplicability(element, propInfo))
						result.Add(factory);
				}
			}

			return result;
		}

		void UpdateControlsState()
		{
			if (GetAvailableBindingPanels().Count > 0)
			{
				CreateAssociationButton.Enabled = true;
				enableInDesignerCheckbox.Enabled = true;
				bindingTypes.Enabled = true;
			}
			else
			{
				CreateAssociationButton.Enabled = false;
				enableInDesignerCheckbox.Enabled = false;
				bindingTypes.Enabled = false;
			}
		}

		private void propertyList_SelectedIndexChanged(object sender, EventArgs e)
		{
			FillBindingTypes();
			UpdateControlsState();
			ShowBindingPanel();
		}

		private void channelsTree_DoubleClick(object sender, EventArgs e)
		{
			if (channelsTree.SelectedNode != null && bindingPanel != null)
			{
				bindingPanel.AddChannel(channelsTree.SelectedNode.Tag as IChannel);
			}
		}

		private void enableInDesignerCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			if (bindingPanel != null)
				bindingPanel.EnableInDesigner = enableInDesignerCheckbox.Checked;
		}

        private void saveButton_Click(object sender, EventArgs e)
        {
			SavePanelStateAndClose();
            if (activeBindings.Count > 0)
            {
				foreach (PropertyInfo key in activeBindings.Keys)
				{
					DependencyObject depObj;
					DependencyProperty depProp;
					System.Windows.Data.BindingBase binding = activeBindings[key];
					BaseBindingPanel.GetPropertyObjects(element, key, out depObj, out depProp);
					if (depObj != null && depProp != null && binding != null)
						BindingOperations.SetBinding(depObj, depProp, binding);
				}
            }
			DialogResult = DialogResult.OK;
			Close();
        }
	}
}