using System;
using System.Xml;
using FreeSCADA.Common;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Communication.Timers
{
	sealed class ChannelFactory
	{
		//Prevent class reation
		private ChannelFactory() { }

		public static IChannel CreateChannel(XmlElement node, Plugin plugin)
		{
			string type = node.Attributes["type"].Value;
			string name = node.Attributes["name"].Value;
			double interval = double.Parse(node.Attributes["interval"].Value);

			return CreateChannel(type, name, plugin, interval);
		}

		public static IChannel CreateChannel(string type, string name, Plugin plugin, double interval)
		{
			IChannel channel = null;
			Type channel_type = Type.GetType(type);

			object[] args = { name, plugin, interval };
			channel = (IChannel)Activator.CreateInstance(channel_type, args);

			return channel;
		}

		public static void SaveChannel(XmlElement node, IChannel channel)
		{
            RelativeTimerChannel channelBase = (RelativeTimerChannel)channel;
			node.SetAttribute("type", channelBase.GetType().FullName);
			node.SetAttribute("name", channelBase.Name);
			node.SetAttribute("interval", channelBase.Interval.ToString());
		}
	}
}
