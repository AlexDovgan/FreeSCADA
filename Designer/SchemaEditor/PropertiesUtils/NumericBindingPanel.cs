using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Data;
using FreeSCADA.Common;
using FreeSCADA.Common.Schema;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Designer.SchemaEditor.PropertiesUtils
{
	internal partial class NumericBindingPanel : BaseBindingPanel
	{
		IChannel channel;

		public NumericBindingPanel()
		{
			InitializeComponent();
			label3.ForeColor = Color.Red;
			channelNameLabel.Text = "";
		}

		public override void AddChannel(IChannel channel)
		{
			if (channel != null)
			{
				this.channel = channel;

				label3.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
				
                string plugName = Env.Current.CommunicationPlugins[channel.PluginId].Name;
                channelNameLabel.Text = string.Format("{0} [{1}]", channel.Name, plugName);
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

				ComposingConverter conv = bind.Converter as ComposingConverter;
				foreach (IValueConverter converter in conv.Converters)
				{
					if (converter is RangeConverter)
					{
						checkBox1.Checked = true;
						RangeConverter rc = converter as RangeConverter;
						minEdit.Value = (Decimal)rc.Min;
						maxEdit.Value = (Decimal)rc.Max;
					}
				}
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

				ComposingConverter conv = new ComposingConverter();
				if (checkBox1.Checked)
				{
					RangeConverter rc = new RangeConverter();
					rc.Min = Decimal.ToDouble(minEdit.Value);
					rc.Max = Decimal.ToDouble(maxEdit.Value);
					conv.Converters.Add(rc);
				}

				conv.Converters.Add(new Kent.Boogaart.Converters.TypeConverter(cdp.Channel.Type, Property.PropertyType));
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

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			groupBox1.Enabled = checkBox1.Checked;
		}
	}

	internal class NumericBindingPanelFactory : BaseBindingPanelFactory
	{
		override public bool CheckApplicability(object element, PropertyWrapper property)
		{
            Type type = property.PropertyType;
            List<Type> types = new List<Type>(new Type[] { 
                typeof(Double), 
                typeof(Int16), 
                typeof(Int32), 
                typeof(Boolean), 
                typeof(Byte), 
                typeof(Single),
                typeof(Char)

            });
			if (types.Contains(type))
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

				if (bind.Converter is ComposingConverter == false)
					return false;

				return true;
			}

			return true;
		}

		override public BaseBindingPanel CreateInstance()
		{
			return new NumericBindingPanel();
		}

		override public string Name
		{
			get { return StringResources.NumericBindingPanelName; }
		}
	}
}
