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
using FreeSCADA.Schema.Manipulators;
using System.Windows.Input;
using FreeSCADA.Schema.Helpers;

namespace FreeSCADA.Schema.Commands
{
    public class UngroupCommand:ICommand
    {
        public UngroupCommand()
        {
        }
        public bool CanExecute(object o)
        {
            if ((o is Viewbox )&& ((o as Viewbox).Child is Panel)&&((o as Viewbox).Parent is Canvas)) 
                return true;
            return false;
        }
        public event EventHandler CanExecuteChanged;
        public void Execute(object o)
        {

            EditorHelper.BreakGroup((Viewbox)o);
        }
        

    }

    public class GroupCommand : ICommand
    {
        public GroupCommand()
        {
        }
        public bool CanExecute(object o)
        {
            if (o is GroupEditManipulator)
                return true;
            return false;
        }
        public event EventHandler CanExecuteChanged;
        public void Execute(object o)
        {

            EditorHelper.CreateGroup(o as GroupEditManipulator);

        }


    }
 
    
 

 


}