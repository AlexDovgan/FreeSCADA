using System.Xml;
using FreeSCADA.ShellInterfaces;

namespace FreeSCADA.Communication.OPCPlug
{
	sealed class ChannelFactory
	{
		//Prevent class reation
		private ChannelFactory() { }

		public static IChannel CreateChannel(XmlElement node, Plugin plugin)
		{
			string name = node.Attributes["name"].Value;
			string opcChannel = node.Attributes["opcChannel"].Value;
			string opcServer = node.Attributes["opcServer"].Value;
			string opcHost = node.Attributes["opcHost"].Value;

			return CreateChannel(name, plugin, opcChannel, opcServer, opcHost);
		}

		public static IChannel CreateChannel(string name, Plugin plugin, string opcChannel, string opcServer, string opcHost)
		{
			return new OpcChannelImp(name, plugin, opcChannel, opcServer, opcHost);
		}

		public static void SaveChannel(XmlElement node, IChannel channel)
		{
			OpcChannelImp channelBase = (OpcChannelImp)channel;
			node.SetAttribute("name", channelBase.Name);
			node.SetAttribute("opcChannel", channelBase.OpcChannel);
			node.SetAttribute("opcServer", channelBase.OpcServer);
			node.SetAttribute("opcHost", channelBase.OpcHost);
		}
	}
}
