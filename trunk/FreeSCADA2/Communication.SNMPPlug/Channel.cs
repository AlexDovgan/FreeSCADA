using System;
using System.Collections.Generic;
using FreeSCADA.Common;


namespace FreeSCADA.Communication.SNMPPlug
{
    /// <summary>
    /// TODO:  may be need to implement one abstract base class for implementation base functionality with 
    /// events
    /// </summary>
    public class SNMPChannelImp: BaseChannel
	{
        string agentName;
        string oid;

        public SNMPChannelImp(string name, Plugin plugin, Type type, string agentName, string oid)
            : base(name, false, plugin, type)
		{
            this.agentName = agentName;
            this.oid = oid;
        }

        public SNMPChannelImp(string name, SNMPChannelImp ch)
            : base(name, false, ch.plugin, ch.Type)
        {
            this.agentName = ch.agentName;
            this.oid = ch.oid;
        }

        public string AgentName
		{
            get { return agentName; }
            set { agentName = value; }
        }

        public string Oid
		{
            get { return oid; }
            set { oid = value; }
        }

        public SNMPAgent MyAgent { get; set; }

        public override object Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                if (!IsReadOnly && plugin.IsConnected && MyAgent != null)
                {
                    //MyAgent.SendValueUpdateToModbusLine(this);
                    base.Value = value;
                }
            }
        }

        public override void DoUpdate()
        {
        }

    }

}
