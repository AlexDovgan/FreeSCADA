using System;
using System.Windows;
using System.IO;
using System.Xml;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Windows.Media;
using FreeSCADA.Common;

namespace FreeSCADA.Schema
{
    public class SchemaDocument
    {
        public String Name;
        public Canvas MainCanvas=new Canvas(); 

      
        public static SchemaDocument LoadSchema(string schemaName)
        {
            try
            {
                SchemaDocument schema = new SchemaDocument();
                MemoryStream ms= Env.Current.Project["Schemas/"+schemaName+"/xaml"];
                XmlReader xmlReader = XmlReader.Create(ms);
                Object obj = XamlReader.Load(xmlReader);
                if (obj is Canvas)
                {

                    schema.MainCanvas = obj as Canvas;
                    schema.Name = schemaName;
                }
                else throw (new Exception("This is not FreeSCADA schema"));
                return schema;
            }
            catch (Exception ex)
            {

                return null;
            }

        }
        public static SchemaDocument CreateNewSchema()
        {

            SchemaDocument schema = null;
            Window w = new NewSchemaDialog();
            NewSchemaDialog.SchemaParams schemaParams = new NewSchemaDialog.SchemaParams();
            w.DataContext = schemaParams;
            
            if (w.ShowDialog() == true)
            {
                schema = new SchemaDocument();
                schema.MainCanvas.ClipToBounds = true;
                schema.MainCanvas.Background = System.Windows.Media.Brushes.White;
                schema.MainCanvas.Width = schemaParams.Width;
                schema.MainCanvas.Height =schemaParams.Height ;
                schema.Name = schemaParams.Name;
                schema.SaveSchema();
            }
            return schema;
        }

        public void SaveSchema()
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;
                XamlWriter.Save(MainCanvas, XmlWriter.Create(Env.Current.Project["Schemas/" + Name + "/xaml"], settings));
                
            }
            catch (Exception ex)
            {

            }
        }


    }

}