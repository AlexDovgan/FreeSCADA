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
                // Display an angle selection control and retrieve the value.
                DoubleBindingControl control = new DoubleBindingControl(context);
                edSvc.DropDownControl(control);
                if (control.SelectedNode != null && control.SelectedNode.Tag != null)
                {

                    //DependencyProperty depprop= context.PropertyDescriptor.Attributes[typeof(OrinalPropertyAttribute)];
                    System.Windows.Data.Binding bind = new System.Windows.Data.Binding("Value");
                    System.Windows.Data.ObjectDataProvider dp;
                    dp = new System.Windows.Data.ObjectDataProvider();
                    Common.Schema.ChannelDataSource chs = new Common.Schema.ChannelDataSource();
                    chs.ChannelName = control.SelectedNode.Tag + "." + control.SelectedNode.Text;
                    dp.ObjectInstance = chs;
                    dp.MethodName = "GetChannel";
                    bind.Source = dp;
                    bind.Converter = new Kent.Boogaart.Converters.TypeConverter(chs.GetChannel().Type, depProp.PropertyType);
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
            DependencyPropertyDescriptor  dpd= DependencyPropertyDescriptor.FromProperty(pw.ControlledProperty);
            if (depObj == null || dpd== null)
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
            return true;
        }
    }

    // Provides a user interface for adjusting an angle value.
    internal class DoubleBindingControl : System.Windows.Forms.TreeView
    {
        public DoubleBindingControl(System.ComponentModel.ITypeDescriptorContext context)
        {
            string channelName = String.Empty;
            PropertiesUtils.PropertyWrapper pw;

            if ((pw = context.PropertyDescriptor as PropertiesUtils.PropertyWrapper) == null)
                return;
            DependencyObject depObj = pw.ControlledObject as DependencyObject;
            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(pw.ControlledProperty);
            if (depObj == null || dpd == null)
                return;
            DependencyProperty depProp = dpd.DependencyProperty;
            System.Windows.Data.Binding bind;

            if ((bind = BindingOperations.GetBinding(depObj, depProp)) != null)
            {
                Common.Schema.ChannelDataSource chs = ((ObjectDataProvider)bind.Source).ObjectInstance as Common.Schema.ChannelDataSource;
                channelName = chs.ChannelName;
            }

            string[] splitStr = channelName.Split('.');
            foreach (string plugId in Env.Current.CommunicationPlugins.PluginIds)
            {
                TreeNode plugNode = this.Nodes.Add(Env.Current.CommunicationPlugins[plugId].Name);
                //if (plugNode.Text == Env.Current.CommunicationPlugins[splitStr[0]].Name)

                if (splitStr.Count(x => x == plugNode.Text) > 0)
                    plugNode.Expand();
                foreach (IChannel ch in Env.Current.CommunicationPlugins[plugId].Channels)
                {
                    TreeNode chNode;
                    chNode = plugNode.Nodes.Add(ch.Name);
                    chNode.Tag = plugId;
                    if (splitStr.Count(x => x == chNode.Text) > 0)
                    {

                        this.SelectedNode = chNode;
                        this.Update();
                    }
                }
            }
            Width = 200;


        }


    }

}
