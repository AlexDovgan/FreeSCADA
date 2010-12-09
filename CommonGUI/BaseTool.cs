using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;


namespace FreeSCADA.Common
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
        protected VisualCollection visualChildren;

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

        protected IDocumentView _view;

        /// <summary>
        /// 
        /// </summary>
        protected BaseTool()
            : base(new FrameworkElement())
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        protected BaseTool(IDocumentView view)
            : base(view.MainPanel)
        {
            _view = view;
            visualChildren = new VisualCollection(this);
            DrawingVisual drawingVisual = new DrawingVisual();
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
        protected void ReinitTool()
        {

            Deactivate();
            Activate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <summary>
        /// tool activating on working  Canvas
        /// </summary>
        /// 
        public virtual void Activate()
        {
            
            Rect rect = new Rect(new Point(0, 0), new Size((AdornedElement as FrameworkElement).ActualWidth, (AdornedElement as FrameworkElement).ActualHeight));
            DrawingContext drawingContext = ((DrawingVisual)visualChildren[0]).RenderOpen();
            drawingContext.DrawRectangle(Brushes.Black, new Pen(Brushes.Black, 0.2), rect);
            drawingContext.Close();
            ((FrameworkElement)AdornedElement).Loaded += new RoutedEventHandler(BaseTool_Loaded);
            ((FrameworkElement) AdornedElement).SizeChanged += new SizeChangedEventHandler(AdornedElementSizeChanged);
            AdornerLayer.GetAdornerLayer(AdornedElement).Add(this);
            _view.SelectionManager.UpdateManipulator();
            
        }

        void BaseTool_Loaded(object sender, RoutedEventArgs e)
        {
            ReinitTool();
        }

        void AdornedElementSizeChanged(object sender, SizeChangedEventArgs e)
        {
         
            ReinitTool();
        }

        /// <summary>
        /// tool deactiavaion on working Canvas
        ///
        ///     </summary>
        public virtual void Deactivate()
        {
            if (AdornerLayer.GetAdornerLayer(AdornedElement) != null)
            {
                AdornerLayer.GetAdornerLayer(AdornedElement).Remove(this);
                AdornerLayer.GetAdornerLayer(AdornedElement).UpdateLayout();
            }
            ((FrameworkElement)AdornedElement).Loaded -= new RoutedEventHandler(BaseTool_Loaded);
            (AdornedElement as FrameworkElement).SizeChanged -= new SizeChangedEventHandler(AdornedElementSizeChanged);
            visualChildren.Clear();
        }
        /// <summary>
        /// 
        /// </summary>
        protected void NotifyToolFinished()
        {
            if (ToolFinished != null)
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

            ((FrameworkElement)obj).Tag = this.GetType().Name;
            if (ObjectCreated != null)
                ObjectCreated(obj, new EventArgs());

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract Type GetToolManipulator();


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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Type ToolEditingType()
        {
            return null;
        }

    }


}
