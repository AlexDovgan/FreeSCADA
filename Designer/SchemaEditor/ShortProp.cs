using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FreeSCADA.Common.Schema;

///this file must deleted
namespace FreeSCADA.Designer.SchemaEditor.ShortProperties
{
    

   
  
    /* this code was product of experementing with type descriptors
     * neded for implementid property editor customisation throw reflections
    
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
		//bool isExtended;
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
        
    }*/
}
