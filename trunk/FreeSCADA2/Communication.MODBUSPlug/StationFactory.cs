using System.Xml;

namespace FreeSCADA.Communication.MODBUSPlug
{
	sealed class StationFactory
	{
		//Prevent class reation
        private StationFactory() { }

		public static ModbusStation CreateStation(XmlElement node, Plugin plugin)
		{
			string name = node.Attributes["name"].Value;
            string ipAddress = node.Attributes["ipAddress"].Value;
            int tcpPort = int.Parse(node.Attributes["tcpPort"].Value);

            return CreateStation(name, plugin, ipAddress, tcpPort);
		}

        public static ModbusStation CreateStation(string name, Plugin plugin, string ipAddress, int tcpPort)
		{
            return new ModbusStation(name, plugin, ipAddress, tcpPort);
		}

        public static void SaveStation(XmlElement node, ModbusStation stat)
		{
            node.SetAttribute("name", stat.Name);
            node.SetAttribute("ipAddress", stat.IPAddress);
            node.SetAttribute("tcpPort", stat.TCPPort.ToString());
		}
	}
}
