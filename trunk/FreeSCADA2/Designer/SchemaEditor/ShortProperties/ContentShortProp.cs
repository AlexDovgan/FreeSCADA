using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FreeSCADA.Designer.SchemaEditor.ShortProperties
{
    class ContentShortProp : ControlShortProp
    {
        ContentControl conentc;
        public ContentShortProp(ContentControl c)
            : base(c)
        {
            conentc = c;
        }
        [Description("Content property"), Category("Appearence")]
        [EditorAttribute(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string Content
        {
            get { return conentc.Content as string; }
            set
            {
                RaisePropertiesBrowserChanged((UIElement)conentc);
                if (!File.Exists(value))
                    conentc.Content = value;
                else
                {
                    bool isSeted = false;
                    DataObject Do = new DataObject(value);

                    if (!isSeted)
                        try
                        {
                            Image simpleImage = new Image();
                            simpleImage.Stretch = Stretch.Fill;
                            // Create source.
                            BitmapImage bi = new BitmapImage();
                            // BitmapImage.UriSource must be in a BeginInit/EndInit block.
                            bi.BeginInit();
                            bi.UriSource = new Uri(value, UriKind.RelativeOrAbsolute);
                            bi.EndInit();
                            // Set the image source.
                            simpleImage.Source = bi;

                            conentc.Content = simpleImage;
                            isSeted = true;

                        }
                        catch (Exception)
                        {
                        }
                    /*if (!isSeted)
                        try
                        {
                            MediaElement media = new MediaElement();
                            media.Stretch = Stretch.Fill;
                            media.Source = new Uri(value, UriKind.RelativeOrAbsolute);

                            conentc.Content = media;
                            //media.Play();
                        }
                        catch (Exception)
                        {
                        }
                    if (!isSeted)
                        try
                        {
                            FlowDocumentScrollViewer fw = new FlowDocumentScrollViewer();
                            fw.Document = new FlowDocument();
                            TextRange tr=new TextRange(fw.Document.ContentStart,fw.Document.ContentEnd);
                            tr.Load(File.Open(value, FileMode.Open), System.IO.Path.GetExtension(value));
                            conentc.Content = fw;
                        }
                        catch (Exception)
                        {
                        }

                    */

                }
            }
        }
    }

}
