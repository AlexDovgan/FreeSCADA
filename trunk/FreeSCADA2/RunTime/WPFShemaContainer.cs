using System.Windows.Controls;
using FreeSCADA.Common.Schema;


namespace FreeSCADA.RunTime
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
			}
		}

		public WPFShemaContainer()
		{
			Child = new ScrollViewer();
			Child.SnapsToDevicePixels = true;
		}
	}
}
