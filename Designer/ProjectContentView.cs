using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeSCADA.Designer
{
	class ProjectContentView:ToolWindow
    {
        private System.Windows.Forms.TreeView treeView1;
    
		public ProjectContentView()
		{
			TabText = "Project Content";
            InitializeComponent();
		}

        private void InitializeComponent()
        {
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(292, 273);
            this.treeView1.TabIndex = 0;
            // 
            // ProjectContentView
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.treeView1);
            this.Name = "ProjectContentView";
            this.ResumeLayout(false);

        }
	}
  
}
