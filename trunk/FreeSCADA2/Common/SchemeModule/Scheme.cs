using System;
using System.Windows;
using System.IO;
using System.Xml;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Windows.Media;
using FreeSCADA.Common;

namespace FreeSCADA.Scheme
{
    public class FSScheme
    {
        public String Name;
        public Canvas MainCanvas=new Canvas(); 

      
        public static FSScheme LoadScheme(string schemeName)
        {
            try
            {
                FSScheme scheme = new FSScheme();
                MemoryStream ms= Env.Current.Project[schemeName+".graphics"];
                XmlReader xmlReader = XmlReader.Create(ms);
                Object obj = XamlReader.Load(xmlReader);
                if (obj is Canvas)
                {

                    scheme.MainCanvas = obj as Canvas;
                    scheme.Name = schemeName;
                }
                else throw (new Exception("This is not FreeSCADA scheme"));
                return scheme;
            }
            catch (Exception ex)
            {

                return null;
            }

        }
        public static FSScheme CreateNewScheme()
        {

            FSScheme scheme = null;
            Window w = new NewSchemeDialog();
            NewSchemeDialog.SchemeParams schemeParams = new NewSchemeDialog.SchemeParams();
            w.DataContext = schemeParams;
            
            if (w.ShowDialog() == true)
            {
                scheme = new FSScheme();
                scheme.MainCanvas.ClipToBounds = true;
                scheme.MainCanvas.Background = System.Windows.Media.Brushes.White;
                scheme.MainCanvas.Width = schemeParams.Width;
                scheme.MainCanvas.Height =schemeParams.Height ;
                scheme.Name = schemeParams.Name;
            }
            return scheme;
        }

      

        public void SaveScheme()
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;
                XamlWriter.Save(MainCanvas, XmlWriter.Create(Env.Current.Project[Name+ ".graphics"], settings));
                
            }
            catch (Exception ex)
            {

            }
        }
    }

}