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
    internal class ValueBindingDialog : System.Windows.Forms.Form
    {
        private TreeView channelsTree;
        private Button button1;
        private NumericUpDown minVal;
        private NumericUpDown maxVal;
        private Label label1;
        private Label label2;
        private TextBox expressionTextBox;
        private Button button2;


        public ValueBindingDialog(System.ComponentModel.ITypeDescriptorContext context)
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

            System.Windows.Data.Binding bind;

            if ((bind = BindingOperations.GetBinding(depObj, depProp)) != null)
            {
                Common.Schema.ChannelDataSource chs = ((ObjectDataProvider)bind.Source).ObjectInstance as Common.Schema.ChannelDataSource;
                channelName = chs.ChannelName;
                try
                {
                    ComposingConverter conv = bind.Converter as ComposingConverter;
                    RangeConverter rc = conv.Converters.Single(x => x is RangeConverter) as RangeConverter;
                    minVal.Value = (Decimal)rc.Min;
                    maxVal.Value = (Decimal)rc.Max;
                }
                catch (Exception) { }
            }

            string[] splitStr = channelName.Split('.');
            foreach (string plugId in Env.Current.CommunicationPlugins.PluginIds)
            {
                TreeNode plugNode = channelsTree.Nodes.Add(Env.Current.CommunicationPlugins[plugId].Name);


                if (splitStr.Count(x => x == plugNode.Text) > 0)
                    plugNode.Expand();
                foreach (IChannel ch in Env.Current.CommunicationPlugins[plugId].Channels)
                {
                    TreeNode chNode;
                    chNode = plugNode.Nodes.Add(ch.Name);
                    chNode.Tag = plugId;
                    if (splitStr.Count(x => x == chNode.Text) > 0)
                    {

                        channelsTree.SelectedNode = chNode;
                        channelsTree.Update();
                    }
                }
            }
        }

        public String Expression
        {
            get { return expressionTextBox.Text; }
        }
        public TreeNode SelectedNode
        {
            get { return channelsTree.SelectedNode; }
        }

        public double Max
        {
            get { return Decimal.ToDouble(maxVal.Value); }
        }
        public double Min
        {
            get { return Decimal.ToDouble(minVal.Value); }
        }

        private void InitializeComponent()
        {
            this.channelsTree = new System.Windows.Forms.TreeView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.minVal = new System.Windows.Forms.NumericUpDown();
            this.maxVal = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.expressionTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.minVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxVal)).BeginInit();
            this.SuspendLayout();
            // 
            // channelsTree
            // 
            this.channelsTree.Location = new System.Drawing.Point(1, 2);
            this.channelsTree.Name = "channelsTree";
            this.channelsTree.Size = new System.Drawing.Size(203, 274);
            this.channelsTree.TabIndex = 0;
            this.channelsTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.channelsTree_AfterSelect);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(259, 205);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 26);
            this.button1.TabIndex = 3;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(259, 237);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 26);
            this.button2.TabIndex = 4;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // minVal
            // 
            this.minVal.DecimalPlaces = 2;
            this.minVal.Enabled = false;
            this.minVal.Location = new System.Drawing.Point(212, 129);
            this.minVal.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.minVal.Minimum = new decimal(new int[] {
            65535,
            0,
            0,
            -2147483648});
            this.minVal.Name = "minVal";
            this.minVal.Size = new System.Drawing.Size(67, 20);
            this.minVal.TabIndex = 1;
            // 
            // maxVal
            // 
            this.maxVal.DecimalPlaces = 2;
            this.maxVal.Enabled = false;
            this.maxVal.Location = new System.Drawing.Point(212, 155);
            this.maxVal.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.maxVal.Minimum = new decimal(new int[] {
            65535,
            0,
            0,
            -2147483648});
            this.maxVal.Name = "maxVal";
            this.maxVal.Size = new System.Drawing.Size(67, 20);
            this.maxVal.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(286, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Min value";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(286, 155);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Max value";
            // 
            // textBox1
            // 
            this.expressionTextBox.Location = new System.Drawing.Point(212, 2);
            this.expressionTextBox.Multiline = true;
            this.expressionTextBox.Name = "textBox1";
            this.expressionTextBox.Size = new System.Drawing.Size(127, 121);
            this.expressionTextBox.TabIndex = 6;
            // 
            // DoubleBindingControl
            // 
            this.AcceptButton = this.button1;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(345, 278);
            this.Controls.Add(this.expressionTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.maxVal);
            this.Controls.Add(this.minVal);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.channelsTree);
            this.Name = "DoubleBindingControl";
            ((System.ComponentModel.ISupportInitialize)(this.minVal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxVal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        private void channelsTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                minVal.Enabled = true;
                maxVal.Enabled = true;
            }
            else
            {
                minVal.Enabled = false;
                maxVal.Enabled = false;

            }
        }



    }

}
