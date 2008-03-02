using System;
using System.Collections.Generic;
using System.Text;
using FreeSCADA.Common.Schema;
using System.Windows.Controls;

namespace FreeSCADA.Designer.SchemaEditor
{
    class WPFShemaContainer:System.Windows.Forms.Integration.ElementHost
    {
        SchemaDocument document;
        public SchemaDocument Document
        {
            get { return document; }
            set 
            {
                document=value;
                (Child as ScrollViewer).Content=document.MainCanvas;
                document.MainCanvas.Focusable = false;
            }

        }

        public WPFShemaContainer()
        {
            
            Child = new ScrollViewer();
            Child.Focusable = false;
        }

    }
}
