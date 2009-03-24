using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows;
using FreeSCADA.Common.Schema;


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
    static class UndoRedoManager
    {
        public static BasicUndoBuffer GetUndoBufferFor(UIElement el)
        {
            System.Windows.Controls.Canvas c = SchemaDocument.GetMainCanvas(el);
            return (c.Tag as Views.SchemaView).UndoBuff;
        }
    }
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
                schemaView.SelectionManager.SelectObject( null);
   
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

    /// <summary>
    /// modify object command for undo redo buffer 
    /// must added to buffer before object whill be changed
    /// </summary>
    class ModifyGraphicsObject : IUndoCommand
    {
        UIElement modifiedObject;
        Views.SchemaView schemaView;
        Dictionary<DependencyProperty, object> values=new Dictionary<DependencyProperty,object>();

        protected bool documentModifiedState;
        public ModifyGraphicsObject(UIElement el)
        {
            modifiedObject = el;


        }
        public void Do(DocumentView doc)
        {
        
            schemaView = doc as Views.SchemaView;
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(modifiedObject,
                 new Attribute[] { new PropertyFilterAttribute(PropertyFilterOptions.All) }))
            {
                DependencyPropertyDescriptor dpd =
                    DependencyPropertyDescriptor.FromProperty(pd);

                if (dpd != null
                    && modifiedObject.ReadLocalValue(dpd.DependencyProperty) != DependencyProperty.UnsetValue
                    && dpd.GetValue(modifiedObject) != dpd.Metadata.DefaultValue)
                {

                    values.Add(dpd.DependencyProperty, CopyValue(dpd.DependencyProperty));
                }
            }

        }

        public void Redo()
        {
            DependencyProperty []dps=new DependencyProperty[values.Keys.Count];
            values.Keys.CopyTo(dps, 0);
            foreach (DependencyProperty dp in dps)
            {
                object val = CopyValue(dp);
                modifiedObject.SetValue(dp, values[dp]);
                values[dp] = val;
            }
          
            modifiedObject.InvalidateVisual();
        }

        public void Undo()
        {
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(modifiedObject,
                 new Attribute[] { new PropertyFilterAttribute(PropertyFilterOptions.All) }))
            {
                DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(pd);
                if (dpd != null
                    && modifiedObject.ReadLocalValue(dpd.DependencyProperty) != DependencyProperty.UnsetValue
                    && dpd.GetValue(modifiedObject) != dpd.Metadata.DefaultValue)
                {

                    if (values.ContainsKey(dpd.DependencyProperty))
                    {
                        object val = CopyValue(dpd.DependencyProperty);
                        modifiedObject.SetValue(dpd.DependencyProperty, values[dpd.DependencyProperty]);
                        values[dpd.DependencyProperty] = val;
                    }
                    else
                    {
                        values.Add(dpd.DependencyProperty, CopyValue(dpd.DependencyProperty));
                        dpd.ResetValue(modifiedObject);
                    }
                    
                }
            }
               
            
            modifiedObject.InvalidateVisual();
        }
        protected object CopyValue(DependencyProperty dp)
        {
            object val = modifiedObject.GetValue(dp);
            if (val is Freezable)
                return  (val as Freezable).Clone();
            else
                return  val;
        }

    }

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
