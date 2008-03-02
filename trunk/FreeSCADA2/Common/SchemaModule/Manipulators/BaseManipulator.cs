using System.Windows;
using System.Windows.Media;


namespace FreeSCADA.Schema.Manipulators
{
              
            
    public class BaseManipulator :FrameworkElement//Adorner
    {
        public SchemaDocument workedSchema;
        public UIElement AdornedElement;
        protected VisualCollection visualChildren;
        public BaseManipulator(UIElement adornedElement,SchemaDocument schema)
            //: base(adornedElement)
        {
            workedSchema = schema;
            AdornedElement = adornedElement;
            visualChildren = new VisualCollection(this);
        }
        public delegate void ObjectChangedDelegate(UIElement sender);
         
        public event ObjectChangedDelegate ObjectChanged;
        protected void RaiseObjectChamnedEvent()
        {
            if (ObjectChanged != null)
                ObjectChanged(AdornedElement);

        }

        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }
    }

 }
