using System;
using System.Xml;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Communication.MODBUSPlug
{
	sealed class ChannelFactory
	{
		//Prevent class reation
		private ChannelFactory() { }

		public static IChannel CreateChannel(XmlElement node, Plugin plugin)
		{
            string name = node.Attributes["name"].Value;
            string type = node.Attributes["type"].Value;
            string modbusStation = node.Attributes["modbusStation"].Value;
            string modbusType = node.Attributes["modbusType"].Value;
            string modbusAddress = node.Attributes["modbusAddress"].Value;
            //Type t = Type.GetType(type);

            return CreateChannel(name, plugin, type, modbusStation, modbusType, modbusAddress);
		}

        public static IChannel CreateChannel(string name, Plugin plugin, string type, string modbusStation, string modbusType, string modbusAddress)
		{
            Type t;
            switch (type)
            {
                case "Unsigned":
                    t = typeof(uint);
                    break;
                case "Float":
                    t = typeof(float);
                    break;
                default:
                    t = typeof(int);
                    break;
            }
            return new ModbusChannelImp(name, plugin, t, modbusStation, modbusType, modbusAddress);
		}

		public static void SaveChannel(XmlElement node, IChannel channel)
		{
			ModbusChannelImp channelBase = (ModbusChannelImp)channel;
			node.SetAttribute("name", channelBase.Name);
            node.SetAttribute("type", channelBase.Type.ToString());
            node.SetAttribute("modbusStation", channelBase.ModbusStation);
            node.SetAttribute("modbusType", channelBase.ModbusType);
            node.SetAttribute("modbusAddress", channelBase.ModbusAddress);
		}
	}
}
