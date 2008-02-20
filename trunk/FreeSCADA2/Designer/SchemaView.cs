using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreeSCADA.Scheme;
using FreeSCADA.ShellInterfaces;

namespace FreeSCADA.Designer
{
    class SchemaView : DocumentWindow
    {
        private System.Windows.Forms.Integration.ElementHost wpfContainerHost;
        public FSSchemeEditor schemeEditor;

		public delegate void ToolsCollectionChangedHandler(List<ITool> tools, Type defaultTool);
		public event ToolsCollectionChangedHandler ToolsCollectionChanged;

        public SchemaView()
        {
            TabText = "Schema #";
			schemeEditor = new FSSchemeEditor(FSSchemeDocument.CreateNewScheme());
            InitializeComponent();

			FreeSCADA.Common.Env.Current.Project.LoadEvent += new EventHandler(OnProjectLoad);
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

            //this.VisibleChanged += new EventHandler(SchemaView_VisibleChanged);
            this.ResumeLayout(false);

        }

		void OnProjectLoad(object sender, EventArgs e)
		{
			//Load schema from project file
		}
       
		public override void  OnActivated()
		{
			base.OnActivated();

			//Notify connected windows about new tools collection
			if(ToolsCollectionChanged != null)
				ToolsCollectionChanged(schemeEditor.toolsList, schemeEditor.CurrentTool);
		}

		public void OnToolActivated(object sender, Type tool)
        {
            schemeEditor.CurrentTool = tool;
        }
    }
}
