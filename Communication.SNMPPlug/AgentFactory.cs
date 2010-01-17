using System;
using System.IO.Ports;
using System.Xml;
using Lextm.SharpSnmpLib;

namespace FreeSCADA.Communication.SNMPPlug
{
	sealed class AgentFactory
	{
		//Prevent class reation
        private AgentFactory() { }

        public static SNMPAgent CreateAgent(XmlElement node, Plugin plugin)
        {
            SNMPAgent apf = null;
            string name = node.Attributes["name"].Value;
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
            bool stationActive = true;
            try { stationActive = bool.Parse(node.Attributes["agentActive"].Value); }
            catch { };
            string ipAddress = node.Attributes["ipAddress"].Value;
            int udpPort = int.Parse(node.Attributes["udpPort"].Value);
            string vCode = node.Attributes["versionCode"].Value;
            VersionCode version;
            switch (vCode)
            {
                case "V1":
                    version = VersionCode.V1;
                    break;
                case "V2":
                    version = VersionCode.V2;
                    break;
                case "V3":
                    version = VersionCode.V3;
                    break;
                default:
                    version = VersionCode.V1;
                    break;
            }
            string getCommunity = node.Attributes["getCommunity"].Value;
            string setCommunity = node.Attributes["setCommunity"].Value;

            apf = CreateAgent(name, plugin, ipAddress, udpPort, version, getCommunity, setCommunity, cycleTimeout, retryTimeout, retryCount, failedCount);
            apf.LoggingLevel = loggingLevel;
            apf.AgentActive = stationActive;
            return apf;
        }

        public static SNMPAgent CreateAgent(string name, Plugin plugin, string ipAddress, int udpPort, VersionCode version, string getCommunity,
                     string setCommunity, int cycleTimeout, int retryTimeout, int retryCount, int failedCount)
        {
            return new SNMPAgent(name, plugin, ipAddress, udpPort, version, getCommunity, setCommunity, cycleTimeout, retryTimeout, retryCount, failedCount);
		}

        public static void SaveAgent(XmlElement node, SNMPAgent agent)
		{
            node.SetAttribute("name", agent.Name);
            node.SetAttribute("ipAddress", agent.AgentIP.Address.ToString());                 //Address.ToString());
            node.SetAttribute("udpPort", agent.AgentIP.Port.ToString());
            node.SetAttribute("versionCode", agent.VersionCode.ToString());
            node.SetAttribute("getCommunity", agent.GetCommunity);
            node.SetAttribute("setCommunity", agent.SetCommunity);
            node.SetAttribute("cycleTimeout", agent.CycleTimeout.ToString());
            node.SetAttribute("retryTimeout", agent.RetryTimeout.ToString());
            node.SetAttribute("retryCount", agent.RetryCount.ToString());
            node.SetAttribute("failedCount", agent.FailedCount.ToString());
            node.SetAttribute("loggingLevel", agent.LoggingLevel.ToString());
            node.SetAttribute("agentActive", agent.AgentActive.ToString());
        }
	}
}
