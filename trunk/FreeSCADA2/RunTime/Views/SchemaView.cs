using System;
using System.Windows.Media;
using FreeSCADA.Common;
using FreeSCADA.Common.Schema;
using FreeSCADA.RunTime.DocumentCommands;

namespace FreeSCADA.RunTime.Views
{
	class SchemaView : DocumentView
	{
		//private WPFShemaContainer wpfSchemaContainer;
        private ScaleTransform SchemaScale = new ScaleTransform();
        private System.Windows.Point SavedScrollPosition;
        private VirtualSchemaContainer virtualSchemaContainer;

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
			//this.wpfSchemaContainer = new WPFShemaContainer();

			// 
			// wpfContainerHost
			// 
		/*	this.wpfSchemaContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.wpfSchemaContainer.Location = new System.Drawing.Point(0, 0);
			this.wpfSchemaContainer.Name = "WPFSchemaContainer";
			this.wpfSchemaContainer.Size = new System.Drawing.Size(292, 273);
			this.wpfSchemaContainer.TabIndex = 0;
			this.wpfSchemaContainer.Text = "WPFSchemaContainer";
            this.wpfSchemaContainer.ZoomInEvent += new WPFShemaContainer.ZoomDelegate(ZoomIn);
            this.wpfSchemaContainer.ZoomOutEvent += new WPFShemaContainer.ZoomDelegate(ZoomOut);
		*/	// 
			// SchemaView
			// 
			this.ClientSize = new System.Drawing.Size(292, 273);
			//this.Controls.Add(this.wpfSchemaContainer);
            this.virtualSchemaContainer = new VirtualSchemaContainer();
            this.virtualSchemaContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.virtualSchemaContainer.Location = new System.Drawing.Point(0, 0);
            this.virtualSchemaContainer.Size = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.virtualSchemaContainer);

			this.Name = "SchemaView";

			this.ResumeLayout(false);

            this.SavedScrollPosition = new System.Windows.Point(0.0, 0.0);
        }


		public System.Windows.Controls.Canvas MainCanvas
		{
            get { return virtualSchemaContainer.MappedCanvas as System.Windows.Controls.Canvas; }
			set
			{

                if (virtualSchemaContainer.MappedCanvas == null)
                    virtualSchemaContainer.MappedCanvas = value;
                else
                    throw new Exception("View has already attached canvas");
      		}
		}

		public bool LoadDocument(string name)
		{

			System.Windows.Controls.Canvas canvas = SchemaDocument.LoadSchema(name);
			if (canvas == null)
				return false;

			MainCanvas = canvas;
            DocumentName = name;
			//schema.LinkActions();
			return true;
		}

        public override void OnActivated()
        {
			base.OnActivated();

			foreach (CommandInfo cmdInfo in DocumentCommands)
			{
				if (cmdInfo.command is BaseDocumentCommand)
				{
					BaseDocumentCommand cmd = (BaseDocumentCommand)cmdInfo.command;
					cmd.ControlledObject = this;
				}
			}

            // Scroll to saved position
            //System.Windows.Controls.ScrollViewer msv = (System.Windows.Controls.ScrollViewer)wpfSchemaContainer.Child;
            //msv.ScrollToVerticalOffset(SavedScrollPosition.Y);
            //msv.ScrollToHorizontalOffset(SavedScrollPosition.X);
        }

		public override void OnDeactivated()
        {
			base.OnDeactivated();
            // Save scroll position
            /*if (wpfSchemaContainer != null)
            {
                System.Windows.Controls.ScrollViewer msv = (System.Windows.Controls.ScrollViewer)wpfSchemaContainer.Child;
                if (msv != null)
                {
                    SavedScrollPosition.Y = msv.VerticalOffset;
                    SavedScrollPosition.X = msv.HorizontalOffset;
                }
            }*/
        }
        
        protected override void OnClosed(EventArgs e)
		{
			//wpfSchemaContainer.Dispose();
			//wpfSchemaContainer = null;
            virtualSchemaContainer.Dispose();
            virtualSchemaContainer = null;
            base.OnClosed(e);
		}
        
        public void ZoomIn()
        {
            ZoomIn(new System.Windows.Point (0,0));
        }

        public void ZoomIn(System.Windows.Point center)
        {
        /*    System.Windows.Controls.ScrollViewer msv = (System.Windows.Controls.ScrollViewer)wpfSchemaContainer.Child;
            SchemaScale.ScaleX *= 1.05;
            SchemaScale.ScaleY *= 1.05;
            MainCanvas.LayoutTransform = SchemaScale;
            msv.ScrollToVerticalOffset(msv.VerticalOffset * 1.05 + center.Y* 0.05);
            msv.ScrollToHorizontalOffset(msv.HorizontalOffset * 1.05 + center.X * 0.05);
            */
			UpdateZoomLevel();
        }

        public void ZoomOut()
        {
            ZoomOut(new System.Windows.Point(0, 0));
        }

        public void ZoomOut(System.Windows.Point center)
        {
            /*System.Windows.Controls.ScrollViewer msv = (System.Windows.Controls.ScrollViewer)wpfSchemaContainer.Child;
            SchemaScale.ScaleX /= 1.05;
            SchemaScale.ScaleY /= 1.05;
            MainCanvas.LayoutTransform = SchemaScale;
            msv.ScrollToVerticalOffset(msv.VerticalOffset / 1.05 - center.Y * 0.05);
            msv.ScrollToHorizontalOffset(msv.HorizontalOffset / 1.05 - center.X* 0.05);
            */
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
                MainCanvas.LayoutTransform = SchemaScale;
            }
        }
    }
}
