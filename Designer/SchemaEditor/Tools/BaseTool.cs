using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using FreeSCADA.Designer.SchemaEditor.Manipulators;
using System.Collections.Generic;

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
    /// 

    abstract class BaseTool : Adorner
    {
        BaseManipulator toolManipulator;
        protected VisualCollection visualChildren;
        protected UIElement workedLayer;
  
        
        public event EventHandler ToolFinished;
		public event EventHandler ToolStarted;
		public event EventHandler ToolWorking;
        public event EventHandler ObjectCreated;
        public event EventHandler ObjectDeleted;
        public event EventHandler ObjectChanged;    
        public delegate void ObjectSeletedDelegate(Object obj);
        public event ObjectSeletedDelegate ObjectSelected;

        protected List<UIElement> selectedElements = new List<UIElement>();
        public List<UIElement> SelectedObjects
        {
            get { return selectedElements; }
        }

        /// <summary>
        /// active manipulator upon  selected object created by tool
        /// may be as default manipulator so as an another manipulator that can be created by tool instance
        /// </summary>
        public BaseManipulator ToolManipulator
        {
            get
            {
                return toolManipulator;
            }
            set
            {
                if (toolManipulator != value)
                {
                    if (toolManipulator != null)
                    {
                        toolManipulator.ObjectChangedPreview -= OnObjectChanged;
                        visualChildren.Remove(toolManipulator);
                        toolManipulator.Deactivate();
                    }
                    if ((toolManipulator=value) != null)
                    { 
                        visualChildren.Add(toolManipulator);
                        toolManipulator.ObjectChangedPreview += OnObjectChanged;
                        //toolManipulator.Activate();
                        
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
                if (selectedElements.Count>0)
                    return selectedElements[selectedElements.Count-1];
                else return null;
            }
            set
            {
                selectedElements.Clear();
                if (value != null)
                {
                    if (ToolManipulator == null || ToolManipulator.AdornedElement != value)
                    {
                        ToolManipulator = CreateToolManipulator(value);
                        if (ToolManipulator != null)
                            ToolManipulator.InvalidateArrange();
                    }
                    selectedElements.Add(value);
                }
                else
                    ToolManipulator = null;
                AdornerLayer.GetAdornerLayer(AdornedElement).Update();
                RaiseObjectSelected(value);
            }

        }

        public BaseTool(UIElement element)
            : base(element)
        {
            workedLayer = element;
            visualChildren = new VisualCollection(this);

            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            Rect rect = new Rect(new Point(0,0), AdornedElement.DesiredSize);

            drawingContext.DrawRectangle(Brushes.Black, new Pen(Brushes.Black, 0.2), rect);

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
            if (ToolManipulator != null)
                manipulatorHit = ToolManipulator.InputHitTest(e.GetPosition(ToolManipulator));
     
            if (manipulatorHit!=null)
            {
                e.Handled = true;
                return;
            }

            HitTestResult result = VisualTreeHelper.HitTest(AdornedElement, pt);
            if (result == null || result.VisualHit == AdornedElement)
                SelectedObject = null;

            else
                if ((result = VisualTreeHelper.HitTest(AdornedElement, pt)).VisualHit != AdornedElement)
                {
                    FrameworkElement el = (FrameworkElement)EditorHelper.FindTopParentUnder(AdornedElement, (DependencyObject)result.VisualHit);
                    SelectedObject = el;
                    //e.Handled = true;

                }
            AdornerLayer.GetAdornerLayer(AdornedElement).Update();
        }
    
        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
		//	NotifyToolFinished();
        }
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
			NotifyToolWorking();
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (toolManipulator != null)
            {
        
                toolManipulator.Arrange(new Rect(finalSize));
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
        public void Update()
        {
            if (ToolManipulator != null)
            {
                ToolManipulator.InvalidateMeasure();
                ToolManipulator.InvalidateArrange();
            }
        }
        /// <summary>
        /// tool deactiavaion on working Canvas
        ///
        /// </summary>
        public virtual void Deactivate()
        {
            SelectedObject=null;
            if(AdornerLayer.GetAdornerLayer(AdornedElement)!=null)
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
        public void NotifyObjectCreated(UIElement obj)
        {
            if (ObjectCreated != null)
                ObjectCreated(obj, new EventArgs());

        }
        public void NotifyObjectDeleted(UIElement obj)
        {
            if (ObjectDeleted != null)
                ObjectDeleted(obj, new EventArgs());

        }
        public void OnObjectChanged(UIElement obj)
        {
            if (ObjectChanged != null)
                ObjectChanged(obj, new EventArgs()); 

        }
        protected virtual BaseManipulator CreateToolManipulator(UIElement obj)
        {
            return new DragResizeRotateManipulator(obj);//GeometryHilightManipulator(obj);
        }
        protected void RaiseObjectSelected(Object obj)
        {
            if (ObjectSelected != null)
                ObjectSelected(obj);
        }
        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            Matrix m = new Matrix();
            m.OffsetX = ((MatrixTransform)transform).Matrix.OffsetX;
            m.OffsetY = ((MatrixTransform)transform).Matrix.OffsetY;

            return transform;//new MatrixTransform(m); //this code neded for right manipulators zooming

        }
       
    }


}
