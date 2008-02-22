using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Markup;
using System.Xml;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Documents;
using System.Windows.Input;



namespace FreeSCADA.Schema
{
    public class SchemaViewer : ScrollViewer
    {
        public SchemaDocument Schema;
        ScaleTransform scale;
        public SchemaViewer(SchemaDocument d)
            : base()
        {
            BorderBrush = Brushes.Black;
            HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            Schema = d;
            Content = d.MainCanvas;
            scale = new ScaleTransform();
            Focusable = false;
            //MouseWheel += new MouseWheelEventHandler(FSSchemeViewer_MouseWheel);

        }


        void FSSchemaViewer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            scale.ScaleX += e.Delta;
            scale.ScaleY += e.Delta;
            Schema.MainCanvas.RenderTransform = scale;
        }
        public void SetScale()
        {


        }
    }

}
