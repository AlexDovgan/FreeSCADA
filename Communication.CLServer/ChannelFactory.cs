using System;
using System.Globalization;
using System.Xml;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Communication.CLServer
{
	sealed class ChannelFactory
	{
		//Prevent class creation
		private ChannelFactory() { }

		public static IChannel CreateChannel(XmlElement node, Plugin plugin)
		{
			string name = node.Attributes["name"].Value;
			string server = node.Attributes["server"].Value;
			string fullId = node.Attributes["fullId"].Value;
			string port = node.Attributes["port"].Value;
			Type type = Type.GetType(node.Attributes["type"].Value);

			return CreateChannel(name, plugin, server, fullId, int.Parse(port, CultureInfo.InvariantCulture), type);
		}

		public static IChannel CreateChannel(string name, Plugin plugin, string server, string fullId, int port,Type type)
		{
			return new RemoutingChannel(name, plugin, server, fullId, port, type);
		}

		public static void SaveChannel(XmlElement node, IChannel channel)
		{
			RemoutingChannel channelBase = (RemoutingChannel)channel;
			node.SetAttribute("name", channelBase.Name);
			node.SetAttribute("server", channelBase.Server);
			node.SetAttribute("fullId", channelBase.ServerFullId);
			node.SetAttribute("port", channelBase.Port.ToString(CultureInfo.InvariantCulture));
			node.SetAttribute("type", channelBase.Type.ToString());
		}
	}
}
