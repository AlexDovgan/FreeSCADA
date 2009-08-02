using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace FreeSCADA.CLServer
{
	interface IDataUpdatedCallback
	{
		[OperationContract(IsOneWay = true)]
		void ValueChanged(string channelId, string value);
	}
}
