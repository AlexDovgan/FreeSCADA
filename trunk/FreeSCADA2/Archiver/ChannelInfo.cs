
namespace FreeSCADA.Archiver
{
	public class ChannelInfo
	{
		string pluginId;
		string channelName;

		public string PluginId
		{
			get { return pluginId; }
			set { pluginId = value; }
		}

		public string ChannelName
		{
			get { return channelName; }
			set { channelName = value; }
		}
	}
}
