using System.ComponentModel;
using System.Drawing.Design;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Designer.SchemaEditor.PropertiesUtils.PropertyGridTypeEditors
{
    /// <summary>
    /// 
    /// </summary>
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class ChannelSelectEditor : System.Drawing.Design.UITypeEditor
    {
        /// <summary>
        /// Indicates whether the UITypeEditor provides a form-based (modal) dialog, 
        /// drop down dialog, or no UI outside of the properties window.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            PropertiesUtils.PropertyWrapper pw;
            if (context == null)
                return UITypeEditorEditStyle.DropDown;
            if ((pw = context.PropertyDescriptor as PropertiesUtils.PropertyWrapper) == null)
                return UITypeEditorEditStyle.None;


            if (pw.ControlledObject is DependencyObject)
                return UITypeEditorEditStyle.Modal;
            else
                return UITypeEditorEditStyle.None;
        }
        /// <summary>
        /// Displays the UI for value selection.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
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
                FreeSCADA.Designer.Dialogs.VariablesDialog dlg;
                if (value == null)
                    dlg = new FreeSCADA.Designer.Dialogs.VariablesDialog(true, "");
                else
                    dlg = new FreeSCADA.Designer.Dialogs.VariablesDialog(true, value.ToString());
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                        foreach (IChannel ch in dlg.SelectedChannels)
                        {
                            // returns the first channel from selection
                            return ch.PluginId + "." + ch.Name;
                        }
                }
            }
            return value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public override void PaintValue(System.Drawing.Design.PaintValueEventArgs e)
        {
            return;
        }

        /// <summary>
        /// Indicates whether the UITypeEditor supports painting a 
        /// representation of a property's value.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context)
        {

            return false;
        }
    }
}
