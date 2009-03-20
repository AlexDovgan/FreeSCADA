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
            Rect rect = new Rect(new Point(0,0), AdornedElement.RenderSize);

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
        public virtual BaseManipulator CreateToolManipulator(UIElement obj)
        {
            return new DragResizeRotateManipulator(obj);//GeometryHilightManipulator(obj);
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        /// 
 
        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            Matrix m = new Matrix();
            m.OffsetX = ((MatrixTransform)transform).Matrix.OffsetX;
            m.OffsetY = ((MatrixTransform)transform).Matrix.OffsetY;

            return transform;//new MatrixTransform(m); ;// //this code neded for right manipulators zooming

        }
        public virtual Type ToolEditingType()
        {
            return null;
        }
       
    }


}
