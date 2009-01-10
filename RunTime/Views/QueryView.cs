using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using FreeSCADA.Archiver;
using FreeSCADA.Common;
using WeifenLuo.WinFormsUI.Docking;

namespace FreeSCADA.RunTime.Views
{
	struct QueryInfo
	{
		public DateTime From;
		public DateTime To;
		public List<ChannelInfo> Channels;

		public override string ToString()
		{
			string pattern = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;
			return string.Format("From {0} to {1}: {2} channels", From.ToString(pattern), To.ToString(pattern), Channels.Count);
		}
	}

	class QueryView:DockContent
	{
		private GroupBox groupBox1;
		private GroupBox groupBox2;
		private Label label2;
		private Label label1;
		private DateTimePicker dateTimePicker2;
		private DateTimePicker dateTimePicker1;
		private Button button1;
		private Button showTableButton;
		private Button showTrendsButton;
		private TreeView channelTree;

		public delegate void OpenTableViewHandler(QueryInfo query);
		public event OpenTableViewHandler OpenTableView;

		public QueryView()
		{
			TabText = "Query view";
			
			InitializeComponent();
			string pattern = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;
			dateTimePicker1.CustomFormat = pattern;
			dateTimePicker2.CustomFormat = pattern;
			dateTimePicker1.Value = DateTime.Now - new TimeSpan(24,0,0);

			RefreshChannelsList();

			Env.Current.Project.ProjectLoaded += new EventHandler(OnProjectLoad);
		}

		private void InitializeComponent()
		{
			this.channelTree = new System.Windows.Forms.TreeView();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.button1 = new System.Windows.Forms.Button();
			this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.showTableButton = new System.Windows.Forms.Button();
			this.showTrendsButton = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// channelTree
			// 
			this.channelTree.CheckBoxes = true;
			this.channelTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.channelTree.Location = new System.Drawing.Point(3, 16);
			this.channelTree.Name = "channelTree";
			this.channelTree.Size = new System.Drawing.Size(278, 145);
			this.channelTree.TabIndex = 0;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.channelTree);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(284, 164);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Channels:";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.button1);
			this.groupBox2.Controls.Add(this.dateTimePicker2);
			this.groupBox2.Controls.Add(this.dateTimePicker1);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox2.Location = new System.Drawing.Point(0, 164);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(284, 78);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Time interval:";
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(239, 47);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(42, 20);
			this.button1.TabIndex = 3;
			this.button1.Text = "Now";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// dateTimePicker2
			// 
			this.dateTimePicker2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dateTimePicker2.Location = new System.Drawing.Point(49, 47);
			this.dateTimePicker2.Name = "dateTimePicker2";
			this.dateTimePicker2.Size = new System.Drawing.Size(184, 20);
			this.dateTimePicker2.TabIndex = 2;
			// 
			// dateTimePicker1
			// 
			this.dateTimePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dateTimePicker1.CustomFormat = "";
			this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dateTimePicker1.Location = new System.Drawing.Point(49, 20);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(184, 20);
			this.dateTimePicker1.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(7, 53);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(23, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "To:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "From:";
			// 
			// showTableButton
			// 
			this.showTableButton.Dock = System.Windows.Forms.DockStyle.Top;
			this.showTableButton.Location = new System.Drawing.Point(0, 242);
			this.showTableButton.Name = "showTableButton";
			this.showTableButton.Size = new System.Drawing.Size(284, 23);
			this.showTableButton.TabIndex = 4;
			this.showTableButton.Text = "Show table";
			this.showTableButton.UseVisualStyleBackColor = true;
			this.showTableButton.Click += new System.EventHandler(this.showTableButton_Click);
			// 
			// showTrendsButton
			// 
			this.showTrendsButton.Dock = System.Windows.Forms.DockStyle.Top;
			this.showTrendsButton.Enabled = false;
			this.showTrendsButton.Location = new System.Drawing.Point(0, 265);
			this.showTrendsButton.Name = "showTrendsButton";
			this.showTrendsButton.Size = new System.Drawing.Size(284, 23);
			this.showTrendsButton.TabIndex = 4;
			this.showTrendsButton.Text = "Show trends";
			this.showTrendsButton.UseVisualStyleBackColor = true;
			// 
			// QueryView
			// 
			this.ClientSize = new System.Drawing.Size(284, 300);
			this.Controls.Add(this.showTrendsButton);
			this.Controls.Add(this.showTableButton);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.MinimumSize = new System.Drawing.Size(300, 336);
			this.Name = "QueryView";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		private void button1_Click(object sender, EventArgs e)
		{
			dateTimePicker2.Value = DateTime.Now;
		}

		void OnProjectLoad(object sender, EventArgs e)
		{
			RefreshChannelsList();
		}

		void RefreshChannelsList()
		{
			channelTree.Nodes.Clear();

			List<ChannelInfo> channels = new List<ChannelInfo>();
			Dictionary<string, TreeNode> topNodes = new Dictionary<string, TreeNode>();

			foreach (Rule rule in ArchiverMain.Current.ChannelsSettings.Rules)
			{
				foreach (ChannelInfo channel in rule.Channels)
				{
					if (channels.Contains(channel) == false)
					{
						channels.Add(channel);

						if (topNodes.ContainsKey(channel.PluginId) == false)
						{
							TreeNode newNode = new TreeNode();
							newNode.Text = Env.Current.CommunicationPlugins[channel.PluginId].Name;
							channelTree.Nodes.Add(newNode);
							topNodes.Add(channel.PluginId, newNode);
						}
					}
				}
			}

			foreach (ChannelInfo channel in channels)
			{
				TreeNode newNode = new TreeNode(channel.ChannelName);
				newNode.Tag = channel;
				newNode.Checked = false;
				topNodes[channel.PluginId].Nodes.Add(newNode);
			}

			foreach(TreeNode node in channelTree.Nodes)
				node.ExpandAll();
		}

		private void showTableButton_Click(object sender, EventArgs e)
		{
			if (OpenTableView != null)
			{
				QueryInfo queryInfo = new QueryInfo();

				queryInfo.Channels = GetCheckedChannels();
				queryInfo.From = dateTimePicker1.Value;
				queryInfo.To = dateTimePicker2.Value;
                if (queryInfo.Channels.Count > 0)
				    OpenTableView(queryInfo);
			}
		}

		private List<ChannelInfo> GetCheckedChannels()
		{
			List<ChannelInfo> channels = new List<ChannelInfo>();

			foreach (TreeNode topNode in channelTree.Nodes)
			{
                if (topNode.Checked == true)
                    foreach (TreeNode node in topNode.Nodes)
                    {
                        channels.Add(node.Tag as ChannelInfo);
                    }
                else
                    foreach (TreeNode node in topNode.Nodes)
				    {
					    if (node.Checked == true)
						    channels.Add(node.Tag as ChannelInfo);
				    }
			}
			return channels;
		}
	}

}
