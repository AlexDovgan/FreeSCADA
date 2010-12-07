using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using FreeSCADA.Common;
using FreeSCADA.Designer.Views;


namespace FreeSCADA.Designer
{
      
    /// <summary>
    /// add graphics element command for undo redo buffer
    /// </summary>
    class AddGraphicsObject : IUndoCommand
    {
        FrameworkElement addedObject;
        protected bool documentModifiedState;
        Views.SchemaView schemaView;
        public AddGraphicsObject(FrameworkElement el)
        {
            addedObject = el;
        }
        public void Do(IDocumentView doc)
        {
            if (!(doc is Views.SchemaView))
                throw new Exception("this is not schema");
            schemaView = doc as Views.SchemaView;
            schemaView.MainPanel.Children.Add(addedObject);
        }
        public void Redo()
        {
            schemaView.MainPanel.Children.Add(addedObject);
         
            
        }
        public void Undo()
        {
            schemaView.MainPanel.Children.Remove(addedObject);
            if (schemaView.ActiveTool!= null)
                schemaView.SelectionManager.SelectObject( null);
   
        }

    }
    class DeleteGraphicsObject : IUndoCommand
    {
        FrameworkElement deletedObject;

        protected bool documentModifiedState;
        Views.SchemaView schemaView;

        public DeleteGraphicsObject(FrameworkElement el)
        {
            deletedObject = el;
        }
        public void Do(IDocumentView doc)
        {

            if (!(doc is Views.SchemaView))
                throw new Exception("this is not schema");
            schemaView = doc as Views.SchemaView;

            schemaView.MainPanel.Children.Remove(deletedObject);
            
        }
        public void Redo()
        {
            schemaView.MainPanel.Children.Remove(deletedObject);
         
        }
        public void Undo()
        {

            schemaView.MainPanel.Children.Add(deletedObject);
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
        public void Do(IDocumentView doc)
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
   //     FrameworkElement modifiedObject;
   //     FrameworkElement oldObject;
   //     FrameworkElement restoredObject;
   //     string objectCopy;
   //     SchemaDocument schemaDocument;
   //     protected bool documentModifiedState;
   //     public ChangeGraphicsObject(FrameworkElement old, FrameworkElement el)
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

   //         modifiedObject = (FrameworkElement)XamlReader.Load(new XmlTextReader(new StringReader(objectCopy)));
   //         objectCopy = XamlWriter.Save(restoredObject);

   //         int i = schemaDocument.MainCanvas.Children.IndexOf(restoredObject);
   //         schemaDocument.MainCanvas.Children.Remove(restoredObject);
   //         schemaDocument.MainCanvas.Children.Insert(i, modifiedObject);

        
   //         AdornerLayer.GetAdornerLayer(modifiedObject).Update();
   //     }
   //     public void Undo()
   //     {
   //         restoredObject = (FrameworkElement)XamlReader.Load(new XmlTextReader(new StringReader(objectCopy)));
   //         objectCopy = XamlWriter.Save(modifiedObject);

   //         int i = schemaDocument.MainCanvas.Children.IndexOf(modifiedObject);
   //         schemaDocument.MainCanvas.Children.Remove(modifiedObject);
   //         schemaDocument.MainCanvas.Children.Insert(i, restoredObject);

   //         AdornerLayer.GetAdornerLayer(restoredObject).Update();
   //     }

   // }
}
