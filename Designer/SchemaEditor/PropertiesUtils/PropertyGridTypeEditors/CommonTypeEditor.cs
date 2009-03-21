using System.Drawing.Design;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using FreeSCADA.Common;
using FreeSCADA.Interfaces;
using FreeSCADA.Interfaces.Plugins;

namespace FreeSCADA.Designer.SchemaEditor.PropertiesUtils.PropertyGridTypeEditors
{
    /// <summary>
    /// 
    /// </summary>
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class CommonTypeEditor: System.Drawing.Design.UITypeEditor
    {
        /// <summary>
        /// 
        /// </summary>
        public CommonTypeEditor()
        {
        }
        /// <summary>
        /// Indicates whether the UITypeEditor provides a form-based (modal) dialog, 
        /// drop down dialog, or no UI outside of the properties window.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            if(context==null)
                return UITypeEditorEditStyle.None;
            PropertiesUtils.PropertyWrapper pw;
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

            PropertiesUtils.PropertyWrapper pw=context.PropertyDescriptor as PropertiesUtils.PropertyWrapper;
            
            DependencyObject depObj ;
            DependencyProperty depProp;
            if(!pw.GetWpfObjects(out depObj ,out depProp))
                return value;

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                // Display an angle selection control and retrieve the value.
                bool found = false;
                foreach (IVisualControlsPlug p in Env.Current.VisualPlugins.Plugins)
                {
                    foreach (IVisualControlDescriptor d in p.Controls)
                    {
                        if (depObj.GetType() == d.Type)
                        {
                            CommonBindingDialog control = new CommonBindingDialog(d.getPropProxy(depObj), pw.PropertyInfo);
                            if (edSvc.ShowDialog(control) == DialogResult.OK)
                            {
                                return pw.GetValue(depObj);
                            }
                            found = true;
                        }
                    }
                }
                if (!found)
                {
                    CommonBindingDialog control = new CommonBindingDialog(new PropProxy(depObj), pw.PropertyInfo);
                    if (edSvc.ShowDialog(control) == DialogResult.OK)
                    {
                        return pw.GetValue(depObj);
                    }
                }
            }
            return value;
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
