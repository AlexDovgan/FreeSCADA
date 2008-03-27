using System;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
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
                    toolsList.Add(new SelectionTool(Schema.MainCanvas));
                    toolsList.Add(new RectangleTool(Schema.MainCanvas));
                    toolsList.Add(new EllipseTool(Schema.MainCanvas));
                    toolsList.Add(new TextBoxTool(Schema.MainCanvas));
                    toolsList.Add(new PolylineTool(Schema.MainCanvas));
                    toolsList.Add(new ActionEditTool(Schema.MainCanvas));
                    toolsList.Add(new ButtonTool(Schema.MainCanvas));
                    toolsList.Add(new GuegeTool(Schema.MainCanvas));
                    toolsList.Add(new ThermoTool(Schema.MainCanvas));
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
                    activeTool = (BaseTool)System.Activator.CreateInstance(defaultTool, new object[] { Schema.MainCanvas });
                    activeTool.Activate();
                    activeTool.ObjectSelected += activeTool_ObjectSelected;
                    activeTool.ToolFinished += new EventHandler(activeTool_ToolFinished);
                    activeTool.ObjectCreated += new EventHandler(activeTool_ObjectCreated);
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
                    activeTool.ObjectCreated -= new EventHandler(activeTool_ObjectCreated);
                }

                if (value != null)
                {
                    activeTool = (BaseTool)System.Activator.CreateInstance(value, new object[] { Schema.MainCanvas });

                    activeTool.ObjectSelected += activeTool_ObjectSelected;
                    activeTool.ToolFinished += new EventHandler(activeTool_ToolFinished);
                    activeTool.ObjectCreated += new EventHandler(activeTool_ObjectCreated);
                    activeTool.Activate();
                }
            }
        }

        void activeTool_ObjectCreated(object sender, EventArgs e)
        {
            undoBuff.AddCommand(new AddGraphicsObject(sender as System.Windows.UIElement));
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

        void OnObjectChenged(object sender, EventArgs e)
        {
            //UndoRedoManager.GetUndoBuffer(workedLayer).AddCommand(new ModifyGraphicsObject(obj));           
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
            if (filename.Contains("xaml"))
            {
                XmlReader xmlReader = XmlReader.Create(filename);
                Object obj = XamlReader.Load(xmlReader);
                Schema.MainCanvas.Children.Add(obj as System.Windows.UIElement);
            }
            else if (filename.Contains("svg"))
            {
                XmlReaderSettings rsettings = new XmlReaderSettings();
                rsettings.ProhibitDtd = false;
                rsettings.ConformanceLevel = ConformanceLevel.Fragment;
                XmlReader reader = XmlReader.Create(filename, rsettings);
                
                try
                {
                    reader.MoveToContent();
                    XslCompiledTransform xslt = new XslCompiledTransform(true);
                    xslt.Load("resources/svg2xaml.xsl", new XsltSettings(true, true), null);

                    //XmlReader.Create(new StringReader (StringResources.svg2xaml))
                    
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.IndentChars = "\t";
                    settings.ConformanceLevel = ConformanceLevel.Fragment;
                    StringWriter sw = new StringWriter();
                    XmlWriter writer = XmlWriter.Create(sw, settings);
                    xslt.Transform(reader, writer);
                    XmlReader xmlReader = XmlReader.Create(new StringReader(sw.ToString()));
                    System.Windows.Controls.Canvas obj = (System.Windows.Controls.Canvas)XamlReader.Load(xmlReader);
                    System.Windows.Controls.Viewbox v = new System.Windows.Controls.Viewbox();
                    v.Child = obj;
                    v.Width = 800;
                    v.Height = 600;
                    Schema.MainCanvas.Children.Add(v);
                    reader.Close();

                }
                catch (Exception ex)
                {
                    if (ex is DirectoryNotFoundException)
                        MessageBox.Show("File resources/svg2xaml.xsl not found");
                    else if((ex is XmlException)&&ex.Message.Contains("DTD"))
                        MessageBox.Show("Please remove DTD declaration from your SVG file");
                    else MessageBox.Show("Sorry but this SVG file can not be converted");
                    reader.Close();
                }

            }
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
