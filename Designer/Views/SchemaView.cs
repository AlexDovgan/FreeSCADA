using System;
using System.IO;
using System.Xml;
using System.Windows.Markup;
using System.Windows.Forms;
using System.Collections.Generic;
using FreeSCADA.ShellInterfaces;
using FreeSCADA.Designer.SchemaEditor;
using FreeSCADA.Common.Schema;
using FreeSCADA.Designer.SchemaEditor.Tools;
using FreeSCADA.Designer.SchemaEditor.UndoRedo;
using FreeSCADA.Designer.SchemaEditor.ShortProperties;

namespace FreeSCADA.Designer.Views
{
	class SchemaView : DocumentView
	{
		private WPFShemaContainer wpfSchemaContainer;
		private BasicUndoBuffer undoBuff;
		private BaseTool activeTool;
		private Type defaultTool = typeof(SelectionTool);
		List<ITool> toolsList;

		public List<ITool> AvailableTools
		{
			get 
			{
				if (toolsList == null)
				{
					toolsList = new List<ITool>();
					toolsList.Add(new SelectionTool(Schema));
					toolsList.Add(new RectangleTool(Schema));
					toolsList.Add(new EllipseTool(Schema));
					toolsList.Add(new TextBoxTool(Schema));
					toolsList.Add(new PolylineTool(Schema));
					toolsList.Add(new ActionEditTool(Schema));
					toolsList.Add(new ButtonTool(Schema));
                    toolsList.Add(new GuegeTool(Schema));
                    toolsList.Add(new ThermoTool(Schema)); 
				}
				return toolsList; 
			}
		}

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
				DocumentName = value.Name;
				wpfSchemaContainer.Document = value;
				undoBuff = UndoRedoManager.GetUndoBuffer(Schema);
			}
		}

		public Type CurrentTool
		{
			get
			{
				if (activeTool == null)
				{
					activeTool = (BaseTool)System.Activator.CreateInstance(defaultTool, new object[] { Schema });
					activeTool.Activate();
					activeTool.ObjectSelected += activeTool_ObjectSelected;
					activeTool.ToolFinished += new EventHandler(activeTool_ToolFinished);
				}
				return activeTool.GetType();
			}
			set
			{
				if (activeTool != null)
				{
					activeTool.Deactivate();

					activeTool.ObjectSelected -= activeTool_ObjectSelected;
					activeTool.ToolFinished -= new EventHandler(activeTool_ToolFinished);
				}

				if (value != null)
				{
					activeTool = (BaseTool)System.Activator.CreateInstance(value, new object[] { Schema });

					activeTool.ObjectSelected += activeTool_ObjectSelected;
					activeTool.ToolFinished += new EventHandler(activeTool_ToolFinished);

					activeTool.Activate();
				}
			}
		}

		void activeTool_ToolFinished(object sender, EventArgs e)
		{
			NotifyToolsCollectionChanged(AvailableTools, defaultTool);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{

			if (e.KeyCode == Keys.Z && e.Control)
			{
				undoBuff.UndoCommand();
			}
			else if (e.KeyCode == Keys.Y && e.Control)
			{
				undoBuff.RedoCommand();
			}
			base.OnKeyDown(e);
		}

		void activeTool_ObjectSelected(Object obj)
		{
			CommonShortProp csp;
			if ((csp = ObjectsFactory.CreateShortProp(obj)) != null)
				RaiseObjectSelected(csp);
			else
				RaiseObjectSelected(obj);
		}

		public override void OnActivated()
		{
			base.OnActivated();

			//Notify connected windows about new tool collection
			NotifyToolsCollectionChanged(AvailableTools, CurrentTool);
		}

		public override void OnToolActivated(object sender, Type tool)
		{
			CurrentTool = tool;
			Focus();
		}

		void OnSchemaIsModifiedChanged(object sender, EventArgs e)
		{
			IsModified = ((SchemaDocument)sender).IsModified;
		}

		public override bool SaveDocument()
		{
			Schema.Name = DocumentName;
			Schema.SaveSchema();
			return true;
		}

		public override bool LoadDocument(string name)
		{
			SchemaDocument schema;
			if ((schema = SchemaDocument.LoadSchema(name)) == null)
				return false;

			Schema = schema;
			Schema.IsModifiedChanged += new EventHandler(OnSchemaIsModifiedChanged);
			return true;
		}

		public override bool CreateNewDocument()
		{
			SchemaDocument schema;

			if ((schema = SchemaDocument.CreateNewSchema()) == null)
				return false;

			Schema = schema;
			Schema.IsModifiedChanged += new EventHandler(OnSchemaIsModifiedChanged);
			IsModified = true;
			return true;
		}

		public bool ImportFile(String filename)
		{

			XmlReader xmlReader = XmlReader.Create(filename);
			Object obj = XamlReader.Load(xmlReader);
			Schema.MainCanvas.Children.Add(obj as System.Windows.UIElement);
			return true;
		}

		protected override void OnClosed(EventArgs e)
		{
			if (wpfSchemaContainer != null && wpfSchemaContainer.Document != null)
			{
				wpfSchemaContainer.Document.IsModifiedChanged -= new EventHandler(OnSchemaIsModifiedChanged);
				UndoRedoManager.ReleaseUndoBuffer(wpfSchemaContainer.Document);
			}

			CurrentTool = null;

			wpfSchemaContainer.Dispose();
			wpfSchemaContainer = null;

			base.OnClosed(e);
		}
	}
}
