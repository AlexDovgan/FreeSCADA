using System.Windows.Controls;
using FreeSCADA.Common.Schema;
using System.Windows.Input;
using System.Windows;


namespace FreeSCADA.RunTime
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
            Child = new myScrollViewer(this);
            //Child.Focusable = false;

            Child.SnapsToDevicePixels = true;
            (Child as myScrollViewer).HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
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
