using System;
using System.Collections.Generic;
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
    internal class StringBindingControl : System.Windows.Forms.Form
    {
        private TreeView channelsTree;
        private TextBox textBox1;
        private ListView listView1;
        private Button button1;
        private Button button2;
        public string BindString
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        } 
        public List<string> BindedChannels
        {
            get
            {
                return listView1.Items.Cast<ListViewItem>().Select<ListViewItem, String>(x => x.Text).ToList();
            }
            set
            {
                listView1.Items.AddRange(value.Select<String, ListViewItem>(x => new ListViewItem(x)).ToArray());
            }
        }

        public StringBindingControl(System.ComponentModel.ITypeDescriptorContext context)
        {
            InitializeComponent();
            string channelName = String.Empty;
            PropertiesUtils.PropertyWrapper pw;

            if ((pw = context.PropertyDescriptor as PropertiesUtils.PropertyWrapper) == null)
                return;
            DependencyObject depObj = pw.ControlledObject as DependencyObject;
            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(pw.ControlledProperty);
            if (depObj == null || dpd == null)
                return;
            DependencyProperty depProp = dpd.DependencyProperty;

            BindString = (String)depObj.GetValue(depProp);
            foreach (string plugId in Env.Current.CommunicationPlugins.PluginIds)
            {
                TreeNode plugNode = channelsTree.Nodes.Add(Env.Current.CommunicationPlugins[plugId].Name);
              
                foreach (IChannel ch in Env.Current.CommunicationPlugins[plugId].Channels)
                {
                    TreeNode chNode;
                    chNode = plugNode.Nodes.Add(ch.Name);
                    chNode.Tag = plugId;
              
                }
            }
            System.Windows.Data.MultiBinding bind;

            if ((bind = BindingOperations.GetMultiBinding(depObj, depProp)) != null)
            {

                BindString = (bind.Converter as Kent.Boogaart.Converters.FormatConverter).FormatString;
                //bind.Bindings.Select<System.Windows.Data.Binding,String>(=>(((ObjectDataProvider)x.Source).ObjectInstance as Common.Schema.ChannelDataSource).ChannelName).ToList()
                BindedChannels = (from b in bind.Bindings
                                  select (((ObjectDataProvider)(((System.Windows.Data.Binding)b).Source)).ObjectInstance as Common.Schema.ChannelDataSource).ChannelName).ToList();
                return;
            }
            textBox1.Text = (String)depObj.GetValue(depProp);
            
        }
        public TreeNode SelectedNode
        {
            get { return channelsTree.SelectedNode; }
        }

        private void InitializeComponent()
        {
            this.channelsTree = new System.Windows.Forms.TreeView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // channelsTree
            // 
            this.channelsTree.Location = new System.Drawing.Point(1, 2);
            this.channelsTree.Name = "channelsTree";
            this.channelsTree.Size = new System.Drawing.Size(202, 274);
            this.channelsTree.TabIndex = 0;
            this.channelsTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.channelsTree_AfterSelect);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(209, 2);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(271, 123);
            this.textBox1.TabIndex = 1;
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(209, 131);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(270, 112);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            this.listView1.ItemActivate += new System.EventHandler(this.listView1_ItemActivate);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(300, 249);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 26);
            this.button1.TabIndex = 3;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(398, 249);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 26);
            this.button2.TabIndex = 4;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // StringBindingControl
            // 
            this.AcceptButton = this.button1;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(486, 278);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.channelsTree);
            this.Name = "StringBindingControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        
        

        private void channelsTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                ListViewItem it = listView1.Items.Add((String)e.Node.Tag +"."+ e.Node.Text);

                textBox1.SelectedText = "{" + it.Index + "}";
            }

        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            ListViewItem it = ((ListView)sender).SelectedItems[0];
            string str="{" + it.Index+ "}";
            if (textBox1.Text.IndexOf(str) != -1)
            {
                textBox1.SelectionStart = textBox1.Text.IndexOf(str); ;
                textBox1.SelectionLength = str.Length;
                textBox1.SelectedText = "";
                for (int i = it.Index + 1; i < listView1.Items.Count; i++)
                {

                    textBox1.SelectionStart = textBox1.Text.IndexOf("{" + i + "}");
                    textBox1.SelectionLength = str.Length;
                    textBox1.SelectedText = "{" + (i - 1) + "}";

                }
            }
            listView1.Items.Remove(it);
        }

    }

    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class StringEditor : System.Drawing.Design.UITypeEditor
    {
        public StringEditor()
        {
        }

        // Indicates whether the UITypeEditor provides a form-based (modal) dialog, 
        // drop down dialog, or no UI outside of the properties window.
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
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
                StringBindingControl control = new StringBindingControl(context);


                if (edSvc.ShowDialog(control) == DialogResult.OK&&control.BindedChannels.Count > 0)
                {
                    System.Windows.Data.MultiBinding multiBind = new MultiBinding();
                    foreach (String item in control.BindedChannels)
                    {
                        System.Windows.Data.Binding bind = new System.Windows.Data.Binding("Value");
                        System.Windows.Data.ObjectDataProvider dp;
                        dp = new System.Windows.Data.ObjectDataProvider();
                        Common.Schema.ChannelDataSource chs = new Common.Schema.ChannelDataSource();
                        chs.ChannelName = item;
                        dp.ObjectInstance = chs;
                        dp.MethodName = "GetChannel";
                        bind.Source = dp;
                        
                        bind.FallbackValue = " xx ";
                        multiBind.Bindings.Add(bind);
                    }
                    multiBind.Converter = new Kent.Boogaart.Converters.FormatConverter(control.BindString);
                    //multiBind.FallbackValue = control.BindString;
                    BindingOperations.SetBinding(depObj, depProp, multiBind);
                    

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
            return true;
        }
    }

    // Provides a user interface for adjusting an angle value.
 
}
