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
using FreeSCADA.Designer.SchemaEditor.Manipulators;
using FreeSCADA.Common;
using FreeSCADA.Designer.SchemaEditor.Tools;
using System.Windows.Input;
using FreeSCADA.Designer.Views;
using System.Drawing;

namespace FreeSCADA.Designer.SchemaEditor.SchemaCommands
{
  
    class ToolContextMenu: ContextMenu
    {
        public MenuItem unGroupMenuItem = new MenuItem();
        public MenuItem groupMenuItem = new MenuItem();
        public MenuItem copyMenuItem = new MenuItem();
        public MenuItem cutMenuItem = new MenuItem();
        public MenuItem pasteMenuItem = new MenuItem();
        public MenuItem viewXamlMenuItem = new MenuItem();
        

        public ToolContextMenu()
        {

            unGroupMenuItem.Header = "Ungroup";
            unGroupMenuItem.Command = new UngroupCommand();
            Items.Add(unGroupMenuItem);


            groupMenuItem.Header = "Group";
            groupMenuItem.Command = new GroupCommand();
            Items.Add(groupMenuItem);

            copyMenuItem.Header = "Copy";
            copyMenuItem.Command = new CopyCommand();
            Items.Add(copyMenuItem);

            cutMenuItem.Header = "Cut";
            cutMenuItem.Command = new CutCommand();
            Items.Add(cutMenuItem);

            pasteMenuItem.Header = "Paste";
            pasteMenuItem.Command = new PasteCommand();
            Items.Add(pasteMenuItem);

            viewXamlMenuItem.Command = new XamlViewCommand();
            viewXamlMenuItem.Header = ((ICommandData)viewXamlMenuItem.Command).CommandName;
            //viewXamlMenuItem.Image = ((ICommandData)viewXamlMenuItem.Tag).CommandIcon;
            Items.Add(viewXamlMenuItem);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public interface ICommandData
    {
        /// <summary>
        /// 
        /// </summary>
        string CommandName
        {
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        Bitmap CommandIcon
        {
            get;
        }
    }

    class UngroupCommand:ICommand
    {
        SelectionTool tool;
        public UngroupCommand()
        {
        }
        public bool CanExecute(object o)
        {
            
            if (o is SelectionTool)
                tool = o as SelectionTool;
            else return false;
            if((tool.ToolManipulator!=null )&& (tool.ToolManipulator.AdornedElement is Viewbox))
                return true;
            return false;
        }
		public event EventHandler CanExecuteChanged;
        public void Execute(object o)
        {
            EditorHelper.BreakGroup(o as SelectionTool);
        }
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged!=null)
                CanExecuteChanged(this,new EventArgs());
        }

    }

    class GroupCommand : ICommand
    {
        SelectionTool tool;
        public GroupCommand()
        {
        }
        public bool CanExecute(object o)
        {
            
            if (o is SelectionTool)
                tool = o as SelectionTool;
            else return false;
            if (tool.selectedElements.Count > 0) return true;
            return false;
        }
        public event EventHandler CanExecuteChanged;
        public void Execute(object o)
        {

            EditorHelper.CreateGroup(o as SelectionTool);

        }
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged!=null)
                CanExecuteChanged(this, new EventArgs());
        }


    }
    
    class CopyCommand : ICommand
    {
        SelectionTool tool;
        public CopyCommand()
        {
        }
        public bool CanExecute(object o)
        {

            if (o is SelectionTool)
                tool = o as SelectionTool;
            else return false;
            if (tool.SelectedObject != null)
                return true;
            return false;
            
        }
        public event EventHandler CanExecuteChanged;
        public void Execute(object o)
        {

            string xaml = XamlWriter.Save((o as SelectionTool).SelectedObject );
            System.Windows.Clipboard.SetText(xaml, System.Windows.TextDataFormat.Xaml);
  
        }
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged!=null)
                CanExecuteChanged(this, new EventArgs());
        }
    }
    class CutCommand : ICommand
    {
        SelectionTool tool;
        public CutCommand()
        {
        }
        public bool CanExecute(object o)
        {
            if (o is SelectionTool)
                tool = o as SelectionTool;
            else return false;
            if (tool.ToolManipulator != null)
                return true;
            return false;
        }
        public event EventHandler CanExecuteChanged;
        public void Execute(object o)
        {


        }
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }
    }
    class PasteCommand : ICommand
    {
        SelectionTool tool;
        public PasteCommand()
        {
        }
        public bool CanExecute(object o)
        {
            if (o is SelectionTool)
                tool = o as SelectionTool;
            else return false;
            if (System.Windows.Clipboard.ContainsText(System.Windows.TextDataFormat.Xaml))
                return true;
            return false;
        }
        public event EventHandler CanExecuteChanged;
        public void Execute(object o)
        {
            string xaml = System.Windows.Clipboard.GetText(System.Windows.TextDataFormat.Xaml);
            if (xaml != null)
            {
                using (MemoryStream stream = new MemoryStream(xaml.Length))
                {
                    using (StreamWriter sw = new StreamWriter(stream))
                    {
                        sw.Write(xaml);
                        sw.Flush();
                        stream.Seek(0, SeekOrigin.Begin);
                        UIElement el = XamlReader.Load(stream) as UIElement;
                        Canvas.SetLeft(el, Mouse.GetPosition(tool).X);
                        Canvas.SetTop(el, Mouse.GetPosition(tool).Y);
                        tool.NotifyObjectCreated(el);
                     }
                }
            }
        }
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }
    }

    class XamlViewCommand : ICommand, ICommandData
    {
        SelectionTool tool;
        public XamlViewCommand()
        {
        }
        public bool CanExecute(object o)
        {
            if (o is SchemaView)
                o = ((SchemaView)o).SchemaViewSelectionTool;
            if (o is SelectionTool)
                tool = o as SelectionTool;
            else return false;
            if (tool.SelectedObject != null)
                return true;
            return false;
            
        }
        public event EventHandler CanExecuteChanged;
        public void Execute(object o)
        {
            if (o is SchemaView)
                o = ((SchemaView)o).SchemaViewSelectionTool;
            //object o = (((MainForm)Env.Current.MainWindow).windowManager.currentDocument as SchemaView).SchemaViewSelectionTool;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            XamlDesignerSerializationManager dsm=new XamlDesignerSerializationManager(XmlWriter.Create(sb, settings));
            dsm.XamlWriterMode = XamlWriterMode.Expression;
            XamlWriter.Save((o as SelectionTool).SelectedObject, dsm);
            //string xaml = XamlWriter.Save((o as SelectionTool).selectedElements);
            Views.XamlInPlaceWiew xw = new Views.XamlInPlaceWiew();
            xw.XAMLtextBox.Text = sb.ToString();
            xw.Owner = (System.Windows.Forms.Form)Env.Current.MainWindow;
            object[] transferdata = { o, (o as SelectionTool).SelectedObject };
            xw.Tag = transferdata;
            xw.Show();
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged!=null)
                CanExecuteChanged(this, new EventArgs());
        }
        public string CommandName
        {
            get { return "XAML representation";}
        }

        public Bitmap CommandIcon
        {
            get { 
                return global::FreeSCADA.Designer.Properties.Resources.shape_ellipse_add;
            }
        }
    }

    class ZoomLevelCommand : ICommand
    {
        double Level;
        public ZoomLevelCommand(double level)
        {
            Level = level;
        }
        public bool CanExecute(object o)
        {
            if (o is SchemaView) return true;
            return false;
        }
        public event EventHandler CanExecuteChanged;
        public void Execute(object o)
        {
            if (o is SchemaView) ((SchemaView)o).ZoomLevel = Level;
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }
    }

    class ZoomInCommand : ICommand
    {
        public ZoomInCommand()
        {
        }
        public bool CanExecute(object o)
        {
            if (o is SchemaView) return true;
            return false;
        }
        public event EventHandler CanExecuteChanged;
        public void Execute(object o)
        {
            if (o is SchemaView) ((SchemaView)o).ZoomIn();
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }
    }

    class ZoomOutCommand : ICommand
    {
        public ZoomOutCommand()
        {
        }
        public bool CanExecute(object o)
        {
            if (o is SchemaView) return true;
            return false;
        }
        public event EventHandler CanExecuteChanged;
        public void Execute(object o)
        {
            if (o is SchemaView) ((SchemaView)o).ZoomOut();
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }
    }
}
