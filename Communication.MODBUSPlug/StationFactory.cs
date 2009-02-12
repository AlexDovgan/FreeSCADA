using System.Xml;
using System.IO.Ports;
using System;

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
            try { failedCount = int.Parse(node.Attributes["failedCount"].Value); }
            catch { };
            int loggingLevel = 0;
            try { loggingLevel = int.Parse(node.Attributes["loggingLevel"].Value); }
            catch { };
            switch (type)
            {
                case "ModbusTCPClientStation":
                    string ipAddress = node.Attributes["ipAddress"].Value;
                    int tcpPort = int.Parse(node.Attributes["tcpPort"].Value);
                    ist = CreateTCPClientStation(name, plugin, ipAddress, tcpPort, cycleTimeout, retryTimeout, retryCount, failedCount);
                    break;
                case "ModbusSerialClientStation":
                    string comPort = node.Attributes["comPort"].Value;
                    ist = CreateSerialClientStation(name, plugin, comPort, cycleTimeout, retryTimeout, retryCount, failedCount);
                    try { (ist as ModbusSerialClientStation).BaudRate = int.Parse(node.Attributes["baudRate"].Value); }
                    catch { }
                    try { (ist as ModbusSerialClientStation).DataBits = int.Parse(node.Attributes["dataBits"].Value); }
                    catch { }
                    try { (ist as ModbusSerialClientStation).SerialType = (ModbusSerialType)Enum.Parse(typeof(ModbusSerialType), node.Attributes["serialType"].Value); }
                    catch { }
                    try { (ist as ModbusSerialClientStation).StopBits = (StopBits)Enum.Parse(typeof(StopBits), node.Attributes["stopBits"].Value); }
                    catch { }
                    try { (ist as ModbusSerialClientStation).Parity = (Parity)Enum.Parse(typeof(Parity), node.Attributes["parity"].Value); }
                    catch { }
                    try { (ist as ModbusSerialClientStation).Handshake = (Handshake)Enum.Parse(typeof(Handshake), node.Attributes["handshake"].Value); }
                    catch { }
                    break;
            }
            ist.LoggingLevel = loggingLevel;
            return ist;
        }

        public static IModbusStation CreateTCPClientStation(string name, Plugin plugin, string ipAddress, int tcpPort, int cycleTimeout, int retryTimeout, int retryCount, int failedCount)
		{
            return new ModbusTCPClientStation(name, plugin, ipAddress, tcpPort, cycleTimeout, retryTimeout, retryCount, failedCount);
		}

        public static IModbusStation CreateSerialClientStation(string name, Plugin plugin, string comPort, int cycleTimeout, int retryTimeout, int retryCount, int failedCount)
        {
            return new ModbusSerialClientStation(name, plugin, comPort, cycleTimeout, retryTimeout, retryCount, failedCount);
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
            if (stat is ModbusSerialClientStation)
            {
                ModbusSerialClientStation serstat = (ModbusSerialClientStation)stat;
                node.SetAttribute("type", serstat.GetType().Name);
                node.SetAttribute("comPort", serstat.ComPort);
                node.SetAttribute("serialType", serstat.SerialType.ToString());
                node.SetAttribute("baudRate", serstat.BaudRate.ToString());
                node.SetAttribute("dataBits", serstat.DataBits.ToString());
                node.SetAttribute("parity", serstat.Parity.ToString());
                node.SetAttribute("stopBits", serstat.StopBits.ToString());
                node.SetAttribute("handshake", serstat.Handshake.ToString());
            }
            node.SetAttribute("cycleTimeout", stat.CycleTimeout.ToString());
            node.SetAttribute("retryTimeout", stat.RetryTimeout.ToString());
            node.SetAttribute("retryCount", stat.RetryCount.ToString());
            node.SetAttribute("failedCount", stat.FailedCount.ToString());
            node.SetAttribute("loggingLevel", stat.LoggingLevel.ToString());
        }
	}
}
