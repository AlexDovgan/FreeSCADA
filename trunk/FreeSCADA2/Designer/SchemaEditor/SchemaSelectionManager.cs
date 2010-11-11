using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using FreeSCADA.Common;

namespace FreeSCADA.Designer.SchemaEditor
{
    class SchemaSelectionManager : FreeSCADA.Designer.ISelectionManager
    {
        BaseManipulator manipulator;

        

        public event ObjectSelectedDelegate SelectionChanged;

        Views.SchemaView _view;


        public List<Object> SelectedObjects
        {
            get;
            protected set;
        }

        /*public static ISelectionManager GetSelectionManagerFor(UIElement el)
        {
            AdornerLayer al = AdornerLayer.GetAdornerLayer(el);
            Canvas c = Common.Schema.SchemaDocument.GetMainCanvas(el);
            return (c.Tag as Views.SchemaView).SelectionManager;
        }*/
        public SchemaSelectionManager(Views.SchemaView view)
        {
            _view = view;
            SelectedObjects = new List<Object>();
        }
        public void AddObject(Object el)
        {
            if(el!=null)
                SelectedObjects.Insert(0,el);;
            UpdateManipulator();
            RaiseSelectionChanged(el);
            
        }
        public void DeleteObject(Object el)
        {
            SelectedObjects.Remove(el);
            UpdateManipulator();
            RaiseSelectionChanged(null);
        }

        public void SelectObject(Object el)
        {
            SelectedObjects.Clear();
            AddObject(el);
        }
        
        public void UpdateManipulator()
        {
            if (manipulator != null)
            {
                manipulator.Deactivate();
                AdornerLayer.GetAdornerLayer(_view.MainPanel).Remove(manipulator);
            }
            if (SelectedObjects.Count > 0)
            {
                try
                {
                    AdornerLayer.GetAdornerLayer(_view.MainPanel).Add(manipulator = _view.ActiveTool.CreateToolManipulator(SelectedObjects.Cast<UIElement>().FirstOrDefault()));
                    manipulator.Activate();
                }catch(Exception)
                {
                    SelectObject(null);
                }
            }
            AdornerLayer.GetAdornerLayer(_view.MainPanel).Update();
        }
        protected void RaiseSelectionChanged(Object obj)
        {
            if (obj == null)
                obj = _view.MainPanel;
            if (SelectionChanged != null)
                SelectionChanged(obj);
        }
    }
}
