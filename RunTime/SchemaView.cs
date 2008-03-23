using System;
using FreeSCADA.Common.Schema;
using WeifenLuo.WinFormsUI.Docking;

namespace FreeSCADA.RunTime
{
	class SchemaView : DockContent
	{
		private WPFShemaContainer wpfSchemaContainer;

		public SchemaView()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			this.wpfSchemaContainer = new WPFShemaContainer();
			// 
			// wpfContainerHost
			// 
			this.wpfSchemaContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.wpfSchemaContainer.Location = new System.Drawing.Point(0, 0);
			this.wpfSchemaContainer.Name = "WPFSchemaContainer";
			this.wpfSchemaContainer.Size = new System.Drawing.Size(292, 273);
			this.wpfSchemaContainer.TabIndex = 0;
			this.wpfSchemaContainer.Text = "WPFSchemaContainer";
			// 
			// SchemaView
			// 
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.Add(this.wpfSchemaContainer);
			this.Name = "SchemaView";

			this.ResumeLayout(false);

		}

		public SchemaDocument Schema
		{
			get { return wpfSchemaContainer.Document; }
			set
			{
				TabText = value.Name;
				wpfSchemaContainer.Document = value;
			}
		}

		public bool LoadDocument(string name)
		{
			SchemaDocument schema;
			if ((schema = SchemaDocument.LoadSchema(name)) == null)
				return false;
			schema.LinkActions();
			Schema = schema;
			return true;
		}

		protected override void OnClosed(EventArgs e)
		{
			wpfSchemaContainer.Dispose();
			wpfSchemaContainer = null;

			base.OnClosed(e);
		}
	}
}
