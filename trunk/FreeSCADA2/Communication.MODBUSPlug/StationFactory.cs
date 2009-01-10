using System.Xml;

namespace FreeSCADA.Communication.MODBUSPlug
{
	sealed class StationFactory
	{
		//Prevent class reation
        private StationFactory() { }

        public static IModbusStation CreateStation(XmlElement node, Plugin plugin)
        {
            IModbusStation ist = null;
            string name = node.Attributes["name"].Value;
            string type;
            try { type = node.Attributes["type"].Value; }
            catch { type = "ModbusTCPClientStation"; }
            int cycleTimeout = 100;
            try { cycleTimeout = int.Parse(node.Attributes["cycleTimeout"].Value); }    // Backward compatibility
            catch { };
            int retryTimeout = 1000;
            try { retryTimeout = int.Parse(node.Attributes["retryTimeout"].Value); }
            catch { };
            int retryCount = 3;
            try { retryCount = int.Parse(node.Attributes["retryCount"].Value); }
            catch { };
            int failedCount = 20;
            try { retryCount = int.Parse(node.Attributes["failedCount"].Value); }
            catch { };
            switch (type)
            {
                case "ModbusTCPClientStation":
                    string ipAddress = node.Attributes["ipAddress"].Value;
                    int tcpPort = int.Parse(node.Attributes["tcpPort"].Value);
                    ist = CreateTCPClientStation(name, plugin, ipAddress, tcpPort, cycleTimeout, retryTimeout, retryCount, failedCount);
                    break;
            }
            return ist;
        }

        public static IModbusStation CreateTCPClientStation(string name, Plugin plugin, string ipAddress, int tcpPort, int cycleTimeout, int retryTimeout, int retryCount, int failedCount)
		{
            return new ModbusTCPClientStation(name, plugin, ipAddress, tcpPort, cycleTimeout, retryTimeout, retryCount, failedCount);
		}

        public static void SaveStation(XmlElement node, IModbusStation stat)
		{
            node.SetAttribute("name", stat.Name);
            if (stat is ModbusTCPClientStation)
            {
                ModbusTCPClientStation tcpstat = (ModbusTCPClientStation)stat;
                node.SetAttribute("type", tcpstat.GetType().Name);
                node.SetAttribute("ipAddress", tcpstat.IPAddress);
                node.SetAttribute("tcpPort", tcpstat.TCPPort.ToString());
            }
            node.SetAttribute("cycleTimeout", stat.CycleTimeout.ToString());
            node.SetAttribute("retryTimeout", stat.RetryTimeout.ToString());
            node.SetAttribute("retryCount", stat.RetryCount.ToString());
		}
	}
}
