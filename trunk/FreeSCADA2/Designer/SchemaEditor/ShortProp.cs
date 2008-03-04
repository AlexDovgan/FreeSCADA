using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Data;
using System.ComponentModel;

namespace FreeSCADA.Designer.SchemaEditor.ShortProperties
{
    class CommonShortProp 
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
			System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(NotifyPropertyChangedAsync), this);
        }

		protected static void NotifyPropertyChangedAsync(Object info)
		{
			CommonShortProp obj = (CommonShortProp)info;
			if (obj.PropertiesChanged != null)
				obj.PropertiesChanged();
		}
        
        object commonObject;
        
    }




    class FrameworkElementShortProp : CommonShortProp, IDisposable
    {
      

        DependencyPropertyDescriptor dpdW;
        DependencyPropertyDescriptor dpdH;
        DependencyPropertyDescriptor dpdX;
        DependencyPropertyDescriptor dpdY;
        DependencyPropertyDescriptor dpdA;

        public FrameworkElementShortProp(FrameworkElement el)
            : base(el)
        {
            frameworkElement = el;
            dpdW = DependencyPropertyDescriptor.FromProperty(FrameworkElement.WidthProperty, typeof(FrameworkElement));
            dpdW.AddValueChanged(el, new EventHandler(propertyValueChanged));
            dpdH = DependencyPropertyDescriptor.FromProperty(FrameworkElement.HeightProperty, typeof(FrameworkElement));
            dpdH.AddValueChanged(el, new EventHandler(propertyValueChanged));
            dpdX=DependencyPropertyDescriptor.FromProperty(Canvas.LeftProperty, typeof(FrameworkElement));
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
                    frameworkElement.RegisterName(value,frameworkElement);
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
        public double PosX
        {
            get { return Canvas.GetLeft(frameworkElement); }
            set { Canvas.SetLeft(frameworkElement, value); }
        }
        [Description("Object's Y position"), Category("Layout")] 
        public double PosY
        {
            get { return Canvas.GetTop(frameworkElement); }
            set { Canvas.SetTop(frameworkElement, value); }
        }

        [Description("Object's Width"), Category("Layout")] 
        public double Width
        {
            get { return frameworkElement.Width; }
            set { frameworkElement.Width= value; }

        }
        [Description("Object's Height"), Category("Layout")] 
        public double Height
        {
            get { return frameworkElement.Height; }
            set { frameworkElement.Height = value; }

        }
        [Description("Object's Rotation angle"), Category("Layout")] 
        public double RotateAngle
        {
            get { return ((frameworkElement.RenderTransform as TransformGroup).Children[1] as RotateTransform).Angle; }
            set { ((frameworkElement.RenderTransform as TransformGroup).Children[1] as RotateTransform).Angle = value;}

        }
        FrameworkElement frameworkElement;
    }
    class ShapeShortProp : FrameworkElementShortProp
    {

        public ShapeShortProp(Shape shp):base(shp)
        {
            shape=shp ;
            
        }
        [Description("Object's Fill color"), Category("Appearence")] 
        public System.Drawing.Color  FillColor
        {
            get 
            {
                System.ComponentModel.TypeConverter ccv = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Drawing.Color));
                return (System.Drawing.Color)ccv.ConvertFromString((shape.Fill as SolidColorBrush).Color.ToString());
                
            }
            set 
            {
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
                shape.Stroke = shape.Fill.Clone();
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
                shape.StrokeThickness = value;

            }
        }
        Shape shape;
        //System.Drawing.Brush brush;
    }

    static class ShortPropFactory
    {
        public static CommonShortProp CreateShortPropFrom(object obj)
        {
            if (obj is Shape)
                return new ShapeShortProp(obj as Shape);
            else if (obj is FrameworkElement)
                return new FrameworkElementShortProp(obj as FrameworkElement);
            return new CommonShortProp(obj);

        }
   }
}
