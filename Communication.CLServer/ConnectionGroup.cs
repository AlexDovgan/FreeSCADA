using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Communication.CLServer
{
	class ConnectionGroup:IDisposable,IDataRetrieverCallback
	{
		Dictionary<string, RemoutingChannel> channels = new Dictionary<string, RemoutingChannel>();
		DataRetrieverClient client;

		public ConnectionGroup(string server, int port, List<RemoutingChannel> channels)
		{
			foreach (RemoutingChannel channel in channels)
				this.channels[channel.ServerFullId] = channel;

			EndpointAddress epAddress = new EndpointAddress(string.Format("http://{0}:{1}/DataRetriever", server, port));
			client = new DataRetrieverClient(new InstanceContext(this), new WSDualHttpBinding(WSDualHttpSecurityMode.None), epAddress);
			if (client != null)
				client.Open();

			if (client != null && client.State == CommunicationState.Opened)
			{
				foreach (RemoutingChannel channel in channels)
					client.RegisterCallback(channel.ServerFullId);

				//ThreadPool.QueueUserWorkItem(new WaitCallback(RefreshChannels), channels);
			}
		}

		public void Dispose()
		{
			if (client != null && client.State == CommunicationState.Opened)
				client.Abort();
		}

		public void ValueChanged(string channelId, FreeSCADA.CLServer.ChannelState state)
		{
			if (channels.ContainsKey(channelId))
				UpdateChannel(state, channels[channelId]);
		}

		private static void UpdateChannel(FreeSCADA.CLServer.ChannelState state, RemoutingChannel channel)
		{
			object channelValue = null;
			Type valueType = Type.GetType(state.Type);
			if (valueType != null)
				channelValue = Convert.ChangeType(state.Value, valueType);
			else
				channelValue = Convert.ChangeType(state.Value, channel.Type);

			ChannelStatusFlags flags = ChannelStatusFlags.Unknown;
			switch (state.Status)
			{
				case FreeSCADA.CLServer.ChannelStatusFlags.Bad:
					flags = ChannelStatusFlags.Bad;
					break;
				case FreeSCADA.CLServer.ChannelStatusFlags.Good:
					flags = ChannelStatusFlags.Good;
					break;
				case FreeSCADA.CLServer.ChannelStatusFlags.NotUsed:
					flags = ChannelStatusFlags.NotUsed;
					break;
			}

			channel.DoUpdate(channelValue, state.ModifyTime, flags);
		}

		private void RefreshChannels(object obj)
		{
			if (client == null || client.State != CommunicationState.Opened)
				return;

			List<RemoutingChannel> channelsToUpdate = obj as List<RemoutingChannel>;
			foreach (RemoutingChannel ch in channelsToUpdate)
			{
				try
				{
					UpdateChannel(client.GetChannelState(ch.ServerFullId), ch);
				}
				catch(Exception)
				{
				}
			}
		}
	}
}
