using System;

namespace FreeSCADA.ShellInterfaces
{
	namespace Plugins
	{
		public interface ICommunicationPlug
		{
			String Name
			{
				get;
			}

			IChannel[] Channels
			{
				get;
			}

			string PluginId
			{
				get;
			}

			bool IsConnected
			{
				get;
			}

			void Initialize(IEnvironment environment);

			void ProcessCommand(int commandId);

			bool Connect();
			void Disconnect();
		}
	}
}
