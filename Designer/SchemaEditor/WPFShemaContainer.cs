using System;
using System.Collections.Generic;
using System.Text;
using FreeSCADA.Common.Schema;
using System.Windows.Controls;
using System.Windows.Media;
namespace FreeSCADA.Designer.SchemaEditor
{
    



    class WPFShemaContainer:System.Windows.Forms.Integration.ElementHost
    {
        SchemaDocument document;
        CanvasResources resources = new CanvasResources();
        public SchemaDocument Document
        {
            get { return document; }
            set 
            {
                document=value;
                (Child as ScrollViewer).Content=document.MainCanvas;
                document.MainCanvas.Focusable = false;
                document.MainCanvas.Background = resources["GridBackgroundBrush"] as DrawingBrush;
               
            }

        }

        public WPFShemaContainer()
        {
            
            Child = new ScrollViewer();
            Child.Focusable = false;
            resources.InitializeComponent();
             
        }

    }
}
