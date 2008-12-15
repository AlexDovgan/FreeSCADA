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
                if (bindingExpression == null)
                    throw new Exception();
                else 
                    return bindingExpression.ParentBinding;
                MultiBindingExpression multiBindingExpression = value as MultiBindingExpression;
                if (multiBindingExpression == null)
                    throw new Exception();
                else
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

            PropertyDescriptorCollection pdc = new PropertyDescriptorCollection(base.GetProperties().Cast<PropertyDescriptor>().ToArray());
            PropertyDescriptor pd;
            if ((pd = pdc.Find("Source", false)) != null)
            {

                pdc.Add(TypeDescriptor.CreateProperty(typeof(System.Windows.Data.Binding), pd, new Attribute[] { new System.ComponentModel.DefaultValueAttribute(null) }));
                pdc.Remove(pd);
            }

            return pdc;
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection pdc = new PropertyDescriptorCollection(base.GetProperties(attributes).Cast<PropertyDescriptor>().ToArray());
            PropertyDescriptor pd;
            if ((pd = pdc.Find("Source", false)) != null)
            {

                pdc.Add(TypeDescriptor.CreateProperty(typeof(System.Windows.Data.Binding), pd, new Attribute[] { new System.ComponentModel.DefaultValueAttribute(null) }));
                pdc.Remove(pd);
            }
            return pdc;
        }

    }
}