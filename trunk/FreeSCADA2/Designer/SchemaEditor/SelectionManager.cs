using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using FreeSCADA.Common.Schema;

namespace FreeSCADA.Designer.SchemaEditor
{
    class SelectionManager
    {
        BaseManipulator manipulator;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        public delegate void SelectionChangedDelegate(UIElement el);

        /// <summary>
        /// 
        /// </summary>
        public event SelectionChangedDelegate SelectionChanged;
        Views.SchemaView _view;
        

        public List<UIElement> SelectedObjects
        {
            get;
            protected set;
        }

        public static SelectionManager GetSelectionManagerFor(UIElement el)
        {
            AdornerLayer al = AdornerLayer.GetAdornerLayer(el);
            Canvas c = Common.Schema.SchemaDocument.GetMainCanvas(el);
            return (c.Tag as Views.SchemaView).SelectionManager;
        }
        public SelectionManager(Views.SchemaView view)
        {
            _view = view;
            SelectedObjects = new List<UIElement>();
        }
        public void AddObject(UIElement el)
        {
            if(el!=null)
                SelectedObjects.Insert(0,el);;
            UpdateManipulator();
            if (SelectionChanged != null)
                SelectionChanged(el);
            
        }
        public void DeleteObject(UIElement el)
        {
            SelectedObjects.Remove(el);
            UpdateManipulator();
            if (SelectionChanged != null)
                SelectionChanged(el);
        }

        public void SelectObject(UIElement el)
        {
            SelectedObjects.Clear();
            AddObject(el);
        }
        public Rect CalculateBounds()
        {
            if (SelectedObjects.Count > 0)
                return EditorHelper.CalculateBounds(SelectedObjects, _view.MainCanvas);
            else return Rect.Empty;
            
        }
        public void UpdateManipulator()
        {
            if (manipulator != null)
            {
                manipulator.Deactivate();
                AdornerLayer.GetAdornerLayer(_view.MainCanvas).Remove(manipulator);
            }
            if (SelectedObjects.Count > 0)
            {
                try
                {
                    AdornerLayer.GetAdornerLayer(_view.MainCanvas).Add(manipulator = _view.ActiveTool.CreateToolManipulator(SelectedObjects[0]));
                    manipulator.Activate();
                }catch(Exception)
                {
                    SelectObject(null);
                }
            }
            AdornerLayer.GetAdornerLayer(_view.MainCanvas).Update();
        }

    }
}
