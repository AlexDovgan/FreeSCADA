using System.Windows.Controls;
using System.Windows.Media;
using FreeSCADA.Common.Schema;
using FreeSCADA.Designer.Views;
using System.Windows.Input;
using System.Windows;

namespace FreeSCADA.Designer.SchemaEditor
{
	class WPFShemaContainer : System.Windows.Forms.Integration.ElementHost
	{
		SchemaDocument document;
        public SchemaView view;
		
		public SchemaDocument Document
		{
			get { return document; }
			set
			{
				document = value;
				(Child as ScrollViewer).Content = document.MainCanvas;
				document.MainCanvas.Focusable = false;
                if (!(document.MainCanvas.RenderTransform is System.Windows.Media.ScaleTransform))
                    document.MainCanvas.RenderTransform = new System.Windows.Media.ScaleTransform();
                    
                
				//document.MainCanvas.Background = resources["GridBackgroundBrush"] as DrawingBrush;

			}

		}

		public WPFShemaContainer()
		{
            this.Initialize();
		}

        public WPFShemaContainer(SchemaView View)
        {
            view = View;
            this.Initialize();
        }

        private void Initialize()
        {
            Child = new ZoomViewer(this);
			//Child.Focusable = false;
			
			Child.SnapsToDevicePixels = true;
            (Child as ZoomViewer).HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
        }

        public void ZoomIn(double x, double y)
        {
            view.ZoomIn(x, y);
        }
        public void ZoomOut(double x, double y)
        {
            view.ZoomOut(x, y);
        }
    }
}
