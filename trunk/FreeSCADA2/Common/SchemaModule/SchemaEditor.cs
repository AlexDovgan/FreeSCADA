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
using FreeSCADA.Schema.Manipulators;
using FreeSCADA.Schema.Tools;
using FreeSCADA.Schema.UndoRedo;
using FreeSCADA.ShellInterfaces;
using FreeSCADA.Schema.ShortProperties;

namespace FreeSCADA.Schema
{
    public class SchemaEditor : SchemaViewer
    {

        BasicUndoBuffer undoBuff;
        BasicTool activeTool;

        public delegate void ObjectSelectedDelegate(object element);
        public event ObjectSelectedDelegate ObjectSelected;
        #region Properties
        public List<ITool> toolsList
        {
            get
            {
                List<ITool> tl = new List<ITool>();
                tl.Add(new SelectionTool(Schema));
                tl.Add(new RectangleTool(Schema));
                tl.Add(new EllipseTool(Schema));
                return tl;

            }

        }
        public Type CurrentTool
        {
            get 
            {
                if (activeTool == null)
                {
                    activeTool = new SelectionTool(Schema);
                    activeTool.Activate();
                    activeTool.ToolFinished += new BasicTool.ToolEvent(Tool_Finished);
            
                }
                return activeTool.GetType(); 
                
            }
            set
            {
                activeTool.Deactivate();
                activeTool.ToolFinished -= Tool_Finished;
                object[] a = new object[1];
                a[0] = Schema;
                activeTool = (BasicTool)System.Activator.CreateInstance(value, a);
                activeTool.Activate();
                activeTool.ToolFinished += new BasicTool.ToolEvent(Tool_Finished);
            }
        }
        #endregion

        public SchemaEditor(SchemaDocument d)
            : base(d)
        {
            Schema = d;
            Schema.MainCanvas.Loaded += new RoutedEventHandler(MainCanvas_Loaded);
            undoBuff = UndoRedoManager.GetUndoBuffer(Schema);
         
        }

        void MainCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            Schema.MainCanvas.Focusable = true;
            Schema.MainCanvas.Focus();
            NameScope.SetNameScope(Schema.MainCanvas, new NameScope());
            Schema.MainCanvas.KeyDown += new KeyEventHandler(MainCanvas_KeyDown);
            
        }

        void MainCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Z && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                undoBuff.UndoCommand();
            }
            else if (e.Key == Key.Y && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                undoBuff.RedoCommand();
            }


        }



        void Tool_Started(BasicTool tool, EventArgs e)
        {
        }

        void Tool_Finished(BasicTool tool, EventArgs e)
        {
            if (ObjectSelected != null && tool.manipulator!=null)
                ObjectSelected(ShortPropFactory.CreateShortPropFrom(tool.manipulator.AdornedElement));

        }
        public void ImportFile(Stream xamlStream)
        {
            XmlReader xmlReader = XmlReader.Create(xamlStream);
            Object obj = XamlReader.Load(xmlReader);
            Schema.MainCanvas.Children.Add(obj as UIElement);
        }


              
    }
}
