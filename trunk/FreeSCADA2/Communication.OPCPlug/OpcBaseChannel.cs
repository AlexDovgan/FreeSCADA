using FreeSCADA.Common;
namespace FreeSCADA.Communication.OPCPlug
{
    /// <summary>
    /// TODO:  may be need to implement one abstract base class for implementation base functionality with 
    /// events
    /// must be a public for possibility loading from XAML murkup
    /// </summary>
    public class OPCBaseChannel:BaseChannel
	{
		string opcChannel;
		string opcServer;
		string opcHost;
		ConnectionGroup opcConnection;
		int opcHandle;
        
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

		public override object Value
		{
			get
			{
				return base.Value;
			}
			set
			{
				if (!IsReadOnly && plugin.IsConnected && opcConnection != null)
				{
					opcConnection.WriteChannel(opcHandle, value);
					base.Value = value;
				}
			}
		}

		internal void Connect(ConnectionGroup opcConnection, int opcHandle, bool readOnly)
		{
			this.opcConnection = opcConnection;
			this.opcHandle = opcHandle;
			this.readOnly = readOnly;
		}

		internal void Disconnect()
		{
			this.opcConnection = null;
			this.opcHandle = 0;
		}

        public override void DoUpdate()
        {
        }
	}
}
