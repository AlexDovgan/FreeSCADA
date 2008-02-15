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
using FreeSCADA.Scheme.Manipulators;
using System.Windows.Input;
using FreeSCADA.Scheme.Helpers;

namespace FreeSCADA.Scheme.Commands
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
 
    
    public interface IUndoCommand
    {
        /**

        * Re-executes a command.

        */
        void Redo();
        /**
        * Reverses the effect of executing the command.
        */
        void Undo();
   }
    public class BasicUndoBuffer
    {
        public void AddCommand(IUndoCommand command)
        {
           // redoStack.Clear();
            //undoStack.Add(command);
        }
        public void UndoCommand()
        {
            IUndoCommand cmd = undoStack.Pop();
            try
            {
                cmd.Undo();
            }
            finally
            {
                redoStack.Push(cmd);
            }
        }


        public void RedoCommand()
        {
            IUndoCommand cmd = redoStack.Pop();
            try
            {
                cmd.Redo();
            }
            finally
            {
                undoStack.Push(cmd);
            }
        }

        private Stack<IUndoCommand> undoStack = new Stack<IUndoCommand>();
        private Stack<IUndoCommand> redoStack = new Stack<IUndoCommand>();
    }

 


}