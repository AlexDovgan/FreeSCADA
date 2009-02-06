using System;
using System.Windows;
using System.Windows.Forms;
using System.ComponentModel;
using FreeSCADA.Common;
using System.Collections.Generic;

namespace FreeSCADA.Designer.SchemaEditor.PropertiesUtils
{
	/// <summary>
	/// Common dialog for bindings
	/// </summary>
	public partial class CommonBindingDialog : Form
	{
		object element;

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
				{
					propertyList.Items.Add((property as PropertyWrapper).PropertyInfo);
				}
				
			}
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
					chNode.Tag = plugId;

				}
			}
		}
	}
}
