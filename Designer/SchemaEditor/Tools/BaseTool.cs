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
    /// Base class for tools implementation
    /// Tool is sutable for objects selection and creation
    /// when object is selected tool must create manipulator for this object
    /// each tool have DefaultManipulator for example for Selection tool  default manipulator is DragResizeRotateManipulator
    /// each tool can work with many types of manipulators but only ONE manipulator can be active
    /// Base class implement objects single selection by default manipulator of tool instance
    /// </summary>
    abstract class BaseTool : Adorner
    {
        protected VisualCollection visualChildren;
        private  BaseManipulator activeManipulator;
        protected SchemaDocument workedSchema;
        protected ToolContextMenu menu;
        public delegate void ToolEvent(BaseTool tool, EventArgs e);
        public event ToolEvent ToolFinished;
        public event ToolEvent ToolStarted;
        public event ToolEvent ToolWorked;
        public delegate void ObjectSeletedDelegate(UIElement obj);
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
                    visualChildren.Remove(activeManipulator);
                    if(activeManipulator is IDisposable)
                        (activeManipulator as IDisposable).Dispose();
                    if ((activeManipulator = value) != null)
                    {
                        visualChildren.Add(activeManipulator);
                        activeManipulator.ObjectChanged += ManipulatorChanged;
                        if (ObjectSelected != null)
                            ObjectSelected(activeManipulator.AdornedElement);
                    }
                    AdornerLayer.GetAdornerLayer(AdornedElement).Update();
                }
            }

        }
        /// <summary>
        /// selected object by active manipulator
        /// if object selected throw this proprty will be create default manipulator for tool instance
        /// </summary>
        public UIElement SelectedObject
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
                        ActiveManipulator = CrateDefaultManipulator(value);
                }
                else
                    ActiveManipulator = null;
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
            AdornedElement.Focus();
            visualChildren.Add(drawingVisual);
        }
        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }
        
        protected override void OnPreviewMouseLeftButtonDown( MouseButtonEventArgs e)
        {
            if(ToolStarted!=null)
                ToolStarted(this,e);

            Point pt = e.GetPosition(this);

            bool isManipulatorHited;
            IInputElement manipulatorHit = null;
            if (ActiveManipulator != null)
                manipulatorHit = ActiveManipulator.InputHitTest(e.GetPosition(ActiveManipulator));
            if (manipulatorHit != null)
                isManipulatorHited = true;
            else isManipulatorHited = false;
            if (isManipulatorHited)
            {
                e.Handled = true;
                return;
            }

            HitTestResult result;
            if (VisualTreeHelper.HitTest(workedSchema.MainCanvas, pt).VisualHit == workedSchema.MainCanvas)
                ActiveManipulator = null;

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
            if(ToolFinished!=null)
                ToolFinished(this,e);
        }
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if(ToolWorked!=null)
                ToolWorked(this,e);
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (activeManipulator != null)
            {
                activeManipulator.Arrange(new Rect(ActiveManipulator.AdornedElement.TranslatePoint(new Point(0, 0), AdornedElement), ActiveManipulator.AdornedElement.RenderSize));
                
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
        protected  void RaiseToolFinished(BaseTool tool, EventArgs e)
        {
            ToolFinished(tool, e);
        }
        protected void RaiseToolStarted(BaseTool tool, EventArgs e)
        {
            ToolStarted(tool, e);
        }
        protected void RaiseToolWorked(BaseTool tool, EventArgs e)
        {
            ToolWorked(tool, e);
        }
        /// <summary>
        /// Each tool must implement this method for defaul manipulator creation
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected abstract BaseManipulator CrateDefaultManipulator(UIElement element);
        protected virtual void ManipulatorChanged(UIElement obj)
        {
            AdornerLayer.GetAdornerLayer(AdornedElement).Update();
        }


    }


}