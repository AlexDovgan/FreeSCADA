using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using FreeSCADA.Designer.SchemaEditor.Manipulators;

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

    abstract public class BaseTool : Adorner
    {
        /// <summary>
        /// 
        /// </summary>
        protected BaseManipulator toolManipulator;
        /// <summary>
        /// 
        /// </summary>
        protected GridManager GridManager
        {
            get
            {
                return GridManager.GetGridManagerFor(AdornedElement);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected VisualCollection visualChildren;
        /// <summary>
        /// 
        /// </summary>
        protected UIElement workedLayer;
  
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ToolFinished;
        /// <summary>
        /// 
        /// </summary>
		public event EventHandler ToolStarted;
        /// <summary>
        /// 
        /// </summary>
		public event EventHandler ToolWorking;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ObjectCreated;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ObjectDeleted;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ObjectChanged;    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public delegate void ObjectSeletedDelegate(Object obj);
        /// <summary>
        /// 
        /// </summary>
        public event ObjectSeletedDelegate ObjectSelected;
        /// <summary>
        /// 
        /// </summary>
        protected List<UIElement> selectedElements = new List<UIElement>();
        /// <summary>
        /// 
        /// </summary>
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
                       
                        visualChildren.Remove(toolManipulator);
                        toolManipulator.Deactivate();
                    }
                    if ((toolManipulator=value) != null)
                    { 
                        visualChildren.Add(toolManipulator);
                        
                        //toolManipulator.Activate();
                        
                    }
                    InvalidateVisual();
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
                InvalidateVisual();
                RaiseObjectSelected(value);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public BaseTool()
            : base(new UIElement())
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
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
        /// <summary>
        /// 
        /// </summary>
        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
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
            InvalidateVisual();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
		//	NotifyToolFinished();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
        	NotifyToolWorking();
            base.OnPreviewMouseMove(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (toolManipulator != null)
            {
                ToolManipulator.InvalidateVisual(); 
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
        /// <summary>
        /// 
        /// </summary>
        protected  void NotifyToolFinished()
        {
			if(ToolFinished != null)
				ToolFinished(this, new EventArgs());
        }
        /// <summary>
        /// 
        /// </summary>
        protected void NotifyToolStarted()
        {
			if (ToolStarted != null)
				ToolStarted(this, new EventArgs());
        }
        /// <summary>
        /// 
        /// </summary>
        protected void NotifyToolWorking()
        {
			if (ToolWorking != null)
				ToolWorking(this, new EventArgs());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void NotifyObjectCreated(UIElement obj)
        {
            if (ObjectCreated != null)
                ObjectCreated(obj, new EventArgs());

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void NotifyObjectDeleted(UIElement obj)
        {
            if (ObjectDeleted != null)
                ObjectDeleted(obj, new EventArgs());

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected virtual BaseManipulator CreateToolManipulator(UIElement obj)
        {
            return new DragResizeRotateManipulator(obj);//GeometryHilightManipulator(obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        protected void RaiseObjectSelected(Object obj)
        {
            if (ObjectSelected != null)
                ObjectSelected(obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            Matrix m = new Matrix();
            m.OffsetX = ((MatrixTransform)transform).Matrix.OffsetX;
            m.OffsetY = ((MatrixTransform)transform).Matrix.OffsetY;

            return transform;//new MatrixTransform(m); ;// //this code neded for right manipulators zooming

        }
     
       
    }


}
