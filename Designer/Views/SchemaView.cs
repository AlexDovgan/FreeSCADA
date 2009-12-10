using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using FreeSCADA.Common;
using FreeSCADA.Common.Schema;
using FreeSCADA.Common.Schema.Gestures;
using FreeSCADA.Designer.SchemaEditor;
using FreeSCADA.Designer.SchemaEditor.PropertiesUtils;
using FreeSCADA.Designer.SchemaEditor.SchemaCommands;
using FreeSCADA.Designer.SchemaEditor.Tools;
using FreeSCADA.Designer.SchemaEditor.UndoRedo;
using FreeSCADA.Interfaces;
using FreeSCADA.Interfaces.Plugins;


namespace FreeSCADA.Designer.Views
{
    class SchemaView : DocumentView
    {

        // TODO: make all actions by Commands, make command execution by pattern Vistor - one 
        #region fields
        WPFShemaContainer wpfSchemaContainer;
        

        BaseTool activeTool;
        Type defaultTool = typeof(SelectionTool);

        GridManager gridManger;

        System.Windows.Forms.ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
        ICommandContext documentMenuContext;
        List<ToolDescriptor> toolsList = new List<ToolDescriptor>();
        SchemaCommand undoCommand, redoCommand;
        #endregion
        #region properties
        public WPFShemaContainer WpfSchemaContainer
        {
            get { return wpfSchemaContainer; }
        }

        public TextBox XamlView
        {
            get;
            private set;
        }
        public MapZoom ZoomManager
        {
            get;
            protected set;
        }

        public System.Windows.Controls.Canvas MainCanvas
        {
            get { return wpfSchemaContainer.View as System.Windows.Controls.Canvas; }
            set
            {
                wpfSchemaContainer.View = value;
            }
        }
        
        public SelectionManager SelectionManager

        {
            get;
            protected set;
        }
        #endregion

        #region Initialization
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

            this.UndoBuff = new BasicUndoBuffer(this);
            this.SelectionManager = new SelectionManager(this);
            this.SelectionManager.SelectionChanged += OnObjectSelected;
            documentMenuContext = new SchemaMenuContext(contextMenu);
            CommandManager.documentMenuContext = documentMenuContext;
            this.ContextMenu = contextMenu;


            this.wpfSchemaContainer.Child.AllowDrop = true;
            this.wpfSchemaContainer.Child.DragEnter += new System.Windows.DragEventHandler(Child_DragEnter);
            this.wpfSchemaContainer.Child.Drop += new System.Windows.DragEventHandler(Child_Drop);
            //this.wpfSchemaContainer.View.ContextMenu = contextMenu;

        }
        void InitEditor()
        {
            gridManger = GridManager.GetGridManagerFor(MainCanvas);
        }


        #endregion
        #region ToolsImplementation
        //TODO: need to refator as ToolsManager

        public List<ToolDescriptor> AvailableTools
        {
            get
            {
                return toolsList;
            }
        }
        public BaseTool ActiveTool
        {
            get { return activeTool; }
        }

        public Type CurrentTool
        {
            get
            {
                if (activeTool == null)
                {
                    activeTool = (BaseTool)System.Activator.CreateInstance(defaultTool, new object[] { MainCanvas });
                    activeTool.Activate();
                    activeTool.ToolFinished += activeTool_ToolFinished;
                    activeTool.ObjectCreated += activeTool_ObjectCreated;
                    //activeTool.ObjectDeleted += activeTool_ObjectDeleted;
                    RaiseObjectSelected(new PropProxy(MainCanvas));
                    //activeTool.ObjectChanged += OnObjectChenged;
                }

                return activeTool.GetType();

            }
            set
            {
                if (activeTool != null)
                {
                    activeTool.ToolFinished -= activeTool_ToolFinished;
                    activeTool.ObjectCreated -= activeTool_ObjectCreated;
                    // activeTool.ObjectChanged -= OnObjectChenged;
                    //activeTool.ObjectDeleted -= activeTool_ObjectDeleted;
                    activeTool.Deactivate();
                    activeTool = null;
                }

                if (value != null)
                {
                    activeTool = (BaseTool)System.Activator.CreateInstance(value, new object[] { MainCanvas });
                    activeTool.ToolFinished += activeTool_ToolFinished;
                    activeTool.ObjectCreated += activeTool_ObjectCreated;
                    //activeTool.ObjectChanged += OnObjectChenged;
                    //activeTool.ObjectDeleted += activeTool_ObjectDeleted;
                    activeTool.Activate();
                    SelectionManager.UpdateManipulator();
                }
            }
        }
        private void CreateToolList()
        {
            toolsList.Clear();
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
                blankBitmap,
                typeof(ControlCreateTool<System.Windows.Controls.Button>)));
            toolsList.Add(new ToolDescriptor(StringResources.ToolToggleButton,
                StringResources.ToolControlsGroupName,
                blankBitmap,
                typeof(ControlCreateTool<System.Windows.Controls.Primitives.ToggleButton>)));
            toolsList.Add(new ToolDescriptor(StringResources.ToolProgressbar,
                StringResources.ToolControlsGroupName,
                blankBitmap,
                typeof(ControlCreateTool<System.Windows.Controls.ProgressBar>)));
            toolsList.Add(new ToolDescriptor(StringResources.ToolScrollbar,
                StringResources.ToolControlsGroupName,
                blankBitmap,
                typeof(ControlCreateTool<System.Windows.Controls.Primitives.ScrollBar>)));
            toolsList.Add(new ToolDescriptor(StringResources.ToolImageControl,
                StringResources.ToolControlsGroupName,
                blankBitmap,
                typeof(ControlCreateTool<FreeSCADA.Common.Schema.AnimatedImage>)));
            toolsList.Add(new ToolDescriptor(StringResources.ToolSlider,
                StringResources.ToolControlsGroupName,
                blankBitmap,
                typeof(ControlCreateTool<System.Windows.Controls.Slider>)));
            toolsList.Add(new ToolDescriptor(StringResources.CheckBox,
                StringResources.ToolControlsGroupName,
                blankBitmap,
                typeof(ControlCreateTool<System.Windows.Controls.CheckBox>)));
            toolsList.Add(new ToolDescriptor(StringResources.TextBox,
                StringResources.ToolControlsGroupName,
                blankBitmap,
                typeof(ControlCreateTool<System.Windows.Controls.TextBox>)));
            toolsList.Add(new ToolDescriptor(StringResources.ToolChart,
                StringResources.ToolControlsGroupName,
                blankBitmap,
                typeof(ControlCreateTool<TimeChartControl>)));

            //--- DYNAMIC GENERATION OF TOOLS ---//
            AssemblyName asmName = new AssemblyName("DynamicAssembly.SchemaView");
            AssemblyBuilder asmBuilder =
            AppDomain.CurrentDomain.DefineDynamicAssembly(
                asmName,
                AssemblyBuilderAccess.RunAndSave);

            ModuleBuilder modBuilder =
            asmBuilder.DefineDynamicModule(
                asmName.Name,
                asmName.Name + ".dll");
            foreach (IVisualControlsPlug p in Env.Current.VisualPlugins.Plugins)
            {
                foreach (IVisualControlDescriptor d in p.Controls)
                {
                    Type toolType = typeof(ControlCreateTool<>);
                    toolType = toolType.MakeGenericType(new Type[] { d.Type });

                    toolsList.Add(new ToolDescriptor(d.Name,
                        p.Name,
                        blankBitmap,
                        toolType));
                }
            }
        }

        private void CreateCommands()
        {
            DocumentCommands.Clear();

            // Commands to ToolStrip
            ICommand copyCommand = new CopyCommand(this);
            ICommand pasteCommand = new PasteCommand(this);
            ICommand cutCommand = new CutCommand(this);
            ICommand bindingCommand = new CommonBindingCommand(this);


            // Commands to ToolStrip
            DocumentCommands.Add(new CommandInfo(new NullCommand((int)CommandManager.Priorities.EditCommands)));    // Separator
            DocumentCommands.Add(new CommandInfo(undoCommand = new UndoCommand(this)));
            DocumentCommands.Add(new CommandInfo(redoCommand = new RedoCommand(this)));
            DocumentCommands.Add(new CommandInfo(new NullCommand((int)CommandManager.Priorities.EditCommands)));    // Separator
            DocumentCommands.Add(new CommandInfo(cutCommand));
            DocumentCommands.Add(new CommandInfo(copyCommand));
            DocumentCommands.Add(new CommandInfo(pasteCommand));
            DocumentCommands.Add(new CommandInfo(new NullCommand((int)CommandManager.Priorities.EditCommands)));    // Separator
            DocumentCommands.Add(new CommandInfo(new XamlViewCommand(this)));
            DocumentCommands.Add(new CommandInfo(new GroupCommand(this)));
            DocumentCommands.Add(new CommandInfo(new UngroupCommand(this)));
            DocumentCommands.Add(new CommandInfo(new NullCommand((int)CommandManager.Priorities.EditCommands)));    // Separator

            DocumentCommands.Add(new CommandInfo(new ZMoveTopCommand(this)));
            DocumentCommands.Add(new CommandInfo(new ZMoveBottomCommand(this)));
            DocumentCommands.Add(new CommandInfo(new NullCommand((int)CommandManager.Priorities.EditCommands)));    // Separator
            DocumentCommands.Add(new CommandInfo(bindingCommand));


            DocumentCommands.Add(new CommandInfo(new NullCommand((int)CommandManager.Priorities.ViewCommands), CommandManager.viewContext));    // Separator
            DocumentCommands.Add(new CommandInfo(new ZoomLevelCommand(this), CommandManager.viewContext));
            DocumentCommands.Add(new CommandInfo(new ZoomOutCommand(this), CommandManager.viewContext));
            DocumentCommands.Add(new CommandInfo(new ZoomInCommand(this), CommandManager.viewContext));

            DocumentCommands.Add(new CommandInfo(cutCommand, CommandManager.documentMenuContext));
            DocumentCommands.Add(new CommandInfo(copyCommand, CommandManager.documentMenuContext));
            DocumentCommands.Add(new CommandInfo(pasteCommand, CommandManager.documentMenuContext));
            DocumentCommands.Add(new CommandInfo(bindingCommand, CommandManager.documentMenuContext));
            DocumentCommands.Add(new CommandInfo(new ImportElementCommand(this), CommandManager.fileContext));

            DocumentCommands.Add(new CommandInfo(new NullCommand((int)CommandManager.Priorities.EditCommands)));    // Separator
            DocumentCommands.Add(new CommandInfo(new ImportElementCommand(this), CommandManager.documentContext));

        }

        #endregion


        #region DocumentBehavior

        public override string DocumentName
        {
            set
            {

                base.DocumentName = value;
            }
        }
        public override void OnActivated()
        {
            ZoomManager = new MapZoom(MainCanvas);
            CreateCommands();
            CreateToolList();
            base.OnActivated();
            //Notify connected windows about new tool collection
          
            CommandManager.documentMenuContext = documentMenuContext;
            NotifyToolsCollectionChanged(AvailableTools, CurrentTool);
            

        }

        public override void OnDeactivated()
        {
            base.OnDeactivated();
        }
        
        public override bool SaveDocument()
        {
            MainCanvas.Tag = null;
            SchemaDocument.SaveSchema(MainCanvas, DocumentName);
            IsModified = false;
            MainCanvas.Tag = this;

            return true;
        }

        public override bool LoadDocument(string name)
        {
			System.Windows.Controls.Canvas canvas = SchemaDocument.LoadSchema(name);
			if(canvas == null)
				return false;

			MainCanvas = canvas;
            DocumentName = name;
            MainCanvas.Tag = this;
            MainCanvas.Loaded += new RoutedEventHandler(MainCanvas_Loaded);

            return true;
        }

        void MainCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            InitEditor();
        }

        public override bool CreateNewDocument()
        {

            if ((MainCanvas = SchemaDocument.CreateNewSchema()) == null)
                return false;
            IsModified = true;
            MainCanvas.Tag = this;
            return true;
        }


        protected override void OnClosed(EventArgs e)
        {
            if (wpfSchemaContainer != null && MainCanvas != null)
            {

                wpfSchemaContainer.Child.KeyDown -= new System.Windows.Input.KeyEventHandler(WpfKeyDown);
            }

            CurrentTool = null;

            wpfSchemaContainer.Dispose();
            wpfSchemaContainer = null;
            base.OnClosed(e);
        }
        public override void OnToolActivated(object sender, Type tool)
        {
            CurrentTool = tool;
        }

        #endregion
        

        void OnObjectSelected(Object obj)
        {

            if (obj == null)
                obj = MainCanvas;
            foreach (IVisualControlsPlug p in Env.Current.VisualPlugins.Plugins)
            {
                foreach (IVisualControlDescriptor d in p.Controls)
                {
                    if (obj.GetType() == d.Type)
                    {
                        RaiseObjectSelected(d.getPropProxy(obj));
                        return;
                    }
                }
            }
            RaiseObjectSelected(new PropProxy(obj));

        }
        


        void Child_Drop(object sender, System.Windows.DragEventArgs e)
        {
            System.Windows.Controls.ContentControl content = new System.Windows.Controls.ContentControl();
            System.Windows.Data.Binding bind = new System.Windows.Data.Binding();

            ChannelDataProvider cdp = new ChannelDataProvider();
            //cdp.ChannelName = node.Tag + "." + node.Text;
			cdp.ChannelName = (string)e.Data.GetData(typeof(string));
            cdp.IsBindInDesign = true;
            bind.Source = cdp;

            System.Windows.Data.BindingOperations.SetBinding(content, System.Windows.Controls.ContentControl.ContentProperty, bind);
            //TODO:need to solve problem with drag mouse capturing
            // System.Windows.Point p=System.Windows.Input.Mouse.GetPosition(Schema.MainCanvas); 

            System.Windows.Controls.Canvas.SetLeft(content, 0);
            System.Windows.Controls.Canvas.SetTop(content, 0);
            content.Width = 60; content.Height = 50;
            if (MainCanvas.Resources[typeof(BaseChannel)] == null)
            {
                System.Windows.DataTemplate dt = ((System.Windows.DataTemplate)XamlReader.Load(new XmlTextReader(new StringReader(StringResources.ChannelDefaultTemplate))));
                dt.DataType = typeof(BaseChannel);
                System.Windows.DataTemplateKey dk = new System.Windows.DataTemplateKey(typeof(BaseChannel));

                MainCanvas.Resources[dk] = dt;
            }
            UndoBuff.AddCommand(new AddGraphicsObject(content));

        }

        void Child_DragEnter(object sender, System.Windows.DragEventArgs e)
        {
			if (e.Data.GetDataPresent(typeof(string)))
			{
				string channelId = (string)e.Data.GetData(typeof(string));
				if(Env.Current.CommunicationPlugins.GetChannel(channelId) != null)
					e.Effects = System.Windows.DragDropEffects.Copy;
				else
					e.Effects = System.Windows.DragDropEffects.None;
			}
			else
				e.Effects = System.Windows.DragDropEffects.None;
        }




        void activeTool_ObjectCreated(object sender, EventArgs e)
        {
            UndoBuff.AddCommand(new AddGraphicsObject(sender as System.Windows.UIElement));
            MainCanvas.UpdateLayout();
            SelectionManager.SelectObject(sender as UIElement);

            UpdateXamlView();
        }


        void activeTool_ToolFinished(object sender, EventArgs e)
        {
            NotifyToolsCollectionChanged(AvailableTools, defaultTool);
            UpdateXamlView();
        }
        void OnObjectChenged(object sender, EventArgs e)
        {
               UpdateXamlView();
        }

        void WpfKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            if (e.Key == System.Windows.Input.Key.Z &&
                (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) != System.Windows.Input.ModifierKeys.None)
            {
                UndoBuff.UndoCommand();
                //SelectionManager.SelectObject(null);

            }
            else if (e.Key == System.Windows.Input.Key.Y &&
                (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) != System.Windows.Input.ModifierKeys.None)
            {
                UndoBuff.RedoCommand();
                //SelectionManager.SelectObject(null);
            }
            else if (e.Key == System.Windows.Input.Key.X &&
                (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) != System.Windows.Input.ModifierKeys.None)
            {
                if (activeTool is SelectionTool && SelectionManager.SelectedObjects.Count > 0)
                {
                    SchemaCommand cmd = new CutCommand(this);
                    cmd.CheckApplicability();
                    cmd.Execute();
                    cmd.Dispose();
                }
            }
            else if (e.Key == System.Windows.Input.Key.C &&
                (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) != System.Windows.Input.ModifierKeys.None)
            {
                if (activeTool is SelectionTool && SelectionManager.SelectedObjects.Count > 0)
                {
                    SchemaCommand cmd = new CopyCommand(this);
                    cmd.Execute();
                    cmd.Dispose();
                }
            }
            else if (e.Key == System.Windows.Input.Key.V &&
                (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) != System.Windows.Input.ModifierKeys.None)
            {
                if (activeTool is SelectionTool)
                {
                    SchemaCommand cmd = new PasteCommand(this);
                    cmd.Execute();
                    cmd.Dispose();
                }
            }
            else if (e.Key == System.Windows.Input.Key.Delete)
            {
                if (SelectionManager.SelectedObjects.Count > 0)
                    UndoBuff.AddCommand(new DeleteGraphicsObject(SelectionManager.SelectedObjects[0]));
                else if (activeTool is SelectionTool && SelectionManager.SelectedObjects.Count > 0)
                {
                    foreach (System.Windows.UIElement el in SelectionManager.SelectedObjects)
                    {
                        UndoBuff.AddCommand(new DeleteGraphicsObject(el));
                    }
                }
                SelectionManager.SelectObject(null);
            }
            else if (e.Key == System.Windows.Input.Key.Add && (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) != System.Windows.Input.ModifierKeys.None)
            {
                DocumentCommands.First(c => c.command is ZoomInCommand).command.Execute();

            }
            else if (e.Key == System.Windows.Input.Key.Subtract && (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) != System.Windows.Input.ModifierKeys.None)
            {
                DocumentCommands.First(c => c.command is ZoomOutCommand).command.Execute();
            }

            else if (e.Key == System.Windows.Input.Key.Escape)
            {
                NotifySetCurrentTool(toolsList[0]);

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


            MainCanvas.UpdateLayout();
            activeTool.InvalidateVisual();
        }

        public void UpdateCanvasByXaml()
        {
            if (XamlView.Visible)
            {
				try
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
							MainCanvas.Children.Clear();
							while (canvas.Children.Count > 0)
							{
								UIElement el = canvas.Children[0];
								canvas.Children.Remove(canvas.Children[0]); ;
								MainCanvas.Children.Add(el);
							}
							CurrentTool = defaultTool;
						}
					}
				}
				catch(Exception e)
				{
					Env.Current.Logger.LogError(string.Format("Cannot update Canvas using entered XAML code: {0}", e.Message));
				}
            }
        }


        public void UpdateXamlView()
        {
            if (XamlView.Visible)
            {
                MainCanvas.Tag = null;
                XamlView.Text = EditorHelper.SerializeObject(MainCanvas);
                MainCanvas.Tag = this;
            }
        }

    }
}
