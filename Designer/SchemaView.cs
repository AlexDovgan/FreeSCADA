using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreeSCADA.Scheme;

namespace FreeSCADA.Designer
{
    class SchemaView : DocumentWindow
    {
        private System.Windows.Forms.Integration.ElementHost wpfContainerHost;
        public FSSchemeEditor schemeEditor;
        public SchemaView(FSSchemeEditor sch)
        {
            TabText = "Schema #";
            schemeEditor = sch;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            
            this.SuspendLayout();
            this.wpfContainerHost = new System.Windows.Forms.Integration.ElementHost();
            // 
            // wpfContainerHost
            // 
            this.wpfContainerHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wpfContainerHost.Location = new System.Drawing.Point(0, 0);
            this.wpfContainerHost.Name = "wpfContainerHost";
            this.wpfContainerHost.Size = new System.Drawing.Size(292, 273);
            this.wpfContainerHost.TabIndex = 0;
            this.wpfContainerHost.Text = "elementHost1";
            this.wpfContainerHost.Child = null;
            // 
            // SchemaView
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.wpfContainerHost);
            this.Name = "SchemaView";
            this.wpfContainerHost.Child = schemeEditor;

            this.VisibleChanged += new EventHandler(SchemaView_VisibleChanged);
            this.ResumeLayout(false);

        }

        void SchemaView_VisibleChanged(object sender, EventArgs e)
        {
            ToolBoxView tbv = (ToolBoxView)WindowManager.GetToolWindow("toolBox");
            if (Visible==true)
            {
                
                
                tbv.ToolsCollectionChanged(schemeEditor.toolsList, schemeEditor.CurrentTool);
                tbv.ToolActivated += new ToolBoxView.ToolActivatedDelegate(ToolActivated);
            }
            else tbv.ToolActivated -= ToolActivated;

        
        }

        void ToolActivated(FreeSCADA.ShellInterfaces.ITool tool)
        {
            schemeEditor.CurrentTool = tool;
        }

        
    }
}
