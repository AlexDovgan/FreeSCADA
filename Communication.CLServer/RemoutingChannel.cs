using System;
using FreeSCADA.Common;

namespace FreeSCADA.Communication.CLServer
{
	class RemoutingChannel:BaseChannel
	{
		string server;
		string serverFullId;
		int port;

		public RemoutingChannel(string name, Plugin plugin, string server, string fullId, int port, Type type)
			: base(name, false, plugin, type)
		{
			this.server = server;
			this.serverFullId = fullId;
			this.port = port;
		}

		public string Server
		{
			get { return server; }
		}

		public string ServerFullId
		{
			get { return serverFullId; }
		}

		public int Port
		{
			get { return port; }
		}

		public override void DoUpdate()
		{
			throw new NotImplementedException();
		}
	}
}
