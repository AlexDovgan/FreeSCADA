using System;
using System.Windows.Controls;
using System.Windows.Media;
using FreeSCADA.Common;
using FreeSCADA.Common.Schema;
using FreeSCADA.RunTime.DocumentCommands;
using FreeSCADA.Common.Schema.Gestures;

namespace FreeSCADA.RunTime.Views
{
	class SchemaView : DocumentView
	{
		private WPFShemaContainer wpfSchemaContainer;
        private ScaleTransform SchemaScale = new ScaleTransform();
        private Canvas mainCanvas;
        VirtualCanvas vCanvas;

        public MapZoom ZoomGesture
        {
            get;
            protected set;
        }

        public Pan PanGesture
        {
            get;
            protected set;
        }
        public RectangleSelectionGesture RectZoomGesture
        {
            get;
            protected set;
        }
        public AutoScroll AutoScrollGesture
        {
            get;
            protected set;
        }
        
        
    
        public WPFShemaContainer WpfSchemaContainer
        {
            get {return wpfSchemaContainer;}
        }
		
        public SchemaView()
		{
			InitializeComponent();

			DocumentCommands.Add(new CommandInfo(new NullCommand((int)CommandManager.Priorities.ViewCommands), CommandManager.viewContext));    // Separator
			DocumentCommands.Add(new CommandInfo(new ZoomLevelCommand(), CommandManager.viewContext));
			DocumentCommands.Add(new CommandInfo(new ZoomOutCommand(), CommandManager.viewContext));
			DocumentCommands.Add(new CommandInfo(new ZoomInCommand(), CommandManager.viewContext));
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
			// SchemaView
			// 
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.Add(this.wpfSchemaContainer);
        
			this.Name = "SchemaView";

			this.ResumeLayout(false);

            //this.SavedScrollPosition = new System.Windows.Point(0.0, 0.0);
        }


		public System.Windows.Controls.Canvas MainCanvas
		{
            get { return vCanvas;}
			set
			{
                mainCanvas = value;
                vCanvas = new VirtualCanvas(mainCanvas);
                
                if (wpfSchemaContainer.View == null)
                {
                    wpfSchemaContainer.View = vCanvas;
              
                }
                else
                    throw new Exception("View has already attached canvas");
                
                ZoomGesture = new MapZoom(vCanvas);
                PanGesture = new Pan(vCanvas, ZoomGesture);
                RectZoomGesture = new RectangleSelectionGesture(vCanvas, ZoomGesture, System.Windows.Input.ModifierKeys.Control);
                RectZoomGesture.ZoomSelection = true;
                AutoScrollGesture = new AutoScroll(vCanvas, ZoomGesture);
        

      		}
		}

		public bool LoadDocument(string name)
		{

			System.Windows.Controls.Canvas canvas = SchemaDocument.LoadSchema(name);
			if (canvas == null)
				return false;

			MainCanvas = canvas;
            DocumentName = name;
			return true;
		}

        public override void OnActivated()
        {
        	base.OnActivated();
            PanGesture.Activate();

			foreach (CommandInfo cmdInfo in DocumentCommands)
			{
				if (cmdInfo.command is BaseDocumentCommand)
				{
					BaseDocumentCommand cmd = (BaseDocumentCommand)cmdInfo.command;
					cmd.ControlledObject = this;
				}
			}

        }

		public override void OnDeactivated()
        {
			base.OnDeactivated();
            //PanGesture.Deactivate();
        }
        
        protected override void OnClosed(EventArgs e)
		{
			wpfSchemaContainer.Dispose();
			wpfSchemaContainer = null;
            
            base.OnClosed(e);
		}
        
    }
}
