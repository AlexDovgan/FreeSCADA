using System;
using System.Windows;
using System.IO;
using System.Xml;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Windows.Media;

namespace FreeSCADA.Common.Schema
{
    public class SchemaDocument
    {
        public event EventHandler IsModifiedChanged;

        bool isModified=false;
        public bool IsModified
        {
            get { return isModified; }
            set 
            {
				isModified = value;
				if (IsModifiedChanged != null)
					IsModifiedChanged(this, new EventArgs());
            }
        }

        public String Name;
        public Canvas MainCanvas=new Canvas(); 
        
      
        public static SchemaDocument LoadSchema(string schemaName)
        {
            try
            {
                SchemaDocument schema = new SchemaDocument();
				using (Stream ms = Env.Current.Project.GetData("Schemas/"+schemaName + "/xaml"))
				using (XmlReader xmlReader = XmlReader.Create(ms))
				{
					Object obj = XamlReader.Load(xmlReader);
					if (obj is Canvas)
					{
						schema.MainCanvas = obj as Canvas;
						schema.Name = schemaName;
                        
					}
					else 
						throw (new Exception("This is not FreeSCADA schema"));
				}
                return schema;
            }
            catch (Exception)
            {
                return null;
            }

        }
		public static SchemaDocument CreateNewSchema()
		{
			SchemaDocument schema = null;

			schema = new SchemaDocument();
			schema.MainCanvas.ClipToBounds = true;
			schema.MainCanvas.Background = System.Windows.Media.Brushes.White;
			schema.MainCanvas.Width = 800;	//TODO: Get default values from application settings
			schema.MainCanvas.Height = 600;	//TODO: Get default values from application settings
			schema.isModified = true;
			return schema;
		}

        public void SaveSchema()
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;
				using (MemoryStream ms = new MemoryStream())
				{
					XamlWriter.Save(MainCanvas, XmlWriter.Create(ms, settings));
					Env.Current.Project.SetData("Schemas/" + Name + "/xaml", ms);
				}
                IsModified = false;
            }
            catch (Exception)
            {

            }
        }
    }

}