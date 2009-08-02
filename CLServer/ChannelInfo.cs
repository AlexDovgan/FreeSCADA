using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FreeSCADA.CLServer
{
	[DataContract]
	public class ChannelInfo
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string PluginId { get; set; }

		[DataMember]
		public string FullId { get; set; }

		[DataMember]
		public string Type { get; set; }

		[DataMember]
		public bool IsReadOnly { get; set; }
	}
}
