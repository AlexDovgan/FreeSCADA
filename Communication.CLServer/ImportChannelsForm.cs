using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows.Forms;
using FreeSCADA.CLServer;
using FreeSCADA.Common;


namespace FreeSCADA.Communication.CLServer
{
	public partial class ImportChannelsForm : Form
	{
		public struct RemoteChannelInfo
		{
			public string server;
			public int port;
			public string channelFullId;
			public Type type;
		}
		List<RemoteChannelInfo> channels = new List<RemoteChannelInfo>();

		public List<RemoteChannelInfo> Channels
		{
			get { return channels; }
		}

		public ImportChannelsForm()
		{
			InitializeComponent();
			ValidateConnectSettings();
		}

		private void OnConnect(object sender, EventArgs e)
		{
			string serverName = localServerButton.Checked ? "localhost" : serverTextBox.Text;
			int port = int.Parse(portTextBox.Text);
			string serverAddress = string.Format("http://{0}:{1}/ChannelInformationRetriever", serverName, port);

			ImportProgressForm dlg = new ImportProgressForm(serverAddress);
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				groupBox1.Enabled = false;
				connectButton.Enabled = false;
				FillChannels(dlg.Channels);
			}
			else
			{
				statusTextBox.Text += dlg.ErrorMessage;
			}
		}

		

		private void FillChannels(ChannelInfo[] channels)
		{
			channelsTree.Nodes.Clear();
			Dictionary<string, TreeNode> rootNodes = new Dictionary<string, TreeNode>();
			foreach (ChannelInfo channel in channels)
			{
				TreeNode pluginIdNode = null;
				if (rootNodes.ContainsKey(channel.PluginId))
					pluginIdNode = rootNodes[channel.PluginId];
				else
				{
					pluginIdNode = channelsTree.Nodes.Add(channel.PluginId);
					pluginIdNode.Expand();
					pluginIdNode.EnsureVisible();
					rootNodes[channel.PluginId] = pluginIdNode;
				}

				TreeNode channelNode = pluginIdNode.Nodes.Add(channel.Name);
				RemoteChannelInfo channelInfo = new RemoteChannelInfo();
				channelInfo.channelFullId = channel.FullId;
				channelInfo.port = int.Parse(portTextBox.Text);
				channelInfo.server = localServerButton.Checked ? "localhost" : serverTextBox.Text;
				channelInfo.type = Type.GetType(channel.Type);
				channelNode.Tag = channelInfo;
			}
		}

		private void OnServerClick(object sender, EventArgs e)
		{
			serverTextBox.Enabled = remoteServerButton.Checked;
			ValidateConnectSettings();
		}

		private void portTextBox_TextChanged(object sender, EventArgs e)
		{
			ValidateConnectSettings();
		}

		private void serverTextBox_TextChanged(object sender, EventArgs e)
		{
			ValidateConnectSettings();
		}

		private void ValidateConnectSettings()
		{
			//Validate that server is correct
			if (remoteServerButton.Enabled)
				connectButton.Enabled = serverTextBox.Text.Length > 0;
			else
				connectButton.Enabled = true;

			if (connectButton.Enabled)
			{
				if (portTextBox.Text.Length > 0)
				{
					int port = 0;
					if (int.TryParse(portTextBox.Text, out port) && port > 0)
						connectButton.Enabled = true;
					else
						connectButton.Enabled = false;
				}
				else
					connectButton.Enabled = false;
			}
		}

		private void OnCancelClick(object sender, EventArgs e)
		{
			Close();
		}

		void SaveChannelInfo(TreeNodeCollection root)
		{
			foreach (TreeNode node in root)
			{
				if (node.Nodes.Count > 0)
					SaveChannelInfo(node.Nodes);
				else if(node.Checked)
					channels.Add((RemoteChannelInfo)node.Tag);
			}
		}

		private void OnOkClick(object sender, EventArgs e)
		{
			channels.Clear();
			SaveChannelInfo(channelsTree.Nodes);
			Close();
		}

		private void channelsTree_AfterCheck(object sender, TreeViewEventArgs e)
		{
			if(e.Node.Nodes.Count > 0)
			{
				foreach(TreeNode node in e.Node.Nodes)
					node.Checked = e.Node.Checked;
			}
		}
	}

	
}
