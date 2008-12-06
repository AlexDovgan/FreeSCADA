using System;
using System.Collections.Generic;
using FreeSCADA.Interfaces;
using OpcRcw.Da;

namespace FreeSCADA.Communication.OPCPlug
{
	class OPCDataCallback : IOPCDataCallback
	{
		List<OPCBaseChannel> channels;

		public OPCDataCallback(List<OPCBaseChannel> channels)
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
				foreach (OPCBaseChannel ch in channels)
				{
					if (ch.GetHashCode() == phClientItems[i])
                    {
                        long ticks;
                        byte[] bticks = new byte[8];
                        BitConverter.GetBytes(pftTimeStamps[i].dwLowDateTime).CopyTo(bticks, 0);
                        BitConverter.GetBytes(pftTimeStamps[i].dwHighDateTime).CopyTo(bticks, 4);
                        ticks = BitConverter.ToInt64(bticks, 0);
                        DateTime dt = DateTime.FromFileTime(ticks);

						ChannelStatusFlags status;
						if ((pwQualities[i] & Q_GOOD) == Q_GOOD)
							status = ChannelStatusFlags.Good;
						else
							status = ChannelStatusFlags.Bad;

                        ch.DoUpdate(pvValues[i], dt, status);
                    }
                }
			}
		}

		public void OnReadComplete(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount, int[] phClientItems, object[] pvValues, short[] pwQualities, FILETIME[] pftTimeStamps, int[] pErrors)
		{
			throw new NotImplementedException();
		}

		public void OnWriteComplete(int dwTransid, int hGroup, int hrMastererr, int dwCount, int[] pClienthandles, int[] pErrors)
		{
			for (int i = 0; i < dwCount; i++)
			{
				foreach (OPCBaseChannel ch in channels)
				{
					if (ch.GetHashCode() == pClienthandles[i])
					{
						ch.StatusFlags = ChannelStatusFlags.Good;
					}
				}
			}
		}

		#endregion
	}
}
