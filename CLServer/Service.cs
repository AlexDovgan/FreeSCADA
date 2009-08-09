using System;
using System.Collections.Generic;
using System.ServiceModel;
using FreeSCADA.Common;
using FreeSCADA.Interfaces;
using FreeSCADA.Interfaces.Plugins;


namespace FreeSCADA.CLServer
{
	class Service:IChannelInformationRetriever,IDataRetriever
	{
		List<ChannelEventHandler> subscribers = new List<ChannelEventHandler>();


		public ChannelInfo[] GetChannels()
		{
			List<ChannelInfo> channels = new List<ChannelInfo>();
			//return channels.ToArray();

			foreach (string pluginId in Env.Current.CommunicationPlugins.PluginIds)
			{
				ICommunicationPlug plug = Env.Current.CommunicationPlugins[pluginId];
				foreach (IChannel channel in plug.Channels)
				{
					ChannelInfo info = new ChannelInfo();
					info.FullId = channel.FullId;
					info.IsReadOnly = channel.IsReadOnly;
					info.Name = channel.Name;
					info.PluginId = channel.PluginId;
					info.Type = channel.Type.FullName;

					channels.Add(info);
				}
			}

			return channels.ToArray();
		}

		public void RegisterCallback(string channelId)
		{
			IDataUpdatedCallback callback = OperationContext.Current.GetCallbackChannel<IDataUpdatedCallback>();

			IChannel channel = Env.Current.CommunicationPlugins.GetChannel(channelId);
			if (channel != null)
			{
				ChannelEventHandler handler = new ChannelEventHandler(channel, callback, OperationContext.Current.Channel);
				handler.Disconnected += new EventHandler(OnHandlerDisconnected);
				subscribers.Add(handler);
			}
		}

		void OnHandlerDisconnected(object sender, EventArgs e)
		{
			ChannelEventHandler handler = (ChannelEventHandler)sender;
			handler.Disconnected -= new EventHandler(OnHandlerDisconnected);
			subscribers.Remove(handler);
		}

		public void SetChannelValue(string channelId, string value)
		{
			IChannel channel = Env.Current.CommunicationPlugins.GetChannel(channelId);
			if (channel != null)
			{
				channel.Value = Convert.ChangeType(value, channel.Type);
			}
		}

		public ChannelState GetChannelState(string channelId)
		{
			ChannelState state = new ChannelState();
			IChannel channel = Env.Current.CommunicationPlugins.GetChannel(channelId);
			if (channel != null)
			{
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
			}

			return state;
		}
	}
}
