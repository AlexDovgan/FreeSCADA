using System;
using System.Collections.Generic;
using OpcRcw.Da;

namespace FreeSCADA.Communication.OPCPlug
{
	class OPCDataCallback : IOPCDataCallback
	{
		List<OpcChannelImp> channels;

		public OPCDataCallback(List<OpcChannelImp> channels)
		{
			this.channels = channels;
		}

		#region IOPCDataCallback Members

		public void OnCancelComplete(int dwTransid, int hGroup)
		{
			throw new NotImplementedException();
		}

		const short Q_GOOD = 0x03 << 6;
		public void OnDataChange(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount, int[] phClientItems, object[] pvValues, short[] pwQualities, FILETIME[] pftTimeStamps, int[] pErrors)
		{
			for (int i = 0; i < dwCount; i++)
			{
				if ((pwQualities[i] & Q_GOOD) != Q_GOOD)
					continue;

				foreach (OpcChannelImp ch in channels)
				{
					if (ch.GetHashCode() == phClientItems[i])
						ch.DoUpdate(pvValues[i]);
				}
			}
		}

		public void OnReadComplete(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount, int[] phClientItems, object[] pvValues, short[] pwQualities, FILETIME[] pftTimeStamps, int[] pErrors)
		{
			throw new NotImplementedException();
		}

		public void OnWriteComplete(int dwTransid, int hGroup, int hrMastererr, int dwCount, int[] pClienthandles, int[] pErrors)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
