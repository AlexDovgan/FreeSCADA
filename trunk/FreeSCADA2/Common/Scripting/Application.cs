using FreeSCADA.Interfaces;

namespace FreeSCADA.Common.Scripting
{
	class Application
	{
		public IChannel GetChannel(string name)
		{
			return Env.Current.CommunicationPlugins.GetChannel(name);
		}
	}
}
