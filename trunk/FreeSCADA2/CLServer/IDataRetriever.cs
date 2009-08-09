using System.ServiceModel;

namespace FreeSCADA.CLServer
{
	[ServiceContract(CallbackContract = typeof(IDataUpdatedCallback), SessionMode = SessionMode.Required)]
	interface IDataRetriever
	{
		[OperationContract(IsOneWay = true)]
		void RegisterCallback(string channelId);

		[OperationContract(IsOneWay = true)]
		void SetChannelValue(string channelId, string value);

		[OperationContract]
		ChannelState GetChannelState(string channelId);
	}	
}
