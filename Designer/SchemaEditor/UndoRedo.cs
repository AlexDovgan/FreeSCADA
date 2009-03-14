using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml;
using FreeSCADA.Common.Schema;
using FreeSCADA.Designer.SchemaEditor.Tools;


namespace FreeSCADA.Designer.SchemaEditor.UndoRedo
{
    /// <summary>
    /// Interface for undo redo command
    /// </summary>
    interface IUndoCommand
    {
        /// <summary>
        /// exexute command 
        /// exexuted when comman add to undo redo buffer
        /// </summary>
        void Do(DocumentView doc);
        /// <summary>
        /// executed when undo redo buffer execute redo for document
        /// </summary>
        void Redo();
        /// <summary>
        /// executed when undo redo buffer execute undo for document
        /// </summary>
        void Undo();
    }
    /// <summary>
    /// static Manager for access undo redo buffers for documents from any context
    /// </summary>
    static class UndoRedoManager
    {
        static UndoRedoManager()
        {
            undoBuffers=new Dictionary<DocumentView ,BasicUndoBuffer>();
        }

        
        public static BasicUndoBuffer GetUndoBuffer(DocumentView doc)
        {
            if(!undoBuffers.ContainsKey(doc)) 
            {
                undoBuffers[doc]=new BasicUndoBuffer(doc);

            }
            return undoBuffers[doc];
        }

        public static void ReleaseUndoBuffer(DocumentView doc)
        {
            if (undoBuffers.ContainsKey(doc))
            {
                undoBuffers.Remove(doc);
  
            }
        }
        static Dictionary<DocumentView, BasicUndoBuffer> undoBuffers;
    }
    /// <summary>
    /// Undo Redo buffer for document instance
    /// </summary>
    class BasicUndoBuffer
    {
        DocumentView view;
        bool documentModifiedState;
        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }


        public BasicUndoBuffer(DocumentView doc)
        {
            view = doc;
        }
        public void AddCommand(IUndoCommand command)
        {
             redoStack.Clear();
             command.Do(view);
             documentModifiedState = view.IsModified;
             view.IsModified = true;

             undoStack.Push(command);
             RaiseCanExecuteChanged();
        }

        public void UndoCommand()
        {
            if(undoStack.Count==0)
                return;
            IUndoCommand cmd = undoStack.Pop();
            
            try
            {
                cmd.Undo();
                view.IsModified = documentModifiedState;
                
            }
            finally
            {
                redoStack.Push(cmd);
                RaiseCanExecuteChanged();
              
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
                //if (!CanRedo()) (Env.Current.MainWindow as MainForm).redoButton.Enabled = false;
                documentModifiedState = view.IsModified;
                view.IsModified = true;

            }
            finally
            {
                undoStack.Push(cmd);
                RaiseCanExecuteChanged();
                //(Env.Current.MainWindow as MainForm).undoButton.Enabled = true;
            }
        }

        public bool CanUndo()
        {
            return undoStack.Count > 0;
        }

        public bool CanRedo()
        {
            return redoStack.Count > 0;
        }

        private Stack<IUndoCommand> undoStack = new Stack<IUndoCommand>();
        private Stack<IUndoCommand> redoStack = new Stack<IUndoCommand>();
    }
    
    /// <summary>
    /// add graphics element command for undo redo buffer
    /// </summary>
    class AddGraphicsObject : IUndoCommand
    {
        UIElement addedObject;
        protected bool documentModifiedState;
        Views.SchemaView schemaView;
        public AddGraphicsObject(UIElement el)
        {
            addedObject = el;
        }
        public void Do(DocumentView doc)
        {
            if (!(doc is Views.SchemaView))
                throw new Exception("this is not schema");
            schemaView = doc as Views.SchemaView;
            schemaView.MainCanvas.Children.Add(addedObject);
        }
        public void Redo()
        {
            schemaView.MainCanvas.Children.Add(addedObject);
         
            
        }
        public void Undo()
        {
            schemaView.MainCanvas.Children.Remove(addedObject);
            if (schemaView.ActiveTool!= null)
                schemaView.ActiveTool.SelectedObject = null;
   
        }

    }
    class DeleteGraphicsObject : IUndoCommand
    {
        UIElement deletedObject;

        protected bool documentModifiedState;
        Views.SchemaView schemaView;

        public DeleteGraphicsObject(UIElement el)
        {
            deletedObject = el;
        }
        public void Do(DocumentView doc)
        {

            if (!(doc is Views.SchemaView))
                throw new Exception("this is not schema");
            schemaView = doc as Views.SchemaView;

            schemaView.MainCanvas.Children.Remove(deletedObject);
            
        }
        public void Redo()
        {
            schemaView.MainCanvas.Children.Remove(deletedObject);
         
        }
        public void Undo()
        {

            schemaView.MainCanvas.Children.Add(deletedObject);
       }

    }

   // /// <summary>
   // /// modify object command for undo redo buffer 
   // /// must added to buffer before object whill be changed
   // /// </summary>
   // class ModifyGraphicsObject : IUndoCommand
   // {
   //     UIElement modifiedObject;
   //     UIElement restoredObject;
   //     string objectCopy;
   //     SchemaDocument schemaDocument;
   //     protected bool documentModifiedState;
   //     public ModifyGraphicsObject(UIElement el)
   //     {
   //         modifiedObject = el;
         
   //     }
   //     public void Do(SchemaDocument doc)
   //     {
   //         objectCopy=XamlWriter.Save(modifiedObject);
   //         schemaDocument = doc;
      
   //     }
   //     public void Redo()
   //     {
        
   //         restoredObject = (UIElement)XamlReader.Load(new XmlTextReader(new StringReader(objectCopy)));
   //         objectCopy = XamlWriter.Save(modifiedObject);
   //         EditorHelper.CopyObjects(restoredObject, modifiedObject);

   //         AdornerLayer.GetAdornerLayer(modifiedObject).Update();
   //     }
   //     public void Undo()
   //     {
   //         restoredObject = (UIElement)XamlReader.Load(new XmlTextReader(new StringReader(objectCopy)));
   //         objectCopy = XamlWriter.Save(modifiedObject);
   //         EditorHelper.CopyObjects(restoredObject, modifiedObject);
   //            if (AdornerLayer.GetAdornerLayer(modifiedObject) != null)
   //             AdornerLayer.GetAdornerLayer(modifiedObject).Update();
   //     }

   // }

   // /// <summary>
   // /// change object command for undo redo buffer 
   // /// 
   // /// </summary>
   //class ChangeGraphicsObject : IUndoCommand
   // {
   //     UIElement modifiedObject;
   //     UIElement oldObject;
   //     UIElement restoredObject;
   //     string objectCopy;
   //     SchemaDocument schemaDocument;
   //     protected bool documentModifiedState;
   //     public ChangeGraphicsObject(UIElement old, UIElement el)
   //     {
   //         oldObject = old;
   //         modifiedObject = el;

   //     }
   //     public void Do(SchemaDocument doc)
   //     {
   //         objectCopy = XamlWriter.Save(oldObject);
   //         schemaDocument = doc;
   //         int i = schemaDocument.MainCanvas.Children.IndexOf(oldObject);
   //         schemaDocument.MainCanvas.Children.Remove(oldObject);
   //         schemaDocument.MainCanvas.Children.Insert(i, modifiedObject);
        
   //         AdornerLayer.GetAdornerLayer(modifiedObject).Update();
   //     }
   //     public void Redo()
   //     {

   //         modifiedObject = (UIElement)XamlReader.Load(new XmlTextReader(new StringReader(objectCopy)));
   //         objectCopy = XamlWriter.Save(restoredObject);

   //         int i = schemaDocument.MainCanvas.Children.IndexOf(restoredObject);
   //         schemaDocument.MainCanvas.Children.Remove(restoredObject);
   //         schemaDocument.MainCanvas.Children.Insert(i, modifiedObject);

        
   //         AdornerLayer.GetAdornerLayer(modifiedObject).Update();
   //     }
   //     public void Undo()
   //     {
   //         restoredObject = (UIElement)XamlReader.Load(new XmlTextReader(new StringReader(objectCopy)));
   //         objectCopy = XamlWriter.Save(modifiedObject);

   //         int i = schemaDocument.MainCanvas.Children.IndexOf(modifiedObject);
   //         schemaDocument.MainCanvas.Children.Remove(modifiedObject);
   //         schemaDocument.MainCanvas.Children.Insert(i, restoredObject);

   //         AdornerLayer.GetAdornerLayer(restoredObject).Update();
   //     }

   // }
}
