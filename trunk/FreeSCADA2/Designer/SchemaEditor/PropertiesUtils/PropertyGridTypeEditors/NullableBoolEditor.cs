using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using FreeSCADA.Common.Schema;


namespace FreeSCADA.Designer.SchemaEditor.PropertiesUtils
{
    
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    class NullableBoolEditor : System.Drawing.Design.UITypeEditor
    {
        public NullableBoolEditor()
        {
        }

        // Indicates whether the UITypeEditor provides a form-based (modal) dialog, 
        // drop down dialog, or no UI outside of the properties window.
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            PropertiesUtils.PropertyWrapper pw;
            if (context == null)
                return UITypeEditorEditStyle.DropDown; 
            if ((pw = context.PropertyDescriptor as PropertiesUtils.PropertyWrapper) == null)
                return UITypeEditorEditStyle.None;


            if (pw.ControlledObject is DependencyObject)
                return UITypeEditorEditStyle.DropDown;
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
                using (ListBox lb = new ListBox())
                {
                    lb.Items.Add("");
                    lb.Items.Add("False");
                    lb.Items.Add("True");
                    lb.Items.Add("Binding...");
                    PropertyGrid pg;
                    System.Drawing.Size sz = new System.Drawing.Size();
                    if ((pg = provider as PropertyGrid) != null)
                        sz.Width = pg.Size.Width;
                    sz.Height = 60;
                    lb.Size = sz;


                    lb.SelectedIndexChanged += new EventHandler((x, y) => { edSvc.CloseDropDown(); });
                    edSvc.DropDownControl(lb);
                    if (lb.SelectedIndex == 3)
                    {
                        // Display an angle selection control and retrieve the value.
                        using (PropertyGridTypeEditors.ValueBindingDialog control = new PropertyGridTypeEditors.ValueBindingDialog(context))
                        {
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
                    }
                    else
                    {
                        if (lb.SelectedItem != null && lb.SelectedItem.ToString()!=String.Empty)
                            return new Nullable<bool>(Boolean.Parse(lb.SelectedItem.ToString()));
                        else return new Nullable<bool>(); 
   
                    }
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
