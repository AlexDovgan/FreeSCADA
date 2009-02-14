using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Data;
using FreeSCADA.Common.Schema;

namespace FreeSCADA.Designer.SchemaEditor
{
    static class BidingHelper
    {
        public static BindingBase CreateBindingFrom(
            List<string> channels,
            DependencyObject converter,
            object defaultValue)
        {
            if (channels.Count == 0)
                return null;
            if (converter is IMultiValueConverter)
            {
                MultiBinding mb = new MultiBinding();
                foreach (string channel in channels)
                {
                    Binding b = new Binding("Value");
                    ChannelDataProvider cdp = new ChannelDataProvider();
                    cdp.ChannelName = channel;
                    b.Source = cdp;
                    mb.Bindings.Add(mb);
                }
                mb.Converter = converter as IMultiValueConverter;
                mb.FallbackValue = defaultValue;
                return mb;

            }
            else if (converter is IValueConverter)
            {
                Binding b = new Binding("Value");
                ChannelDataProvider cdp = new ChannelDataProvider();
                cdp.ChannelName = channels[0];
                b.Source = cdp;
                b.Converter = converter as IValueConverter;
                b.FallbackValue = defaultValue;
                return b;
            }
            return null;
        }
        public static List<String> GetChannelsFromBinding(BindingBase binding)
        {
            List<string> channels=new List<string>();
            if(binding is Binding)
            {
                channels.Add(((binding as Binding).Source as ChannelDataProvider).ChannelName);

            }
            else if(binding is MultiBinding)
            {
                foreach (Binding b in (binding as MultiBinding).Bindings)
                {
                    channels.Add(((b as Binding).Source as ChannelDataProvider).ChannelName);
                }
            }
            return channels;
        }
    }
}
