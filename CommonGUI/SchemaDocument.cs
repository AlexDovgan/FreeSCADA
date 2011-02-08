using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using System.Collections.Generic;

namespace FreeSCADA.Common.Documents
{
    public class SchemaDocument : FreeSCADA.Interfaces.IDocument
    {
        public ProjectEntityType Type
        {
            get { return ProjectEntityType.Schema; }
        }
        public String Name
        {
            get;
            protected set;

        }
        public object Content
        {
            get;
            protected set;
        }
        public SchemaDocument()
        {
            Canvas c=new Canvas();
            c.Background = Brushes.White;
            Content = c;
           
            Name = Env.Current.Project.GenerateUniqueName(ProjectEntityType.Schema, "Untitled_");
            Save(Name,Content);
        }
        public SchemaDocument(string name)
        {
            Name = name;
            Load(Name);
        }

        public Object Load(string name)
        {
            System.Globalization.CultureInfo originalCulture = System.Windows.Forms.Application.CurrentCulture;
            try
            {

                using (Stream ms = Env.Current.Project.GetData("Schemas/" + name + "/xaml"))
                using (XmlReader xmlReader = XmlReader.Create(ms))
                {

                    System.Windows.Forms.Application.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
                    ms.Seek(0, SeekOrigin.Begin);
                    Object obj= XamlReader.Load(ms);
                    System.Windows.Forms.Application.CurrentCulture = originalCulture;
                    Content = obj as Canvas;
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.Application.CurrentCulture = originalCulture;
                Env.Current.Logger.LogError(string.Format("Cannot load schema: {0}", e.Message));
                Content = null;
            }
            return Content;  
        }

        public void Save(String name)
        {
            Save(name,Content);
        }
        public void Save(String name,object content)
        {
            
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            using (MemoryStream ms = new MemoryStream())
            {
                XamlDesignerSerializationManager dsm = new XamlDesignerSerializationManager(XmlWriter.Create(ms, settings));
                dsm.XamlWriterMode = XamlWriterMode.Expression;
                XamlWriter.Save(content, dsm);
                Env.Current.Project.SetData("Schemas/" + name + "/xaml", ms);
                Name = name;
                Content = content;
            }

        }
    }

}