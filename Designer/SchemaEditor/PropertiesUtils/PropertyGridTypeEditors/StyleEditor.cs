using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace FreeSCADA.Designer.SchemaEditor.PropertiesUtils.PropertyGridTypeEditors
{

    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    class StyleEditor : System.Drawing.Design.UITypeEditor
    {
        public StyleEditor()
        {
        }

        // Indicates whether the UITypeEditor provides a form-based (modal) dialog, 
        // drop down dialog, or no UI outside of the properties window.
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
             return UITypeEditorEditStyle.DropDown;
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
                    StylesLibrary.StylesLibrary  sl=StylesLibrary.StylesLibrary.Instance;
                    if (sl[depObj.GetType()] != null)
                    {
                        foreach (String name in sl[depObj.GetType()].Keys)
                            lb.Items.Add(name);
                       
                        lb.SelectedIndexChanged += new EventHandler((x, y) => { edSvc.CloseDropDown(); });
                        edSvc.DropDownControl(lb);
                        if(lb.SelectedItem!=null)
                            depObj.SetValue(depProp, sl[depObj.GetType()][lb.SelectedItem.ToString()]);
                        

                    }
                }
            }

            return value;
        }


        public override void PaintValue(System.Drawing.Design.PaintValueEventArgs e)
        {
            return;
        }


        // Indicates whether the UITypeEditor supports painting a 
        // representation of a property's value.
        public override bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            
                return false;
        }
    }



}
