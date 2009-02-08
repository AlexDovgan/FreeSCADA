using System;
using System.ComponentModel;
using System.Reflection;
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

		private void CreateAssociationButton_Click(object sender, EventArgs e)
		{
			if (bindingPanel != null)
			{
				bindingPanel.Dispose();
				bindingPanel = null;
			}

			if (propertyList.SelectedIndex >= 0)
			{
				bindingPanel = GetBindingPanel();
				if (bindingPanel != null)
				{
					bindingPanel.Parent = panel1;
					bindingPanel.Dock = DockStyle.Fill;
				}
				CreateAssociationButton.Enabled = false;
			}
		}

		void ShowBindingPanel()
		{
			if (bindingPanel != null)
			{
				bindingPanel.Dispose();
				bindingPanel = null;
			}

			if (propertyList.SelectedIndex >= 0 && IsBindingExist(propertyList.SelectedItem as PropertyInfo))
			{
				bindingPanel = GetBindingPanel();
				if (bindingPanel != null)
				{
					bindingPanel.Parent = panel1;
					bindingPanel.Dock = DockStyle.Fill;
					enableInDesignerCheckbox.Checked = bindingPanel.EnableInDesigner;
				}
				CreateAssociationButton.Enabled = false;
			}
		}

		bool IsBindingExist(PropertyInfo property)
		{
			PropertyDescriptor pd = TypeDescriptor.GetProperties(element).Find(property.SourceProperty, true);
			if(pd == null || !(pd is PropertiesUtils.PropertyWrapper))
				return false;

			DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty((pd as PropertiesUtils.PropertyWrapper).ControlledProperty);
            if (dpd == null)
                return false;

			DependencyObject depObj = (pd as PropertiesUtils.PropertyWrapper).ControlledObject as DependencyObject;
			System.Windows.Data.Binding binding = BindingOperations.GetBinding(depObj, dpd.DependencyProperty);

			return binding != null;
		}

		BaseBindingPanel GetBindingPanel()
		{
			PropertyInfo propInfo = null;

			if (propertyList.SelectedIndex >= 0)
				propInfo = propertyList.Items[propertyList.SelectedIndex] as PropertyInfo;

			if (propInfo == null)
				return null;

			Assembly archiverAssembly = this.GetType().Assembly;
			foreach (Type type in archiverAssembly.GetTypes())
			{
				if (type.IsSubclassOf(typeof(BaseBindingPanel)))
				{
					BaseBindingPanel obj = (BaseBindingPanel)Activator.CreateInstance(type);

					if (obj.CheckApplicability(element, propInfo))
					{
						obj.Initialize(element, propInfo);
						return obj;
					}
					else
						obj.Dispose();
				}
			}

			return null;
		}

		void UpdateControlsState()
		{
			BaseBindingPanel panel = GetBindingPanel();
			if (panel != null)
			{
				panel.Dispose();
				CreateAssociationButton.Enabled = true;
				enableInDesignerCheckbox.Enabled = true;
			}
			else
			{
				CreateAssociationButton.Enabled = false;
				enableInDesignerCheckbox.Enabled = false;
			}
		}

		private void propertyList_SelectedIndexChanged(object sender, EventArgs e)
		{
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

		private void CommonBindingDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (bindingPanel != null)
			{
				bindingPanel.Dispose();
				bindingPanel = null;
			}
		}

		private void enableInDesignerCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			if (bindingPanel != null)
				bindingPanel.EnableInDesigner = enableInDesignerCheckbox.Checked;
		}
	}
}