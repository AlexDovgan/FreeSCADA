using System;
using System.ComponentModel;
using System.Globalization;
using System.Drawing;
namespace FreeSCADA.Designer.SchemaEditor.StylesLibrary
{

    public class StyleConverter : TypeConverter
    {
        // Overrides the CanConvertFrom method of TypeConverter.
        // The ITypeDescriptorContext interface provides the context for the
        // conversion. Typically, this interface is used at design time to 
        // provide information about the design-time container.
        public override bool CanConvertTo(ITypeDescriptorContext context, Type sourceType)
        {
            
            if (sourceType == typeof(string)&&context!=null)
            {
                if (TypeDescriptor.GetAttributes(context.Instance)[typeof(System.Windows.Markup.RuntimeNamePropertyAttribute)] != null)
                    return true;
                else
                    return false;
            }
            return base.CanConvertTo(context, sourceType);
        }
        // Overrides the ConvertFrom method of TypeConverter. 
        public override object ConvertFrom(ITypeDescriptorContext context,
           CultureInfo culture, object value)
        {
            return base.ConvertFrom(context, culture, value);
        }
        // Overrides the ConvertTo method of TypeConverter.
        public override object ConvertTo(ITypeDescriptorContext context,
           CultureInfo culture, object value, Type destinationType)
        {
            if (TypeDescriptor.GetAttributes(value)[typeof(System.Windows.Markup.RuntimeNamePropertyAttribute)] != null)
                return (TypeDescriptor.GetAttributes(value)[typeof(System.Windows.Markup.RuntimeNamePropertyAttribute)]
                    as System.Windows.Markup.RuntimeNamePropertyAttribute).Name;

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}