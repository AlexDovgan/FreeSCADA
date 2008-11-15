using System;
using System.ComponentModel;
using FreeSCADA.Common;
namespace FreeSCADA.Communication.OPCPlug
{
    /// <summary>
    /// TODO:  may be need to implement one abstract base class for implementation base functionality with 
    /// events
    /// </summary>
    class OpcChannelImp:BaseChannel
	{

		string opcChannel;
		string opcServer;
		string opcHost;
		public OpcChannelImp(string name, Plugin plugin, string opcChannel, string opcServer, string opcHost)
            :base(name,false,plugin,typeof(Double))
		{
			
			this.opcChannel = opcChannel;
			this.opcHost = opcHost;
			this.opcServer = opcServer;
		}


		public string OpcChannel
		{
			get { return opcChannel; }
		}
		public string OpcServer
		{
			get { return opcServer; }
		}

		public string OpcHost
		{
			get { return opcHost; }
		}
        public override void DoUpdate()
        {
        }

	}
}
