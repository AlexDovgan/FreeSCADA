using System;
using FreeSCADA.Common;
namespace FreeSCADA.Communication.OPCPlug
{
    /// <summary>
    /// TODO:  may be need to implement one abstract base class for implementation base functionality with 
    /// events
    /// </summary>
    class OPCBaseChannel:BaseChannel
	{
		string opcChannel;
		string opcServer;
		string opcHost;
        
		public OPCBaseChannel(string name, Plugin plugin, string opcChannel, string opcServer, string opcHost)
            :base(name,false,plugin,typeof(object))
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
        public override void ExternalSetValue(object value)
        {
            IntPtr addErrors;
            //opcItem.WriteVQT(1, new string[] { opcChannel }, new OPCITEMVQT[] { value }, out addErrors);
        }
	}
}
