using System.Collections.Generic;
using System.Xml.Serialization;
using FreeSCADA.Common;
using FreeSCADA.Interfaces;


namespace FreeSCADA.Archiver
{
	public class ChannelsSettings
	{
		List<Rule> rules = new List<Rule>();

		public List<ChannelInfo> Channels
		{
			get
			{
				List<ChannelInfo> res = new List<ChannelInfo>();

				foreach (string plugId in Env.Current.CommunicationPlugins.PluginIds)
				{
					FreeSCADA.Interfaces.Plugins.ICommunicationPlug plug = Env.Current.CommunicationPlugins[plugId];

					foreach (IChannel channel in plug.Channels)
					{
						ChannelInfo item = new ChannelInfo();
						item.PluginId = plugId;
						item.ChannelName = channel.Name;
						res.Add(item);
					}
				}
				return res;
			}
		}

		public List<Rule> Rules
		{
			get
			{
				return rules;
			}
			set
			{
				rules = value;
			}
		}

		public void Clear()
		{
			rules.Clear();
		}

		public void AddRule(Rule rule)
		{
			rules.Add(rule);
		}

		public void Load()
		{
			using (System.IO.Stream ms = Env.Current.Project["settings/archiver/rules.cfg"])
			{
				if (ms == null || ms.Length == 0)
					return;

				XmlSerializer serializer = new XmlSerializer(typeof(List<Rule>));
				rules = (List<Rule>)serializer.Deserialize(ms);
			}
		}

		public void Save()
		{
			using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
			{
				XmlSerializer serializer = new XmlSerializer(typeof(List<Rule>));
				serializer.Serialize(ms, rules);

				Env.Current.Project.SetData("settings/archiver/rules.cfg", ms);
			}
		}
	}
}
