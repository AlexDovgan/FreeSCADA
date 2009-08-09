﻿using System;
using System.ServiceModel;

namespace FreeSCADA.CLServer
{
	interface IDataUpdatedCallback
	{
		[OperationContract(IsOneWay = true)]
		void ValueChanged(string channelId, ChannelState state);
	}
}
