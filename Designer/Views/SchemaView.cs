using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using System.Xml.Xsl;
using FreeSCADA.Common;
using FreeSCADA.Common.Schema;
using FreeSCADA.Designer.SchemaEditor;
using FreeSCADA.Designer.SchemaEditor.SchemaCommands;
using FreeSCADA.Designer.SchemaEditor.ShortProperties;
using FreeSCADA.Designer.SchemaEditor.Tools;
using FreeSCADA.Designer.SchemaEditor.UndoRedo;

namespace FreeSCADA.Designer.Views
{
    class SchemaView : DocumentView
    {
        private WPFShemaContainer wpfSchemaContainer;
        
        private BaseTool activeTool;
        private Type defaultTool = typeof(SelectionTool);
        private ScaleTransform SchemaScale = new ScaleTransform();
        private System.Windows.Point SavedScrollPosition;
        private SchemaDocument schema;
        
        List<ToolDescriptor> toolsList=new List<ToolDescriptor>();
        ICommandData undoCommand, redoCommand;

        public TextBox XamlView
        {
            get;
            private set;
        }


        public List<ToolDescriptor> AvailableTools
        {
            get
            {
                return toolsList;
            }
        }

        public double ZoomLevel
        {
            get
            {
                return SchemaScale.ScaleX;
            }
            set
            {
                SchemaScale.ScaleX = value;
                SchemaScale.ScaleY = value;
                Schema.MainCanvas.LayoutTransform = SchemaScale;
            }
        }

        public SchemaDocument Schema
        {
            get { return schema; }
            set
            {
                schema = value;
                DocumentName = value.Name;
                wpfSchemaContainer.View= value.MainCanvas;
                undoBuff = UndoRedoManager.GetUndoBuffer(Schema);
                undoBuff.CanExecuteChanged += new EventHandler(undoBuff_CanExecuteChanged);
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
                    activeTool.ObjectCreated += activeTool_ObjectCreated;
                    activeTool.ObjectDeleted += activeTool_ObjectDeleted;
                    activeTool.ObjectChanged += OnObjectChenged;
                    foreach (ICommandData icd in DocumentCommands)
                    {
                        if (icd is ZoomInCommand || icd is ZoomOutCommand || icd is UndoCommand || icd is RedoCommand)
                            icd.CommandToolStripItem.Tag = this;
                        else
                            icd.CommandToolStripItem.Tag = activeTool;
                    }
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
                    activeTool = null;
                }

                if (value != null)
                {
                    activeTool = (BaseTool)System.Activator.CreateInstance(value, new object[] { Schema.MainCanvas });

                    activeTool.ObjectSelected += activeTool_ObjectSelected;
                    activeTool.ToolFinished += activeTool_ToolFinished;
                    activeTool.ObjectCreated += activeTool_ObjectCreated;
                    activeTool.ObjectChanged += OnObjectChenged;
                    activeTool.Activate();
                    foreach (ICommandData icd in DocumentCommands)
                    {
                        if (icd is ZoomInCommand || icd is ZoomOutCommand || icd is UndoCommand || icd is RedoCommand)
                            icd.CommandToolStripItem.Tag = this;
                        else
                            icd.CommandToolStripItem.Tag = activeTool;
                    }
                }
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
            this.wpfSchemaContainer.Child.KeyDown += new System.Windows.Input.KeyEventHandler(WpfKeyDown);
            this.wpfSchemaContainer.ZoomInEvent+=new WPFShemaContainer.ZoomDelegate(ZoomIn);
            this.wpfSchemaContainer.ZoomOutEvent+=new WPFShemaContainer.ZoomDelegate(ZoomOut);
            this.XamlView = new TextBox();
            this.XamlView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.XamlView.Multiline = true;
            this.XamlView.ScrollBars = ScrollBars.Both;
            this.XamlView.Size = new System.Drawing.Size(200, 200);
            this.XamlView.Hide();
            XamlView.MaxLength = int.MaxValue;
            // 
            // SchemaView
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.wpfSchemaContainer);
            this.Controls.Add(XamlView);
            this.Name = "SchemaView";

            this.ResumeLayout(false);
            this.SavedScrollPosition = new System.Windows.Point(0.0, 0.0);

            // Commands to ToolStrip
            DocumentCommands.Add(new NullCommand());    // Separator
            DocumentCommands.Add(undoCommand = new UndoCommand());
            DocumentCommands.Add(redoCommand = new RedoCommand());
            DocumentCommands.Add(new NullCommand());    // Separator
            DocumentCommands.Add(new CutCommand());
            DocumentCommands.Add(new CopyCommand());
            DocumentCommands.Add(new PasteCommand());
            DocumentCommands.Add(new NullCommand());    // Separator
            DocumentCommands.Add(new XamlViewCommand(this));
            DocumentCommands.Add(new GroupCommand());
            DocumentCommands.Add(new UngroupCommand());
            DocumentCommands.Add(new NullCommand());    // Separator
            DocumentCommands.Add(new ZoomOutCommand());
            DocumentCommands.Add(new ZoomInCommand());
            DocumentCommands.Add(new NullCommand());    // Separator
            this.ContextMenu = new ToolContextMenu();

            CreateToolList();
            /*
            menu.groupMenuItem.CommandParameter = this;
            menu.unGroupMenuItem.CommandParameter = this;
            menu.cutMenuItem.CommandParameter = this;
            menu.copyMenuItem.CommandParameter = this;
            menu.pasteMenuItem.CommandParameter = this;
            menu.viewXamlMenuItem.CommandParameter = this;
            */
        }
        private void CreateToolList()
        {
            System.Drawing.Bitmap blankBitmap = new System.Drawing.Bitmap(10, 10);
			toolsList.Add(new ToolDescriptor(StringResources.ToolSelection,
				StringResources.ToolEditorGroupName,
                global::FreeSCADA.Designer.Properties.Resources.cursor,
                typeof(SelectionTool)));
            toolsList.Add(new ToolDescriptor(StringResources.ToolRectangle,
				StringResources.ToolGrphicsGroupName,
                global::FreeSCADA.Designer.Properties.Resources.shape_square_add,
                typeof(RectangleTool)));
            toolsList.Add(new ToolDescriptor(StringResources.ToolEllipse,
				StringResources.ToolGrphicsGroupName,
                global::FreeSCADA.Designer.Properties.Resources.shape_ellipse_add,
                typeof(EllipseTool)));
            toolsList.Add(new ToolDescriptor(StringResources.ToolTextbox,
				StringResources.ToolGrphicsGroupName,
                global::FreeSCADA.Designer.Properties.Resources.textfield_add,
                typeof(TextBoxTool)));
            toolsList.Add(new ToolDescriptor(StringResources.ToolPolyline,
                StringResources.ToolGrphicsGroupName,
                global::FreeSCADA.Designer.Properties.Resources.shape_line_add,
                typeof(PolylineTool)));
            toolsList.Add(new ToolDescriptor(StringResources.ToolActionEdit,
                StringResources.ToolEditorGroupName,
                global::FreeSCADA.Designer.Properties.Resources.cog_edit,
                typeof(ActionEditTool)));
            toolsList.Add(new ToolDescriptor(StringResources.ToolButton,
                StringResources.ToolControlsGroupName,
                blankBitmap ,
                 typeof(ControlCreateTool<System.Windows.Controls.Button>)));
            toolsList.Add(new ToolDescriptor(StringResources.ToolProgressbar,
				StringResources.ToolControlsGroupName,
                blankBitmap,
                typeof(ControlCreateTool<System.Windows.Controls.ProgressBar>)));
            toolsList.Add(new ToolDescriptor(StringResources.ToolProgressbar,
				StringResources.ToolControlsGroupName,
                blankBitmap,
                typeof(ControlCreateTool<System.Windows.Controls.Primitives.ScrollBar>)));
            toolsList.Add(new ToolDescriptor(StringResources.ToolContentControl,
				StringResources.ToolControlsGroupName,
                blankBitmap,
                typeof(ControlCreateTool<System.Windows.Controls.ContentControl>)));
            toolsList.Add(new ToolDescriptor(StringResources.ToolSlider,
				StringResources.ToolControlsGroupName,
                blankBitmap,
                typeof(ControlCreateTool<System.Windows.Controls.Slider>)));
        }

        void undoBuff_CanExecuteChanged(object sender, EventArgs e)
        {
            undoCommand.CanExecute(this);
            redoCommand.CanExecute(this);
        }

        void activeTool_ObjectCreated(object sender, EventArgs e)
        {
            undoBuff.AddCommand(new AddGraphicsObject(sender as System.Windows.UIElement));
            UpdateXamlView();
        }

        void activeTool_ObjectDeleted(object sender, EventArgs e)
        {
            undoBuff.AddCommand(new DeleteGraphicsObject(sender as System.Windows.UIElement));
            UpdateXamlView();
        }

        void activeTool_ToolFinished(object sender, EventArgs e)
        {
            NotifyToolsCollectionChanged(AvailableTools, defaultTool);
            UpdateXamlView();
        }
        void OnObjectChenged(object sender, EventArgs e)
        {
            undoBuff.AddCommand(new ModifyGraphicsObject((System.Windows.UIElement)sender));
            UpdateXamlView();
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
            else if (e.Key == System.Windows.Input.Key.X &&
                (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) != System.Windows.Input.ModifierKeys.None)
            {
                if (activeTool is SelectionTool && activeTool.SelectedObject != null)
                    new CutCommand().Execute(activeTool);
            }
            else if (e.Key == System.Windows.Input.Key.C &&
                (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) != System.Windows.Input.ModifierKeys.None)
            {
                if (activeTool is SelectionTool && activeTool.SelectedObject != null)
                    new CopyCommand().Execute(activeTool);
            }
            else if (e.Key == System.Windows.Input.Key.V &&
                (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) != System.Windows.Input.ModifierKeys.None)
            {
                if (activeTool is SelectionTool)
                    new PasteCommand().Execute(activeTool);
            }
            else if (e.Key == System.Windows.Input.Key.Delete)
            {
                if (activeTool is SelectionTool && activeTool.SelectedObject != null)
                    undoBuff.AddCommand(new DeleteGraphicsObject(activeTool.SelectedObject));
                else if (activeTool is SelectionTool && (activeTool as SelectionTool).selectedElements.Count > 0)
                {
                    foreach (System.Windows.UIElement el in (activeTool as SelectionTool).selectedElements)
                    {
                        undoBuff.AddCommand(new DeleteGraphicsObject(el));
                    }
                }
                activeTool.SelectedObject = null;
            }
            else if (e.Key == System.Windows.Input.Key.Add && (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) != System.Windows.Input.ModifierKeys.None)
            {
                ZoomIn();

            }
            else if (e.Key == System.Windows.Input.Key.Subtract && (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) != System.Windows.Input.ModifierKeys.None)
            {
                ZoomOut();
            }

            else if (e.Key == System.Windows.Input.Key.Escape)
            {
                //CurrentTool = null;
                //CurrentTool = defaultTool;
                NotifySetCurrentTool(toolsList[0]);
                //NotifyToolsCollectionChanged(AvailableTools, activeTool.GetType());
                //windowManager.SetCurrentTool(AvailableTools, defaultTool);
            }
            else if (e.Key == System.Windows.Input.Key.F5)
            {
                UpdateCanvasByXaml();
            
            }
            else if (activeTool is SelectionTool)
            {
                if (e.Key == System.Windows.Input.Key.Left)
                {
                    (activeTool as SelectionTool).MoveHelper(-1, 0);
                }
                if (e.Key == System.Windows.Input.Key.Right)
                {
                    (activeTool as SelectionTool).MoveHelper(1, 0);
                }
                if (e.Key == System.Windows.Input.Key.Up)
                {
                    (activeTool as SelectionTool).MoveHelper(0, -1);
                }
                if (e.Key == System.Windows.Input.Key.Down)
                {
                    (activeTool as SelectionTool).MoveHelper(0, 1);
                }
            }

            e.Handled = true;
            Schema.MainCanvas.UpdateLayout();
            activeTool.Update();
        }
        
        void activeTool_ObjectSelected(Object obj)
        {
            CommonShortProp csp;
            if (obj == null)
                obj = Schema.MainCanvas;
            if ((csp = ObjectsFactory.CreateShortProp(obj)) != null)
            {
                RaiseObjectSelected(csp);
                csp.PropertiesBrowserChanged += new CommonShortProp.PropertiesBrowserChangedDelegate(OnPropertiesBrowserChanged);
            }
            else
                RaiseObjectSelected(obj);
            // Menu items
            foreach (ICommandData icd in DocumentCommands)
            {
                icd.CanExecute(activeTool);
            }
        }

        public override void OnActivated()
        {
            base.OnActivated();

            //Notify connected windows about new tool collection
            NotifyToolsCollectionChanged(AvailableTools, CurrentTool);
            // Scroll to saved position
            System.Windows.Controls.ScrollViewer msv = (System.Windows.Controls.ScrollViewer)wpfSchemaContainer.Child;
            msv.ScrollToVerticalOffset(SavedScrollPosition.Y);
            msv.ScrollToHorizontalOffset(SavedScrollPosition.X);
        }

        public override void OnDeactivated()
        {
            base.OnDeactivated();

            // Save scroll position
            if (wpfSchemaContainer != null)
            {
                System.Windows.Controls.ScrollViewer msv = (System.Windows.Controls.ScrollViewer)wpfSchemaContainer.Child;
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

        
        void OnSchemaIsModifiedChanged(object sender, EventArgs e)
        {
            IsModified = ((SchemaDocument)sender).IsModified;
            Schema.MainCanvas.UpdateLayout();
            activeTool.Update();
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
            //creating dynamic object with binding
            System.Windows.Controls.ProgressBar pb = new System.Windows.Controls.ProgressBar();
            pb.Width = 100; pb.Height = 100;
            Schema.MainCanvas.Children.Add(pb);
            System.Windows.Data.Binding bind=new System.Windows.Data.Binding("Value");
            System.Windows.Data.ObjectDataProvider dp;
            dp = new System.Windows.Data.ObjectDataProvider();
            ChannelDataSource chs=new ChannelDataSource();
            chs.ChannelName="data_simulator_plug.sin";
            dp.ObjectInstance = chs;
            dp.MethodName = "GetChannel";
            bind.Source = dp; 
            pb.SetBinding(System.Windows.Controls.ProgressBar.ValueProperty, bind);
            
              
            
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
            if (wpfSchemaContainer != null && Schema != null)
            {
                Schema.IsModifiedChanged -= new EventHandler(OnSchemaIsModifiedChanged);
                wpfSchemaContainer.Child.KeyDown -= new System.Windows.Input.KeyEventHandler(WpfKeyDown);
                UndoRedoManager.ReleaseUndoBuffer(Schema);
            }

            CurrentTool = null;

            wpfSchemaContainer.Dispose();
            wpfSchemaContainer = null;
            base.OnClosed(e);
        }

        public void ZoomIn()
        {
            ZoomIn(new System.Windows.Point(0.0, 0.0));
        }

        public void ZoomIn(System.Windows.Point center)
        {
            System.Windows.Controls.ScrollViewer msv = (System.Windows.Controls.ScrollViewer)wpfSchemaContainer.Child;
            SchemaScale.ScaleX *= 1.05;
            SchemaScale.ScaleY *= 1.05;
            Schema.MainCanvas.LayoutTransform = SchemaScale;
            msv.ScrollToVerticalOffset(msv.VerticalOffset * 1.05 + center.Y* 0.05);
            msv.ScrollToHorizontalOffset(msv.HorizontalOffset * 1.05 + center.X * 0.05);
            //this is must be an event 
            (Env.Current.MainWindow as MainForm).zoomLevelComboBox_SetZoomLevelTxt(SchemaScale.ScaleX);
        }

        public void ZoomOut()
        {
            ZoomOut(new System.Windows.Point(0.0, 0.0));
        }

        public void ZoomOut(System.Windows.Point center)
        {
            System.Windows.Controls.ScrollViewer msv = (System.Windows.Controls.ScrollViewer)wpfSchemaContainer.Child;
            SchemaScale.ScaleX /= 1.05;
            SchemaScale.ScaleY /= 1.05;
            Schema.MainCanvas.LayoutTransform = SchemaScale;
            msv.ScrollToVerticalOffset(msv.VerticalOffset / 1.05 - center.Y * 0.05);
            msv.ScrollToHorizontalOffset(msv.HorizontalOffset / 1.05 - center.X * 0.05);
            (Env.Current.MainWindow as MainForm).zoomLevelComboBox_SetZoomLevelTxt(SchemaScale.ScaleX);
        }

      
        public void UpdateCanvasByXaml()
        {
            if (XamlView.Visible)
            {
                using (MemoryStream stream = new MemoryStream(this.XamlView.Text.Length))
                {
                    using (StreamWriter sw = new StreamWriter(stream))
                    {
                        sw.Write(this.XamlView.Text);
                        sw.Flush();
                        stream.Seek(0, SeekOrigin.Begin);
                        System.Windows.Controls.Canvas canvas = XamlReader.Load(stream) as System.Windows.Controls.Canvas;
                        CurrentTool = null;
                        Schema.MainCanvas = canvas;
                        wpfSchemaContainer.View = Schema.MainCanvas;
                        wpfSchemaContainer.Child.UpdateLayout();
                        NotifyToolsCollectionChanged(AvailableTools, defaultTool);
                        CurrentTool = defaultTool;

                        //EditorHelper.CopyObjects(canvas, Schema.MainCanvas);

                    }
                }
            }
        }

        public override void OnPropertiesBrowserChanged(object el)
        {
            IsModified = true;
            if (el != null)
                undoBuff.AddCommand((new ModifyGraphicsObject((System.Windows.UIElement)el)));
        }

        public void UpdateXamlView()
        {
            if (XamlView.Visible)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;
                XamlDesignerSerializationManager dsm = new XamlDesignerSerializationManager(XmlWriter.Create(sb, settings));
                dsm.XamlWriterMode = XamlWriterMode.Expression;
                XamlWriter.Save(Schema.MainCanvas, dsm);
                XamlView.Text = sb.ToString();
            }
        }

    }
}
