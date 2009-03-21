using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using FreeSCADA.Archiver;
using FreeSCADA.Interfaces;
using FreeSCADA.Common;

namespace FreeSCADA.Designer.Views
{
	class ArchiverSettingsView:DocumentView
	{
		private SplitContainer splitContainer1;
		private SplitContainer splitContainer2;
		private GroupBox groupBox2;
		private ListView rulesList;
		private ColumnHeader columnHeader1;
		private Button removeRuleButton;
		private Button newRuleButton;
		private GroupBox groupBox3;
		private Button removeChannelButton;
		private Button addChannelButton;
		private GroupBox groupBox1;
		private Label label1;
		private PropertyGrid conditionProperties;
		private ListView conditionsList;
		private ColumnHeader conditionNameColumn;
		private ColumnHeader conditionDescriptionColumn;
		private ListBox channelsList;
	
		public ArchiverSettingsView()
		{
			InitializeComponent();

			TabText = "Archiver Settings";
			UpdateControlStates();
		}

		private void UpdateControlStates()
		{
			bool ruleSelected = rulesList.SelectedItems.Count > 0;

			SuspendLayout();
			groupBox1.Enabled = ruleSelected;
			groupBox3.Enabled = ruleSelected;
			removeRuleButton.Enabled = ruleSelected;
			ResumeLayout();
		}

		private void InitializeComponent()
		{
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.rulesList = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.removeRuleButton = new System.Windows.Forms.Button();
			this.newRuleButton = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.removeChannelButton = new System.Windows.Forms.Button();
			this.addChannelButton = new System.Windows.Forms.Button();
			this.channelsList = new System.Windows.Forms.ListBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.conditionProperties = new System.Windows.Forms.PropertyGrid();
			this.conditionsList = new System.Windows.Forms.ListView();
			this.conditionNameColumn = new System.Windows.Forms.ColumnHeader();
			this.conditionDescriptionColumn = new System.Windows.Forms.ColumnHeader();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
			this.splitContainer1.Size = new System.Drawing.Size(799, 415);
			this.splitContainer1.SplitterDistance = 255;
			this.splitContainer1.TabIndex = 0;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.groupBox2);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.groupBox3);
			this.splitContainer2.Size = new System.Drawing.Size(799, 255);
			this.splitContainer2.SplitterDistance = 377;
			this.splitContainer2.TabIndex = 0;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.rulesList);
			this.groupBox2.Controls.Add(this.removeRuleButton);
			this.groupBox2.Controls.Add(this.newRuleButton);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox2.Location = new System.Drawing.Point(0, 0);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(377, 255);
			this.groupBox2.TabIndex = 6;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Rules";
			// 
			// rulesList
			// 
			this.rulesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.rulesList.CheckBoxes = true;
			this.rulesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
			this.rulesList.FullRowSelect = true;
			this.rulesList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.rulesList.HideSelection = false;
			this.rulesList.LabelEdit = true;
			this.rulesList.Location = new System.Drawing.Point(12, 19);
			this.rulesList.MultiSelect = false;
			this.rulesList.Name = "rulesList";
			this.rulesList.Size = new System.Drawing.Size(359, 201);
			this.rulesList.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.rulesList.TabIndex = 4;
			this.rulesList.UseCompatibleStateImageBehavior = false;
			this.rulesList.View = System.Windows.Forms.View.Details;
			this.rulesList.Resize += new System.EventHandler(this.rulesList_Resize);
			this.rulesList.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.rulesList_ItemChecked);
			this.rulesList.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.rulesList_AfterLabelEdit);
			this.rulesList.SelectedIndexChanged += new System.EventHandler(this.rulesList_SelectedIndexChanged);
			this.rulesList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rulesList_KeyDown);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Width = 220;
			// 
			// removeRuleButton
			// 
			this.removeRuleButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.removeRuleButton.Location = new System.Drawing.Point(261, 226);
			this.removeRuleButton.Name = "removeRuleButton";
			this.removeRuleButton.Size = new System.Drawing.Size(110, 23);
			this.removeRuleButton.TabIndex = 2;
			this.removeRuleButton.Text = "Remove rule";
			this.removeRuleButton.UseVisualStyleBackColor = true;
			this.removeRuleButton.Click += new System.EventHandler(this.removeRuleButton_Click);
			// 
			// newRuleButton
			// 
			this.newRuleButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.newRuleButton.Location = new System.Drawing.Point(12, 226);
			this.newRuleButton.Name = "newRuleButton";
			this.newRuleButton.Size = new System.Drawing.Size(110, 23);
			this.newRuleButton.TabIndex = 1;
			this.newRuleButton.Text = "New rule";
			this.newRuleButton.UseVisualStyleBackColor = true;
			this.newRuleButton.Click += new System.EventHandler(this.newRuleButton_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.label1);
			this.groupBox3.Controls.Add(this.removeChannelButton);
			this.groupBox3.Controls.Add(this.addChannelButton);
			this.groupBox3.Controls.Add(this.channelsList);
			this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox3.Location = new System.Drawing.Point(0, 0);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(418, 255);
			this.groupBox3.TabIndex = 9;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Channels";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 207);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(396, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Hint: You can drag and drop channels from Project Content view into this window";
			// 
			// removeChannelButton
			// 
			this.removeChannelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.removeChannelButton.Location = new System.Drawing.Point(296, 226);
			this.removeChannelButton.Name = "removeChannelButton";
			this.removeChannelButton.Size = new System.Drawing.Size(110, 23);
			this.removeChannelButton.TabIndex = 1;
			this.removeChannelButton.Text = "Remove channel";
			this.removeChannelButton.UseVisualStyleBackColor = true;
			this.removeChannelButton.Click += new System.EventHandler(this.removeChannelButton_Click);
			// 
			// addChannelButton
			// 
			this.addChannelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.addChannelButton.Location = new System.Drawing.Point(6, 226);
			this.addChannelButton.Name = "addChannelButton";
			this.addChannelButton.Size = new System.Drawing.Size(110, 23);
			this.addChannelButton.TabIndex = 1;
			this.addChannelButton.Text = "Add channel";
			this.addChannelButton.UseVisualStyleBackColor = true;
			this.addChannelButton.Click += new System.EventHandler(this.addChannelButton_Click);
			// 
			// channelsList
			// 
			this.channelsList.AllowDrop = true;
			this.channelsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.channelsList.FormattingEnabled = true;
			this.channelsList.IntegralHeight = false;
			this.channelsList.Location = new System.Drawing.Point(6, 19);
			this.channelsList.Name = "channelsList";
			this.channelsList.Size = new System.Drawing.Size(400, 185);
			this.channelsList.Sorted = true;
			this.channelsList.TabIndex = 0;
			this.channelsList.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBox1_DragDrop);
			this.channelsList.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBox1_DragEnter);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.conditionProperties);
			this.groupBox1.Controls.Add(this.conditionsList);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(799, 156);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Conditions";
			// 
			// conditionProperties
			// 
			this.conditionProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.conditionProperties.HelpVisible = false;
			this.conditionProperties.Location = new System.Drawing.Point(482, 19);
			this.conditionProperties.Name = "conditionProperties";
			this.conditionProperties.Size = new System.Drawing.Size(302, 125);
			this.conditionProperties.TabIndex = 1;
			this.conditionProperties.ToolbarVisible = false;
			// 
			// conditionsList
			// 
			this.conditionsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.conditionsList.CheckBoxes = true;
			this.conditionsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.conditionNameColumn,
            this.conditionDescriptionColumn});
			this.conditionsList.FullRowSelect = true;
			this.conditionsList.GridLines = true;
			this.conditionsList.Location = new System.Drawing.Point(12, 19);
			this.conditionsList.MultiSelect = false;
			this.conditionsList.Name = "conditionsList";
			this.conditionsList.Size = new System.Drawing.Size(464, 125);
			this.conditionsList.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.conditionsList.TabIndex = 0;
			this.conditionsList.UseCompatibleStateImageBehavior = false;
			this.conditionsList.View = System.Windows.Forms.View.Details;
			this.conditionsList.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.conditionsList_ItemChecked);
			this.conditionsList.SelectedIndexChanged += new System.EventHandler(this.conditionsList_SelectedIndexChanged);
			// 
			// conditionNameColumn
			// 
			this.conditionNameColumn.Text = "Name";
			this.conditionNameColumn.Width = 118;
			// 
			// conditionDescriptionColumn
			// 
			this.conditionDescriptionColumn.Text = "Description";
			this.conditionDescriptionColumn.Width = 209;
			// 
			// ArchiverSettingsView
			// 
			this.ClientSize = new System.Drawing.Size(799, 415);
			this.Controls.Add(this.splitContainer1);
			this.Name = "ArchiverSettingsView";
			this.Load += new System.EventHandler(this.ArchiverSettingsView_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		private void ArchiverSettingsView_Load(object sender, System.EventArgs e)
		{
			RefreshRulesList();
			splitContainer1.SplitterDistance = (int)(splitContainer1.Size.Height * 0.25);
			splitContainer2.SplitterDistance = (int)(splitContainer2.Size.Width * 0.5);
		}

		private void RefreshRulesList()
		{
			ListView.SelectedIndexCollection selected = rulesList.SelectedIndices;
			rulesList.Items.Clear();

			for(int i=0;i<ArchiverMain.Current.ChannelsSettings.Rules.Count;i++)
			{
				Rule rule = ArchiverMain.Current.ChannelsSettings.Rules[i];
				ListViewItem item = rulesList.Items.Add(rule.Name);
				item.Tag = rule;
				item.Checked = rule.Enable;
				item.Selected = selected.Contains(i);
			}
			rulesList.Sort();
		}

		private void RefreshChannelsList()
		{
			int selected = channelsList.SelectedIndex;
			channelsList.Items.Clear();

			if (rulesList.SelectedItems.Count > 0)
			{
				Rule rule = rulesList.SelectedItems[0].Tag as Rule;
				if (rule != null)
				{
					foreach (ChannelInfo channel in rule.Channels)
					{
						ChannelItem item = new ChannelItem();
						item.channel = channel;
						channelsList.Items.Add(item);
					}
				}
			}

			if (selected >= channelsList.Items.Count)
				selected = channelsList.Items.Count - 1;

			channelsList.SelectedIndex = selected;
		}

		private void RefreshConditionsList()
		{
			List<ConditionItem> conditions = new List<ConditionItem>();

			if (rulesList.SelectedItems.Count > 0)
			{
				Rule rule = rulesList.SelectedItems[0].Tag as Rule;
				if (rule != null)
				{
					//Get list of existing conditions
					foreach(BaseCondition condition in rule.Conditions)
					{
						ConditionItem item = new ConditionItem();
						item.condition = condition;
						item.enabled = true;
						conditions.Add(item);
					}

					//Get list of all possible conditions
					Assembly archiverAssembly = typeof(FreeSCADA.Archiver.ArchiverMain).Assembly;
					foreach (Type type in archiverAssembly.GetExportedTypes())
					{
						if (type.IsSubclassOf(typeof(BaseCondition)))
						{
							bool isExists = false;
							foreach (ConditionItem item in conditions)
							{
								if (item.condition.GetType() == type)
								{
									isExists = true;
									break;
								}
							}

							if (isExists == false)
							{
								ConditionItem item = new ConditionItem();
								item.condition = (BaseCondition)Activator.CreateInstance(type);
								item.enabled = false;
								conditions.Add(item);
							}
						}
					}
				}
			}

			conditionsList.Tag = conditions;

			//Fill control
			conditionsList.ItemChecked -= new System.Windows.Forms.ItemCheckedEventHandler(conditionsList_ItemChecked);
			ListView.SelectedIndexCollection selected = conditionsList.SelectedIndices;
			conditionsList.Items.Clear();
			for (int i = 0; i < conditions.Count; i++)
			{
				ConditionItem condition = conditions[i];
				ListViewItem item = conditionsList.Items.Add(condition.condition.Name);
				item.Tag = condition;
				item.Checked = condition.enabled;
				item.Selected = selected.Count>0 ? selected.Contains(i):false;

				item.SubItems.Add(condition.condition.Description);
			}
			if (conditions.Count > 0)
			{
				conditionsList.Columns[0].Width = -2;
				conditionsList.Columns[1].Width = -2;
			}
			conditionsList.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(conditionsList_ItemChecked);
		}

		private void newRuleButton_Click(object sender, System.EventArgs e)
		{
			ArchiverMain.Current.ChannelsSettings.AddRule(new Rule());
			ArchiverMain.Current.ChannelsSettings.Save();
			RefreshRulesList();
			rulesList.SelectedItems.Clear();
			rulesList.Items[rulesList.Items.Count - 1].Selected = true;
		}

		private void rulesList_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			Rule rule = e.Item.Tag as Rule;
			if (rule != null)
			{
				rule.Enable = e.Item.Checked;
				ArchiverMain.Current.ChannelsSettings.Save();
			}
		}

		private void rulesList_AfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			if (e.CancelEdit)
				return;

			Rule rule = rulesList.Items[e.Item].Tag as Rule;
			if (rule != null && e.Label != null)
			{
				rule.Name = e.Label;
				ArchiverMain.Current.ChannelsSettings.Save();
			}
		}

		private void rulesList_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F2)
			{
				if (rulesList.SelectedItems.Count > 0)
				{
					rulesList.SelectedItems[0].EnsureVisible();
					rulesList.SelectedItems[0].BeginEdit();
				}
			}
		}

		private void listBox1_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(string)))
			{
				string channelId = (string)e.Data.GetData(typeof(string));
				if (Env.Current.CommunicationPlugins.GetChannel(channelId) != null)
					e.Effect = DragDropEffects.Copy;
				else
					e.Effect = DragDropEffects.None;
			}
			else
				e.Effect = DragDropEffects.None;
		}

		private void listBox1_DragDrop(object sender, DragEventArgs e)
		{
			IChannel ch = Env.Current.CommunicationPlugins.GetChannel((string)e.Data.GetData(typeof(string)));
			
			ChannelInfo channel = new ChannelInfo();
			channel.ChannelName = ch.Name;
			channel.PluginId = ch.PluginId;

			Rule rule = rulesList.SelectedItems[0].Tag as Rule;
			if (rule != null)
			{
				rule.Channels.Add(channel);
				ArchiverMain.Current.ChannelsSettings.Save();
				RefreshChannelsList();
			}
		}

		private void rulesList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			RefreshChannelsList();
			RefreshConditionsList();

			UpdateControlStates();
		}

		private void rulesList_Resize(object sender, System.EventArgs e)
		{
			rulesList.Columns[0].Width = -2;
		}

		private void conditionsList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (conditionsList.SelectedItems.Count > 0)
			{
				ConditionItem condition = conditionsList.SelectedItems[0].Tag as ConditionItem;
				conditionProperties.SelectedObject = condition.condition;
			}
			else
				conditionProperties.SelectedObject = null;
		}

		private void conditionsList_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			List<ConditionItem> conditionItems = conditionsList.Tag as List<ConditionItem>;
			ConditionItem affectedItem = e.Item.Tag as ConditionItem;
			affectedItem.enabled = e.Item.Checked;

			List<BaseCondition> conditions = new List<BaseCondition>();
			foreach (ConditionItem item in conditionItems)
			{
				if (item.enabled)
					conditions.Add(item.condition);
			}

			Rule rule = rulesList.SelectedItems[0].Tag as Rule;
			if (rule != null)
			{
				rule.Conditions = conditions;
				ArchiverMain.Current.ChannelsSettings.Save();
			}
		}

		private void addChannelButton_Click(object sender, EventArgs e)
		{
			FreeSCADA.Designer.Dialogs.VariablesDialog dlg = new FreeSCADA.Designer.Dialogs.VariablesDialog(true);
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				Rule rule = rulesList.SelectedItems[0].Tag as Rule;
				if (rule != null)
				{
					foreach (IChannel ch in dlg.SelectedChannels)
					{
						ChannelInfo channel = new ChannelInfo();
						channel.ChannelName = ch.Name;
						channel.PluginId = ch.PluginId;

						rule.Channels.Add(channel);
					}
				}
				ArchiverMain.Current.ChannelsSettings.Save();
				RefreshChannelsList();
			}
		}

		private void removeRuleButton_Click(object sender, EventArgs e)
		{
			List<Rule> rulesToRemove = new List<Rule>();
			foreach (ListViewItem item in rulesList.SelectedItems)
				rulesToRemove.Add(item.Tag as Rule);

			foreach (Rule rule in rulesToRemove)
				ArchiverMain.Current.ChannelsSettings.Rules.Remove(rule);

			ArchiverMain.Current.ChannelsSettings.Save();

			RefreshRulesList();
		}

		private void removeChannelButton_Click(object sender, EventArgs e)
		{
			if (channelsList.SelectedIndex >= 0)
			{
				ChannelItem channelToRemove = (ChannelItem)channelsList.Items[channelsList.SelectedIndex];

				Rule rule = rulesList.SelectedItems[0].Tag as Rule;
				rule.Channels.Remove(channelToRemove.channel);
				ArchiverMain.Current.ChannelsSettings.Save();
				RefreshChannelsList();
			}
		}
	}

	class ChannelItem
	{
		public ChannelInfo channel;
		public override string ToString()
		{
			return channel.ChannelName;
		}
	}

	class ConditionItem
	{
		public BaseCondition condition;
		public bool enabled;
	}
}
