using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace FreeSCADA.Designer.SchemaEditor
{
    class BindingConvertor : ExpressionConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            //return base.CanConvertTo(context, destinationType);
            if (destinationType == typeof(MarkupExtension))
                return true;
            else return false;
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(MarkupExtension))
            {
                BindingExpression bindingExpression = value as BindingExpression;
                if (bindingExpression != null)
                    return bindingExpression.ParentBinding;
                MultiBindingExpression multiBindingExpression = value as MultiBindingExpression;
                if (multiBindingExpression != null)
                    return multiBindingExpression.ParentMultiBinding;

            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    class BindingTypeDescriptionProvider : TypeDescriptionProvider
    {
        private static TypeDescriptionProvider defaultTypeProvider =
                       TypeDescriptor.GetProvider(typeof(System.Windows.Data.Binding));

        public BindingTypeDescriptionProvider()
            : base(defaultTypeProvider)
        {
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType,
                                                                object instance)
        {
            ICustomTypeDescriptor defaultDescriptor =
                                  base.GetTypeDescriptor(objectType, instance);

            return instance == null ? defaultDescriptor :
                new BindingCustomTypeDescriptor(defaultDescriptor);
        }
    }

    class BindingCustomTypeDescriptor : CustomTypeDescriptor
    {
        public BindingCustomTypeDescriptor(ICustomTypeDescriptor parent)
            : base(parent)
        {


        }

        public override PropertyDescriptorCollection GetProperties()
        {


            return GetProperties(new Attribute[]{});
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection pdc = new PropertyDescriptorCollection(base.GetProperties().Cast<PropertyDescriptor>().ToArray());

            string[] props = { "Source","ValidationRules"};

            foreach (PropertyDescriptor pd in props.Select(x => pdc.Find(x, false)))
            {
                PropertyDescriptor pd2;
                pd2 = TypeDescriptor.CreateProperty(typeof(System.Windows.Data.Binding), pd, new Attribute[] { new System.ComponentModel.DefaultValueAttribute(null),new System.ComponentModel.DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content) });
                
                //pd2.Attributes.
                    //[typeof(ReadOnlyAttribute)] = null;

                pdc.Add(pd2);

                pdc.Remove(pd);
            }

            return pdc;
        }

    }
}