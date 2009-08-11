using System;
using System.Runtime.Serialization;

namespace FreeSCADA.CLServer
{
	public enum ChannelStatusFlags
	{
		Unknown = 0,
		Good = 1,
		Bad = 2,
		NotUsed = 0xFFFF
	}

	[DataContract]
	public class ChannelState
	{
		[DataMember]
		public string Type { get; set; }

		[DataMember]
		public string Value { get; set; }

		[DataMember]
		public DateTime ModifyTime { get; set; }

		[DataMember]
		public ChannelStatusFlags Status { get; set; }
	}
}
