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
using FreeSCADA.CommonUI.Interfaces;
using FreeSCADA.CommonUI;
using FreeSCADA.CommonUI.GlobalCommands;

namespace FreeSCADA.Designer.Views
{
    sealed class SchemaView : DocumentView
    {

        // TODO: make all actions by Commands, make command execution by pattern Vistor - one 
        #region fields
        WPFShemaContainer _wpfSchemaContainer;


        ITool _activeTool;
        Type _defaultTool = typeof(SelectionTool);
        //temp solution
        public GridManager GridManager
        {
            get;
            set;
        }


        ContextMenuStrip contextMenu = new ContextMenuStrip();
        ICommandContext _documentMenuContext;

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



        public override  ITool ActiveTool
        {
            get
            {
                if (_activeTool == null)

                    ActiveTool = new SelectionTool(this);

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
                }
            }
        }
        #endregion

        #region Initialization
        public SchemaView(IDocument doc)
            : base(doc)
        {
            Document = doc;

            this._wpfSchemaContainer = new WPFShemaContainer();
            IsModified = true;
            MainPanel =Document.Content as System.Windows.Controls.Panel;
            
            InitializeComponent();
           
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
            _documentMenuContext = new MenuCommandContext(contextMenu);
            this.ContextMenuStrip = contextMenu;
            //CommandManager.documentMenuContext = _documentMenuContext;////????????????????????????????


            this._wpfSchemaContainer.Child.AllowDrop = true;
            this._wpfSchemaContainer.Child.DragEnter += new System.Windows.DragEventHandler(Child_DragEnter);
            this._wpfSchemaContainer.Child.Drop += new System.Windows.DragEventHandler(Child_Drop);
            
            ZoomManager = new MapZoom(MainPanel);
            CreateCommands();
            MainPanel.Loaded += new RoutedEventHandler(MainCanvas_Loaded);
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
            DocumentCommands.Add(new CommandInfo(new NullCommand((int)CommandManager.Priorities.EditCommands),
                new string[]{ "DocumentContext",PredefinedContexts.GlobalToolbar}));    // Separator
            DocumentCommands.Add(new CommandInfo(_undoCommand = new UndoCommand(this),
                new string[] { "DocumentContext", PredefinedContexts.GlobalToolbar }));
            DocumentCommands.Add(new CommandInfo(_redoCommand = new RedoCommand(this),
                new string[] { "DocumentContext", PredefinedContexts.GlobalToolbar }));
            DocumentCommands.Add(new CommandInfo(new NullCommand((int)CommandManager.Priorities.EditCommands),
                new string[] { "DocumentContext", PredefinedContexts.GlobalToolbar }));    // Separator
            DocumentCommands.Add(new CommandInfo(cutCommand, 
                new string[] { "DocumentContext", PredefinedContexts.GlobalToolbar }));
            DocumentCommands.Add(new CommandInfo(copyCommand, 
                new string[] { "DocumentContext", PredefinedContexts.GlobalToolbar }));
            DocumentCommands.Add(new CommandInfo(pasteCommand, 
                new string[] { "DocumentContext", PredefinedContexts.GlobalToolbar }));
            DocumentCommands.Add(new CommandInfo(new NullCommand((int)CommandManager.Priorities.EditCommands),
                new string[] { "DocumentContext", PredefinedContexts.GlobalToolbar }));    // Separator
            DocumentCommands.Add(new CommandInfo(new XamlViewCommand(this),
                new string[] { "DocumentContext", PredefinedContexts.GlobalToolbar }));
            DocumentCommands.Add(new CommandInfo(new GroupCommand(this),
                new string[] { "DocumentContext", PredefinedContexts.GlobalToolbar }));
            DocumentCommands.Add(new CommandInfo(new UngroupCommand(this),
                new string[] { "DocumentContext", PredefinedContexts.GlobalToolbar }));
            DocumentCommands.Add(new CommandInfo(new NullCommand((int)CommandManager.Priorities.EditCommands),
                new string[] { "DocumentContext", PredefinedContexts.GlobalToolbar }));    // Separator

            DocumentCommands.Add(new CommandInfo(new ZMoveTopCommand(this),
                new string[] { "DocumentContext", PredefinedContexts.GlobalToolbar }));
            DocumentCommands.Add(new CommandInfo(new ZMoveBottomCommand(this),
                new string[] { "DocumentContext", PredefinedContexts.GlobalToolbar }));
            DocumentCommands.Add(new CommandInfo(new NullCommand((int)CommandManager.Priorities.EditCommands),
                 new string[] { "DocumentContext", PredefinedContexts.GlobalToolbar }));    // Separator
            DocumentCommands.Add(new CommandInfo(bindingCommand,
                new string[] { "DocumentContext", PredefinedContexts.GlobalToolbar }));


            DocumentCommands.Add(new CommandInfo(new NullCommand((int)CommandManager.Priorities.ViewCommands), 
                new string[] { "ViewContext"}));    
            DocumentCommands.Add(new CommandInfo(new ZoomLevelCommand(this),
                new string[] { "ViewContext" }));
            DocumentCommands.Add(new CommandInfo(new ZoomOutCommand(this),
                new string[] { "ViewContext" }));
            DocumentCommands.Add(new CommandInfo(new ZoomInCommand(this),
                new string[] { "ViewContext" }));

            /*DocumentCommands.Add(new CommandInfo(cutCommand, _documentMenuContext));
            DocumentCommands.Add(new CommandInfo(copyCommand, _documentMenuContext));
            DocumentCommands.Add(new CommandInfo(pasteCommand, _documentMenuContext));
            DocumentCommands.Add(new CommandInfo(bindingCommand, _documentMenuContext));
             * */
            DocumentCommands.Add(new CommandInfo(new ImportElementCommand(this),
                new string[] { "FileContext" }));
            
            
            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolSelection,
                StringResources.ToolEditorGroupName,
                global::FreeSCADA.Designer.Resources.cursor,
                typeof(SelectionTool)),
                new string[] { "ToolboxContext" }));

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolRectangle,
                    StringResources.ToolGrphicsGroupName,
                    global::FreeSCADA.Designer.Resources.shape_square_add,
                    typeof(RectangleTool)),
                    new string[] { "ViewContext" }));

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolEllipse,
                    StringResources.ToolGrphicsGroupName,
                    global::FreeSCADA.Designer.Resources.shape_ellipse_add,
                    typeof(EllipseTool)),
                    new string[] { "ToolboxContext" }));

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolTextbox,
                    StringResources.ToolGrphicsGroupName,
                    global::FreeSCADA.Designer.Resources.textfield_add,
                    typeof(TextBoxTool)),
                    new string[] { "ToolboxContext" }));

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolPolyline,
                    StringResources.ToolGrphicsGroupName,
                    global::FreeSCADA.Designer.Resources.shape_line_add,
                    typeof(PolylineTool)),
                    new string[] { "ToolboxContext" }));
            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolPolygon,
                    StringResources.ToolGrphicsGroupName,
                    global::FreeSCADA.Designer.Resources.shape_line_add,
                    typeof(PolygonTool)),
                    new string[] { "ToolboxContext" }));

            /*DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolActionEdit,
                    StringResources.ToolEditorGroupName,
                    global::FreeSCADA.Designer.Resources.cog_edit,
                    typeof(ActionEditTool)),
                    CommandManager.toolboxContext));*/

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolButton,
                    StringResources.ToolControlsGroupName,
                    blankBitmap,
                    typeof(ControlCreateTool<System.Windows.Controls.Button>)),
                    new string[] { "ToolboxContext" }));

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolToggleButton,
                    StringResources.ToolControlsGroupName,
                    blankBitmap,
                    typeof(ControlCreateTool<System.Windows.Controls.Primitives.ToggleButton>)),
                    new string[] { "ToolboxContext" }));

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolProgressbar,
                    StringResources.ToolControlsGroupName,
                    blankBitmap,
                    typeof(ControlCreateTool<System.Windows.Controls.ProgressBar>)),
                    new string[] { "ToolboxContext" }));

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolScrollbar,
                    StringResources.ToolControlsGroupName,
                    blankBitmap,
                    typeof(ControlCreateTool<System.Windows.Controls.Primitives.ScrollBar>)),
                    new string[] { "ToolboxContext" }));

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolImageControl,
                    StringResources.ToolControlsGroupName,
                    blankBitmap,
                    typeof(ControlCreateTool<FreeSCADA.Common.Schema.AnimatedImage>)),
                    new string[] { "ToolboxContext" }));

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolSlider,
                    StringResources.ToolControlsGroupName,
                    blankBitmap,
                    typeof(ControlCreateTool<System.Windows.Controls.Slider>)),
                    new string[] { "ToolboxContext"}));

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.CheckBox,
                    StringResources.ToolControlsGroupName,
                    blankBitmap,
                    typeof(ControlCreateTool<System.Windows.Controls.CheckBox>)),
                    new string[] { "ToolboxContext" }));

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.TextBox,
                    StringResources.ToolControlsGroupName,
                    blankBitmap,
                    typeof(ControlCreateTool<System.Windows.Controls.TextBox>)),
                    new string[] { "ToolboxContext" }));

            DocumentCommands.Add(new CommandInfo(
                new ToolCommand(this,
                    StringResources.ToolChart,
                    StringResources.ToolControlsGroupName,
                    blankBitmap,
                    typeof(ControlCreateTool<TimeChartControl>)),
                    new string[] { "ToolboxContext" }));


        }

        void ReInitEditor()
        {
            GridManager = new GridManager(MainPanel);
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
            Document.Save(DocumentName,MainPanel);
            IsModified = false;
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
            UndoBuff.AddCommand(new AddGraphicsObject(sender as System.Windows.FrameworkElement));
            MainPanel.UpdateLayout();
            SelectionManager.SelectObject(sender as FrameworkElement);

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
                    UndoBuff.AddCommand(new DeleteGraphicsObject(SelectionManager.SelectedObjects.Cast<FrameworkElement>().FirstOrDefault()));
                else if (_activeTool is SelectionTool && SelectionManager.SelectedObjects.Count > 0)
                {
                    foreach (var el in SelectionManager.SelectedObjects.Cast<FrameworkElement>())
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
            XamlView.Text = EditorHelper.SerializeObject(MainPanel);
            
        }

    }
}
