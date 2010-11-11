using System;
using System.Linq;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Xml;
using FreeSCADA.Common;
using FreeSCADA.Common.Schema;
using FreeSCADA.Common.Gestures;
using FreeSCADA.Designer.SchemaEditor.Tools;
using FreeSCADA.Designer.SchemaEditor;
using FreeSCADA.Designer.SchemaEditor.PropertiesUtils;
using FreeSCADA.Designer.SchemaEditor.SchemaCommands;

using FreeSCADA.Interfaces;


namespace FreeSCADA.Designer.Views
{
    sealed class SchemaView : DocumentView
    {

        // TODO: make all actions by Commands, make command execution by pattern Vistor - one 
        #region fields
        WPFShemaContainer _wpfSchemaContainer;


        BaseTool _activeTool;
        Type _defaultTool = typeof(SelectionTool);

        GridManager _gridManger;

        ContextMenu contextMenu = new ContextMenu();

        ICommandContext _documentMenuContext;
        ICommandContext _toolboxContext;
        SchemaCommand _undoCommand, _redoCommand;
        #endregion

        #region properties
        public TextBox XamlView
        {
            get;
            private set;
        }

        
        public override  System.Windows.Controls.Panel MainPanel
        {
            get { return _wpfSchemaContainer.View as System.Windows.Controls.Panel; }
            protected set
            {
                _wpfSchemaContainer.View = value;
            }
        }



        public override  BaseTool ActiveTool
        {
            get
            {
                if (_activeTool == null)

                    ActiveTool = new SelectionTool(MainPanel,SelectionManager);

                return _activeTool;

            }
            set
            {
                if (_activeTool != null)
                {
                    _activeTool.ToolFinished -= ActiveToolToolFinished;
                    _activeTool.ObjectCreated -= ActiveToolObjectCreated;
                    _activeTool.Deactivate();
                    _activeTool = null;
                }

                if (value != null)
                {
                    _activeTool = value;
                    _activeTool.ToolFinished += ActiveToolToolFinished;
                    _activeTool.ObjectCreated += ActiveToolObjectCreated;
                    _activeTool.Activate();
                    SelectionManager.UpdateManipulator();
                }
            }
        }
        #endregion

        #region Initialization
        public SchemaView(string docName)
            : base(docName)
        {
            this._wpfSchemaContainer = new WPFShemaContainer();
            var canvas = SchemaDocument.LoadSchema(DocumentName);
            if (canvas == null)
            {
                canvas = SchemaDocument.CreateNewSchema();
                IsModified = true;
            }
            if (canvas == null)
                throw new Exception("can not create new schema");

            MainPanel = canvas;
            MainPanel.Tag = this;

            InitializeComponent();
            MainPanel.Loaded += new RoutedEventHandler(MainCanvas_Loaded);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // wpfContainerHost
            // 
            this._wpfSchemaContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._wpfSchemaContainer.Location = new System.Drawing.Point(0, 0);
            this._wpfSchemaContainer.Name = "WPFSchemaContainer";
            this._wpfSchemaContainer.Size = new System.Drawing.Size(292, 273);
            this._wpfSchemaContainer.TabIndex = 0;
            this._wpfSchemaContainer.Text = "WPFSchemaContainer";
            this._wpfSchemaContainer.Child.KeyDown += new System.Windows.Input.KeyEventHandler(WpfKeyDown);
            this.XamlView = new TextBox
                                {
                                    Dock = System.Windows.Forms.DockStyle.Bottom,
                                    Multiline = true,
                                    ScrollBars = ScrollBars.Both,
                                    Size = new System.Drawing.Size(200, 200)
                                };
            this.XamlView.Hide();
            XamlView.MaxLength = int.MaxValue;
            // 
            // SchemaView
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this._wpfSchemaContainer);
            this.Controls.Add(XamlView);
            this.Name = "SchemaView";

            this.ResumeLayout(false);

            this.UndoBuff = new BaseUndoBuffer(this);
            this.SelectionManager = new SchemaSelectionManager(this);
            _documentMenuContext = new SchemaMenuContext(contextMenu);
            this.ContextMenu = contextMenu;
            CommandManager.documentMenuContext = _documentMenuContext;


            this._wpfSchemaContainer.Child.AllowDrop = true;
            this._wpfSchemaContainer.Child.DragEnter += new System.Windows.DragEventHandler(Child_DragEnter);
            this._wpfSchemaContainer.Child.Drop += new System.Windows.DragEventHandler(Child_Drop);
            //this._wpfSchemaContainer.View.ContextMenu = contextMenu;
            ZoomManager = new MapZoom(MainPanel);
            CreateCommands();

        }
        private void CreateToolList()
        {

            //--- DYNAMIC GENERATION OF TOOLS ---//
            //AssemblyName asmName = new AssemblyName("DynamicAssembly.SchemaView");
            //AssemblyBuilder asmBuilder =
            //AppDomain.CurrentDomain.DefineDynamicAssembly(
            //    asmName,
            //    AssemblyBuilderAccess.RunAndSave);

            //ModuleBuilder modBuilder =
            //asmBuilder.DefineDynamicModule(
            //    asmName.Name,
            //    asmName.Name + ".dll");
            //foreach (IVisualControlsPlug p in Env.Current.VisualPlugins.Plugins)
            //{
            //    foreach (IVisualControlDescriptor d in p.Controls)
            //    {
            //        Type toolType = typeof(ControlCreateTool<>);
            //        toolType = toolType.MakeGenericType(new Type[] { d.Type });

            //        toolsList.Add(new ToolDescriptor(d.Name,
            //            p.Name,
            //            blankBitmap,
            //            toolType));
            //    }
            //}
        }

        private void CreateCommands()
        {
            DocumentCommands.Clear();

            // Commands to ToolStrip
            ICommand copyCommand = new CopyCommand(this);
            ICommand pasteCommand = new PasteCommand(this);
            ICommand cutCommand = new CutCommand(this);
            ICommand bindingCommand = new CommonBindingCommand(this);

            var blankBitmap = new System.Drawing.Bitmap(10, 10);

            // Commands to ToolStrip
            DocumentCommands.Add(new CommandInfo(new NullCommand((int)CommandManager.Priorities.EditCommands)));    // Separator
            DocumentCommands.Add(new CommandInfo(_undoCommand = new UndoCommand(this)));
            DocumentCommands.Add(new CommandInfo(_redoCommand = new RedoCommand(this)));
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

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolSelection,
                StringResources.ToolEditorGroupName,
                global::FreeSCADA.Designer.Properties.Resources.cursor,
                typeof(SelectionTool)),
                CommandManager.toolboxContext));

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolRectangle,
                    StringResources.ToolGrphicsGroupName,
                    global::FreeSCADA.Designer.Properties.Resources.shape_square_add,
                    typeof(RectangleTool)),
                    CommandManager.toolboxContext));

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolEllipse,
                    StringResources.ToolGrphicsGroupName,
                    global::FreeSCADA.Designer.Properties.Resources.shape_ellipse_add,
                    typeof(EllipseTool)),
                    CommandManager.toolboxContext));

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolTextbox,
                    StringResources.ToolGrphicsGroupName,
                    global::FreeSCADA.Designer.Properties.Resources.textfield_add,
                    typeof(TextBoxTool)),
                    CommandManager.toolboxContext));

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolPolyline,
                    StringResources.ToolGrphicsGroupName,
                    global::FreeSCADA.Designer.Properties.Resources.shape_line_add,
                    typeof(PolylineTool)),
                    CommandManager.toolboxContext));

            /*DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolActionEdit,
                    StringResources.ToolEditorGroupName,
                    global::FreeSCADA.Designer.Properties.Resources.cog_edit,
                    typeof(ActionEditTool)),
                    CommandManager.toolboxContext));*/

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolButton,
                    StringResources.ToolControlsGroupName,
                    blankBitmap,
                    typeof(ControlCreateTool<System.Windows.Controls.Button>)),
                    CommandManager.toolboxContext));

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolToggleButton,
                    StringResources.ToolControlsGroupName,
                    blankBitmap,
                    typeof(ControlCreateTool<System.Windows.Controls.Primitives.ToggleButton>)),
                    CommandManager.toolboxContext));

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolProgressbar,
                    StringResources.ToolControlsGroupName,
                    blankBitmap,
                    typeof(ControlCreateTool<System.Windows.Controls.ProgressBar>)),
                    CommandManager.toolboxContext));

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolScrollbar,
                    StringResources.ToolControlsGroupName,
                    blankBitmap,
                    typeof(ControlCreateTool<System.Windows.Controls.Primitives.ScrollBar>)),
                    CommandManager.toolboxContext));

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolImageControl,
                    StringResources.ToolControlsGroupName,
                    blankBitmap,
                    typeof(ControlCreateTool<FreeSCADA.Common.Schema.AnimatedImage>)),
                    CommandManager.toolboxContext));

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolSlider,
                    StringResources.ToolControlsGroupName,
                    blankBitmap,
                    typeof(ControlCreateTool<System.Windows.Controls.Slider>)),
                    CommandManager.toolboxContext));

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.CheckBox,
                    StringResources.ToolControlsGroupName,
                    blankBitmap,
                    typeof(ControlCreateTool<System.Windows.Controls.CheckBox>)),
                    CommandManager.toolboxContext));

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.TextBox,
                    StringResources.ToolControlsGroupName,
                    blankBitmap,
                    typeof(ControlCreateTool<System.Windows.Controls.TextBox>)),
                    CommandManager.toolboxContext));

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolChart,
                    StringResources.ToolControlsGroupName,
                    blankBitmap,
                    typeof(ControlCreateTool<TimeChartControl>)),
                    CommandManager.toolboxContext));


        }

        void ReInitEditor()
        {
            _gridManger = GridManager.GetGridManagerFor(MainPanel);
        }


        void MainCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            ReInitEditor();
        }

        #endregion


        #region DocumentBehavior
        public override void OnActivated()
        {
            base.OnActivated();
        }

        public override bool SaveDocument()
        {
            MainPanel.Tag = null;
            SchemaDocument.SaveSchema(MainPanel, DocumentName);
            IsModified = false;
            MainPanel.Tag = this;

            return true;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _wpfSchemaContainer.Child.KeyDown -= new System.Windows.Input.KeyEventHandler(WpfKeyDown);
            _wpfSchemaContainer.Dispose();
            _wpfSchemaContainer = null;
         }


        #endregion




        void Child_Drop(object sender, System.Windows.DragEventArgs e)
        {
            var content = new System.Windows.Controls.ContentControl();
            var bind = new System.Windows.Data.Binding();

            var cdp = new ChannelDataProvider
                          {
                              ChannelName = (string)e.Data.GetData(typeof(string)),
                              IsBindInDesign = true
                          };
            //cdp.ChannelName = node.Tag + "." + node.Text;
            bind.Source = cdp;

            System.Windows.Data.BindingOperations.SetBinding(content, System.Windows.Controls.ContentControl.ContentProperty, bind);
            //TODO:need to solve problem with drag mouse capturing
            // System.Windows.Point p=System.Windows.Input.Mouse.GetPosition(Schema.MainCanvas); 

            System.Windows.Controls.Canvas.SetLeft(content, 0);
            System.Windows.Controls.Canvas.SetTop(content, 0);
            content.Width = 60; content.Height = 50;
            if (MainPanel.Resources[typeof(BaseChannel)] == null)
            {
                var dt = ((DataTemplate)XamlReader.Load(new XmlTextReader(new StringReader(StringResources.ChannelDefaultTemplate))));
                dt.DataType = typeof(BaseChannel);
                var dk = new DataTemplateKey(typeof(BaseChannel));

                MainPanel.Resources[dk] = dt;
            }
            UndoBuff.AddCommand(new AddGraphicsObject(content));

        }

        void Child_DragEnter(object sender, System.Windows.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(string)))
            {
                var channelId = (string)e.Data.GetData(typeof(string));
                e.Effects = Env.Current.CommunicationPlugins.GetChannel(channelId) != null ?
                    System.Windows.DragDropEffects.Copy :
                    System.Windows.DragDropEffects.None;
            }
            else
                e.Effects = System.Windows.DragDropEffects.None;
        }




        void ActiveToolObjectCreated(object sender, EventArgs e)
        {
            UndoBuff.AddCommand(new AddGraphicsObject(sender as System.Windows.UIElement));
            MainPanel.UpdateLayout();
            SelectionManager.SelectObject(sender as UIElement);

            UpdateXamlView();
        }


        void ActiveToolToolFinished(object sender, EventArgs e)
        {

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
                if (_activeTool is SelectionTool && SelectionManager.SelectedObjects.Count > 0)
                {
                    var cmd = new CutCommand(this);
                    cmd.CheckApplicability();
                    cmd.Execute();
                    cmd.Dispose();
                }
            }
            else if (e.Key == System.Windows.Input.Key.C &&
                (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) != System.Windows.Input.ModifierKeys.None)
            {
                if (_activeTool is SelectionTool && SelectionManager.SelectedObjects.Count > 0)
                {
                    var cmd = new CopyCommand(this);
                    cmd.Execute();
                    cmd.Dispose();
                }
            }
            else if (e.Key == System.Windows.Input.Key.V &&
                (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) != System.Windows.Input.ModifierKeys.None)
            {
                if (_activeTool is SelectionTool)
                {
                    var cmd = new PasteCommand(this);
                    cmd.Execute();
                    cmd.Dispose();
                }
            }
            else if (e.Key == System.Windows.Input.Key.Delete)
            {
                if (SelectionManager.SelectedObjects.Count > 0)
                    UndoBuff.AddCommand(new DeleteGraphicsObject(SelectionManager.SelectedObjects.Cast<UIElement>().FirstOrDefault()));
                else if (_activeTool is SelectionTool && SelectionManager.SelectedObjects.Count > 0)
                {
                    foreach (var el in SelectionManager.SelectedObjects.Cast<UIElement>())
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
                //NotifySetCurrentTool(toolsList[0]);

            }
            else if (e.Key == System.Windows.Input.Key.F5)
            {
                UpdateCanvasByXaml();

            }
            else if (_activeTool is SelectionTool)
            {
                if (e.Key == System.Windows.Input.Key.Left)
                {
                    (_activeTool as SelectionTool).MoveHelper(-1, 0);
                }
                if (e.Key == System.Windows.Input.Key.Right)
                {
                    (_activeTool as SelectionTool).MoveHelper(1, 0);
                }
                if (e.Key == System.Windows.Input.Key.Up)
                {
                    (_activeTool as SelectionTool).MoveHelper(0, -1);
                }
                if (e.Key == System.Windows.Input.Key.Down)
                {
                    (_activeTool as SelectionTool).MoveHelper(0, 1);
                }
            }


            MainPanel.UpdateLayout();
            _activeTool.InvalidateVisual();
        }

        private void UpdateCanvasByXaml()
        {
            if (!XamlView.Visible) return;
            try
            {
                using (var stream = new MemoryStream(this.XamlView.Text.Length))
                {
                    using (var sw = new StreamWriter(stream))
                    {
                        sw.Write(this.XamlView.Text);
                        sw.Flush();
                        stream.Seek(0, SeekOrigin.Begin);
                        var canvas = XamlReader.Load(stream) as System.Windows.Controls.Canvas;

                        MainPanel.Children.Clear();
                        if (canvas != null)
                            while (canvas.Children.Count > 0)
                            {
                                var el = canvas.Children[0];
                                canvas.Children.Remove(canvas.Children[0]); ;
                                MainPanel.Children.Add(el);
                            }
                        ReInitEditor();
                    }
                }
            }
            catch (Exception e)
            {
                Env.Current.Logger.LogError(string.Format("Cannot update Canvas using entered XAML code: {0}", e.Message));
            }
        }



        public void UpdateXamlView()
        {
            if (!XamlView.Visible) return;
            MainPanel.Tag = null;
            XamlView.Text = EditorHelper.SerializeObject(MainPanel);
            MainPanel.Tag = this;
        }

    }
}
