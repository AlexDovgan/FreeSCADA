using System;
using System.Windows;
using System.Windows.Media;
using FreeSCADA.Common.Schema;
using FreeSCADA.Designer.SchemaEditor.Manipulators.Controlls;

namespace FreeSCADA.Designer.SchemaEditor.Manipulators
{
              
    /// <summary>
    /// Base Class for manipulators
    /// 
    /// /// </summary>
        
    class BaseManipulator :FrameworkElement//Adorner
    {
        
        /// <summary>
        /// Element that manipulator is decorate
        /// </summary>
        public UIElement AdornedElement;
        /// <summary>
        /// Container for manipulator controlls
        /// </summary>
        protected VisualCollection visualChildren;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="adornedElement"></param>
        /// 
  
        public BaseManipulator(UIElement adornedElement)
            //: base(adornedElement)
        {
            ThumbsResources tr = new ThumbsResources();
            tr.InitializeComponent();
            Resources = tr;

            
            AdornedElement = adornedElement;
            if (!(AdornedElement .RenderTransform is TransformGroup))
            {
                TransformGroup t = new TransformGroup();
                t.Children.Add(new MatrixTransform());
                t.Children.Add(new RotateTransform());
                AdornedElement.RenderTransform = t;
            }
            AdornedElement.RenderTransformOrigin = new Point(0.5, 0.5); 
            visualChildren = new VisualCollection(this);
        }
        
        /// <summary>
        /// delegate for ObjectChanged event
        /// </summary>
        /// <param name="sender"></param>
        public delegate void ObjectChangedDelegate(UIElement sender);
         /// <summary>
         /// event thet emit when decorated element is changed
         /// </summary>
        public event ObjectChangedDelegate ObjectChanged;
        public event ObjectChangedDelegate ObjectChangedPreview;
        
        protected void  RaiseObjectChamnedPrevewEvent()
        {
            if (ObjectChangedPreview != null)
                ObjectChangedPreview(AdornedElement);
        }
        protected void RaiseObjectChamnedEvent()
        {
            if (ObjectChanged != null)
                ObjectChanged(AdornedElement);

        }
        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }
        public virtual void Activate()
        {
        }
        public virtual void Deactivate()
        {
        }


    }

 }
