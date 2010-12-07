using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using FreeSCADA.Common;

namespace FreeSCADA.Designer.SchemaEditor
{
    class SchemaSelectionManager : ISelectionManager
    {
        BaseManipulator _manipulator;

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
            if (_manipulator != null)
            {
                _manipulator.Deactivate();
                AdornerLayer.GetAdornerLayer(_view.MainPanel).Remove(_manipulator);
            }
            if (SelectedObjects.Count > 0)
            {
                try
                {
                    _manipulator=(BaseManipulator)Activator.CreateInstance(
                        _view.ActiveTool.GetToolManipulator(), 
                        new object[] {_view, SelectedObjects.Cast<FrameworkElement>().FirstOrDefault()});
                    
                    AdornerLayer.GetAdornerLayer(_view.MainPanel).Add(_manipulator);
                    _manipulator.Activate();
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
                SelectionChanged(new PropertiesUtils.PropProxy( obj));
        }
    }
}
