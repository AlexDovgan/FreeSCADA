using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreeSCADA.Schema;
using FreeSCADA.ShellInterfaces;

namespace FreeSCADA.Designer.Views
{
    class SchemaView : DocumentView
    {
        private System.Windows.Forms.Integration.ElementHost wpfContainerHost;
        public SchemaEditor schemaEditor;

		public delegate void ToolsCollectionChangedHandler(List<ITool> tools, Type defaultTool);
		public event ToolsCollectionChangedHandler ToolsCollectionChanged;

        public SchemaView()
        {
			InitializeComponent();

            SchemaDocument schema;

            if ((schema = SchemaDocument.CreateNewSchema()) != null)
            {
                schemaEditor = new SchemaEditor(schema);
                TabText = schema.Name;
            }
            else throw new Exception("Canceled");
            schemaEditor.ObjectSelected += new SchemaEditor.ObjectSelectedDelegate(objectSelected);
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
            this.wpfContainerHost.Child = schemaEditor;

            this.ResumeLayout(false);

        }
       
		public override void  OnActivated()
		{
			base.OnActivated();

			//Notify connected windows about new tools collection
			if(ToolsCollectionChanged != null)
				ToolsCollectionChanged(schemaEditor.toolsList, schemaEditor.CurrentTool);
		}

		public override void OnToolActivated(object sender, Type tool)
        {
            schemaEditor.CurrentTool = tool;
        }
        
        public void objectSelected(object element)
        {
            
            RaiseObjectSelected(element);
        }
    }
}
