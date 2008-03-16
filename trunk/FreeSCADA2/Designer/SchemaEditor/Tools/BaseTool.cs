using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using FreeSCADA.Common.Schema;
using FreeSCADA.Designer.SchemaEditor.Context_Menu;
using FreeSCADA.Designer.SchemaEditor.Manipulators;
using FreeSCADA.Designer.SchemaEditor.UndoRedo;
using FreeSCADA.ShellInterfaces;

namespace FreeSCADA.Designer.SchemaEditor.Tools
{
    /// <summary>
    /// Base class for tools implementation.
    /// A tool is applicable for objects selection and creation.
    /// When an object is selected tool should create some manipulator for this object.
    /// Each tool has DefaultManipulator. For example Selection tool has default manipulator DragResizeRotateManipulator.
    /// Each tool can work with many types of manipulators but the only ONE manipulator can be active.
    /// Base class implement objects single selection by default manipulator of tool instance
    /// </summary>
    abstract class BaseTool : Adorner
    {
        protected VisualCollection visualChildren;
        private  BaseManipulator activeManipulator;
        protected SchemaDocument workedSchema;
        protected ToolContextMenu menu;
        
        public event EventHandler ToolFinished;
		public event EventHandler ToolStarted;
		public event EventHandler ToolWorking;
        public delegate void ObjectSeletedDelegate(Object obj);
        public event ObjectSeletedDelegate ObjectSelected;

        /// <summary>
        /// active manipulator upon  selected object created by tool
        /// may be as default manipulator so as an another manipulator that can be created by tool instance
        /// </summary>
        public BaseManipulator ActiveManipulator
        {
            get
            {
                return activeManipulator;
            }
            set
            {
                if (activeManipulator != value)
                {
                    if (activeManipulator != null)
                    {
                        activeManipulator.ObjectChangedPreview -= ObjectChangedPreview;
                        visualChildren.Remove(activeManipulator);
                        if (activeManipulator is IDisposable)
                        (activeManipulator as IDisposable).Dispose();
                    }
                    if ((activeManipulator=value) != null)
                    { 
                        visualChildren.Add(activeManipulator);
                        activeManipulator.ObjectChangedPreview += ObjectChangedPreview;
                        
                    }
                    AdornerLayer.GetAdornerLayer(AdornedElement).Update();
                }
            }

        }
        /// <summary>
        /// selected object by active manipulator
        /// if object selected throw this proprty will be create default manipulator for tool instance
        /// </summary>
        public  UIElement SelectedObject
        {
            get
            {
                if (ActiveManipulator != null)
                    return ActiveManipulator.AdornedElement;
                else return null;
            }
            set
            {
                if (value != null)
                {
                    if (ActiveManipulator == null || ActiveManipulator.AdornedElement != value)
                    {
                        ActiveManipulator = CreateToolManipulator(value);
                        ActiveManipulator.InvalidateArrange();
                    }
                        
                }   
                else
                    ActiveManipulator = null;
                RaiseObjectSelected(SelectedObject);
            }

        }
        public BaseTool(SchemaDocument schema)
            : base(schema.MainCanvas)
        {
            workedSchema = schema;
            visualChildren = new VisualCollection(this);

            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            Rect rect = new Rect(new Point(0,0), AdornedElement.DesiredSize);

            drawingContext.DrawRectangle(Brushes.White, new Pen(Brushes.Black, 0.2), rect);

            drawingContext.Close();
            drawingVisual.Opacity = 0;
            visualChildren.Add(drawingVisual);
        }
        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }
        
        protected override void OnPreviewMouseLeftButtonDown( MouseButtonEventArgs e)
        {
			NotifyToolStarted();
            Point pt = e.GetPosition(this);

            
            IInputElement manipulatorHit = null;
            if (ActiveManipulator != null)
                manipulatorHit = ActiveManipulator.InputHitTest(e.GetPosition(ActiveManipulator));
     
            if (manipulatorHit!=null)
            {
                e.Handled = true;
                return;
            }

            HitTestResult result;
            if (VisualTreeHelper.HitTest(workedSchema.MainCanvas, pt).VisualHit == workedSchema.MainCanvas)
                SelectedObject = null;

            else
                if ((result = VisualTreeHelper.HitTest(workedSchema.MainCanvas, pt)).VisualHit != workedSchema.MainCanvas)
                {
                    FrameworkElement el = (FrameworkElement)EditorHelper.FindTopParentUnder(workedSchema.MainCanvas, (DependencyObject)result.VisualHit);
                    SelectedObject = el;
                    //e.Handled = true;

                }
            AdornerLayer.GetAdornerLayer(AdornedElement).Update();
        }
    
        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
			NotifyToolFinished();
        }
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
			NotifyToolWorking();
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (activeManipulator != null)
            {
                //activeManipulator.Arrange(new Rect(ActiveManipulator.AdornedElement.TranslatePoint(new Point(0, 0), AdornedElement), ActiveManipulator.AdornedElement.RenderSize));
                activeManipulator.Arrange(new Rect(finalSize));
            }
            return finalSize;
        }
        /// <summary>
        /// tool activating on working  Canvas
        /// </summary>
        public virtual void Activate()
        {
            
           AdornerLayer.GetAdornerLayer(AdornedElement).Add(this);

        }
        /// <summary>
        /// tool deactiavaion on working Canvas
        ///
        /// </summary>
        public virtual void Deactivate()
        {
            ActiveManipulator = null;
            AdornerLayer.GetAdornerLayer(AdornedElement).Remove(this);
        }
        protected  void NotifyToolFinished()
        {
			if(ToolFinished != null)
				ToolFinished(this, new EventArgs());
        }
        protected void NotifyToolStarted()
        {
			if (ToolStarted != null)
				ToolStarted(this, new EventArgs());
        }
        protected void NotifyToolWorking()
        {
			if (ToolWorking != null)
				ToolWorking(this, new EventArgs());
        }
        
        protected virtual void ObjectChangedPreview(UIElement obj)
        {
            
            UndoRedoManager.GetUndoBuffer(workedSchema).AddCommand(new ModifyGraphicsObject(obj));           
        }
        protected virtual BaseManipulator CreateToolManipulator(UIElement obj)
        {
            return new GeometryHilightManipulator(obj);
        }
        protected void RaiseObjectSelected(Object obj)
        {
            if (ObjectSelected != null)
                ObjectSelected(obj);
        }
    }


}