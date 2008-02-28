using System;
using FreeSCADA.Common;
using System.Windows.Forms;

namespace FreeSCADA.Designer.Views
{
	class ProjectContentView:ToolWindow
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

			Env.Current.Project.LoadEvent += new EventHandler(OnProjectLoad);
		}

		void OnProjectLoad(object sender, EventArgs e)
		{
            Project project = sender as Project;

			projectTree.Nodes.Clear();
            TreeNode root = projectTree.Nodes.Add(project.ProjectName);
            
			foreach (string entity in Env.Current.Project.GetEntities())
			{
                //filtering schema names
                TreeNode node = null;
                string [] splited= entity.Split('/');
                if (splited.Length>1)
                {

                    if (root.Nodes.Find(splited[0], false).Length == 0)
                    {
                        node = root.Nodes.Add(splited[0]);
                        node.Tag = splited[0];
                    }
                    else node = root.Nodes.Find(splited[0], false)[0];
                    if (node.Nodes.Find(splited[1],false).Length == 0)
                    {
                        node=node.Nodes.Add(splited[1]);
                        node.Tag = splited[1];
                    }

                }
                else
                {
                    node = root.Nodes.Add(entity);
                    node.Tag = entity;
                }
                
            }
		}

		private void OnNodeDblClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (OpenEntity != null)
				OpenEntity((string)e.Node.Tag);
		}
	}
  
}
