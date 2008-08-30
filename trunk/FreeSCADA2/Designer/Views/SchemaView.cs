using System;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Forms;
using System.Collections.Generic;
using FreeSCADA.ShellInterfaces;
using FreeSCADA.Designer.SchemaEditor;
using FreeSCADA.Common.Schema;
using FreeSCADA.Common;
using FreeSCADA.Designer.SchemaEditor.Tools;
using FreeSCADA.Designer.SchemaEditor.UndoRedo;
using FreeSCADA.Designer.SchemaEditor.ShortProperties;

namespace FreeSCADA.Designer.Views
{
    class SchemaView : DocumentView
    {
        private WPFShemaContainer wpfSchemaContainer;
        private BaseTool activeTool;
        private Type defaultTool = typeof(SelectionTool);
        private ScaleTransform SchemaScale = new ScaleTransform();
        private System.Windows.Point SavedScrollPosition;
        private SelectionTool selectionTool;
        private WindowManager windowManager;
        List<ITool> toolsList;

        public SelectionTool SchemaViewSelectionTool {
            get { 
                //return selectionTool; 
                return (SelectionTool)toolsList[0];
            }
        }

        public List<ITool> AvailableTools
        {
            get
            {
                if (toolsList == null)
                {
                    toolsList = new List<ITool>();
                    selectionTool = new SelectionTool(Schema.MainCanvas);
                    toolsList.Add(selectionTool);
                    toolsList.Add(new RectangleTool(Schema.MainCanvas));
                    toolsList.Add(new EllipseTool(Schema.MainCanvas));
                    toolsList.Add(new TextBoxTool(Schema.MainCanvas));
                    toolsList.Add(new PolylineTool(Schema.MainCanvas));
                    toolsList.Add(new ActionEditTool(Schema.MainCanvas));
                    //toolsList.Add(new ButtonTool(Schema.MainCanvas));
                   // toolsList.Add(new GuegeTool(Schema.MainCanvas));
                   // toolsList.Add(new ThermoTool(Schema.MainCanvas));
                    toolsList.Add(new ControlCreateTool<System.Windows.Controls.Button>(Schema.MainCanvas));
                    toolsList.Add(new ControlCreateTool<System.Windows.Controls.ProgressBar>(Schema.MainCanvas));
                    toolsList.Add(new ControlCreateTool<System.Windows.Controls.Primitives.ScrollBar>(Schema.MainCanvas));
                    toolsList.Add(new ControlCreateTool<System.Windows.Controls.ContentControl>(Schema.MainCanvas));
                    toolsList.Add(new ControlCreateTool<System.Windows.Controls.Slider>(Schema.MainCanvas));

                }
                return toolsList;
            }
        }

        public SchemaView(WindowManager wm)
        {
            windowManager = wm;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.wpfSchemaContainer = new WPFShemaContainer(this);
            // 
            // wpfContainerHost
            // 
            this.wpfSchemaContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wpfSchemaContainer.Location = new System.Drawing.Point(0, 0);
            this.wpfSchemaContainer.Name = "WPFSchemaContainer";
            this.wpfSchemaContainer.Size = new System.Drawing.Size(292, 273);
            this.wpfSchemaContainer.TabIndex = 0;
            this.wpfSchemaContainer.Text = "WPFSchemaContainer";
            this.wpfSchemaContainer.Child.KeyDown += new System.Windows.Input.KeyEventHandler(WpfKeyDown);
            // 
            // SchemaView
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.wpfSchemaContainer);
            this.Name = "SchemaView";

            this.ResumeLayout(false);
            this.SavedScrollPosition = new System.Windows.Point(0.0, 0.0);
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
                    activeTool.ToolFinished += activeTool_ToolFinished;
                    activeTool.ObjectCreated +=activeTool_ObjectCreated;
                    activeTool.ObjectChanged += OnObjectChenged;
                }
                return activeTool.GetType();
            }
            set
            {
                if (activeTool != null)
                {
                    

                    activeTool.ObjectSelected -= activeTool_ObjectSelected;
                    activeTool.ToolFinished -= activeTool_ToolFinished;
                    activeTool.ObjectCreated -= activeTool_ObjectCreated;
                    activeTool.ObjectChanged -= OnObjectChenged;
                    activeTool.Deactivate();
                }

                if (value != null)
                {
                    activeTool = (BaseTool)System.Activator.CreateInstance(value, new object[] { Schema.MainCanvas });

                    activeTool.ObjectSelected += activeTool_ObjectSelected;
                    activeTool.ToolFinished += activeTool_ToolFinished;
                    activeTool.ObjectCreated += activeTool_ObjectCreated;
                    activeTool.ObjectChanged += OnObjectChenged;
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
        void WpfKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //MessageBox.Show("WpfKeyDown here " + e.Key);
            if (e.Key == System.Windows.Input.Key.Z &&
                (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) != System.Windows.Input.ModifierKeys.None)
            {
                undoBuff.UndoCommand();
                //activeTool.SelectedObject = null;

            }
            else if (e.Key == System.Windows.Input.Key.Y &&
                (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) != System.Windows.Input.ModifierKeys.None)
            {
                undoBuff.RedoCommand();
            }
            else if (e.Key == System.Windows.Input.Key.Delete)
            {

                undoBuff.AddCommand(new DeleteGraphicsObject(activeTool.SelectedObject));
                activeTool.SelectedObject = null;
            }
            else if (e.Key == System.Windows.Input.Key.Add)
            {
                ZoomIn();

            }
            else if (e.Key == System.Windows.Input.Key.Subtract)
            {
                ZoomOut();
            }

            else if (e.Key == System.Windows.Input.Key.Escape)
            {
                CurrentTool = null;
                selectionTool.SelectedObject = null;
                activeTool = selectionTool;
                activeTool.Update();
                windowManager.SetCurrentTool(AvailableTools, defaultTool);
            }
            else if (activeTool is SelectionTool)
            {
                List<System.Windows.UIElement> lst;
                if (e.Key == System.Windows.Input.Key.Left)
                {
                    lst = (activeTool as SelectionTool).MoveHelper(-1, 0);
                    foreach (System.Windows.UIElement el in lst)
                        undoBuff.AddCommand(new ModifyGraphicsObject(el));
                }
                if (e.Key == System.Windows.Input.Key.Right)
                {
                    lst = (activeTool as SelectionTool).MoveHelper(1, 0);
                    foreach (System.Windows.UIElement el in lst)
                        undoBuff.AddCommand(new ModifyGraphicsObject(el));
                }
                if (e.Key == System.Windows.Input.Key.Up)
                {
                    lst = (activeTool as SelectionTool).MoveHelper(0, -1);
                    foreach (System.Windows.UIElement el in lst)
                        undoBuff.AddCommand(new ModifyGraphicsObject(el));
                }
                if (e.Key == System.Windows.Input.Key.Down)
                {
                    lst = (activeTool as SelectionTool).MoveHelper(0, 1);
                    foreach (System.Windows.UIElement el in lst)
                        undoBuff.AddCommand(new ModifyGraphicsObject(el));
                }
            }

            e.Handled = true;
            Schema.MainCanvas.UpdateLayout();
            activeTool.Update();
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
            // Scroll to saved position
            ZoomViewer msv = (ZoomViewer)wpfSchemaContainer.Child;
            msv.ScrollToVerticalOffset(SavedScrollPosition.Y);
            msv.ScrollToHorizontalOffset(SavedScrollPosition.X);
        }

        public override void OnDeactivated()
        {
            base.OnDeactivated();

            // Save scroll position
            if (wpfSchemaContainer != null)
            {
                ZoomViewer msv = (ZoomViewer)wpfSchemaContainer.Child;
                if (msv != null)
                {
                    SavedScrollPosition.Y = msv.VerticalOffset;
                    SavedScrollPosition.X = msv.HorizontalOffset;
                }
            }
        }

        public override void OnToolActivated(object sender, Type tool)
        {
            CurrentTool = tool;
            Focus();
        }

        void OnObjectChenged(object sender, EventArgs e)
        {
            undoBuff.AddCommand(new ModifyGraphicsObject((System.Windows.UIElement)sender));           
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
                    xslt.Load("resources\\svg2xaml.xsl", new XsltSettings(true, true), null);

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
                    v.Width = 640;
                    v.Height = 480;
                    v.Stretch = System.Windows.Media.Stretch.Fill;
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
                wpfSchemaContainer.Child.KeyDown -= new System.Windows.Input.KeyEventHandler(WpfKeyDown);
                UndoRedoManager.ReleaseUndoBuffer(wpfSchemaContainer.Document);
            }

            CurrentTool = null;

            wpfSchemaContainer.Dispose();
            wpfSchemaContainer = null;

            base.OnClosed(e);
        }

        public void ZoomIn()
        {
            ZoomIn(0.0, 0.0);
        }

        public void ZoomIn(double CenterX, double CenterY)
        {
            ZoomViewer msv = (ZoomViewer)wpfSchemaContainer.Child;
            SchemaScale.ScaleX *= 1.05;
            SchemaScale.ScaleY *= 1.05;
            Schema.MainCanvas.LayoutTransform = SchemaScale;
            msv.ScrollToVerticalOffset(msv.VerticalOffset * 1.05 + CenterY * 0.05);
            msv.ScrollToHorizontalOffset(msv.HorizontalOffset * 1.05 + CenterX * 0.05);
            (Env.Current.MainWindow as MainForm).zoomLevelComboBox_SetZoomLevelTxt(SchemaScale.ScaleX);
        }

        public void ZoomOut()
        {
            ZoomOut(0.0, 0.0);
        }

        public void ZoomOut(double CenterX, double CenterY)
        {
            ZoomViewer msv = (ZoomViewer)wpfSchemaContainer.Child;
            SchemaScale.ScaleX /= 1.05;
            SchemaScale.ScaleY /= 1.05;
            Schema.MainCanvas.LayoutTransform = SchemaScale;
            msv.ScrollToVerticalOffset(msv.VerticalOffset / 1.05 - CenterY * 0.05);
            msv.ScrollToHorizontalOffset(msv.HorizontalOffset / 1.05 - CenterX * 0.05);
            (Env.Current.MainWindow as MainForm).zoomLevelComboBox_SetZoomLevelTxt(SchemaScale.ScaleX);
        }

        public double ZoomLevel
        {
            get {
                return SchemaScale.ScaleX;
            }
            set
            {
                SchemaScale.ScaleX = value;
                SchemaScale.ScaleY = value;
                Schema.MainCanvas.LayoutTransform = SchemaScale;
            }
        }

        public void ChangeGraphicsObject(System.Windows.UIElement old, System.Windows.UIElement el)
        {
            MessageBox.Show("SchemaView: " + el.ToString());
            undoBuff.AddCommand(new ChangeGraphicsObject(old, el));
        }

    }
}
