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
using System.Windows.Input;
using System.Windows.Documents;
using System.ComponentModel;
namespace FreeSCADA.Schema.UndoRedo
{
    public interface IUndoCommand
    {
        /// <summary>
        /// exexute command
        /// </summary>
        void Do(SchemaDocument doc);
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
            undoBuffers=new Dictionary<SchemaDocument,BasicUndoBuffer>();
        }
        

        public static BasicUndoBuffer GetUndoBuffer(SchemaDocument doc)
        {
            if(!undoBuffers.ContainsKey(doc)) 
            {
                undoBuffers[doc]=new BasicUndoBuffer(doc);

            }
            return undoBuffers[doc];
        }
    
        static Dictionary<SchemaDocument,BasicUndoBuffer> undoBuffers;
    }

    public class BasicUndoBuffer
    {
        SchemaDocument schemaDocument;
        

        public BasicUndoBuffer(SchemaDocument doc)
        {
            schemaDocument = doc;
        }
        public void AddCommand(IUndoCommand command)
        {
             redoStack.Clear();
             command.Do(schemaDocument);
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
    
    
    public class AddGraphicsObject : IUndoCommand
    {
        UIElement addedObject;
        SchemaDocument schemaDocument;
        protected bool documentModifiedState;
        public AddGraphicsObject(UIElement el)
        {
            addedObject = el;
        }
        public void Do(SchemaDocument doc)
        {

            schemaDocument = doc;
            schemaDocument.MainCanvas.Children.Add(addedObject);
            documentModifiedState = schemaDocument.IsModified;
            schemaDocument.IsModified = true;
        }
        public void Redo()
        {
            schemaDocument.MainCanvas.Children.Add(addedObject);
            documentModifiedState = schemaDocument.IsModified;
            schemaDocument.IsModified = true;
            
        }
        public void Undo()
        {
            schemaDocument.MainCanvas.Children.Remove(addedObject);
            schemaDocument.IsModified=documentModifiedState;
            
        }

    }

    public class ModifyGraphicsObject : IUndoCommand
    {
        UIElement modifiedObject;
        UIElement restoredObject;
        string objectCopy;
        SchemaDocument schemaDocument;
        protected bool documentModifiedState;
        public ModifyGraphicsObject(UIElement el)
        {
            modifiedObject = el;
         
        }
        public void Do(SchemaDocument doc)
        {
            objectCopy=XamlWriter.Save(modifiedObject);
            schemaDocument = doc;
            documentModifiedState = schemaDocument.IsModified;
            schemaDocument.IsModified = true;
        }
        public void Redo()
        {
        
            restoredObject = (UIElement)XamlReader.Load(new XmlTextReader(new StringReader(objectCopy)));
            objectCopy = XamlWriter.Save(modifiedObject);
            EditorHelper.CopyObjects(restoredObject, modifiedObject);

            documentModifiedState = schemaDocument.IsModified;
            schemaDocument.IsModified = true;
            AdornerLayer.GetAdornerLayer(modifiedObject).Update();
        }
        public void Undo()
        {
            restoredObject = (UIElement)XamlReader.Load(new XmlTextReader(new StringReader(objectCopy)));
            objectCopy = XamlWriter.Save(modifiedObject);
            EditorHelper.CopyObjects(restoredObject, modifiedObject);
            schemaDocument.IsModified = documentModifiedState;
            AdornerLayer.GetAdornerLayer(modifiedObject).Update();
        }

    }
}
