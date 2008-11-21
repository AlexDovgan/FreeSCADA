using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows.Documents;
using System.Windows.Markup;
using FreeSCADA.Common.Schema;



namespace FreeSCADA.Designer.SchemaEditor.ShortProperties
{
    class CommonShortProp
    {
        public delegate void PropertiesChangedDelegate();
        public event PropertiesChangedDelegate PropertiesChanged;

        public delegate void PropertiesBrowserChangedDelegate(UIElement el);
        public event PropertiesBrowserChangedDelegate PropertiesBrowserChanged;
 
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
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(NotifyPropertyChangedAsync), this);
        }

        public void RaisePropertiesBrowserChanged(UIElement el)
        {
            if (PropertiesBrowserChanged != null)
                PropertiesBrowserChanged(el);
        }

        protected static void NotifyPropertyChangedAsync(Object info)
        {
            CommonShortProp obj = (CommonShortProp)info;
            if (obj.PropertiesChanged != null)
                obj.PropertiesChanged();
        }

        object commonObject;

    }


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
                frameworkElement.Width = value;
            }
        }

        [Description("Object's Height"), Category("Layout")]
        public double Height
        {
            get { return frameworkElement.Height; }
            set 
            {
                RaisePropertiesBrowserChanged((UIElement)frameworkElement); 
                frameworkElement.Height = value;
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
        [ReadOnly(true)]
        [Description("Object's X position"), Category("Layout")]
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
        public double RotateAngle
        {
            get { return ((frameworkElement.RenderTransform as TransformGroup).Children[1] as RotateTransform).Angle; }
            set 
            {
                RaisePropertiesBrowserChanged((UIElement)frameworkElement); 
                ((frameworkElement.RenderTransform as TransformGroup).Children[1] as RotateTransform).Angle = value;
            }

        }
        [Description("Object's Z-Order"), Category("Layout")]
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

    class ShapeShortProp : FrameworkElementShortProp
    {

        public ShapeShortProp(Shape shp)
            : base(shp)
        {
            shape = shp;

        }
        [Description("Object's Fill color"), Category("Appearence")]
        public System.Drawing.Color FillColor
        {
            get
            {
                System.ComponentModel.TypeConverter ccv = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Drawing.Color));
                return (System.Drawing.Color)ccv.ConvertFromString((shape.Fill as SolidColorBrush).Color.ToString());

            }
            set
            {
                RaisePropertiesBrowserChanged((UIElement)shape); 
                shape.Fill = shape.Fill.Clone();
                (shape.Fill as SolidColorBrush).Color = Color.FromArgb(value.A, value.R, value.G, value.B); ;

            }
        }
        [Description("Object's Stroke color"), Category("Appearence")]
        public System.Drawing.Color StrokeColor
        {
            get
            {
                System.ComponentModel.TypeConverter ccv = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Drawing.Color));
                return (System.Drawing.Color)ccv.ConvertFromString((shape.Stroke as SolidColorBrush).Color.ToString());

            }
            set
            {
                RaisePropertiesBrowserChanged((UIElement)shape); 
                shape.Stroke = shape.Stroke.Clone();
                (shape.Stroke as SolidColorBrush).Color = Color.FromArgb(value.A, value.R, value.G, value.B); ;

            }
        }
        [Description("Object's Stroke thickness"), Category("Appearence")]
        public double StrokeThickness
        {
            get
            {

                return shape.StrokeThickness;

            }
            set
            {
                RaisePropertiesBrowserChanged((UIElement)shape); 
                shape.StrokeThickness = value;

            }
        }
        Shape shape;
        //System.Drawing.Brush brush;
    }

    class ControlShotProp : FrameworkElementShortProp
    {
        Control control;
        public ControlShotProp(Control c)
            : base(c)
        {
            control = c;
        }
        [EditorAttribute(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Object's Style"), Category("Appearence")]
        public string Style
        {
            get 
            {
                FrameworkElement c = (FrameworkElement)control.Parent;
                if (control.Style != null)
                {
                    foreach (System.Collections.DictionaryEntry  de in c.Resources)
                    {
                        if (de.Value == control.Style)
                            return de.Key.ToString();
                       
                    }
                    return control.Style.GetType().Name;
                }
                else return "";
            }   
            set
            {
                RaisePropertiesBrowserChanged((UIElement)control); 
                Style st = (Style)XamlReader.Load(File.Open(value, FileMode.Open));
                FrameworkElement c = (FrameworkElement)control.Parent;
                if (c.Resources == null)
                    c.Resources = new ResourceDictionary();
                if(!(c.Resources as ResourceDictionary).Contains(System.IO.Path.GetFileNameWithoutExtension(value)))
                    (c.Resources as ResourceDictionary).Add(System.IO.Path.GetFileNameWithoutExtension(value), st);
                control.SetResourceReference(FrameworkElement.StyleProperty, System.IO.Path.GetFileNameWithoutExtension(value));
                
            }
        }


    }

    class ContentShortProp : ControlShotProp
    {
        ContentControl conentc;
        public ContentShortProp(ContentControl c)
            : base(c)
        {
            conentc = c;
        }
        [Description("Content property"), Category("Appearence")]
        [EditorAttribute(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string Content
        {
            get { return conentc.Content as string; }
            set
            {
                RaisePropertiesBrowserChanged((UIElement)conentc); 
                if (!File.Exists(value))
                    conentc.Content = value;
                else
                {
                    bool isSeted = false;
                    DataObject Do = new DataObject(value);

                    if (!isSeted)
                        try
                        {
                            Image simpleImage = new Image();
                            simpleImage.Stretch = Stretch.Fill;
                            // Create source.
                            BitmapImage bi = new BitmapImage();
                            // BitmapImage.UriSource must be in a BeginInit/EndInit block.
                            bi.BeginInit();
                            bi.UriSource = new Uri(value, UriKind.RelativeOrAbsolute);
                            bi.EndInit();
                            // Set the image source.
                            simpleImage.Source = bi;

                            conentc.Content = simpleImage;
                            isSeted = true;

                        }
                        catch (Exception)
                        {
                        }
                    /*if (!isSeted)
                        try
                        {
                            MediaElement media = new MediaElement();
                            media.Stretch = Stretch.Fill;
                            media.Source = new Uri(value, UriKind.RelativeOrAbsolute);

                            conentc.Content = media;
                            //media.Play();
                        }
                        catch (Exception)
                        {
                        }
                    if (!isSeted)
                        try
                        {
                            FlowDocumentScrollViewer fw = new FlowDocumentScrollViewer();
                            fw.Document = new FlowDocument();
                            TextRange tr=new TextRange(fw.Document.ContentStart,fw.Document.ContentEnd);
                            tr.Load(File.Open(value, FileMode.Open), System.IO.Path.GetExtension(value));
                            conentc.Content = fw;
                        }
                        catch (Exception)
                        {
                        }

                    */

                }
            }
        }
    }
    class RangeBaseShortProp : ControlShotProp
    {
        public RangeBaseShortProp(RangeBase rb)
            : base(rb)
        {
            rangebase = rb;
        }
        [Description("Minimum Scale Value"), Category("Appearence")]
        public double MinValue
        {
            get { return rangebase.Minimum; }
            set 
            {
                RaisePropertiesBrowserChanged((UIElement)rangebase); 
                rangebase.Minimum = value;
            }
        }
        [Description("Maximum Scale Value"), Category("Appearence")]
        public double MaxValue
        {
            get { return rangebase.Maximum; }
            set 
            {
                RaisePropertiesBrowserChanged((UIElement)rangebase); 
                rangebase.Maximum = value;
            }
        }
        public double Value
        {
            get { return rangebase.Value; }
            set { rangebase.Value = value; }
        }
        RangeBase rangebase;

    }

    class UIElementTypeDescriptionProvider : TypeDescriptionProvider
    {
        private static TypeDescriptionProvider defaultTypeProvider =
                       TypeDescriptor.GetProvider(typeof(System.Windows.UIElement));

        public UIElementTypeDescriptionProvider()
            : base(defaultTypeProvider)
        {
        }

        public override ICustomTypeDescriptor GetExtendedTypeDescriptor(object instance)
        {
            ICustomTypeDescriptor defaultDescriptor =
                                  base.GetExtendedTypeDescriptor(instance);

            return instance == null ? defaultDescriptor :
                new UIElementCustomTypeDescriptor(defaultDescriptor);
           
        }
        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType,
                                                                object instance)
        {
            ICustomTypeDescriptor defaultDescriptor =
                                  base.GetTypeDescriptor(objectType, instance);

            return instance == null ? defaultDescriptor :
                new UIElementCustomTypeDescriptor (defaultDescriptor);
        }
    }

    class UIElementCustomTypeDescriptor : CustomTypeDescriptor
    {
        ICustomTypeDescriptor defaultTypeDescriptor;
        bool isExtended;
        public UIElementCustomTypeDescriptor(ICustomTypeDescriptor parent)
            : base(parent)
        {
            defaultTypeDescriptor = parent;
           
        }
        
        public override PropertyDescriptorCollection GetProperties()
        {
            return GetProperties(
                new Attribute[] { new PropertyFilterAttribute(PropertyFilterOptions.All) });
            
            
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            
            List<string> props=new List<string>(
                new string[]{
                    "Width",
                    "Height",
                    "Background",
                    "Opacity",
                    "Canvas.Top",
                    "Canvas.Left",
                    "Canvas.ZIndex",
                    "Value",
                    "Maximum",
                    "Mimimum",
                    "Content",
                    "Style",
                    "StrokeThickness",
                    "Stroke",
                    "Fill",
                    "RenderTransform",
                    "RenderTransformOrigin",
                    "Name",
                    "ContentTemplate",
                    "Orientation"

                    }
                    );
            IEnumerable<PropertyDescriptor> ie ;
           // if (!isExtended)
                ie = base.GetProperties(attributes).Cast<PropertyDescriptor>()
                    .Where(x => DependencyPropertyDescriptor.FromProperty(x)!=null
                        &&props.Contains(x.Name) == true);
            //else ie = new List<PropertyDescriptor>();
            return new PropertyDescriptorCollection(ie.ToArray());
            
            //props.Exists(y => y == x.Name)
            //return new PropertyDescriptorCollection(base.GetProperties(attributes).Cast<PropertyDescriptor>().Where(x => DependencyPropertyDescriptor.FromProperty(x) != null&&x.IsBrowsable==true).ToArray());
            
        }
        
    }
}
