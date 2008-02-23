using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;
using FreeSCADA.Schema.Commands;

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
