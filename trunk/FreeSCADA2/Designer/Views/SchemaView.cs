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
using FreeSCADA.Designer.SchemaEditor.PropertiesUtils;
using FreeSCADA.Designer.SchemaEditor.SchemaCommands;
using FreeSCADA.Designer.SchemaEditor.Tools;
using FreeSCADA.Designer.SchemaEditor.UndoRedo;
using FreeSCADA.Interfaces;
using FreeSCADA.Interfaces.Plugins;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows;

namespace FreeSCADA.Designer.Views
{
    class SchemaView : DocumentView
    {

        // TODO: make all actions by Commands, make command execution by pattern Vistor - one 

        private WPFShemaContainer wpfSchemaContainer;

        private BaseTool activeTool;
        private Type defaultTool = typeof(SelectionTool);
        private SchemaDocument schema;
        private GridManager gridManger;
        System.Windows.Forms.ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
		ICommandContext documentMenuContext;
        List<ToolDescriptor> toolsList=new List<ToolDescriptor>();
		
        SchemaCommand undoCommand, redoCommand;

        

        public TextBox XamlView
        {
            get;
            private set;
        }

        #region ZoomImplementation

        //TODO: should be implemented as separate class - ZoomManager like GridManager
        private ScaleTransform SchemaScale = new ScaleTransform();
        private System.Windows.Point SavedScrollPosition;
        
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
            msv.ScrollToVerticalOffset(msv.VerticalOffset * 1.05 + center.Y * 0.05);
            msv.ScrollToHorizontalOffset(msv.HorizontalOffset * 1.05 + center.X * 0.05);

            UpdateZoomLevel();
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

            UpdateZoomLevel();
        }

        private void UpdateZoomLevel()
        {
            foreach (CommandInfo cmdInfo in DocumentCommands)
            {
                if (cmdInfo.command is ZoomLevelCommand)
                    (cmdInfo.command as ZoomLevelCommand).Level = SchemaScale.ScaleX;
            }
        }
        #endregion 

        public SchemaDocument Schema
        {
            get { return schema; }
            set
            {
                schema = value;
                DocumentName = value.Name;
                wpfSchemaContainer.View = value.MainCanvas;
                undoBuff = UndoRedoManager.GetUndoBuffer(this);
            

             
            }
        }

        
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
                gridManger = GridManager.GetGridManagerFor(schema.MainCanvas);
                if (activeTool == null)
                {
                    activeTool = (BaseTool)System.Activator.CreateInstance(defaultTool, new object[] { Schema.MainCanvas });
                    activeTool.Activate();
                    activeTool.ObjectSelected += activeTool_ObjectSelected;
                    activeTool.ToolFinished += activeTool_ToolFinished;
                    activeTool.ObjectCreated += activeTool_ObjectCreated;
                    activeTool.ObjectDeleted += activeTool_ObjectDeleted;
                   // activeTool.ObjectChanged += OnObjectChenged;
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
                   // activeTool.ObjectChanged -= OnObjectChenged;
                    activeTool.ObjectDeleted -= activeTool_ObjectDeleted;
                    activeTool.Deactivate();
                    activeTool = null;
                }

                if (value != null)
                {
                    activeTool = (BaseTool)System.Activator.CreateInstance(value, new object[] { Schema.MainCanvas });
                    activeTool.ObjectSelected += activeTool_ObjectSelected;
                    activeTool.ToolFinished += activeTool_ToolFinished;
                    activeTool.ObjectCreated += activeTool_ObjectCreated;
                    //activeTool.ObjectChanged += OnObjectChenged;
                    activeTool.ObjectDeleted += activeTool_ObjectDeleted;
                    activeTool.Activate();
                }
            }
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
                blankBitmap,
                 typeof(ControlCreateTool<System.Windows.Controls.Button>)));
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
                    Type baseType = typeof(UserControlCreateTool);
                    TypeBuilder typeBuilder = modBuilder.DefineType(
                                "UserControlCreateTool_" + d.Name,
                                TypeAttributes.NotPublic | TypeAttributes.Sealed | TypeAttributes.Class,
                                baseType);
                    // Constructor 1 PARAMETERLESS
                    ConstructorBuilder myCtor1 = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[0]);
                    ILGenerator ctorIL1 = myCtor1.GetILGenerator();
                    ConstructorInfo baseCtor1 = baseType.GetConstructor(new Type[0]);
                    ctorIL1.Emit(OpCodes.Ldarg_0);
                    ctorIL1.Emit(OpCodes.Call, baseCtor1);
                    ctorIL1.Emit(OpCodes.Ret);
                    // Constructor 2 with 1 parameter
                    ConstructorBuilder myCtor2 = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeof(UIElement) });
                    ILGenerator ctorIL2 = myCtor2.GetILGenerator();
                    ConstructorInfo baseCtor2 = baseType.GetConstructor(new Type[] { typeof(UIElement) });
                    ctorIL2.Emit(OpCodes.Ldarg_0);
                    ctorIL2.Emit(OpCodes.Ldarg_1);
                    ctorIL2.Emit(OpCodes.Call, baseCtor2);
                    ctorIL2.Emit(OpCodes.Ret);

                    Type myType = typeBuilder.CreateType();
                    MethodInfo mi = myType.GetMethod("setControlType");
                    Object obj = Activator.CreateInstance(myType);
                    Object[] par = { d.Type };
                    mi.Invoke(obj, par);    // Trick with static base type field
                    toolsList.Add(new ToolDescriptor(d.Name,
                        p.Name,
                        blankBitmap,
                        myType));
                }
            }
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
            this.wpfSchemaContainer.ZoomInEvent += new WPFShemaContainer.ZoomDelegate(ZoomIn);
            this.wpfSchemaContainer.ZoomOutEvent += new WPFShemaContainer.ZoomDelegate(ZoomOut);
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

            documentMenuContext = new SchemaMenuContext(contextMenu);
            CommandManager.documentMenuContext = documentMenuContext;
            this.ContextMenu = contextMenu;


			// Commands
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

            CreateToolList();


            this.wpfSchemaContainer.Child.AllowDrop = true;
            this.wpfSchemaContainer.Child.DragEnter += new System.Windows.DragEventHandler(Child_DragEnter);
            this.wpfSchemaContainer.Child.Drop += new System.Windows.DragEventHandler(Child_Drop);
            //this.wpfSchemaContainer.View.ContextMenu = contextMenu;
        }
        #endregion 

        #region DocumentBehavior 
        


        public override void OnActivated()
        {
            base.OnActivated();

            CommandManager.documentMenuContext = documentMenuContext;

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
            return true;
        }

        public override bool CreateNewDocument()
        {
            SchemaDocument schema;

            if ((schema = SchemaDocument.CreateNewSchema()) == null)
                return false;
            
            Schema = schema;
            IsModified = true;
            
         /*   System.Windows.Shapes.Rectangle r = new System.Windows.Shapes.Rectangle();
            r.Width = 100;
            r.Height = 100;
            r.Fill = System.Windows.Media.Brushes.AliceBlue;
            System.Windows.Input.MouseBinding mb=new System.Windows.Input.MouseBinding();
            mb.Command=new FreeSCADA.Common.Schema.Commands.SchemaOpenCommand();
            mb.CommandParameter="test";
            r.InputBindings.Add(mb);
            mb.MouseAction = System.Windows.Input.MouseAction.LeftClick;
            Schema.MainCanvas.Children.Add(r);*/
            return true;
        }

        
        protected override void OnClosed(EventArgs e)
        {
            if (wpfSchemaContainer != null && Schema != null)
            {
             
                wpfSchemaContainer.Child.KeyDown -= new System.Windows.Input.KeyEventHandler(WpfKeyDown);
                UndoRedoManager.ReleaseUndoBuffer(this);
            }

            CurrentTool = null;

            wpfSchemaContainer.Dispose();
            wpfSchemaContainer = null;
            base.OnClosed(e);
        }
        void activeTool_ObjectSelected(Object obj)
        {

            if (obj == null)
                obj = Schema.MainCanvas;
            RaiseObjectSelected(new PropProxy(obj));

        }



        public override void OnToolActivated(object sender, Type tool)
        {
            CurrentTool = tool;
        }

        #endregion

        void Child_Drop(object sender, System.Windows.DragEventArgs e)
        {
            TreeNode node = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");

            System.Windows.Controls.ContentControl content = new System.Windows.Controls.ContentControl();
            System.Windows.Data.Binding bind = new System.Windows.Data.Binding();

            ChannelDataProvider cdp = new ChannelDataProvider();
            cdp.ChannelName = node.Tag + "." + node.Text;
            bind.Source = cdp;
           
            bind.Source = cdp;

            System.Windows.Data.BindingOperations.SetBinding(content, System.Windows.Controls.ContentControl.ContentProperty, bind);
            //TODO:need to solve problem with drag mouse capturing
            // System.Windows.Point p=System.Windows.Input.Mouse.GetPosition(Schema.MainCanvas); 

            System.Windows.Controls.Canvas.SetLeft(content, 0);
            System.Windows.Controls.Canvas.SetTop(content, 0);
            content.Width = 60; content.Height = 50;
            if (Schema.MainCanvas.Resources[typeof(BaseChannel)] == null)
            {
                System.Windows.DataTemplate dt = ((System.Windows.DataTemplate)XamlReader.Load(new XmlTextReader(new StringReader(StringResources.ChannelDefaultTemplate))));
                dt.DataType = typeof(BaseChannel);
                System.Windows.DataTemplateKey dk = new System.Windows.DataTemplateKey(typeof(BaseChannel));

                Schema.MainCanvas.Resources[dk] = dt;
            }
            undoBuff.AddCommand(new AddGraphicsObject(content));

        }

        void Child_DragEnter(object sender, System.Windows.DragEventArgs e)
        {
            TreeNode node = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
            if (node != null)
            {
                e.Effects = System.Windows.DragDropEffects.Copy;

            }
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
            //undoBuff.AddCommand(new ModifyGraphicsObject((System.Windows.UIElement)sender));
            UpdateXamlView();
        }

        void WpfKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            
            if (e.Key == System.Windows.Input.Key.Z &&
                (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) != System.Windows.Input.ModifierKeys.None)
            {
                undoBuff.UndoCommand();
                activeTool.SelectedObject = null;

            }
            else if (e.Key == System.Windows.Input.Key.Y &&
                (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) != System.Windows.Input.ModifierKeys.None)
            {
                undoBuff.RedoCommand();
                activeTool.SelectedObject = null;
            }
            else if (e.Key == System.Windows.Input.Key.X &&
                (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) != System.Windows.Input.ModifierKeys.None)
            {
                if (activeTool is SelectionTool && activeTool.SelectedObject != null)
                {
                    SchemaCommand cmd = new CutCommand(this);
                    cmd.Execute();
                    cmd.Dispose();
                }
            }
            else if (e.Key == System.Windows.Input.Key.C &&
                (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) != System.Windows.Input.ModifierKeys.None)
            {
                if (activeTool is SelectionTool && activeTool.SelectedObject != null)
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
                if (activeTool.SelectedObject != null)
                    undoBuff.AddCommand(new DeleteGraphicsObject(activeTool.SelectedObject));
                else if (activeTool is SelectionTool && (activeTool as SelectionTool).SelectedObjects.Count > 0)
                {
                    foreach (System.Windows.UIElement el in (activeTool as SelectionTool).SelectedObjects)
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


            Schema.MainCanvas.UpdateLayout();
            activeTool.InvalidateVisual();
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
                    }
                }
            }
        }

        
        public void UpdateXamlView()
        {
            if (XamlView.Visible)
            {

                XamlView.Text = EditorHelper.SerializeObject(Schema.MainCanvas);
            }
        }

    }
}
