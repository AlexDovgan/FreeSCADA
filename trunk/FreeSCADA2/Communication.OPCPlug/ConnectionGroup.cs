using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using OpcRcw.Da;

namespace FreeSCADA.Communication.OPCPlug
{
	class ConnectionGroup
	{
		OPCDataCallback callback;
		IOPCItemMgt group;
		int callbackCookie;
		IOPCServer server;

		public ConnectionGroup(string opcServer, string opcHost, List<Channel> channels)
		{
			Type t = Type.GetTypeFromProgID(opcServer, opcHost);
			server = (IOPCServer)Activator.CreateInstance(t);
			int groupClientId = 1;
			int groupId;
			int updateRate = 0;
			object group_obj;
			Guid tmp_guid = typeof(IOPCItemMgt).GUID;
			server.AddGroup("", 1, updateRate, groupClientId, new IntPtr(), new IntPtr(), 0, out groupId, out updateRate, ref tmp_guid, out group_obj);

			group = (IOPCItemMgt)group_obj;
			OPCITEMDEF[] items = new OPCITEMDEF[channels.Count];
			for (int i = 0; i < channels.Count; i++)
			{
				items[i].bActive = 1;
				items[i].szItemID = channels[i].OpcChannel;
				items[i].hClient = channels[i].GetHashCode();
			}
			IntPtr addResult;
			IntPtr addErrors;
			group.AddItems(items.Length, items, out addResult, out addErrors);
			addResult = IntPtr.Zero;
			addErrors = IntPtr.Zero;

			IConnectionPointContainer cpc = (IConnectionPointContainer)group_obj;
			IConnectionPoint cp;
			Guid dataCallbackGuid = typeof(IOPCDataCallback).GUID;
			cpc.FindConnectionPoint(ref dataCallbackGuid, out cp);

			callback = new OPCDataCallback(channels);
			cp.Advise(callback, out callbackCookie);
		}

		~ConnectionGroup()
		{
			try
			{
				IConnectionPointContainer cpc = (IConnectionPointContainer)group;
				IConnectionPoint cp;
				Guid dataCallbackGuid = typeof(IOPCDataCallback).GUID;
				cpc.FindConnectionPoint(ref dataCallbackGuid, out cp);

				cp.Unadvise(callbackCookie);
			}
			catch (System.Runtime.InteropServices.COMException)
			{
			}

			group = null;
			callback = null;
			server = null;
		}
	}
}
