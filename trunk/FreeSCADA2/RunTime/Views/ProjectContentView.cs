using System;
using System.Windows.Forms;
using FreeSCADA.Common;
using WeifenLuo.WinFormsUI.Docking;

namespace FreeSCADA.RunTime.Views
{
	class ProjectContentView:DockContent
    {
        private System.Windows.Forms.TreeView projectTree;

		/// <summary>
		/// Notify that user double clicked on some node
		/// </summary>
		/// <param name="entity_name">Full name of the node how it is in the project</param>
		public delegate void OpenEntityHandler(string entity_name);
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
			this.projectTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.projectTree.Location = new System.Drawing.Point(0, 0);
			this.projectTree.Name = "projectTree";
			this.projectTree.Size = new System.Drawing.Size(292, 273);
			this.projectTree.TabIndex = 0;
			this.projectTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnNodeDblClick);
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

			Env.Current.Project.ProjectLoaded += new EventHandler(OnProjectLoad);
		}

		public void RefreshContent(Project project)
		{
			projectTree.Nodes.Clear();
			TreeNode root = projectTree.Nodes.Add(project.FileName);
			TreeNode schemas = root.Nodes.Add(StringResources.SchemasItemName);
			foreach (string entity in Env.Current.Project.GetEntities(ProjectEntityType.Schema))
			{
				TreeNode node = schemas.Nodes.Add(entity);
				node.Tag = entity;
				node.EnsureVisible();
			}
		}

		void OnProjectLoad(object sender, EventArgs e)
		{
			 RefreshContent((Project)sender);
		}

		private void OnNodeDblClick(object sender, TreeNodeMouseClickEventArgs e)
		{
            if (OpenEntity != null && e.Node.Tag!=null)
				OpenEntity((string)e.Node.Tag);
		}
	}
  
}
