using System.Windows;
using System.Windows.Media;
using FreeSCADA.Common.Schema;

namespace FreeSCADA.Designer.SchemaEditor.Manipulators
{
              
    /// <summary>
    /// Base Class for manipulators
    /// 
    /// /// </summary>
        
    class BaseManipulator :FrameworkElement//Adorner
    {
        /// <summary>
        /// Shcema document wich manipulator  belong
        /// </summary>
        public SchemaDocument workedSchema;
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
        /// <param name="schema"></param>
        public BaseManipulator(UIElement adornedElement,SchemaDocument schema)
            //: base(adornedElement)
        {
            workedSchema = schema;
            AdornedElement = adornedElement;
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
        /// method for raising ObjectChanged event
        /// </summary>
        protected void RaiseObjectChamnedEvent()
        {
            if (ObjectChanged != null)
                ObjectChanged(AdornedElement);

        }
        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }
    }

 }
