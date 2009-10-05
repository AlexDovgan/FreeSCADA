using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

		enum ServerWriteCapabilities
		{
			Unknown,
			None,
			Async,
			Sync
		};
		ServerWriteCapabilities serverWriteCapabilities = ServerWriteCapabilities.Unknown;


		const int OPC_READABLE = 1;
		const int OPC_WRITEABLE = 2;

		public ConnectionGroup(string opcServer, string opcHost, List<OPCBaseChannel> channels)
		{
			Type t;
			if(opcHost.ToLowerInvariant() == "localhost")
				t = Type.GetTypeFromProgID(opcServer);
			else
				t = Type.GetTypeFromProgID(opcServer, opcHost);

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
			for (int i = 0; i < channels.Count; i++)
			{
				IntPtr pos = new IntPtr(addResult.ToInt32() + Marshal.SizeOf(typeof(OPCITEMRESULT)) * i);
				OPCITEMRESULT res = (OPCITEMRESULT)Marshal.PtrToStructure(pos, typeof(OPCITEMRESULT));

				bool readOnly = (res.dwAccessRights & OPC_WRITEABLE) != OPC_WRITEABLE;
				channels[i].Connect(this, res.hServer, readOnly);
			}
			Marshal.FreeCoTaskMem(addResult);
			Marshal.FreeCoTaskMem(addErrors);            
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

		public bool WriteChannel(int channelHandle, object value)
		{
			IOPCAsyncIO2 asyncIO = null;
			IOPCSyncIO syncIO = null;

			lock (this)
			{
				if (serverWriteCapabilities == ServerWriteCapabilities.Unknown)
				{
					try
					{
						asyncIO = (IOPCAsyncIO2)group;
						if (asyncIO != null)
							serverWriteCapabilities = ServerWriteCapabilities.Async;
					}
					catch (System.Runtime.InteropServices.COMException)
					{
					}

					if (asyncIO == null)
					{
						try
						{
							syncIO = (IOPCSyncIO)group;
							if (syncIO != null)
								serverWriteCapabilities = ServerWriteCapabilities.Sync;
						}
						catch (System.Runtime.InteropServices.COMException)
						{
						}
					}

					if (asyncIO == null && syncIO == null)
						serverWriteCapabilities = ServerWriteCapabilities.None;
				}

				try
				{
					switch(serverWriteCapabilities)
					{
						case ServerWriteCapabilities.Async:
							syncIO = (IOPCSyncIO)group;
							break;
						case ServerWriteCapabilities.Sync:
							syncIO = (IOPCSyncIO)group;
							break;
						default:
							return false;
					}
				}
				catch (System.Runtime.InteropServices.COMException)
				{
				}

				if (asyncIO == null && syncIO == null)
					return false;

				if (asyncIO != null)
				{
					int cancelID;
					IntPtr ppErrors;
					asyncIO.Write(1, new int[] { channelHandle }, new object[] { value }, 0, out cancelID, out ppErrors);
					Marshal.FreeCoTaskMem(ppErrors);
				}
				else if (syncIO != null)
				{
					IntPtr ppErrors;
					syncIO.Write(1, new int[] { channelHandle }, new object[] { value }, out ppErrors);
					Marshal.FreeCoTaskMem(ppErrors);
				}

				return true;
			}
		}
	}
}
