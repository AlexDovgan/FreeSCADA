using System.Windows.Controls;
using System.Windows.Media;
using FreeSCADA.Common.Schema;

namespace FreeSCADA.Designer.SchemaEditor
{
	class WPFShemaContainer : System.Windows.Forms.Integration.ElementHost
	{
		SchemaDocument document;
		
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

			Child = new ScrollViewer();
			//Child.Focusable = false;
			
			Child.SnapsToDevicePixels = true;
            (Child as ScrollViewer).HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
		}

	}
}
