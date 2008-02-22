using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeSCADA.Designer
{
	class ProjectContentView:ToolWindow
    {
    
		public ProjectContentView()
		{
			TabText = "Project Content";
		}

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ProjectContentView
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Name = "ProjectContentView";
            this.ResumeLayout(false);

        }
	}
  
}
