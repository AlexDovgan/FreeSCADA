using System;
using System.Collections.Generic;
using System.ServiceModel;
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
			client = new DataRetrieverClient(new InstanceContext(this), new WSDualHttpBinding(), epAddress);
			if (client != null)
				client.Open();

			if (client != null && client.State == CommunicationState.Opened)
			{
				foreach (RemoutingChannel channel in channels)
					client.RegisterCallback(channel.ServerFullId);
			}
		}

		public void Dispose()
		{
			if (client != null && client.State == CommunicationState.Opened)
				client.Close();
		}

		public void ValueChanged(string channelId, string value, DateTime modifyTime, string status)
		{
			if (channels.ContainsKey(channelId))
			{
				RemoutingChannel channel = channels[channelId];
				object channelValue = Convert.ChangeType(value, channel.Type);
				ChannelStatusFlags flags = ChannelStatusFlags.Unknown;
				if(status == "Good")
					flags = ChannelStatusFlags.Good;
				else if(status == "Bad")
					flags = ChannelStatusFlags.Bad;

				channel.DoUpdate(channelValue, modifyTime, flags);
			}
		}
	}
}
