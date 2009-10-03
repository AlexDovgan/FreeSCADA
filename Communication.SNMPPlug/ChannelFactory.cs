using System;
using System.Globalization;
using System.Xml;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Communication.SNMPPlug
{
    sealed class ChannelFactory
    {
        //Prevent class creation
        private ChannelFactory() { }

        public static IChannel CreateChannel(XmlElement node, Plugin plugin)
        {
            string name = node.Attributes["name"].Value;
            string type = node.Attributes["type"].Value;
            string agent = node.Attributes["agent"].Value;
            string oid = node.Attributes["oid"].Value;

            Type t = Type.GetType("System." + type);

            SNMPChannelImp ch = (SNMPChannelImp)CreateChannel(name, plugin, t, agent, oid);

            return ch;
        }

        public static IChannel CreateChannel(string name, Plugin plugin, Type type, string agent, string oid)
        {
            return new SNMPChannelImp(name, plugin, type, agent, oid);
        }

        public static void SaveChannel(XmlElement node, IChannel channel)
        {
            SNMPChannelImp channelBase = (SNMPChannelImp)channel;
            node.SetAttribute("name", channelBase.Name);
            node.SetAttribute("type", channelBase.GetType().ToString());
            node.SetAttribute("agent", channelBase.AgentName);
            node.SetAttribute("oid", channelBase.Oid);
        }
    }
}
