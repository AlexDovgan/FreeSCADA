using System;
using System.Drawing;
using System.Windows;
using System.Windows.Data;
using FreeSCADA.Common.Schema;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Designer.SchemaEditor.PropertiesUtils
{
    internal partial class SolidBrushBindingPanel : BaseBindingPanel
    {
        IChannel channel;

        public SolidBrushBindingPanel()
        {
            InitializeComponent();
            label3.ForeColor = Color.Red;
            label4.Text = "";
        }

        public override void AddChannel(IChannel channel)
        {
            if (channel != null)
            {
                this.channel = channel;

                label3.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
                label4.Text = channel.Name;
            }
        }

        public override void Initialize(object element, PropertyWrapper property, System.Windows.Data.BindingBase binding)
        {
            base.Initialize(element, property, binding);

            System.Windows.Data.Binding bind = binding as System.Windows.Data.Binding;
            if (bind != null)
            {
                ChannelDataProvider cdp = (ChannelDataProvider)bind.Source;
                AddChannel(cdp.Channel);
                SolidBrushConverter sbc = bind.Converter as SolidBrushConverter;
                minEdit.Value = (Decimal)sbc.MinValue;
                maxEdit.Value = (Decimal)sbc.MaxValue;
                startColorButton.BackColor = Color.FromArgb(
                    sbc.StartColor.A,
                    sbc.StartColor.R,
                    sbc.StartColor.G,
                    sbc.StartColor.B);
                endColorButton.BackColor = Color.FromArgb(
                    sbc.EndColor.A,
                    sbc.EndColor.R,
                    sbc.EndColor.G,
                    sbc.EndColor.B);



            }
        }
        public override System.Windows.Data.BindingBase Save()
        {
            if (channel != null)
            {
                System.Windows.Data.Binding bind = new System.Windows.Data.Binding("Value");
                ChannelDataProvider cdp = new ChannelDataProvider();
                cdp.ChannelName = channel.PluginId + "." + channel.Name;
                bind.Source = cdp;
                cdp.Refresh();

                SolidBrushConverter conv = new SolidBrushConverter();
                conv.MinValue = Decimal.ToDouble(minEdit.Value);
                conv.MaxValue = Decimal.ToDouble(maxEdit.Value);
                conv.StartColor = System.Windows.Media.Color.FromArgb(
                    startColorButton.BackColor.A,
                    startColorButton.BackColor.R,
                    startColorButton.BackColor.G,
                    startColorButton.BackColor.B);
                conv.EndColor = System.Windows.Media.Color.FromArgb(
                    endColorButton.BackColor.A,
                    endColorButton.BackColor.R,
                    endColorButton.BackColor.G,
                    endColorButton.BackColor.B);

                bind.Converter = conv;

                bind.Mode = BindingMode.TwoWay;


                DependencyObject depObj;
                DependencyProperty depProp;
                if (Property.GetWpfObjects(out depObj, out depProp))
                    bind.FallbackValue = depObj.GetValue(depProp);

                return bind;
            }
            else
                return base.Save();
        }

        private void button_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ColorDialog cd = new System.Windows.Forms.ColorDialog();
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                (sender as System.Windows.Forms.Button).BackColor = cd.Color;

            }
        }


    }

    internal class BrushBindingPanelFactory : BaseBindingPanelFactory
    {
        override public bool CheckApplicability(object element, PropertyWrapper property)
        {
            Type type = property.PropertyType;
            if (type.Equals(typeof(System.Windows.Media.Brush)))
                return true;

            return false;
        }

        override public bool CanWorkWithBinding(System.Windows.Data.BindingBase binding)
        {
            if (binding != null && binding is System.Windows.Data.Binding)
            {
                System.Windows.Data.Binding bind = binding as System.Windows.Data.Binding;
                if (bind.Source is ChannelDataProvider == false)
                    return false;

                if (bind.Converter is SolidBrushConverter == false)
                    return false;

                return true;
            }

            return false;
        }

        override public BaseBindingPanel CreateInstance()
        {
            return new SolidBrushBindingPanel();
        }

        override public string Name
        {
            get { return StringResources.SolidColorBindingPanelName; }
        }
    }
}
