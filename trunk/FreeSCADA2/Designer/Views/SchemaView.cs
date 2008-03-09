using System;
using System.IO;
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

		
        public List<ITool> toolsList
        {
            get
            {
                List<ITool> tl = new List<ITool>();
                tl.Add(new SelectionTool(Schema));
                tl.Add(new RectangleTool(Schema));
                tl.Add(new EllipseTool(Schema));
                tl.Add(new TextBoxTool(Schema));
                tl.Add(new PolylineTool(Schema));
                return tl;

            }

        }
        public Type CurrentTool
        {
            get
            {
                if (activeTool == null)
                {
                    activeTool = new SelectionTool(Schema);
                    activeTool.Activate();
                    activeTool.ObjectSelected += activeTool_ObjectSelected;
                }
                return activeTool.GetType();
            }
            set
            {
                activeTool.Deactivate();
                activeTool.ObjectSelected -= activeTool_ObjectSelected;
                object[] a = new object[1];
                a[0] = Schema;
                activeTool = (BaseTool)System.Activator.CreateInstance(value, a);
                activeTool.ObjectSelected += activeTool_ObjectSelected;
                activeTool.Activate();
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
        void activeTool_ObjectSelected(System.Windows.UIElement obj)
        {
            
       
                RaiseObjectSelected(ObjectsFactory.CreateShortProp(obj));

        }

    
        public override void OnActivated()
        {
            base.OnActivated();

            //Notify connected windows about new tools collection
            
            RaiseToolsCollectionChanged(toolsList, CurrentTool);
        
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

        public void ImportFile(Stream xamlStream)
        {
            /*XmlReader xmlReader = XmlReader.Create(xamlStream);
            Object obj = XamlReader.Load(xmlReader);
            Schema.MainCanvas.Children.Add(obj as UIElement);
             */
        }
        protected override void OnClosed(EventArgs e)
        {
 
            if (wpfSchemaContainer != null && wpfSchemaContainer.Document != null)
            {
                wpfSchemaContainer.Document.IsModifiedChanged -= new EventHandler(OnSchemaIsModifiedChanged);
                UndoRedoManager.ReleaseUndoBuffer(wpfSchemaContainer.Document);
            }
            if (activeTool != null)
            {
                activeTool.Deactivate();
                activeTool.ObjectSelected -= activeTool_ObjectSelected;
                activeTool = null;
            }
            
            
            wpfSchemaContainer.Dispose();
            wpfSchemaContainer = null;

            base.OnClosed(e);
        }
    }
}
