using System.Windows;
using System.Windows.Media;
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
        protected UIElement adornedElement;
        /// <summary>
        /// Container for manipulator controlls
        /// </summary>
        protected VisualCollection visualChildren;

        public  UIElement AdornedElement
        {
            get { return adornedElement; }
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

        public BaseManipulator(UIElement element)
            //: base(element)
        {
            adornedElement = element;
            if (!(adornedElement.RenderTransform is TransformGroup))
            {
                TransformGroup t = new TransformGroup();
                t.Children.Add(new MatrixTransform());
                t.Children.Add(new RotateTransform());
                adornedElement.RenderTransform = t;
                adornedElement.RenderTransformOrigin = new Point(0.5, 0.5);

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
        public event ObjectChangedDelegate ObjectChangedPreview;
        
        protected void  RaiseObjectChangedPrevewEvent()
        {
            if (ObjectChangedPreview != null)
                ObjectChangedPreview(AdornedElement);
        }
        protected void RaiseObjectChangedEvent()
        {
            if (ObjectChanged != null)
                ObjectChanged(AdornedElement);

        }
        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }

        

        public virtual void Activate()
        {
            this.Visibility = Visibility.Visible;
            //UpdateLayout();
            InvalidateMeasure();
            InvalidateArrange();
        }
        public virtual void Deactivate()
        {
            this.Visibility = Visibility.Collapsed;
            //UpdateLayout();
            InvalidateMeasure();
            InvalidateArrange();
        }
        public virtual bool IsSelactable(UIElement el)
        {
            return true;
        }

    }

 }
