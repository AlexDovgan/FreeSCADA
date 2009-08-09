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
				ChannelState state = new ChannelState();
				state.ModifyTime = channel.ModifyTime;
				switch (channel.StatusFlags)
				{
					case FreeSCADA.Interfaces.ChannelStatusFlags.Bad:
						state.Status = ChannelStatusFlags.Bad;
						break;
					case FreeSCADA.Interfaces.ChannelStatusFlags.Good:
						state.Status = ChannelStatusFlags.Good;
						break;
					case FreeSCADA.Interfaces.ChannelStatusFlags.NotUsed:
						state.Status = ChannelStatusFlags.NotUsed;
						break;
					default:
						state.Status = ChannelStatusFlags.Unknown;
						break;
				}
				state.Type = channel.Type.FullName;
				state.Value = channel.Value.ToString();

				try
				{
					callback.ValueChanged(channel.FullId, state);
				}
				catch (Exception)
				{
				}
			}
		}
	}
}
