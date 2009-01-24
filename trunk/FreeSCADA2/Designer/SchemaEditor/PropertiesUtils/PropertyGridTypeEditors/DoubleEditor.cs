using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using FreeSCADA.Common;
using FreeSCADA.Common.Schema;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Designer.SchemaEditor.PropertiesUtils
{
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class DoubleEditor : System.Drawing.Design.UITypeEditor
    {
        public DoubleEditor()
        {
        }

        // Indicates whether the UITypeEditor provides a form-based (modal) dialog, 
        // drop down dialog, or no UI outside of the properties window.
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            if(context==null)
                return UITypeEditorEditStyle.Modal;
            PropertiesUtils.PropertyWrapper pw;
            if ((pw = context.PropertyDescriptor as PropertiesUtils.PropertyWrapper) == null)
                return UITypeEditorEditStyle.None;


            if (pw.ControlledObject is DependencyObject)
                return UITypeEditorEditStyle.Modal;
            else
                return UITypeEditorEditStyle.None;
        }

        // Displays the UI for value selection.
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {

            PropertiesUtils.PropertyWrapper pw;
            if ((pw = context.PropertyDescriptor as PropertiesUtils.PropertyWrapper) == null)
                return value;

            DependencyObject depObj = pw.ControlledObject as DependencyObject;
            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(pw.ControlledProperty);
            if (depObj == null || dpd == null)
                return value;
            DependencyProperty depProp = dpd.DependencyProperty;
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                // Display an angle selection control and retrieve the value.
                ValueBindingDialog control = new ValueBindingDialog(context);


                if (edSvc.ShowDialog(control) == DialogResult.OK)
                {

                    System.Windows.Data.Binding bind = new System.Windows.Data.Binding("Value");
                    System.Windows.Data.ObjectDataProvider dp;
                    dp = new System.Windows.Data.ObjectDataProvider();
                    Common.Schema.ChannelDataSource chs = new Common.Schema.ChannelDataSource();
                    chs.ChannelName = control.SelectedNode.Tag + "." + control.SelectedNode.Text;
                    dp.ObjectInstance = chs;
                    dp.MethodName = "GetChannel";
                    bind.Source = dp;
                    ComposingConverter conv = new ComposingConverter();

                    RangeConverter rc = new RangeConverter();
                    rc.Min = control.Min;
                    rc.Max = control.Max;
                    conv.Converters.Add(rc);
                    try
                    {
                        Kent.Boogaart.Converters.ExpressionConverter ec = new Kent.Boogaart.Converters.ExpressionConverter();
                        ec.Expression = control.Expression;
                        conv.Converters.Add(ec);


                    }
                    catch (System.Exception)
                    {
                        conv.Converters.Add(new Kent.Boogaart.Converters.TypeConverter(chs.GetChannel().Type, depProp.PropertyType));
                    }
                    bind.Converter = conv;
                    bind.Mode = BindingMode.TwoWay;
                    bind.FallbackValue = value;
                    BindingOperations.SetBinding(depObj, depProp, bind);

                }
            }



            return value;
        }
        
        public override void PaintValue(System.Drawing.Design.PaintValueEventArgs e)
        {
            PropertiesUtils.PropertyWrapper pw;
            if ((pw = e.Context.PropertyDescriptor as PropertiesUtils.PropertyWrapper) == null)
                return;


            DependencyObject depObj = pw.ControlledObject as DependencyObject;
            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(pw.ControlledProperty);
            if (depObj == null || dpd == null)
                return;
            DependencyProperty depProp = dpd.DependencyProperty;
            System.Windows.Data.Binding bind;
            if ((bind = BindingOperations.GetBinding(depObj, depProp)) != null)
            {

                SolidBrush drawBrush = new SolidBrush(Color.Black);
                e.Graphics.FillRectangle(drawBrush, e.Bounds);
            }
        }


        // Indicates whether the UITypeEditor supports painting a 
        // representation of a property's value.
        public override bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            PropertiesUtils.PropertyWrapper pw;
            if ((pw = context.PropertyDescriptor as PropertiesUtils.PropertyWrapper) == null)
                return false;


            if (pw.ControlledObject is DependencyObject)
                return true;
            else
                return false;
        }
    }


}
