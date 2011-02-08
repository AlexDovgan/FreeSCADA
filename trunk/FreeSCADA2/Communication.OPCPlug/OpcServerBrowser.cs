namespace OPC
{

    
    using OpcRcw.Comn;
    using System;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Permissions;

    [ComImport, Guid("0002E000-0000-0000-C000-000000000046"), ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IEnumGUID
    {
        int Next([In] int celt, [In] ref Guid rgelt, out int pceltFetched);
        int Skip([In] int celt);
        int Reset();
        int Clone([MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);
    }

    [ComImport, ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("13486D50-4821-11D2-A494-3CB306C10000")]
    internal interface IOPCServerList
    {
        int EnumClassesOfCategories([In] int cImplemented, [In, MarshalAs(UnmanagedType.LPArray)] Guid[] catidImpl, [In] int cRequired, [In] Guid[] catidReq, [MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);
        int GetClassDetails([In] ref Guid clsid, [MarshalAs(UnmanagedType.LPWStr)] out string ppszProgID, [MarshalAs(UnmanagedType.LPWStr)] out string ppszUserType);
        int CLSIDFromProgID([In, MarshalAs(UnmanagedType.LPWStr)] string szProgId, out Guid clsid);
    }

    [ComVisible(true), SuppressUnmanagedCodeSecurity, SecurityPermission(SecurityAction.Assert, Flags=SecurityPermissionFlag.UnmanagedCode), ReflectionPermission(SecurityAction.Assert, Unrestricted=true)]
    public class OpcServerBrowser
    {
        private Guid CLSID_OPCEnum;
        private IOPCServerList ifSrvList;

        public OpcServerBrowser()
        {
            this.CLSID_OPCEnum = new Guid("{13486D51-4821-11D2-A494-3CB306C10000}");
            Type typeFromCLSID = Type.GetTypeFromCLSID(this.CLSID_OPCEnum);
            this.ifSrvList = (IOPCServerList) Activator.CreateInstance(typeFromCLSID);
        }

        
        public OpcServerBrowser(string ComputerName)
        {
            this.CLSID_OPCEnum = new Guid("{13486D51-4821-11D2-A494-3CB306C10000}");
            if ((ComputerName != null) && (ComputerName != ""))
            {
                Type typeFromCLSID = Type.GetTypeFromCLSID(this.CLSID_OPCEnum, ComputerName);
                this.ifSrvList = (IOPCServerList)Activator.CreateInstance(typeFromCLSID);
            }
            else
            {
                Type typeFromCLSID = Type.GetTypeFromCLSID(this.CLSID_OPCEnum);
                this.ifSrvList = (IOPCServerList) Activator.CreateInstance(typeFromCLSID);
            }
        }

        public void CLSIDFromProgID(string progId, out Guid clsid)
        {
            this.ifSrvList.CLSIDFromProgID(progId, out clsid);
            
        }

        internal void EnumClassesOfCategories(int catListLength, Guid[] catList, int reqListLenght, Guid[] reqList, out object enumtemp)
        {
            this.ifSrvList.EnumClassesOfCategories(catListLength, catList, reqListLenght, reqList, out enumtemp);
            
        }

        ~OpcServerBrowser()
        {
            if (this.ifSrvList != null)
            {
                Marshal.ReleaseComObject(this.ifSrvList);
                this.ifSrvList = null;
            }
        }

        public void GetClassDetails(ref Guid clsid, out string progID, out string userType)
        {
            this.ifSrvList.GetClassDetails(ref clsid, out progID, out userType);
            
        }

        public void GetServerList(out string[] Servers)
        {
            Guid[] catList = new Guid[] { new Guid("{63D5F432-CFE4-11d1-B2C8-0060083BA1FB}"), new Guid("{63D5F430-CFE4-11d1-B2C8-0060083BA1FB}"), new Guid("{CC603642-66D7-48f1-B69A-B625E73652D7}") };
            this.GetServerList(catList, out Servers);
        }

        public void GetServerList(Guid[] catList, out string[] Servers)
        {
            Guid[] guidArray;
            this.GetServerList(catList, out Servers, out guidArray);
            guidArray = null;
        }

        public void GetServerList(out string[] Servers, out Guid[] ClsIDs)
        {
            Guid[] catList = new Guid[] { new Guid("{63D5F432-CFE4-11d1-B2C8-0060083BA1FB}"), new Guid("{63D5F430-CFE4-11d1-B2C8-0060083BA1FB}"), new Guid("{CC603642-66D7-48f1-B69A-B625E73652D7}") };
            this.GetServerList(catList, out Servers, out ClsIDs);
        }

        public void GetServerList(Guid[] catList, out string[] Servers, out Guid[] ClsIDs)
        {
            object obj2;
            Servers = null;
            ClsIDs = null;
            if (this.ifSrvList == null)
            {
                throw new Exception(String.Format("GetServerList failed with error code {hr}",-2147467262));
            }
            this.ifSrvList.EnumClassesOfCategories(catList.Length, catList, 0, null, out obj2);
            if (obj2 != null)
            {
                IEnumGUID o = (IEnumGUID) obj2;
                obj2 = null;
                o.Reset();
                int index = 0;
                Guid rgelt = this.CLSID_OPCEnum;
                Guid[] guidArray = new Guid[50];
                
                int pceltFetched = 0;
                
                do
                {
                    o.Next(1, ref rgelt, out pceltFetched);
                    if (pceltFetched > 0)
                    {
                        guidArray[index] = rgelt;
                        index++;
                    }
                }
                while ((pceltFetched > 0) && (index < guidArray.Length));

                Marshal.ReleaseComObject(o);
                o = null;
                string[] strArray = new string[index];
                Guid[] guidArray2 = new Guid[index];
                int num3 = 0;
                for (int i = 0; i < index; i++)
                {
                    string ppszProgID = null;
                    string ppszUserType = null;
                    try
                    {
                        this.ifSrvList.GetClassDetails(ref guidArray[i], out ppszProgID, out ppszUserType);
                        strArray[num3] = ppszProgID;
                        guidArray2[num3] = guidArray[i];
                        num3++;
                    }
                    catch
                    {
                    }
                }
                if (num3 > 0)
                {
                    Servers = new string[num3];
                    ClsIDs = new Guid[num3];
                    for (int j = 0; j < num3; j++)
                    {
                        Servers[j] = strArray[j];
                        ClsIDs[j] = guidArray2[j];
                    }
                }
                strArray = null;
                guidArray2 = null;
            }
        }

        public void GetServerList(bool V2, bool V3, out string[] Servers)
        {
            Guid[] catList = null;
            if (!V2 && !V3)
            {
                Servers = new string[0];
            }
            else
            {
                if (V2 && V3)
                {
                    catList = new Guid[] { new Guid("{63D5F432-CFE4-11d1-B2C8-0060083BA1FB}"), new Guid("{CC603642-66D7-48f1-B69A-B625E73652D7}") };
                }
                else if (V2)
                {
                    catList = new Guid[] { new Guid("{63D5F432-CFE4-11d1-B2C8-0060083BA1FB}") };
                }
                else if (V3)
                {
                    catList = new Guid[] { new Guid("{CC603642-66D7-48f1-B69A-B625E73652D7}") };
                }
                this.GetServerList(catList, out Servers);
            }
        }
    }
}

