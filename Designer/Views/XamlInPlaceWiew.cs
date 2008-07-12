using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Windows;
using FreeSCADA.Designer.SchemaEditor.Tools;
using System.Windows.Markup;
using FreeSCADA.Common;

namespace FreeSCADA.Designer.Views
{
    /// <summary>
    /// Window
    /// </summary>
    public partial class XamlInPlaceWiew : Form
    {
        /// <summary>
        /// Con
        /// </summary>
        public XamlInPlaceWiew()
        {
            InitializeComponent();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void changeButton_Click(object sender, EventArgs e)
        {
            using (MemoryStream stream = new MemoryStream(this.XAMLtextBox.Text.Length))
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.Write(this.XAMLtextBox.Text);
                    sw.Flush();
                    stream.Seek(0, SeekOrigin.Begin);
                    UIElement el = XamlReader.Load(stream) as UIElement;
                    object[] arr = (object[])this.Tag;
                    UIElement oldel = (UIElement)arr[1];
                    (Env.Current.MainWindow as MainForm).ChangeGraphicsObject(oldel, el);
                }
            }
            this.Close();
        }
    }
}
