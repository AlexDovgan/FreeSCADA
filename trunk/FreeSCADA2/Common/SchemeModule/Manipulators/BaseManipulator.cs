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
using FreeSCADA.Scheme.Commands;
using FreeSCADA.Scheme.Helpers;

namespace FreeSCADA.Scheme.Manipulators
{

    public class BaseManipulator : Adorner
    {
        public BaseManipulator(UIElement adornedElement)
            : base(adornedElement)
        {

        }
        public delegate void ObjectChanged(UIElement sender);
        event ObjectChanged Changed;
    }

 }
