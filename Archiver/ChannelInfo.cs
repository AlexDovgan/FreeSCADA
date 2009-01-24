
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

		public override bool Equals(object obj)
		{
			if (obj is ChannelInfo)
			{
				ChannelInfo ci = obj as ChannelInfo;
				return ci.ChannelName == ChannelName && ci.PluginId == PluginId;
			}
			else
				return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
