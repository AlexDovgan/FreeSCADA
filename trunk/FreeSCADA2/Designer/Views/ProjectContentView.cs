using System;
using System.Windows.Forms;
using FreeSCADA.Common;
using FreeSCADA.Archiver;

namespace FreeSCADA.Designer.Views
{
	class ProjectContentView:ToolWindow
    {
        private System.Windows.Forms.TreeView projectTree;

		/// <summary>
		/// Notify that user double clicked on some node
		/// </summary>
		
        public delegate void OpenEntityHandler(ProjectEntityType entity_type, string entity_name);
		/// <summary>Occurs when user double clicks a node from the list</summary>
		public event OpenEntityHandler OpenEntity;

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
        

		public ProjectContentView()
		{
			InitializeComponent();

			TabText = "Project Content";

			RefreshContent(Env.Current.Project);
            AllowDrop = true;
            this.projectTree.DragEnter += new DragEventHandler(projectTree_DragEnter);

			Env.Current.Project.ProjectLoaded += new EventHandler(OnProjectLoad);
			Env.Current.CommunicationPlugins.ChannelsChanged += new CommunationPlugs.ChannelsChangedHandler(OnCommunicationPluginsChannelsChanged);

			this.FormClosed += new FormClosedEventHandler(OnFormClosed);
  		}

		void OnFormClosed(object sender, FormClosedEventArgs e)
		{
			Env.Current.Project.ProjectLoaded -= new EventHandler(OnProjectLoad);
			Env.Current.CommunicationPlugins.ChannelsChanged -= new CommunationPlugs.ChannelsChangedHandler(OnCommunicationPluginsChannelsChanged);
		}

        void projectTree_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
        

        void projectTree_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if((e.Item as TreeNode).Tag!=null)
                DoDragDrop(e.Item, DragDropEffects.All);
        }

		public void RefreshContent(Project project)
		{
			projectTree.Nodes.Clear();
			TreeNode root;
			
			if(string.IsNullOrEmpty(project.FileName))
				root = projectTree.Nodes.Add(StringResources.UnsavedProjectName);
			else
				root = projectTree.Nodes.Add(project.FileName);

			TreeNode schemas = root.Nodes.Add(StringResources.SchemasItemName);
			foreach (string entity in Env.Current.Project.GetSchemas())
			{
				TreeNode node = schemas.Nodes.Add(entity);
				node.Tag = entity;
				node.EnsureVisible();
			}
            TreeNode channelsTree = root.Nodes.Add(StringResources.ChannelsItemName);
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
            TreeNode archivers = root.Nodes.Add(StringResources.ArchiverItemName);
            foreach (Rule rule in ArchiverMain.Current.ChannelsSettings.Rules)
            {
                TreeNode chNode;
                chNode = archivers.Nodes.Add(rule.Name);
                //chNode.Tag = plugId;

            }
        }

		void OnProjectLoad(object sender, EventArgs e)
		{
			 RefreshContent((Project)sender);
		}

		void OnCommunicationPluginsChannelsChanged(FreeSCADA.Interfaces.Plugins.ICommunicationPlug plug)
		{
			RefreshContent(Env.Current.Project);
		}

		private void OnNodeDblClick(object sender, TreeNodeMouseClickEventArgs e)
		{
            if (OpenEntity != null && e.Node.Tag != null && e.Node.Parent.Text == StringResources.SchemasItemName)
                OpenEntity(ProjectEntityType.Schema, (string)e.Node.Tag);
            else if (OpenEntity != null && (e.Node.Text == StringResources.ArchiverItemName || e.Node.Parent.Text == StringResources.ArchiverItemName))
                OpenEntity(ProjectEntityType.Archiver, (string)e.Node.Text);
        }
       
	}
  
}
