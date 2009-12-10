using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using System.Collections.Generic;
namespace FreeSCADA.Common.Schema
{
    public static class SchemaDocument
    {
        public static Canvas GetMainCanvas(DependencyObject element)
        {
            if (element == null)
                return null;

            DependencyObject top = element;
            while (!(VisualTreeHelper.GetParent(top) is ScrollContentPresenter) && VisualTreeHelper.GetParent(top)!=null)
            {
                top = VisualTreeHelper.GetParent(top);
            }
            return top as Canvas;

        }

        public static Canvas LoadSchema(string schemaName)
        {
            using (Stream ms = Env.Current.Project.GetData("Schemas/" + schemaName + "/xaml"))
            using (XmlReader xmlReader = XmlReader.Create(ms))
            {
                
                System.Globalization.CultureInfo originalCulture = System.Windows.Forms.Application.CurrentCulture;
                System.Windows.Forms.Application.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
                ms.Seek(0, SeekOrigin.Begin);
                try
                {
                    Object obj = XamlReader.Load(ms);
                    System.Windows.Forms.Application.CurrentCulture = originalCulture;
                    if (obj is Canvas)
                        return obj as Canvas;
                    else
                        return null;
                }
                catch (Exception e)
                {
                    System.Windows.Forms.Application.CurrentCulture = originalCulture;
                    
                    Env.Current.Logger.LogError(string.Format("Cannot load schema: {0}", e.Message));
                    return null;
                }				
            }

        }



        public static Canvas CreateNewSchema()
        {
            Canvas schema = null;

            schema = new Canvas();
            schema.ClipToBounds = true;
            schema.Background = System.Windows.Media.Brushes.White;
            schema.Width = 800;	//TODO: Get default values from application settings
            schema.Height = 600;	//TODO: Get default values from application settings
            return schema;
        }

        public static void SaveSchema(Canvas c,string name)
        {
            
            //   WPFShemaContainer.ViewGrid(MainCanvas as Canvas, false);    // delete grid before save
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            using (MemoryStream ms = new MemoryStream())
            {
                XamlDesignerSerializationManager dsm = new XamlDesignerSerializationManager(XmlWriter.Create(ms, settings));
                dsm.XamlWriterMode = XamlWriterMode.Expression;
                XamlWriter.Save(c, dsm);
                Env.Current.Project.SetData("Schemas/" + name + "/xaml", ms);
            }

        }
    }

}