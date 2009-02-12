using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using FreeSCADA.Interfaces;
using Modbus.Data;
using Modbus.Device;
using FreeSCADA.Common;
using System.Text;

namespace FreeSCADA.Communication.MODBUSPlug
{
    public class ModbusTCPClientStation : IModbusStation
    {
        private string name;
        private string ipAddress;
        private int tcpPort;
        private Plugin plugin;
        private int cycleTimeout;
        private int retryTimeout;
        private int retryCount;
        private int failedCount;
        private List<ModbusChannelImp> channels = new List<ModbusChannelImp>();
        private List<ModbusBuffer> buffers = new List<ModbusBuffer>();
        private List<ModbusChannelImp> failures = new List<ModbusChannelImp>();
        private Thread channelUpdaterThread;
        private Queue<ModbusChannelImp> sendQueue = new Queue<ModbusChannelImp>();
        private object sendQueueSyncRoot = new object();
        private ManualResetEvent sendQueueEndWaitEvent = new ManualResetEvent(false);
        private List<ModbusChannelImp> channelsToSend = new List<ModbusChannelImp>();

        public ModbusTCPClientStation(string name, Plugin plugin, string ipAddress, int tcpPort, int cycleTimeout, int retryTimeout, int retryCount, int failedCount)
        {
            this.name = name;
            this.plugin = plugin;
            this.ipAddress = ipAddress;
            this.tcpPort = tcpPort;
            this.cycleTimeout = Math.Max(cycleTimeout, 10);
            this.retryTimeout = Math.Max(retryTimeout, 100);
            this.retryCount = Math.Max(retryCount, 1);
            this.failedCount = Math.Max(failedCount, 1);
            this.LoggingLevel = 0;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string IPAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }
        public int TCPPort
        {
            get { return tcpPort; }
            set { tcpPort = value; }
        }
        public int CycleTimeout
        {
            get { return cycleTimeout; }
            set { cycleTimeout = value; }
        }
        public int RetryTimeout
        {
            get { return retryTimeout; }
            set { retryTimeout = value; }
        }
        public int RetryCount
        {
            get { return retryCount; }
            set { retryCount = value; }
        }

        public int FailedCount
        {
            get { return failedCount; }
            set { failedCount = value; }
        }

        public int LoggingLevel
        {
            get;
            set;
        }

        public void AddChannel(ModbusChannelImp channel)
        {
            channels.Add(channel);
        }

        public void ClearChannels()
        {
            channels.Clear();
        }

        public void SendValueUpdateToModbusLine(ModbusChannelImp ch)
        {
            lock (sendQueueSyncRoot)
            {
                sendQueue.Enqueue(ch);
                sendQueueEndWaitEvent.Set();
            }
        }

        public void Stop()
        {
            //MessageBox.Show("Stopping Modbus station " + name, "Info");
            if (channelUpdaterThread != null)
            {
                channelUpdaterThread.Abort();
                channelUpdaterThread.Join();
                channelUpdaterThread = null;
            }
        }

        public int Start()
        {
            //MessageBox.Show("Starting Modbus station " + name, "Info");
            buffers.Clear();
            failures.Clear();

            if (channels.Count > 0)
            {
                //// Data Space analyse
                channels.Sort(channels[0].Compare);

                foreach (ModbusChannelImp ch in channels)
                {
                    if (ch.ModbusReadWrite != ModbusReadWrite.WriteOnly && ch.ModbusDataType != ModbusDataTypeEx.DeviceFailureInfo)
                    {
                        bool found = false;
                        foreach (ModbusBuffer buf in buffers)
                        {
                            if (ch.SlaveId == buf.slaveId && ch.ModbusDataType == buf.ModbusDataType)
                            {
                                int mult;
                                if (ch.ModbusDataType == ModbusDataTypeEx.Input || ch.ModbusDataType == ModbusDataTypeEx.Coil)
                                    mult = 16;
                                else
                                    mult = 1;
                                if (((ch.ModbusDataAddress - buf.lastAddress) < 4 * mult) && ((buf.lastAddress - buf.startAddress + ch.DeviceDataLen) < 120 * mult))
                                // Optimization - "holes" in address space less than 4 words
                                // will be included in one read until max frame 2*120 bytes is reached
                                {
                                    buf.lastAddress = (ushort)(ch.ModbusDataAddress + ch.DeviceDataLen - 1);
                                    buf.numInputs = (ushort)(buf.lastAddress - buf.startAddress + ch.DeviceDataLen);
                                    buf.channels.Add(ch);
                                    found = true;
                                }
                            }
                        }
                        if (!found)
                        {
                            // Set up a new buffer
                            ModbusBuffer buf = new ModbusBuffer();
                            buf.slaveId = ch.SlaveId;
                            buf.numInputs = 1;
                            buf.startAddress = buf.lastAddress = ch.ModbusDataAddress;
                            buf.ModbusDataType = ch.ModbusDataType;
                            buf.channels.Add(ch);
                            buf.pauseCounter = 0;
                            buffers.Add(buf);
                        }
                    }
                    if (ch.ModbusDataType == ModbusDataTypeEx.DeviceFailureInfo)
                    {
                        failures.Add(ch);
                        ch.Value = true;
                    }
                }
                //// Run Thread
                channelUpdaterThread = new Thread(new ParameterizedThreadStart(ChannelUpdaterThreadProc));
                channelUpdaterThread.Start(this);
                return 0;
            }
            return 1;
        }

        private static void ChannelUpdaterThreadProc(object obj)
        {
            try
            {
                ModbusTCPClientStation self = (ModbusTCPClientStation)obj;
                for (; ; )
                {
                    try
                    {
                        foreach (ModbusBuffer buf in self.buffers)
                        {
                            buf.pauseCounter = 0;
                        }
                        if (self.LoggingLevel >= ModbusLog.logInfos)
                            Env.Current.Logger.LogInfo(string.Format(StringConstants.InfoTCPStarting, self.Name, self.ipAddress, self.tcpPort));
                        using (TcpClient client = new TcpClient(self.ipAddress, self.tcpPort))
                        {
                            if (self.LoggingLevel >= ModbusLog.logInfos)
                                Env.Current.Logger.LogInfo(string.Format(StringConstants.InfoTCPStarted, self.Name, self.ipAddress, self.tcpPort));
                            ModbusIpMaster master = ModbusIpMaster.CreateIp(client);
                            master.Transport.Retries = self.retryCount;
                            master.Transport.WaitToRetryMilliseconds = self.retryTimeout;
                            ushort[] registers;
                            byte[] adr;

                            for (; ; )
                            {
                                foreach (ModbusBuffer buf in self.buffers)
                                {
                                    try
                                    {
                                        if (buf.pauseCounter == 0)
                                        {
                                            ushort startAddress = buf.startAddress;
                                            ushort numInputs = buf.numInputs;
                                            switch (buf.ModbusDataType)
                                            {
                                                case ModbusDataTypeEx.InputRegister:
                                                case ModbusDataTypeEx.HoldingRegister:
                                                    if (buf.ModbusDataType == ModbusDataTypeEx.InputRegister)
                                                        registers = master.ReadInputRegisters(buf.slaveId, startAddress, numInputs);
                                                    else
                                                        registers = master.ReadHoldingRegisters(buf.slaveId, startAddress, numInputs);
                                                    DateTime dt = DateTime.Now;
                                                    int iresult = 0;
                                                    uint uresult = 0;
                                                    double fresult = 0.0;
                                                    foreach (ModbusChannelImp ch in buf.channels)
                                                    {
                                                        switch (ch.DeviceDataType)
                                                        {
                                                            case ModbusDeviceDataType.Int:
                                                               adr = BitConverter.GetBytes(registers[ch.ModbusDataAddress - buf.startAddress]);
                                                               switch (ch.ConversionType)
                                                                {
                                                                    case ModbusConversionType.SwapBytes:
                                                                        byte tmp = adr[0]; adr[0] = adr[1]; adr[1] = tmp;
                                                                        iresult = BitConverter.ToInt16(adr, 0);
                                                                        break;
                                                                    default:
                                                                        iresult = BitConverter.ToInt16(adr, 0);
                                                                        break;
                                                                }
                                                                if (ch.ModbusFs2InternalType == ModbusFs2InternalType.Int32)
                                                                {
                                                                    ch.DoUpdate(iresult, dt, ChannelStatusFlags.Good);
                                                                }
                                                                else if (ch.ModbusFs2InternalType == ModbusFs2InternalType.Double)
                                                                {
                                                                    ch.DoUpdate((double)(ch.K * iresult + ch.D), dt, ChannelStatusFlags.Good);
                                                                }
                                                                else
                                                                    if (self.LoggingLevel >= ModbusLog.logWarnings)
                                                                        Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrConvert,
                                                                            self.Name, ch.Name, ch.DeviceDataType.ToString(), ch.ModbusFs2InternalType.ToString()));
                                                                break;
                                                            case ModbusDeviceDataType.UInt:
                                                                adr = BitConverter.GetBytes(registers[ch.ModbusDataAddress - buf.startAddress]);
                                                                switch (ch.ConversionType)
                                                                {
                                                                    case ModbusConversionType.SwapBytes:
                                                                        byte tmp = adr[0]; adr[0] = adr[1]; adr[1] = tmp;
                                                                        uresult = BitConverter.ToUInt16(adr, 0);
                                                                        break;
                                                                    default:
                                                                        uresult = BitConverter.ToUInt16(adr, 0);
                                                                        break;
                                                                }
                                                                if (ch.ModbusFs2InternalType == ModbusFs2InternalType.UInt32)
                                                                {
                                                                    ch.DoUpdate(uresult, dt, ChannelStatusFlags.Good);
                                                                }
                                                                else if (ch.ModbusFs2InternalType == ModbusFs2InternalType.Double)
                                                                {
                                                                    ch.DoUpdate((double)(ch.K * uresult + ch.D), dt, ChannelStatusFlags.Good);
                                                                }
                                                                else
                                                                    if (self.LoggingLevel >= ModbusLog.logWarnings)
                                                                        Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrConvert,
                                                                            self.Name, ch.Name, ch.DeviceDataType.ToString(), ch.ModbusFs2InternalType.ToString()));
                                                                break;
                                                            case ModbusDeviceDataType.DInt:
                                                                byte[] adr0 = BitConverter.GetBytes(registers[ch.ModbusDataAddress - buf.startAddress]);
                                                                byte[] adr1 = BitConverter.GetBytes(registers[ch.ModbusDataAddress - buf.startAddress + 1]);
                                                                byte[] res = new byte[4];
                                                                res = self.SwapBytes(adr0, adr1, ch.ConversionType);
                                                                iresult = BitConverter.ToInt32(res, 0);
                                                                if (ch.ModbusFs2InternalType == ModbusFs2InternalType.Int32)
                                                                {
                                                                    ch.DoUpdate(iresult, dt, ChannelStatusFlags.Good);
                                                                }
                                                                else if (ch.ModbusFs2InternalType == ModbusFs2InternalType.Double)
                                                                {
                                                                    ch.DoUpdate((double)(ch.K * iresult + ch.D), dt, ChannelStatusFlags.Good);
                                                                }
                                                                else
                                                                    if (self.LoggingLevel >= ModbusLog.logWarnings)
                                                                        Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrConvert,
                                                                            self.Name, ch.Name, ch.DeviceDataType.ToString(), ch.ModbusFs2InternalType.ToString()));
                                                                break;
                                                            case ModbusDeviceDataType.DUInt:

                                                                adr0 = BitConverter.GetBytes(registers[ch.ModbusDataAddress - buf.startAddress]);
                                                                adr1 = BitConverter.GetBytes(registers[ch.ModbusDataAddress - buf.startAddress + 1]);
                                                                res = self.SwapBytes(adr0, adr1, ch.ConversionType);
                                                                uresult = BitConverter.ToUInt32(res, 0);
                                                                if (ch.ModbusFs2InternalType == ModbusFs2InternalType.UInt32)
                                                                {
                                                                    ch.DoUpdate(uresult, dt, ChannelStatusFlags.Good);
                                                                }
                                                                else if (ch.ModbusFs2InternalType == ModbusFs2InternalType.Double)
                                                                {
                                                                    ch.DoUpdate((double)(ch.K * uresult + ch.D), dt, ChannelStatusFlags.Good);
                                                                }
                                                                else
                                                                    if (self.LoggingLevel >= ModbusLog.logWarnings)
                                                                        Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrConvert,
                                                                            self.Name, ch.Name, ch.DeviceDataType.ToString(), ch.ModbusFs2InternalType.ToString()));
                                                                break;
                                                            case ModbusDeviceDataType.Float:

                                                                adr0 = BitConverter.GetBytes(registers[ch.ModbusDataAddress - buf.startAddress]);
                                                                adr1 = BitConverter.GetBytes(registers[ch.ModbusDataAddress - buf.startAddress + 1]);
                                                                res = self.SwapBytes(adr0, adr1, ch.ConversionType);
                                                                fresult = BitConverter.ToSingle(res, 0);
                                                                if (ch.ModbusFs2InternalType == ModbusFs2InternalType.Double)
                                                                {
                                                                    ch.DoUpdate((double)(ch.K * fresult + ch.D), dt, ChannelStatusFlags.Good);
                                                                }
                                                                else
                                                                    if (self.LoggingLevel >= ModbusLog.logWarnings)
                                                                        Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrConvert,
                                                                            self.Name, ch.Name, ch.DeviceDataType.ToString(), ch.ModbusFs2InternalType.ToString()));
                                                                break;
                                                            case ModbusDeviceDataType.Bool:

                                                                bool bit = (registers[ch.ModbusDataAddress - buf.startAddress] & (0x01 << ch.BitIndex)) > 0;
                                                                if (ch.ModbusFs2InternalType == ModbusFs2InternalType.Boolean)
                                                                {
                                                                    ch.DoUpdate(bit, dt, ChannelStatusFlags.Good);
                                                                }
                                                                else
                                                                    if (self.LoggingLevel >= ModbusLog.logWarnings)
                                                                        Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrConvert,
                                                                            self.Name, ch.Name, ch.DeviceDataType.ToString(), ch.ModbusFs2InternalType.ToString()));
                                                                break;
                                                            case ModbusDeviceDataType.String:

                                                                byte[] str = new byte[2 * ch.DeviceDataLen];
                                                                Decoder ascii = (new ASCIIEncoding()).GetDecoder();
                                                                int bytesUsed = 0;
                                                                int charsUsed = 0;
                                                                bool completed = false;

                                                                for (int i = 0; i < ch.DeviceDataLen; i++)
                                                                {
                                                                    Array.Copy(BitConverter.GetBytes(registers[ch.ModbusDataAddress - buf.startAddress + i]), 0, str, 2 * i, 2);
                                                                }
                                                                char[] chars = new char[2 * ch.DeviceDataLen];
                                                                ascii.Convert(str, 0, (int)(2 * ch.DeviceDataLen), chars, 0, 2 * ch.DeviceDataLen, true, out bytesUsed, out charsUsed, out completed);
                                                                string sresult = new String(chars);
                                                                if (ch.ModbusFs2InternalType == ModbusFs2InternalType.String)
                                                                {
                                                                    ch.DoUpdate(sresult, dt, ChannelStatusFlags.Good);
                                                                }
                                                                else
                                                                    if (self.LoggingLevel >= ModbusLog.logWarnings)
                                                                        Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrConvert,
                                                                            self.Name, ch.Name, ch.DeviceDataType.ToString(), ch.ModbusFs2InternalType.ToString()));
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case ModbusDataTypeEx.Coil:
                                                    bool[] inputs = master.ReadCoils(buf.slaveId, startAddress, numInputs);
                                                    dt = DateTime.Now;
                                                    foreach (ModbusChannelImp ch in buf.channels)
                                                    {
                                                        if (ch.ModbusFs2InternalType == ModbusFs2InternalType.Int32)
                                                        {
                                                            int val = inputs[ch.ModbusDataAddress - buf.startAddress] ? 1 : 0;
                                                            ch.DoUpdate(val, dt, ChannelStatusFlags.Good);
                                                        }
                                                        if (ch.ModbusFs2InternalType == ModbusFs2InternalType.UInt32)
                                                        {
                                                            uint val = (uint)(inputs[ch.ModbusDataAddress - buf.startAddress] ? 1 : 0);
                                                            ch.DoUpdate(val, dt, ChannelStatusFlags.Good);
                                                        }
                                                        if (ch.ModbusFs2InternalType == ModbusFs2InternalType.Double)
                                                        {
                                                            double val = inputs[ch.ModbusDataAddress - buf.startAddress] ? 1 : 0;
                                                            ch.DoUpdate(val, dt, ChannelStatusFlags.Good);
                                                        }
                                                    }
                                                    break;
                                                case ModbusDataTypeEx.Input:
                                                    inputs = master.ReadInputs(buf.slaveId, startAddress, numInputs);
                                                    dt = DateTime.Now;
                                                    foreach (ModbusChannelImp ch in buf.channels)
                                                    {
                                                        if (ch.ModbusFs2InternalType == ModbusFs2InternalType.Int32)
                                                        {
                                                            int val = inputs[ch.ModbusDataAddress - buf.startAddress] ? 1 : 0;
                                                            ch.DoUpdate(val, dt, ChannelStatusFlags.Good);
                                                        }
                                                        if (ch.ModbusFs2InternalType == ModbusFs2InternalType.UInt32)
                                                        {
                                                            uint val = (uint)(inputs[ch.ModbusDataAddress - buf.startAddress] ? 1 : 0);
                                                            ch.DoUpdate(val, dt, ChannelStatusFlags.Good);
                                                        }
                                                        if (ch.ModbusFs2InternalType == ModbusFs2InternalType.Double)
                                                        {
                                                            double val = inputs[ch.ModbusDataAddress - buf.startAddress] ? 1 : 0;
                                                            ch.DoUpdate(val, dt, ChannelStatusFlags.Good);
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            buf.pauseCounter--;
                                        }
                                    }
                                    catch (Modbus.SlaveException e)
                                    {
                                        buf.pauseCounter = self.FailedCount;
                                        if (self.LoggingLevel >= ModbusLog.logWarnings)
                                            Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrReceive,
                                                self.Name, buf.slaveId, buf.ModbusDataType.ToString(), buf.startAddress, buf.numInputs, e.Message));
                                    }
                                    // This implementation causes new reading cycle after writing
                                    // anything to MODBUS
                                    // The sending strategy should be also considered and enhanced, but:
                                    // As I think for now ... it does not matter...
                                    self.sendQueueEndWaitEvent.WaitOne(self.cycleTimeout);

                                    // fast action first - copy of the queue content to my own buffer
                                    lock (self.sendQueueSyncRoot)
                                    {
                                        if (self.sendQueue.Count > 0)
                                        {
                                            self.channelsToSend.Clear();
                                            while (self.sendQueue.Count > 0)
                                            {
                                                self.channelsToSend.Add(self.sendQueue.Dequeue());
                                            }
                                        }
                                    }
                                    // ... and the slow action last - writing to MODBUS
                                    // NO optimization, each channel is written into its own MODBUS message
                                    // and waited for an answer
                                    if (self.channelsToSend.Count > 0)
                                        foreach (ModbusChannelImp ch in self.channelsToSend)
                                        {
                                        }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        self.sendQueueEndWaitEvent.Reset();
                        if (self.LoggingLevel >= ModbusLog.logWarnings)
                            Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrException, self.Name, e.Message));
                        if (e is ThreadAbortException)
                            throw e;
                        // if (e is )   // Communication timeout to a device
                    }
                    // safety Sleep()
                    Thread.Sleep(5000);
                }
            }
            catch (ThreadAbortException e)
            {
                if (((ModbusTCPClientStation)obj).LoggingLevel >= ModbusLog.logErrors)
                    Env.Current.Logger.LogError(string.Format(StringConstants.ErrException, ((ModbusTCPClientStation)obj).Name, e.Message));
            }
        }

        byte[] SwapBytes(byte[] adr0, byte[] adr1, ModbusConversionType con)
        {
            byte[] res = new byte[4];
            switch (con)
            {
                case ModbusConversionType.SwapBytes:
                    res[0] = adr1[1];
                    res[1] = adr1[0];
                    res[2] = adr0[1];
                    res[3] = adr0[0];
                    break;
                case ModbusConversionType.SwapWords:
                    res[0] = adr0[0];
                    res[1] = adr0[1];
                    res[2] = adr1[0];
                    res[3] = adr1[1];
                    break;
                case ModbusConversionType.SwapAll:
                    res[0] = adr0[1];
                    res[1] = adr0[0];
                    res[2] = adr1[1];
                    res[3] = adr1[0];
                    break;
                default:
                    res[0] = adr1[0];
                    res[1] = adr1[1];
                    res[2] = adr0[0];
                    res[3] = adr0[1];
                    break;
            }
            return res;
        }

        private class ModbusBuffer
        {
            public ModbusDataTypeEx ModbusDataType;
            public ushort startAddress;
            public ushort lastAddress;
            public ushort numInputs;
            public List<ModbusChannelImp> channels = new List<ModbusChannelImp>();
            public byte slaveId;
            public int pauseCounter;
        }
    }
}
