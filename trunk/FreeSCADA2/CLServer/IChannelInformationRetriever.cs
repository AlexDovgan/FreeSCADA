using System.ServiceModel;

namespace FreeSCADA.CLServer
{
	[ServiceContract]
	interface IChannelInformationRetriever
	{
		[OperationContract]
		ChannelInfo[] GetChannels();

		[OperationContract]
		long GetChannelsCount();

		[OperationContract]
		ChannelInfo GetChannel(long index);
	}
}
