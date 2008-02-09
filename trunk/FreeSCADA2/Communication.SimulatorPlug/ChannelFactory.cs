using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreeSCADA.ShellInterfaces;
using System.Xml;

namespace FreeSCADA.Communication.SimulatorPlug
{
	sealed class ChannelFactory
	{
		public static IChannel CreateChannel(XmlElement node)
		{
			return null;
		}

		public static void SaveChannel(XmlElement node, IChannel channel)
		{
		}
	}
}
