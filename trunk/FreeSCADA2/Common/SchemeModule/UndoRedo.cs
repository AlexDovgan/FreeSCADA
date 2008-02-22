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

namespace FreeSCADA.Scheme.UndoRedo
{
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
    public static class UndoRedoManager
    {
        static UndoRedoManager()
        {
            undoBuffers=new Dictionary<Canvas,BasicUndoBuffer>();
        }
        

        public static BasicUndoBuffer GetUndoBuffer(Canvas c)
        {
            if(!undoBuffers.ContainsKey(c)) 
            {
                undoBuffers[c]=new BasicUndoBuffer();

            }
            return undoBuffers[c];
        }
    
        static Dictionary<Canvas,BasicUndoBuffer> undoBuffers;
    }
    public class BasicUndoBuffer
    {
        public void AddCommand(IUndoCommand command)
        {
             redoStack.Clear();
            undoStack.Push(command);
        }
        public void UndoCommand()
        {
            if(undoStack.Count==0)
                return;
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
            if(redoStack.Count==0) 
                return;
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
    
    
    public class AddObject : IUndoCommand
    {
        UIElement addedObject;
        Canvas workCanvas;
        Canvas editedCanvas{
            get { return workCanvas; }
        }
        public AddObject(UIElement el,Canvas c)
        {
            addedObject = el;
            workCanvas = c;
        }
        public void Redo()
        {
            workCanvas.Children.Add(addedObject);
            
        }
        public void Undo()
        {
            workCanvas.Children.Remove(addedObject);
        }

    }
    public class ModifyObject : IUndoCommand
    {
        UIElement modifiedObject;
        
        
        public ModifyObject(UIElement el)
        {
            modifiedObject = el;
         
        }
        public void Redo()
        {
            

        }
        public void Undo()
        {
            
        }

    }
}
