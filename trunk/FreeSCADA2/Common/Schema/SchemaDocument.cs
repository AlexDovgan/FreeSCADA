﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using FreeSCADA.Common.Schema.Actions;
namespace FreeSCADA.Common.Schema
{
    public class SchemaDocument
    {
       
        public String Name
        {
            get;
            set;
        }
        public Canvas MainCanvas 
        {
            get;
            set;
        }

		public SchemaDocument()
		{
			MainCanvas = new Canvas();
			MainCanvas.Tag = this;
		}

		public static SchemaDocument GetSchemaDocument(DependencyObject element)
		{
			if (element == null)
				return null;

			DependencyObject parent = element;
			while(parent != null)
			{
				if((parent is Canvas) && (parent as Canvas).Tag != null && (parent as Canvas).Tag is SchemaDocument)
					break;

				parent = VisualTreeHelper.GetParent(parent);
			}

			if(parent != null && parent is Canvas)
			{
				Canvas top = parent as Canvas;
				if(top.Tag != null && top.Tag is SchemaDocument)
					return top.Tag as SchemaDocument;
			}

			return null;
		}

        public static SchemaDocument LoadSchema(string schemaName)
        {
            try
            {
                SchemaDocument schema = new SchemaDocument();
                using (Stream ms = Env.Current.Project.GetData("Schemas/" + schemaName + "/xaml"))
                using (XmlReader xmlReader = XmlReader.Create(ms))
                {
                    System.Windows.Forms.Application.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
                    Object obj = XamlReader.Load(xmlReader);
                    if (obj is Canvas)
                    {
                        schema.MainCanvas = obj as Canvas;
						schema.MainCanvas.Tag = schema;
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
            return schema;
        }

        public void SaveSchema()
        {
			MainCanvas.Tag = null;
         //   WPFShemaContainer.ViewGrid(MainCanvas as Canvas, false);    // delete grid before save
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            using (MemoryStream ms = new MemoryStream())
            {
                XamlDesignerSerializationManager dsm=new XamlDesignerSerializationManager(XmlWriter.Create(ms, settings));
                dsm.XamlWriterMode = XamlWriterMode.Expression;
                XamlWriter.Save(MainCanvas, dsm);
                Env.Current.Project.SetData("Schemas/" + Name + "/xaml", ms);
            }
			MainCanvas.Tag = this;
        }
        public void LinkActions()
        {
            foreach (FrameworkElement el in MainCanvas.Children)
            {
                TransformGroup tg = new TransformGroup();
                tg.Children.Add(el.RenderTransform);
                tg.Children.Add(new ScaleTransform());
                tg.Children.Add(new SkewTransform());
                tg.Children.Add(new RotateTransform());
                tg.Children.Add(new TranslateTransform());
                el.RenderTransform = tg;


                ActionsCollection ac = ActionsCollection.GetActions(el);
                foreach (BaseAction a in ac.ActionsList)
                {
                    a.ActivateActionFor(el);
                }
            }
        }
    }

}