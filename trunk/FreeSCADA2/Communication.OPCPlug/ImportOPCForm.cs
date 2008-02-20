using System;
using System.Windows.Forms;
using OpcRcw.Da;
using OpcRcw.Comn;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace FreeSCADA.Communication.OPCPlug
{
	public partial class ImportOPCForm : Form
	{
		public struct OPCChannelInfo
		{
			public string progId;
			public string host;
			public string channel;
		}
		List<OPCChannelInfo> channels = new List<OPCChannelInfo>();

		public List<OPCChannelInfo> Channels
		{
			get { return channels; }
		}

		public ImportOPCForm()
		{
			InitializeComponent();
			FillServerList();
		}

		private void OnConnect(object sender, EventArgs e)
		{
			string hostName = localServerButton.Checked ? "localhost" : serverTextBox.Text;
			string serverName = serversComboBox.Text;

			Type t = Type.GetTypeFromProgID(serverName, hostName);
			object obj = Activator.CreateInstance(t);
			IOPCBrowseServerAddressSpace srv = (IOPCBrowseServerAddressSpace)obj;

			//IntPtr statusPtr;
			//server.GetStatus(out statusPtr);
			//OPCSERVERSTATUS status = (OPCSERVERSTATUS)Marshal.PtrToStructure(statusPtr, typeof(OPCSERVERSTATUS));
			//statusPtr = IntPtr.Zero;

			if (srv != null)
			{
				try
				{
					for (; ; )
						srv.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_UP, "");
				}
				catch (COMException) { };
				channelsTree.Nodes.Clear();
				ImportOPCChannels(srv, channelsTree.Nodes);
			}

			groupBox1.Enabled = false;
			connectButton.Enabled = false;
		}

		void ImportOPCChannels(IOPCBrowseServerAddressSpace srv, TreeNodeCollection root)
		{
			OPCNAMESPACETYPE nsType;
			srv.QueryOrganization(out nsType);
			OpcRcw.Da.IEnumString es;
			if(nsType == OPCNAMESPACETYPE.OPC_NS_HIERARCHIAL)
			{				
				try{srv.BrowseOPCItemIDs(OPCBROWSETYPE.OPC_BRANCH, "", 0, 0, out es);}
				catch(COMException){return;}
				
				int fetched;
				do 
				{
					string[] tmp = new string[100];
					es.RemoteNext(tmp.Length, tmp, out fetched);
					for (int i = 0; i < fetched; i++)
					{
						try{srv.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_DOWN, tmp[i]);}
						catch (COMException) { return; };
						TreeNode node = root.Add(tmp[i]);
						ImportOPCChannels(srv, node.Nodes);
						srv.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_UP, tmp[i]);
					}
				} while(fetched>0);

				try{srv.BrowseOPCItemIDs(OPCBROWSETYPE.OPC_LEAF, "", 0, 0, out es);}
				catch(COMException){return;}
				IterateOPCItems(srv, root, es);
			}
			else if(nsType == OPCNAMESPACETYPE.OPC_NS_FLAT)
			{
				try { srv.BrowseOPCItemIDs(OPCBROWSETYPE.OPC_FLAT, "", 0, 0, out es); }
				catch (COMException) { return; }
				IterateOPCItems(srv, root, es);
			}
			es = null;
		}

		private void IterateOPCItems(IOPCBrowseServerAddressSpace srv, TreeNodeCollection root, OpcRcw.Da.IEnumString es)
		{
			int fetched;
			do
			{
				string[] tmp = new string[100];
				es.RemoteNext(tmp.Length, tmp, out fetched);
				for (int i = 0; i < fetched; i++)
					AddTreeNode(srv, root, tmp[i]);
			} while (fetched > 0);
		}

		private void AddTreeNode(IOPCBrowseServerAddressSpace srv, TreeNodeCollection root, string tag)
		{
			TreeNode item = root.Add(tag);
			OPCChannelInfo channel = new OPCChannelInfo();
			channel.progId = serversComboBox.Text;
			channel.host = localServerButton.Checked ? "localhost" : serverTextBox.Text;
			srv.GetItemID(tag, out channel.channel);
			item.Tag = channel;
		}

		void FillServerList()
		{
			string hostName = localServerButton.Checked ? "localhost" : serverTextBox.Text;
			serversComboBox.Items.Clear();

			try
			{
				Type serverListType = Type.GetTypeFromProgID("OPC.ServerList", hostName);
				IOPCServerList serverList = (IOPCServerList)Activator.CreateInstance(serverListType);
				Guid[] categories = { typeof(CATID_OPCDAServer20).GUID };
				IEnumGUID enumGuids;
				serverList.EnumClassesOfCategories(categories.Length, categories, 0, null, out enumGuids);
				int fetched;
				do
				{
					Guid[] ids = new Guid[10];
					enumGuids.Next(ids.Length, ids, out fetched);
					for (int i = 0; i < fetched; i++)
					{
						string progId;
						string name;
						serverList.GetClassDetails(ref ids[i], out progId, out name);
						serversComboBox.Items.Add(progId);
					}
				} while (fetched > 0);

				if (serversComboBox.Items.Count > 0)
					serversComboBox.Text = serversComboBox.Items[0].ToString();
			}
			catch(System.Exception)
			{
			}
		}

		private void OnServerClick(object sender, EventArgs e)
		{
			serverTextBox.Enabled = remoteServerButton.Checked;
		}

		private void OnRefreshServersClick(object sender, EventArgs e)
		{
			FillServerList();
		}

		private void OnServerChanged(object sender, EventArgs e)
		{
			connectButton.Enabled = serversComboBox.Text.Length > 0;
		}

		private void OnCancelClick(object sender, EventArgs e)
		{
			Close();
		}

		void SaveOPCChannels(TreeNodeCollection root)
		{
			foreach (TreeNode node in root)
			{
				if (node.Nodes.Count > 0)
					SaveOPCChannels(node.Nodes);
				else if(node.Checked)
					channels.Add((OPCChannelInfo)node.Tag);
			}
		}

		private void OnOkClick(object sender, EventArgs e)
		{
			channels.Clear();
			SaveOPCChannels(channelsTree.Nodes);
			Close();
		}
	}
}
