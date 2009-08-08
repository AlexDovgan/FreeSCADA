using System;
using System.ServiceModel;
using FreeSCADA.Interfaces;

namespace FreeSCADA.CLServer
{
	class ChannelEventHandler
	{
		IDataUpdatedCallback callback;
		IChannel channel;

		public event EventHandler Disconnected; 
	

		public ChannelEventHandler(IChannel channel, IDataUpdatedCallback callback, IContextChannel contextChannel)
		{
			this.callback = callback;
			this.channel = channel;

			channel.ValueChanged += new EventHandler(OnChannelValueChanged);
			contextChannel.Closing += new EventHandler(OnContextChannelClosing);
		}

		void OnContextChannelClosing(object sender, EventArgs e)
		{
			if (Disconnected != null)
			{
				channel.ValueChanged -= new EventHandler(OnChannelValueChanged);
				Disconnected(this, new EventArgs());
			}
		}


		void OnChannelValueChanged(object sender, EventArgs e)
		{
			if (callback != null)
			{
				IChannel channel = (IChannel)sender;
				try
				{
					callback.ValueChanged(channel.FullId, channel.Value.ToString(), channel.ModifyTime, channel.Status);
				}
				catch (Exception)
				{
				}
			}
		}
	}
}
