using System;
using System.Windows.Forms;
using FreeSCADA.Common;
using FreeSCADA.CommonUI.Interfaces;
using FreeSCADA.Designer.Views.ProjectNodes;
using FreeSCADA.Designer.Dialogs;

namespace FreeSCADA.Designer.Views
{
	class ProjectContentView:ToolWindow
    {
        private System.Windows.Forms.TreeView projectTree;

		/// <summary>
		/// Notify that user double clicked on some node
		/// </summary>
		
        public delegate void OpenEntityHandler(DocumentView view);
		/// <summary>Occurs when user double clicks a node from the list</summary>
		public event OpenEntityHandler OpenEntity;

		public delegate void SelectNodeHandler(BaseNode node);
		/// <summary>Occurs when user clicks on a node from the list</summary>
		public event SelectNodeHandler SelectNode;

		#region Windows Form FreeSCADA.Designer generated code
		private void InitializeComponent()
		{
			this.projectTree = new System.Windows.Forms.TreeView();
			this.SuspendLayout();
			// 
			// projectTree
			// 
			this.projectTree.AllowDrop = true;
			this.projectTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.projectTree.Location = new System.Drawing.Point(0, 0);
			this.projectTree.Name = "projectTree";
			this.projectTree.Size = new System.Drawing.Size(292, 273);
			this.projectTree.TabIndex = 0;
			this.projectTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnNodeDblClick);
			this.projectTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.projectTree_AfterSelect);
			this.projectTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.projectTree_NodeMouseClick);
			this.projectTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.projectTree_ItemDrag);
			// 
			// ProjectContentView
			// 
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.Add(this.projectTree);
			this.Name = "ProjectContentView";
			this.ResumeLayout(false);

        }

        #endregion

        IWindowManager _windowManager;

		public ProjectContentView(IWindowManager wm)
		{
            _windowManager = wm;
			InitializeComponent();

			TabText = "Project Content";

            projectTree.ImageList = TreeResources.TreeIcons;

			RefreshContent(Env.Current.Project);
            AllowDrop = true;
            this.projectTree.DragEnter += new DragEventHandler(projectTree_DragEnter);

			Env.Current.Project.ProjectLoaded += new EventHandler(OnProjectLoad);
			Env.Current.CommunicationPlugins.ChannelsChanged += new CommunationPlugs.ChannelsChangedHandler(OnCommunicationPluginsChannelsChanged);
			Env.Current.ScriptManager.ScriptsUpdated += new EventHandler(OnScriptsUpdated);

			this.FormClosed += new FormClosedEventHandler(OnFormClosed);
  		}

		void OnFormClosed(object sender, FormClosedEventArgs e)
		{
			Env.Current.Project.ProjectLoaded -= new EventHandler(OnProjectLoad);
			Env.Current.CommunicationPlugins.ChannelsChanged -= new CommunationPlugs.ChannelsChangedHandler(OnCommunicationPluginsChannelsChanged);
			Env.Current.ScriptManager.ScriptsUpdated -= new EventHandler(OnScriptsUpdated);
		}

        void projectTree_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        void projectTree_ItemDrag(object sender, ItemDragEventArgs e)
        {
			if ((e.Item as TreeNode).Tag != null && (e.Item as TreeNode).Tag is ChannelNode)
			{
				//By some reason I cannot pass channelNode object to this function. therefore I send just channel id string.
				//We need better solution here.
				ChannelNode node = (e.Item as TreeNode).Tag as ChannelNode;
				DoDragDrop(node.FullId, DragDropEffects.All);
			}
        }

		public void RefreshContent(Project project)
		{
			projectTree.BeginUpdate();
			ProjectNode prjNode = new ProjectNode();
			prjNode.CreateTreeNode(projectTree.Nodes, null);
			projectTree.EndUpdate();
        }

		void OnProjectLoad(object sender, EventArgs e)
		{
			 RefreshContent((Project)sender);
		}

		void OnCommunicationPluginsChannelsChanged(FreeSCADA.Interfaces.Plugins.ICommunicationPlug plug)
		{
			RefreshContent(Env.Current.Project);
		}

		void OnScriptsUpdated(object sender, EventArgs e)
		{
			RefreshContent(Env.Current.Project);
		}

		private void OnNodeDblClick(object sender, TreeNodeMouseClickEventArgs e)
		{
            BaseNode node= e.Node.Tag as BaseNode;
            _windowManager.ActivateDocument(node.GetView());
            //foreach( BaseCommand act in node.GetActions())
            //    if(act.IsDoubleClicked())
              //      act.Execute();


        }

		private void projectTree_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (SelectNode != null && e.Node.Tag != null)
			{
				if (e.Node.Tag is BaseNode)
					SelectNode(e.Node.Tag as BaseNode);
			}
		}

		private void projectTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Button == MouseButtons.Right && e.Node != null)
			{
				projectTree.SelectedNode = e.Node;

				ContextMenuStrip menu = new ContextMenuStrip();

				if (e.Node.Tag != null && e.Node.Tag is BaseEntityNode)
				{
                    foreach (BaseCommand act in (e.Node.Tag as BaseEntityNode).GetActions())
                    {
                        ToolStripItem item;

                        item = menu.Items.Add(act.Name);
                        item.Tag = act;
                        item.Click += new EventHandler(OnNodeCommandClick);
                    }
               	}
				
				if(menu.Items.Count > 0)
					menu.Show(projectTree,e.Location);
			}
		}

        void OnNodeCommandClick(object sender, EventArgs e)
		{
			ToolStripItem item = sender as ToolStripItem;
			BaseCommand act = item.Tag as BaseCommand;
			act.Execute();
		}

		
	}
}
