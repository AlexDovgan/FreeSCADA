using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FreeSCADA.Schema.ShortProperties
{
    public class CommonShortProp
    {
        public delegate void PropertiesChangedDelegate();
        public event PropertiesChangedDelegate PropertiesChanged;

        public CommonShortProp(object obj)
        {
            commonObject = obj;
        }
        public string ObjectType
        {
            get { return commonObject.GetType().ToString(); }
        }
        public void RaisePropertiesChanged()
        {
            if (PropertiesChanged!=null)
                PropertiesChanged();
        }
        object commonObject;
    }


    public class FrameworkElementShortProp : CommonShortProp
    {
        public FrameworkElementShortProp(FrameworkElement el)
            : base(el)
        {
            frameworkElement = el;
            frameworkElement.LayoutUpdated += new EventHandler(frameworkElement_LayoutUpdated);
        }

        void frameworkElement_LayoutUpdated(object sender, EventArgs e)
        {
            RaisePropertiesChanged();
        }
        public string Name
        {
            get { return frameworkElement.Name; }
            set { frameworkElement.Name = value; }
        }

        public double PosX
        {
            get { return Canvas.GetLeft(frameworkElement); }
            set { Canvas.SetLeft(frameworkElement, value); }
        }
        public double PosY
        {
            get { return Canvas.GetTop(frameworkElement); }
            set { Canvas.SetTop(frameworkElement, value); }
        }


        public double Width
        {
            get { return frameworkElement.Width; }
            set { frameworkElement.Width = value; }

        }
        public double Height
        {
            get { return frameworkElement.Height; }
            set { frameworkElement.Height = value; }

        }

        FrameworkElement frameworkElement;
    }

    static class ShortPropFactory
    {
        public static CommonShortProp CreateShortPropFrom(object obj)
        {
            if (obj is FrameworkElement)
                return new FrameworkElementShortProp(obj as FrameworkElement);
            return new CommonShortProp(obj);

        }
   }
}
