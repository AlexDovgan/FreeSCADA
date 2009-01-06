using System;

namespace FreeSCADA.Interfaces
{
	namespace Plugins
	{
		public interface ICommunicationPlug
		{
			event EventHandler ChannelsChanged;

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

			bool Connect();
			void Disconnect();
		}
	}
}
