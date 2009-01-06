using System;
using System.Collections.Generic;

namespace FreeSCADA.Archiver
{
	[Serializable]
	public class Rule
	{
		bool enable = false;
		string name = "<no name>";
		List<ChannelInfo> channels = new List<ChannelInfo>();
		List<BaseCondition> conditions = new List<BaseCondition>();

		public bool Enable
		{
			get { return enable; }
			set { enable = value; }
		}

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public List<ChannelInfo> Channels
		{
			get { return channels; }
			set { channels = value; }
		}

		public List<BaseCondition> Conditions
		{
			get { return conditions; }
			set { conditions = value; }
		}

		public bool Archive
		{
			get 
			{
				if (Enable == false)
					return false;

				foreach (BaseCondition cond in conditions)
				{
					if (cond.IsValid == false)
						return false;
				}
				return true;
			}
		}

		public void AddChannel(ChannelInfo channel)
		{
			channels.Add(channel);
		}

		public void AddCondition(BaseCondition condition)
		{
			conditions.Add(condition);
		}
	}
}
