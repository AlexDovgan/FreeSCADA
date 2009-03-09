using System;
using System.Windows.Forms;
using FreeSCADA.Common;
using FreeSCADA.Common.Scripting;

namespace FreeSCADA.Designer.Views
{
	/// <summary>
	/// Represents available scripts for the project.
	/// </summary>
	class ScriptsToolBoxView : ToolWindow
	{
		private ToolStrip toolStrip1;
		private ToolStripButton toolStripButton1;
		private ToolStripButton toolStripDropDownButton1;
		private TreeView tree;

		public event ScriptManager.NewScriptCreatedHandler OpenScript;
	
		public ScriptsToolBoxView()
		{
			TabText = "Scripts";
			AutoHidePortion = 0.15;

			InitializeComponent();

			RefreshContent(Env.Current.Project);

			Env.Current.Project.ProjectLoaded += new EventHandler(OnProjectLoad);
			Env.Current.ScriptManager.ScriptsUpdated += new EventHandler(OnScriptsUpdated);

			this.FormClosed += new FormClosedEventHandler(OnFormClosed);
		}

		private void InitializeComponent()
		{
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.tree = new System.Windows.Forms.TreeView();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripButton1});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(284, 25);
			this.toolStrip1.TabIndex = 2;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripDropDownButton1
			// 
			this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripDropDownButton1.Image = global::FreeSCADA.Designer.Properties.Resources.script_add;
			this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
			this.toolStripDropDownButton1.Size = new System.Drawing.Size(23, 22);
			this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
			this.toolStripDropDownButton1.Click += new System.EventHandler(this.OnNewScriptClick);
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.Image = global::FreeSCADA.Designer.Properties.Resources.script_remove;
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton1.Text = "toolStripButton1";
			// 
			// tree
			// 
			this.tree.AllowDrop = true;
			this.tree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tree.Location = new System.Drawing.Point(0, 25);
			this.tree.Name = "tree";
			this.tree.Size = new System.Drawing.Size(284, 239);
			this.tree.TabIndex = 3;
			this.tree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnNodeDblClick);
			// 
			// ScriptsToolBoxView
			// 
			this.ClientSize = new System.Drawing.Size(284, 264);
			this.Controls.Add(this.tree);
			this.Controls.Add(this.toolStrip1);
			this.Name = "ScriptsToolBoxView";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		void OnFormClosed(object sender, FormClosedEventArgs e)
		{
			Env.Current.Project.ProjectLoaded -= new EventHandler(OnProjectLoad);
		}

		public void RefreshContent(Project project)
		{
			tree.Nodes.Clear();
			TreeNode root;

			if (string.IsNullOrEmpty(project.FileName))
				root = tree.Nodes.Add(StringResources.UnsavedProjectName);
			else
				root = tree.Nodes.Add(project.FileName);

			foreach (string entity in Env.Current.Project.GetEntities(ProjectEntityType.Script))
			{
				TreeNode node = root.Nodes.Add(entity);
				node.Tag = entity;
				node.EnsureVisible();
			}
		}

		void OnProjectLoad(object sender, EventArgs e)
		{
			RefreshContent((Project)sender);
		}

		void OnScriptsUpdated(object sender, EventArgs e)
		{
			RefreshContent(Env.Current.Project);
		}

		private void OnNodeDblClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (OpenScript != null && e.Node.Tag != null)
			{
				string name = e.Node.Tag as string;
				Script script = Env.Current.ScriptManager.GetScript(name);
				if (script != null)
					OpenScript(this, script);
			}
		}

		private void OnNewScriptClick(object sender, EventArgs e)
		{
			string newName = Env.Current.Project.GenerateUniqueName(ProjectEntityType.Script, StringResources.UntitledScript);
			Env.Current.ScriptManager.CreateNewScript(newName);
		}
	}
}
