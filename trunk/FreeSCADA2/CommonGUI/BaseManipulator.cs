using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace FreeSCADA.Common
{
              
    /// <summary>
    /// Base Class for manipulators
    /// 
    /// /// </summary>

    public class BaseManipulator : Adorner,IObjectEditor
    {   
        /// <summary>
        /// Container for manipulator controlls
        /// </summary>
        protected VisualCollection visualChildren;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        //
        public event ObjectChanged ObjectChangedEvent;

        protected IDocumentView _view;


        public BaseManipulator(IDocumentView view,FrameworkElement el)
            : base(el)
        {
            _view = view;
            if (!(AdornedElement.RenderTransform is TransformGroup))
            {
                
                TransformGroup t = new TransformGroup();
                t.Children.Add(new MatrixTransform());
                t.Children.Add(new RotateTransform());
                AdornedElement.RenderTransform = t;
                AdornedElement.RenderTransformOrigin = new Point(0.5, 0.5);
                
            }
            this.Visibility = Visibility.Collapsed;                       
            visualChildren = new VisualCollection(this);
            
       
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
        public virtual void Activate()
        {
            this.Visibility = Visibility.Visible;
            InvalidateVisual();
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void Deactivate()
        {
            this.Visibility = Visibility.Collapsed;
            InvalidateVisual();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <returns></returns>
        public virtual bool IsApplicableFor(FrameworkElement el)
        {
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            var m = new Matrix();
            m.OffsetX = ((MatrixTransform)transform).Matrix.OffsetX;
            m.OffsetY = ((MatrixTransform)transform).Matrix.OffsetY;

            return new MatrixTransform();//new MatrixTransform(m); ;// //this code neded for right manipulators zooming
            
        }
        protected void RaiseObjectChanged(IUndoCommand cmd)
        {
            if (ObjectChangedEvent != null)
                ObjectChangedEvent(cmd);
        }
     

    }

 }
