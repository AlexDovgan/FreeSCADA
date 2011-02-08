using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using FreeSCADA.Common;
using OpcRcw.Comn;
using OpcRcw.Da;

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
			string serverName = serversComboBox.Text;

			Type t;
			if (localServerButton.Checked)
				t = Type.GetTypeFromProgID(serverName);
			else
				t = Type.GetTypeFromProgID(serverName, serverTextBox.Text);

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
            channelsTree.AfterCheck += new TreeViewEventHandler(channelsTree_AfterCheck);
			groupBox1.Enabled = false;
			connectButton.Enabled = false;
		}
        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    // If the current node has child nodes, call the CheckAllChildsNodes method recursively.
                    this.CheckAllChildNodes(node, nodeChecked);
                }
            }
        }

        void channelsTree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0)
                {
                    /* Calls the CheckAllChildNodes method, passing in the current 
                    Checked value of the TreeNode whose checked state changed. */
                    this.CheckAllChildNodes(e.Node, e.Node.Checked);
                }
            }

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
						catch (Exception e) 
						{
							Env.Current.Logger.LogWarning(string.Format("OPC server failed to handle OPC_BROWSE_DOWN request for item '{0}' ({1})", tmp[i], e.Message));
							continue; 
						};
						TreeNode node = root.Add(tmp[i]);
						ImportOPCChannels(srv, node.Nodes);
						try { srv.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_UP, ""); }
						catch (Exception e) 
						{
							Env.Current.Logger.LogWarning(string.Format("OPC server failed to handle OPC_BROWSE_UP request for item '{0}' ({1})", tmp[i], e.Message));
							continue; 
						};
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
                IOPCServerList2 serverList = (IOPCServerList2)Activator.CreateInstance(serverListType);
				Guid[] categories = { typeof(CATID_OPCDAServer10).GUID };
				IOPCEnumGUID enumGuids;
				serverList.EnumClassesOfCategories(categories.Length, categories, 0, null, out enumGuids);
				int fetched;
                enumGuids.Reset();
				do
				{
					Guid[] ids = new Guid[10];
					enumGuids.Next(ids.Length, ids, out fetched);
					for (int i = 0; i < fetched; i++)
					{
						string progId;
						string name;
                        string vendorId;
						serverList.GetClassDetails(ref ids[i], out progId, out name,out vendorId);
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
