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
using FreeSCADA.Designer.SchemaEditor.Tools;
using System.Windows.Input;

namespace FreeSCADA.Designer.SchemaEditor.Context_Menu
{
  
    class ToolContextMenu: ContextMenu
    {
        public MenuItem unGroupMenuItem = new MenuItem();
        public MenuItem groupMenuItem = new MenuItem();
        public ToolContextMenu()
        {

            unGroupMenuItem.Header = "Ungroup";
            unGroupMenuItem.Command = new UngroupCommand();
            Items.Add(unGroupMenuItem);


            groupMenuItem.Header = "Group";
            groupMenuItem.Command = new GroupCommand();
            Items.Add(groupMenuItem);
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
            if((tool.ActiveManipulator!=null )&& (tool.ActiveManipulator.AdornedElement is Viewbox))
                return true;
            return false;
        }
		public event EventHandler CanExecuteChanged;
        public void Execute(object o)
        {
            EditorHelper.BreakGroup(tool);
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

            EditorHelper.CreateGroup(tool);

        }
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged!=null)
                CanExecuteChanged(this, new EventArgs());
        }


    }
 }