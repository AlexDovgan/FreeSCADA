using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Mib;
using Lextm.SharpSnmpLib.Messaging;
using System.Threading;

namespace FreeSCADA.Communication.SNMPPlug
{
    public class SNMPAgent
	{
        private string _get;
	    private string _set;
	    private VersionCode _version;
        private IPEndPoint _agentIP;
        private string _name;
        private bool _agentActive = true;
        protected List<SNMPChannelImp> channels = new List<SNMPChannelImp>();
        int _cycleTimeout = 100;
        int _retryTimeout = 1000;
        int _retryCount = 3;
        int _failedCount = 20;
        int _loggingLevel = 0;

        protected Queue<SNMPChannelImp> sendQueue = new Queue<SNMPChannelImp>();
        protected object sendQueueSyncRoot = new object();
        protected ManualResetEvent sendQueueEndWaitEvent = new ManualResetEvent(false);
        protected List<SNMPChannelImp> channelsToSend = new List<SNMPChannelImp>();
        protected volatile bool runThread;

        internal SNMPAgent(VersionCode version, IPEndPoint agentIP, string getCommunity, string setCommunity, string name)
	    {
	        _get = getCommunity;
	        _set = setCommunity;
	        _version = version;
            _agentIP = agentIP;
            _name = name;
	    }

        public SNMPAgent(string name, Plugin plugin, string ipAddress, int udpPort, VersionCode version, string getCommunity,
                     string setCommunity, int cycleTimeout, int retryTimeout, int retryCount, int failedCount)
        {
	        _get = getCommunity;
	        _set = setCommunity;
	        _version = version;
            _agentIP = new IPEndPoint(IPAddress.Parse(ipAddress), udpPort);
            _name = name;
            _cycleTimeout = cycleTimeout;
            _retryTimeout = retryTimeout;
            _retryCount = retryCount;
            _failedCount = failedCount;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public IPEndPoint AgentIP
        {
            get
            {
                return _agentIP;
            }
            set
            {
                _agentIP = value;
            }
        }

        internal VersionCode VersionCode
        {
            get { return _version; }
            set { _version = value; }
        }

        public bool AgentActive
        {
            get { return _agentActive; }
            set { _agentActive = value; }
        }

        public int CycleTimeout
        {
            get { return _cycleTimeout; }
            set { _cycleTimeout = value; }
        }

        public int RetryTimeout
        {
            get { return _retryTimeout; }
            set { _retryTimeout = value; }
        }

        public int RetryCount
        {
            get { return _retryCount; }
            set { _retryCount = value; }
        }

        public int FailedCount
        {
            get { return _failedCount; }
            set { _failedCount = value; }
        }

        public int LoggingLevel
        {
            get { return _loggingLevel; }
            set { _loggingLevel = value; }
        }

        public string GetCommunity
        {
            get { return _get; }
            set { _get = value; }
        }

        public string SetCommunity
        {
            get { return _set; }
            set { _set = value; }
        }
	
        internal string Get(Manager manager, string textual)
        {
            Variable var = manager.Objects.CreateVariable(textual);
            TraceSource source = new TraceSource("Browser");
            source.TraceInformation(manager.GetSingle(_agentIP, _get, var).ToString());
            source.Flush();
            source.Close();
            return var.ToString();
        }

        internal string GetValue(Manager manager, string textual)
        {
            Variable var = manager.Objects.CreateVariable(textual);

            return manager.GetSingle(_agentIP, _get, var).Data.ToString();
        }

        internal string GetNext(Manager manager, string textual)
        {
            //Variable var = new Variable(textual);

            //Report(manager.GetNext(_agent.Address, _agent.Port, _get, var));
            return "";
        }
	
        //
        // TODO: return success if it succeeded!
        //
	    internal void Set(Manager manager, string textual, ISnmpData data)
	    {
            manager.SetSingle(_agentIP, _set, manager.Objects.CreateVariable(textual, data));
	    }
	
        internal static bool IsValidIPAddress(string address, out IPAddress ip)
        {
            return IPAddress.TryParse(address, out ip);
        }
	
	    internal void Walk(Manager manager, IDefinition def)
	    {
            IList<Variable> list = new List<Variable>();

	        int rows = Messenger.Walk(VersionCode, AgentIP, new OctetString(GetCommunity), new ObjectIdentifier(def.GetNumericalForm()), list, 1000, WalkMode.WithinSubtree);
            
            // 
            // How many rows are there?
            //
            if (rows > 0)
            {
                FormTable newTable = new FormTable(def);
                newTable.SetRows(rows);
                newTable.PopulateGrid(list);
                newTable.Show();
            }
            else
            {
                TraceSource source = new TraceSource("Browser");
                for (int i = 0; i < list.Count; i++)
                {
                    source.TraceInformation(list[i].ToString());
                }

                source.Flush();
                source.Close();
            }

	    }

        public void AddChannel(SNMPChannelImp channel)
        {
            channels.Add(channel);
        }

        public void ClearChannels()
        {
            channels.Clear();
        }

        public void Stop()
        {
            //MessageBox.Show("Stopping Modbus station " + name, "Info");
            if (!AgentActive) return;

            runThread = false;
            lock (sendQueueSyncRoot)
            {
                sendQueue.Clear();
            }
        }

        public int Start()
        {
            //MessageBox.Show("Starting Modbus station " + name, "Info");
            if (!AgentActive) return 1;
            return 0;
        }

    }
}
