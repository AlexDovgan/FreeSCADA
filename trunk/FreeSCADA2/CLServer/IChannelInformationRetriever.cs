using System.ServiceModel;

namespace FreeSCADA.CLServer
{
	[ServiceContract]
	interface IChannelInformationRetriever
	{
		[OperationContract]
		ChannelInfo[] GetChannels();
	}
}
