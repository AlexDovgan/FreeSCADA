using System.Windows;
using System.Windows.Media;
using FreeSCADA.Designer.SchemaEditor.Manipulators.Controls;

namespace FreeSCADA.Designer.SchemaEditor.Manipulators
{
              
    /// <summary>
    /// Base Class for manipulators
    /// 
    /// /// </summary>
        
    public class BaseManipulator :FrameworkElement//Adorner
    {
        
        /// <summary>
        /// Element that manipulator is decorate
        /// </summary>
        protected UIElement adornedElement_;
        /// <summary>
        /// Container for manipulator controlls
        /// </summary>
        protected VisualCollection visualChildren;
        /// <summary>
        /// 
        /// </summary>
        public  UIElement AdornedElement
        {
            get { return adornedElement_; }
            /*set
            {
                if (adornedElement == value) return;
                if (adornedElement!=null) Deactivate();

                adornedElement = value;
                if (adornedElement != null)
                {
                    if (!(adornedElement.RenderTransform is TransformGroup))
                    {
                        TransformGroup t = new TransformGroup();
                        t.Children.Add(new MatrixTransform());
                        t.Children.Add(new RotateTransform());
                        adornedElement.RenderTransform = t;
                        adornedElement.RenderTransformOrigin = new Point(0.5, 0.5);

                    }
 
                    Activate();
                }
      
            }*/
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public BaseManipulator(UIElement element)
            //: base(element)
        {
            adornedElement_ = element;
            if (!(adornedElement_.RenderTransform is TransformGroup))
            {
                TransformGroup t = new TransformGroup();
                t.Children.Add(new MatrixTransform());
                t.Children.Add(new RotateTransform());
                adornedElement_.RenderTransform = t;
                adornedElement_.RenderTransformOrigin = new Point(0.5, 0.5);

            }
            ThumbsResources tr = new ThumbsResources();
            tr.InitializeComponent();
            Resources = tr;
            this.Visibility = Visibility.Collapsed;                       
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
        /// <summary>
        /// 
        /// </summary>
        public event ObjectChangedDelegate ObjectChangedPreview;
        /// <summary>
        /// 
        /// </summary>
        protected void  RaiseObjectChangedPrevewEvent()
        {
            if (ObjectChangedPreview != null)
                ObjectChangedPreview(AdornedElement);
        }
        /// <summary>
        /// 
        /// </summary>
        protected void RaiseObjectChangedEvent()
        {
            if (ObjectChanged != null)
                ObjectChanged(AdornedElement);

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
            //UpdateLayout();
            InvalidateMeasure();
            InvalidateArrange();
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void Deactivate()
        {
            this.Visibility = Visibility.Collapsed;
            //UpdateLayout();
            InvalidateMeasure();
            InvalidateArrange();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <returns></returns>
        public virtual bool IsSelactable(UIElement el)
        {
            return true;
        }

    }

 }
