using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FreeSCADA.Designer.SchemaEditor.PropertyGridTypeEditors;

namespace FreeSCADA.Designer.SchemaEditor.ShortProperties
{
    class FrameworkElementShortProp : CommonShortProp, IDisposable
    {


        DependencyPropertyDescriptor dpdW;
        DependencyPropertyDescriptor dpdH;
        DependencyPropertyDescriptor dpdX;
        DependencyPropertyDescriptor dpdY;
        DependencyPropertyDescriptor dpdA;
        FrameworkElement frameworkElement;

        public FrameworkElementShortProp(FrameworkElement el)
            : base(el)
        {
            frameworkElement = el;
            dpdW = DependencyPropertyDescriptor.FromProperty(FrameworkElement.WidthProperty, typeof(FrameworkElement));
            dpdW.AddValueChanged(el, new EventHandler(propertyValueChanged));
            dpdH = DependencyPropertyDescriptor.FromProperty(FrameworkElement.HeightProperty, typeof(FrameworkElement));
            dpdH.AddValueChanged(el, new EventHandler(propertyValueChanged));
            dpdX = DependencyPropertyDescriptor.FromProperty(Canvas.LeftProperty, typeof(FrameworkElement));
            dpdX.AddValueChanged(el, new EventHandler(propertyValueChanged));
            dpdY = DependencyPropertyDescriptor.FromProperty(Canvas.TopProperty, typeof(FrameworkElement));
            dpdY.AddValueChanged(el, new EventHandler(propertyValueChanged));
            dpdA = DependencyPropertyDescriptor.FromProperty(FrameworkElement.RenderTransformProperty, typeof(FrameworkElement));
            dpdA.AddValueChanged(el, new EventHandler(propertyValueChanged));
        }
        ~FrameworkElementShortProp()
        {
            Dispose();
        }
        public void Dispose()
        {
            dpdW.RemoveValueChanged(frameworkElement, propertyValueChanged);
            dpdH.RemoveValueChanged(frameworkElement, propertyValueChanged);
            dpdX.RemoveValueChanged(frameworkElement, propertyValueChanged);
            dpdY.RemoveValueChanged(frameworkElement, propertyValueChanged);
            dpdA.RemoveValueChanged(frameworkElement, propertyValueChanged);
            GC.SuppressFinalize(this);
        }
        void propertyValueChanged(object sender, EventArgs e)
        {
            //commened beoause UI is lag...
            //RaisePropertiesChanged();


        }


        public string Name
        {
            get { return frameworkElement.Name; }
            set
            {
                try
                {
                    //RaisePropertiesBrowserChanged((UIElement)frameworkElement); 
                    frameworkElement.RegisterName(value, frameworkElement);
                    frameworkElement.Name = value;
                }
                catch (Exception ex)
                {
                    if (ex is ArgumentException)
                        MessageBox.Show("This name is used on this Schema. Pls. try another");

                }
            }
        }

        [Description("Object's X position"), Category("Layout")]
        [Editor(typeof(DoubleEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [OriginalProperty(typeof(Canvas), "Left")]
        public double PosX
        {
             get { return Canvas.GetLeft(frameworkElement); }
             set
             {
                 RaisePropertiesBrowserChanged((UIElement)frameworkElement);
                 Canvas.SetLeft(frameworkElement, value);
             }
          
        }
        [Description("Object's Y position"), Category("Layout")]
        [Editor(typeof(DoubleEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [OriginalProperty(typeof(Canvas), "Top")]
        public double PosY
        {
            get { return Canvas.GetTop(frameworkElement); }
            set
            {
                RaisePropertiesBrowserChanged((UIElement)frameworkElement);
                Canvas.SetTop(frameworkElement, value);
            }
        }

        [Description("Object's Width"), Category("Layout")]
        [Editor(typeof(DoubleEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [OriginalProperty(typeof(FrameworkElement), "Width")]
        public double Width
        {
            get { return frameworkElement.Width; }
            set
            {
                RaisePropertiesBrowserChanged((UIElement)frameworkElement);
                frameworkElement.Width = value;
            }

        }
        [Description("Object's Height"), Category("Layout")]
        [Editor(typeof(DoubleEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [OriginalProperty(typeof(FrameworkElement), "Height")]
        public double Height
        {
            get { return frameworkElement.Height; }
            set
            {
                RaisePropertiesBrowserChanged((UIElement)frameworkElement);
                frameworkElement.Height = value;
            }

        }
        [Description("Object's Rotation angle"), Category("Layout")]
        //[Editor(typeof(DoubleEditor), typeof(System.Drawing.Design.UITypeEditor))]
        //[OriginalProperty(typeof(FrameworkElement), "Angle")]
        public TransformGroup RotateAngle
        {
            get
            {

                //return ((frameworkElement.RenderTransform as TransformGroup).Children[1] as RotateTransform).Angle;
                return frameworkElement.RenderTransform as TransformGroup;
            }
            set
            {
                RaisePropertiesBrowserChanged((UIElement)frameworkElement);
                //((frameworkElement.RenderTransform as TransformGroup).Children[1] as RotateTransform).Angle = value;
                frameworkElement.RenderTransform = value;
            }

        }
        [Description("Object's Z-Order"), Category("Layout")]
        [Editor(typeof(DoubleEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [OriginalProperty(typeof(FrameworkElement), "ZIndex")]
         public int ZOrder
        {
            get { return Canvas.GetZIndex(frameworkElement); }
            set
            {
                RaisePropertiesBrowserChanged((UIElement)frameworkElement);
                Canvas.SetZIndex(frameworkElement, value);
            }

        }

        [Description("Object's Opacity"), Category("Appearence")]
        [Editor(typeof(DoubleEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [OriginalProperty(typeof(FrameworkElement), "Opacity")]
        public double Opacity
        {
            get { return frameworkElement.Opacity; }
            set
            {
                RaisePropertiesBrowserChanged((UIElement)frameworkElement);
                frameworkElement.Opacity = value;
            }

        }
    }
}
