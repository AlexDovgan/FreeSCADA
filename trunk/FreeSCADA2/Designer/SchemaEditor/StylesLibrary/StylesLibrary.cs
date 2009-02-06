using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Xml;
namespace FreeSCADA.Designer.SchemaEditor.StylesLibrary
{
    //TODO:reimplement as DataSourceProvider
    class StylesLibrary:IDisposable
    {
        static StylesLibrary instance=null;
        Dictionary<Type, Dictionary<string, Style>> styles;
        static object mutex=new object();
        StylesLibrary()
        {
            styles = new Dictionary<Type,Dictionary<string,Style>>();
            if (Directory.Exists(Environment.CurrentDirectory + "\\Styles"))
            {
                foreach (string fileName in Directory.GetFiles(Environment.CurrentDirectory + "\\Styles"))
                {
                    try
                    {
                        using (XmlReader xmlReader = XmlReader.Create(fileName))
                        {
                            Object obj = XamlReader.Load(xmlReader);
                            if (obj is Style)
                            {
                                Style s = obj as Style;
                                if (this[s.TargetType] == null)
                                    styles.Add(s.TargetType,new Dictionary<string,Style>());
                                FileInfo fi = new FileInfo(fileName);
                                string styleName=fi.Name.Split(new char []{'.'})[0];
                                styles[s.TargetType].Add(styleName, s);
                                TypeDescriptor.AddAttributes(s, new Attribute[] { new System.Windows.Markup.RuntimeNamePropertyAttribute(styleName) });

                                    
                            }
                            
                        }
                    }
                    catch(Exception)
                    { 

                    }
                }

            }


        }
        public static StylesLibrary Instance
        {
            get
            {
                lock (mutex)
                {
                    if (instance == null)
                        instance =new StylesLibrary();
                    return instance;

                }
            }
        }
        public Dictionary<string,Style> this[Type type]
        {
            get {
                if (styles.Keys.Contains(type))
                    return styles[type];
                else return null;
                }
        }
        
        public void Dispose()
        {

        }
        
    }
}
