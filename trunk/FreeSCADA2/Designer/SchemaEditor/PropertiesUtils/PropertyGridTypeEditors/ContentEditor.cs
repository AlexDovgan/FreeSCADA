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


namespace FreeSCADA.Designer.SchemaEditor.PropertiesUtils.PropertyGridTypeEditors
{
    class ContentEditor:System.Drawing.Design.UITypeEditor
    {
        public ContentEditor()
        {
        }

        // Indicates whether the UITypeEditor provides a form-based (modal) dialog, 
        // drop down dialog, or no UI outside of the properties window.
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
           // if(context.Instance is System.Windows.Controls.ContentControl)
                return UITypeEditorEditStyle.Modal;
           // else
           //     return UITypeEditorEditStyle.None;
        }

        // Displays the UI for value selection.
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {

            PropertiesUtils.PropertyWrapper pw;
            if ((pw = context.PropertyDescriptor as PropertiesUtils.PropertyWrapper) == null)
                return value;
             
            DependencyObject depObj = pw.ControlledObject as DependencyObject;
            if(!(depObj is System.Windows.Controls.ContentControl))
                return value;

            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(pw.ControlledProperty);
            if (depObj == null || dpd == null)
                return value;
            DependencyProperty depProp = dpd.DependencyProperty;
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                
                ContentEditorDialog ced = new ContentEditorDialog();
                edSvc.ShowDialog(ced);
                FreeSCADA.Common.Schema.MediaProvider mp = new MediaProvider();
                if (ced.ImagesList.SelectedItems.Count > 0)
                {
                    mp.MediaFileName = ced.ImagesList.SelectedItems[0].Text;

                    System.Windows.Data.Binding b = new System.Windows.Data.Binding();
                    b.Mode = BindingMode.OneTime;

                    b.Source = mp;
                    mp.Refresh();
                    depObj.SetValue(System.Windows.Controls.ContentControl.ContentProperty, b);
                    (depObj as System.Windows.Controls.ContentControl).InvalidateProperty(System.Windows.Controls.ContentControl.ContentProperty);
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
