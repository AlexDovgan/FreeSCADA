using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using FreeSCADA.Common;
using FreeSCADA.CommonUI.Interfaces;

namespace FreeSCADA.Designer.SchemaEditor
{
    class SchemaSelectionManager : ISelectionManager
    {
        IManipulator _manipulator;

        public Type ManipulatorType
        {
            get;
            set;
        }
        public event ObjectSelectedDelegate SelectionChanged;

        Views.SchemaView _view;

        
        public List<Object> SelectedObjects
        {
            get;
            protected set;
        }

    
        public SchemaSelectionManager(Views.SchemaView view)
        {
            _view = view;
            SelectedObjects = new List<Object>();
        }
        public void AddObject(Object el)
        {

            if (el != null)
            {
                SelectedObjects.Insert(0, el); ;
                UpdateManipulator();
            }
            else if (_manipulator!=null)
            {
                _manipulator.Deactivate();
                _manipulator = null;
            }
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
            if (_manipulator != null)
                _manipulator.Deactivate();
            if (SelectedObjects.Count > 0)
            {
                _manipulator = (IManipulator)Activator.CreateInstance(
                     _view.ActiveTool.GetToolManipulator(),
                     new object[] { _view, SelectedObjects.Cast<FrameworkElement>().FirstOrDefault() });
                if (_manipulator.IsApplicable())
                    _manipulator.Activate();
                else
                    _manipulator = null;
                

            }
            AdornerLayer.GetAdornerLayer(_view.MainPanel).Update();
        }
        protected void RaiseSelectionChanged(Object obj)
        {
            if (obj == null)
                obj = _view.MainPanel;
            if (SelectionChanged != null)
                SelectionChanged(new PropertiesUtils.PropProxy( obj,_view.Document));
        }
    }
}
