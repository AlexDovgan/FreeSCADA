using System;
using System.Collections.Generic;
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

            this.ResumeLayout(false);

        }

		void OnSchemaIsModifiedChanged(object sender, EventArgs e)
		{
			IsModified = ((SchemaDocument)sender).IsModified;
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

        public override bool SaveDocument()
        {
			schemaEditor.Schema.Name = DocumentName;
            schemaEditor.Schema.SaveSchema();
			return true;
        }
        
        public override bool LoadDocument(string name)
        {
            SchemaDocument schema;
			if ((schema = SchemaDocument.LoadSchema(name)) == null)
				return false;

			schemaEditor = new SchemaEditor(schema);
			DocumentName = schema.Name;
            schemaEditor.ObjectSelected += new SchemaEditor.ObjectSelectedDelegate(objectSelected);
			schemaEditor.Schema.IsModifiedChanged += new EventHandler(OnSchemaIsModifiedChanged);
            wpfContainerHost.Child = schemaEditor;
			return true;
        }
        public override bool CreateNewDocument()
        {
            SchemaDocument schema;

            if ((schema = SchemaDocument.CreateNewSchema()) == null)
				return false;

			schemaEditor = new SchemaEditor(schema);
			DocumentName = schema.Name;
            schemaEditor.ObjectSelected += new SchemaEditor.ObjectSelectedDelegate(objectSelected);
			schemaEditor.Schema.IsModifiedChanged += new EventHandler(OnSchemaIsModifiedChanged);
            wpfContainerHost.Child = schemaEditor;
			IsModified = true;
			return true;
        }
    }
}
