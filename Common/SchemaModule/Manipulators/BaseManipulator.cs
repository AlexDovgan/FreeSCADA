using System.Windows;
using System.Windows.Documents;

namespace FreeSCADA.Schema.Manipulators
{
    public class BaseManipulator : Adorner
    {
        public SchemaDocument workSchema;
        public BaseManipulator(UIElement adornedElement,SchemaDocument schema)
            : base(adornedElement)
        {
            workSchema = schema;
        }
        /*
        public delegate void ObjectSelectedDelegate(FrameworkElement sender);
        public delegate void ObjectChangedDelegate(FrameworkElement sender);
        public event ObjectChangedDelegate Changed;
        public event ObjectSelectedDelegate Changed;
        */
    }

 }
