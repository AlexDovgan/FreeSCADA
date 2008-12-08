using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FreeSCADA.Common.Schema;

namespace FreeSCADA.Designer.SchemaEditor.ShortProperties
{
    class CanvasShortProp : CommonShortProp, IDisposable
    {
        DependencyPropertyDescriptor dpdW;
        DependencyPropertyDescriptor dpdH;
        FrameworkElement frameworkElement;

        public CanvasShortProp(FrameworkElement el)
            : base(el)
        {
            frameworkElement = el;
            dpdW = DependencyPropertyDescriptor.FromProperty(FrameworkElement.WidthProperty, typeof(FrameworkElement));
            dpdW.AddValueChanged(el, new EventHandler(propertyValueChanged));
            dpdH = DependencyPropertyDescriptor.FromProperty(FrameworkElement.HeightProperty, typeof(FrameworkElement));
            dpdH.AddValueChanged(el, new EventHandler(propertyValueChanged));
        }

        ~CanvasShortProp()
        {
            Dispose();
        }

        public void Dispose()
        {
            dpdW.RemoveValueChanged(frameworkElement, propertyValueChanged);
            dpdH.RemoveValueChanged(frameworkElement, propertyValueChanged);
            GC.SuppressFinalize(this);
        }

        void propertyValueChanged(object sender, EventArgs e)
        {
        }

        [Description("Object's Width"), Category("Layout")]
        public double Width
        {
            get { return frameworkElement.Width; }
            set
            {
                RaisePropertiesBrowserChanged((UIElement)frameworkElement);
                EditorHelper.SetDependencyProperty(frameworkElement, FrameworkElement.WidthProperty, value);
            }
        }

        [Description("Object's Height"), Category("Layout")]
        public double Height
        {
            get { return frameworkElement.Height; }
            set
            {
                RaisePropertiesBrowserChanged((UIElement)frameworkElement);
                EditorHelper.SetDependencyProperty(frameworkElement, FrameworkElement.HeightProperty, value);
            }
        }

        [Description("Object's Fill color"), Category("Appearence")]
        public System.Drawing.Color FillColor
        {
            get
            {
                System.ComponentModel.TypeConverter ccv = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Drawing.Color));
                if ((frameworkElement as Canvas).Background is SolidColorBrush)
                    return (System.Drawing.Color)ccv.ConvertFromString(((frameworkElement as Canvas).Background as SolidColorBrush).Color.ToString());
                else
                    return (System.Drawing.Color)ccv.ConvertFromString(((((frameworkElement as Canvas).Background as DrawingBrush).Drawing as GeometryDrawing).Brush as SolidColorBrush).Color.ToString());
            }
            set
            {
                RaisePropertiesBrowserChanged((UIElement)frameworkElement);
                if ((frameworkElement as Canvas).Background is SolidColorBrush)
                {
                    (frameworkElement as Canvas).Background = (frameworkElement as Canvas).Background.Clone();
                    ((frameworkElement as Canvas).Background as SolidColorBrush).Color = Color.FromArgb(value.A, value.R, value.G, value.B); ;
                }
                else
                {
                    (((frameworkElement as Canvas).Background as DrawingBrush).Drawing as GeometryDrawing).Brush.Clone();
                    ((((frameworkElement as Canvas).Background as DrawingBrush).Drawing as GeometryDrawing).Brush as SolidColorBrush).Color = Color.FromArgb(value.A, value.R, value.G, value.B); ;
                }
            }
        }

        [Description("Grid active"), Category("Appearence")]
        public bool GridOn
        {
            get
            {
                if (!frameworkElement.Resources.Contains("DesignerSettings_GridOn"))
                {
                    frameworkElement.Resources.Add("DesignerSettings_GridOn", true);
                }
                return (bool)frameworkElement.FindResource("DesignerSettings_GridOn");
            }
            set
            {
                RaisePropertiesBrowserChanged((UIElement)frameworkElement);
                if (frameworkElement.Resources.Contains("DesignerSettings_GridOn"))
                {
                    frameworkElement.Resources.Remove("DesignerSettings_GridOn");
                }
                frameworkElement.Resources.Add("DesignerSettings_GridOn", value);
                WPFShemaContainer.ViewGrid(frameworkElement as Canvas, value);
            }
        }

        [Description("Grid delta"), Category("Appearence")]
        public double GridDelta
        {
            get
            {
                if (!frameworkElement.Resources.Contains("DesignerSettings_GridDelta"))
                {
                    frameworkElement.Resources.Add("DesignerSettings_GridDelta", 10.0);
                }
                return (double)frameworkElement.FindResource("DesignerSettings_GridDelta");
            }
            set
            {
                RaisePropertiesBrowserChanged((UIElement)frameworkElement);
                if (frameworkElement.Resources.Contains("DesignerSettings_GridDelta"))
                {
                    frameworkElement.Resources.Remove("DesignerSettings_GridDelta");
                }
                frameworkElement.Resources.Add("DesignerSettings_GridDelta", value);
                WPFShemaContainer.ViewGrid(frameworkElement as Canvas, false);
                if ((bool)(frameworkElement as Canvas).FindResource("DesignerSettings_GridOn") == true)
                {
                    WPFShemaContainer.ViewGrid(frameworkElement as Canvas, true);
                }
            }
        }
    }

}
