using System;
using FreeSCADA.Common;
using FreeSCADA.Common.Schema;
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Media;

namespace FreeSCADA.RunTime
{
	class SchemaView : DockContent
	{
		private WPFShemaContainer wpfSchemaContainer;
        private ScaleTransform SchemaScale = new ScaleTransform();
        private System.Windows.Point SavedScrollPosition;
        private SchemaDocument document;
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
            this.wpfSchemaContainer.ZoomInEvent += new WPFShemaContainer.ZoomDelegate(ZoomIn);
            this.wpfSchemaContainer.ZoomOutEvent += new WPFShemaContainer.ZoomDelegate(ZoomOut);
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
            get { return document; }
			set
			{
				TabText = value.Name;
                document = value;
			}
		}

		public bool LoadDocument(string name)
		{
			SchemaDocument schema;
			if ((schema = SchemaDocument.LoadSchema(name)) == null)
				return false;
			schema.LinkActions();
			Schema = schema;
			return true;
		}

        public void OnActivated()
        {
            // Scroll to saved position
            System.Windows.Controls.ScrollViewer msv = (System.Windows.Controls.ScrollViewer)wpfSchemaContainer.Child;
            msv.ScrollToVerticalOffset(SavedScrollPosition.Y);
            msv.ScrollToHorizontalOffset(SavedScrollPosition.X);
        }

        public void OnDeactivated()
        {
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
        
        protected override void OnClosed(EventArgs e)
		{
			wpfSchemaContainer.Dispose();
			wpfSchemaContainer = null;

			base.OnClosed(e);
		}

        public void ZoomIn()
        {
            ZoomIn(new System.Windows.Point (0,0));
        }

        public void ZoomIn(System.Windows.Point center)
        {
            System.Windows.Controls.ScrollViewer msv = (System.Windows.Controls.ScrollViewer)wpfSchemaContainer.Child;
            SchemaScale.ScaleX *= 1.05;
            SchemaScale.ScaleY *= 1.05;
            Schema.MainCanvas.LayoutTransform = SchemaScale;
            msv.ScrollToVerticalOffset(msv.VerticalOffset * 1.05 + center.Y* 0.05);
            msv.ScrollToHorizontalOffset(msv.HorizontalOffset * 1.05 + center.X * 0.05);
            (Env.Current.MainWindow as MainForm).zoomLevelComboBox_SetZoomLevelTxt(SchemaScale.ScaleX);
        }

        public void ZoomOut()
        {
            ZoomOut(new System.Windows.Point(0, 0));
        }

        public void ZoomOut(System.Windows.Point center)
        {
            System.Windows.Controls.ScrollViewer msv = (System.Windows.Controls.ScrollViewer)wpfSchemaContainer.Child;
            SchemaScale.ScaleX /= 1.05;
            SchemaScale.ScaleY /= 1.05;
            Schema.MainCanvas.LayoutTransform = SchemaScale;
            msv.ScrollToVerticalOffset(msv.VerticalOffset / 1.05 - center.Y * 0.05);
            msv.ScrollToHorizontalOffset(msv.HorizontalOffset / 1.05 - center.X* 0.05);
            (Env.Current.MainWindow as MainForm).zoomLevelComboBox_SetZoomLevelTxt(SchemaScale.ScaleX);
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
    }
}
