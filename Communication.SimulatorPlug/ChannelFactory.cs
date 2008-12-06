using System;
using System.Xml;
using FreeSCADA.Common;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Communication.SimulatorPlug
{
	sealed class ChannelFactory
	{
		//Prevent class reation
		private ChannelFactory() { }

		public static IChannel CreateChannel(XmlElement node, Plugin plugin)
		{
			string type = node.Attributes["type"].Value;
			string name = node.Attributes["name"].Value;
			bool readOnly = node.HasAttribute("readOnly")?bool.Parse(node.Attributes["readOnly"].Value):true;

			return CreateChannel(type, name, readOnly, plugin);
		}

		public static IChannel CreateChannel(string type, string name, bool readOnly, Plugin plugin)
		{
			IChannel channel = null;
			Type channel_type = Type.GetType(type);

            if (channel_type == typeof(CurrentTimeChannel) ||
                channel_type == typeof(RandomIntegerChannel) ||
                channel_type == typeof(SawIntegerChannel) ||
                channel_type == typeof(RampIntegerChannel) ||
                channel_type == typeof(SinusDoubleChannel))
			{
				object[] args = { name, plugin };
				channel = (IChannel)Activator.CreateInstance(channel_type, args);
			}
			else
			{
				object[] args = { name, readOnly, plugin };
				channel = (IChannel)Activator.CreateInstance(channel_type, args);
			}

			return channel;
		}

		public static void SaveChannel(XmlElement node, IChannel channel)
		{
			BaseChannel channelBase = (BaseChannel)channel;
			node.SetAttribute("type", channelBase.GetType().FullName);
			node.SetAttribute("name", channelBase.Name);
			node.SetAttribute("readOnly", channelBase.IsReadOnly.ToString());
		}
	}
}
