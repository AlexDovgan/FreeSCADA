using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Drawing;
using FreeSCADA.ShellInterfaces;
using FreeSCADA.Common;
using System.Windows.Data;

namespace FreeSCADA.Designer.SchemaEditor.PropertyGridTypeEditors
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
            // Return the value if the value is not of type Int32, Double and Single.
            //if (value.GetType() != typeof(double) && value.GetType() != typeof(float) && value.GetType() != typeof(int))
              //  return value;

            // Uses the IWindowsFormsEditorService to display a 
            // drop-down UI in the Properties window.
            
            if (context.Instance is ShortProperties.CommonShortProp
                    && (context.Instance as ShortProperties.CommonShortProp).WrapedObject is DependencyObject)
            {
                DependencyObject depObj = (context.Instance as ShortProperties.CommonShortProp).WrapedObject as DependencyObject;
                OriginalPropertyAttribute atr;
                if ((atr = context.PropertyDescriptor.Attributes[typeof(OriginalPropertyAttribute)] as OriginalPropertyAttribute) == null)
                    return value;
                DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromName(atr.PropertyName, atr.ObjectType, depObj.GetType());
                DependencyProperty depProp = dpd.DependencyProperty;
                
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (edSvc != null)
                {
                    // Display an angle selection control and retrieve the value.
                    DoubleBindingControl control = new DoubleBindingControl(context);
                    edSvc.DropDownControl(control);
                    if (control.SelectedNode != null&&control.SelectedNode.Tag!=null )
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
                        bind.Converter = new Kent.Boogaart.Converters.TypeConverter(chs.GetChannel().Value.GetType(), depProp.PropertyType);
                        bind.Mode = BindingMode.TwoWay;
                        BindingOperations.SetBinding(depObj, depProp, bind);
                        depObj.SetValue(depProp, value);
                        
                        
                        
                    }


                }
            }
            return value;
        }
        
        public override void PaintValue(System.Drawing.Design.PaintValueEventArgs e)
        {
            
            if (e.Context.Instance is ShortProperties.CommonShortProp &&
                (e.Context.Instance as ShortProperties.CommonShortProp).WrapedObject is DependencyObject)
            {

                DependencyObject depObj = (e.Context.Instance as ShortProperties.CommonShortProp).WrapedObject as DependencyObject;
                OriginalPropertyAttribute atr;
                if ((atr = e.Context.PropertyDescriptor.Attributes[typeof(OriginalPropertyAttribute)] as OriginalPropertyAttribute) == null)
                    return;
                DependencyPropertyDescriptor dpd= DependencyPropertyDescriptor.FromName(atr.PropertyName, atr.ObjectType, depObj.GetType());
                DependencyProperty depProp = dpd.DependencyProperty;
                System.Windows.Data.Binding bind;
                string channelName;
                if((bind=BindingOperations.GetBinding(depObj,depProp))!=null)
                {
                      
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        e.Graphics.FillRectangle(drawBrush, e.Bounds);
                                        
                }
                    

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
            OriginalPropertyAttribute atr;
            string channelName=String.Empty;
            if ((atr = context.PropertyDescriptor.Attributes[typeof(OriginalPropertyAttribute)] as OriginalPropertyAttribute) != null)
            {
                DependencyObject depObj = (context.Instance as ShortProperties.CommonShortProp).WrapedObject as DependencyObject;
                DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromName(atr.PropertyName, atr.ObjectType, depObj.GetType());
                DependencyProperty depProp = dpd.DependencyProperty;
             
                System.Windows.Data.Binding bind;

                if ((bind = BindingOperations.GetBinding(depObj, depProp)) != null)
                {
                    Common.Schema.ChannelDataSource chs = ((ObjectDataProvider)bind.Source).ObjectInstance as Common.Schema.ChannelDataSource;
                    channelName = chs.ChannelName;
                }
            }
            string[] splitStr = channelName.Split('.');
            foreach (string plugId in Env.Current.CommunicationPlugins.PluginIds)
            {
                TreeNode plugNode = this.Nodes.Add(Env.Current.CommunicationPlugins[plugId].Name);
                //if (plugNode.Text == Env.Current.CommunicationPlugins[splitStr[0]].Name)
                    
                if(splitStr.Count(x=>x==plugNode.Text)>0)
                    plugNode.Expand();
                foreach (IChannel ch in Env.Current.CommunicationPlugins[plugId].Channels)
                {
                    TreeNode chNode;
                    chNode = plugNode.Nodes.Add(ch.Name);
                    chNode.Tag = plugId;
                    if (splitStr.Count(x => x == chNode.Text)>0)
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
