using System.Windows.Controls;
using System.Windows.Media;
using FreeSCADA.Common.Schema;

namespace FreeSCADA.Designer.SchemaEditor
{
	class WPFShemaContainer : System.Windows.Forms.Integration.ElementHost
	{
		SchemaDocument document;
		CanvasResources resources = new CanvasResources();
		public SchemaDocument Document
		{
			get { return document; }
			set
			{
				document = value;
				(Child as ScrollViewer).Content = document.MainCanvas;
				document.MainCanvas.Focusable = false;

				//document.MainCanvas.Ba    ckground = resources["GridBackgroundBrush"] as DrawingBrush;

			}

		}

		public WPFShemaContainer()
		{

			Child = new ScrollViewer();
			//Child.Focusable = false;
			resources.InitializeComponent();
			Child.SnapsToDevicePixels = true;
            (Child as ScrollViewer).HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
		}

	}
}
