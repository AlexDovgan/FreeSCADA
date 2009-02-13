using System;
using System.Collections.Generic;
using System.Threading;
using FreeSCADA.Interfaces;
using Modbus.Data;
using Modbus.Device;
using FreeSCADA.Common;
using System.Text;

namespace FreeSCADA.Communication.MODBUSPlug
{
    public abstract class ModbusBaseClientStation
    {
        protected string name;
        protected Plugin plugin;
        protected int cycleTimeout;
        protected int retryTimeout;
        protected int retryCount;
        protected int failedCount;
        protected List<ModbusChannelImp> channels = new List<ModbusChannelImp>();
        protected List<ModbusBuffer> buffers = new List<ModbusBuffer>();
        protected Dictionary<byte, ModbusChannelImp> failures = new Dictionary<byte, ModbusChannelImp>();
        protected Queue<ModbusChannelImp> sendQueue = new Queue<ModbusChannelImp>();
        protected object sendQueueSyncRoot = new object();
        protected ManualResetEvent sendQueueEndWaitEvent = new ManualResetEvent(false);
        protected List<ModbusChannelImp> channelsToSend = new List<ModbusChannelImp>();

        public ModbusBaseClientStation(string name, Plugin plugin, int cycleTimeout, int retryTimeout, int retryCount, int failedCount)
        {
            this.name = name;
            this.plugin = plugin;
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
                        if (failures.ContainsKey(ch.SlaveId))
                        {
                            // failure signal already defined, parameterization error
                            if (LoggingLevel >= ModbusLog.logErrors)
                                Env.Current.Logger.LogError(string.Format(StringConstants.ErrFailureTwice, this.Name, failures[ch.SlaveId].Name, ch.Name));
                        }
                        else
                        {
                            failures.Add(ch.SlaveId, ch);
                            ch.Value = true;
                        }
                    }
                }
                return 0;
            }
            return 1;
        }

        byte[] SwapBytesIn(byte[] adr0, byte[] adr1, ModbusConversionType con)
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

        void SwapBytesOut(byte[] inp, out byte[] adr0, out byte[] adr1, ModbusConversionType con)
        {
            adr0 = new byte[2];
            adr1 = new byte[2];
            switch (con)
            {
                case ModbusConversionType.SwapBytes:
                    adr1[1] = inp[0];
                    adr1[0] = inp[1];
                    adr0[1] = inp[2];
                    adr0[0] = inp[3];
                    break;
                case ModbusConversionType.SwapWords:
                    adr0[0] = inp[0];
                    adr0[1] = inp[1];
                    adr1[0] = inp[2];
                    adr1[1] = inp[3];
                    break;
                case ModbusConversionType.SwapAll:
                    adr0[1] = inp[0];
                    adr0[0] = inp[1];
                    adr1[1] = inp[2];
                    adr1[0] = inp[3];
                    break;
                default:
                    adr1[0] = inp[0];
                    adr1[1] = inp[1];
                    adr0[0] = inp[2];
                    adr0[1] = inp[3];
                    break;
            }
        }

        protected void ReadBuffer(ModbusBaseClientStation self, IModbusMaster master, ModbusBuffer buf)
        {
            try
            {
                ushort[] registers;
                byte[] adr;
                bool[] inputs;

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
                                        res = self.SwapBytesIn(adr0, adr1, ch.ConversionType);
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
                                        res = self.SwapBytesIn(adr0, adr1, ch.ConversionType);
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
                                        res = self.SwapBytesIn(adr0, adr1, ch.ConversionType);
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
                                            if (ch.ConversionType == ModbusConversionType.SwapBytes)
                                            {
                                                byte[] word = BitConverter.GetBytes(registers[ch.ModbusDataAddress - buf.startAddress + i]);
                                                str[2 * i] = word[1];
                                                str[2 * i + 1] = word[0];
                                            }
                                            else
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
                        case ModbusDataTypeEx.Input:
                            if (buf.ModbusDataType == ModbusDataTypeEx.Coil)
                                inputs = master.ReadCoils(buf.slaveId, startAddress, numInputs);
                            else
                                inputs = master.ReadInputs(buf.slaveId, startAddress, numInputs);
                            dt = DateTime.Now;
                            foreach (ModbusChannelImp ch in buf.channels)
                            {
                                if (ch.ModbusFs2InternalType == ModbusFs2InternalType.UInt32)
                                {
                                    uint val = (uint)(inputs[ch.ModbusDataAddress - buf.startAddress] ? 1 : 0);
                                    ch.DoUpdate(val, dt, ChannelStatusFlags.Good);
                                }
                                else if (ch.ModbusFs2InternalType == ModbusFs2InternalType.Boolean)
                                {
                                    bool val = inputs[ch.ModbusDataAddress - buf.startAddress];
                                    ch.DoUpdate(val, dt, ChannelStatusFlags.Good);
                                }
                                else
                                    if (self.LoggingLevel >= ModbusLog.logWarnings)
                                        Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrConvert,
                                            self.Name, ch.Name, ch.DeviceDataType.ToString(), ch.ModbusFs2InternalType.ToString()));
                            }
                            break;
                    }   // Case
                    if (self.failures.ContainsKey(buf.slaveId))
                    {
                        // failure signal defined
                        self.failures[buf.slaveId].Value = false;
                    }
                }   // If
                else
                {
                    buf.pauseCounter--;
                }
            }   // Try
            catch (Modbus.SlaveException e)
            {
                buf.pauseCounter = self.FailedCount;
                if (self.failures.ContainsKey(buf.slaveId))
                {
                    // failure signal defined
                    self.failures[buf.slaveId].Value = true;
                }
                if (self.LoggingLevel >= ModbusLog.logWarnings)
                    Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrReceive,
                        self.Name, buf.slaveId, buf.ModbusDataType.ToString(), buf.startAddress, buf.numInputs, e.Message));
            }
            catch (TimeoutException e)
            {
                buf.pauseCounter = self.FailedCount;
                if (self.failures.ContainsKey(buf.slaveId))
                {
                    // failure signal defined
                    self.failures[buf.slaveId].Value = true;
                }
                if (self.LoggingLevel >= ModbusLog.logWarnings)
                    Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrReceive,
                        self.Name, buf.slaveId, buf.ModbusDataType.ToString(), buf.startAddress, buf.numInputs, e.Message));
            }
        }

        protected void WriteChannel(ModbusBaseClientStation self, IModbusMaster master, ModbusChannelImp ch)
        {
            try
            {
                ushort[] registers;
                byte[] adr = new byte[4];
                byte[] adr0;
                byte[] adr1;
                bool conv_ok = true;

                ushort startAddress = ch.ModbusDataAddress;
                ushort numInputs = ch.DeviceDataLen;

                switch (ch.ModbusDataType)
                {
                    case ModbusDataTypeEx.HoldingRegister:
                        switch (ch.DeviceDataType)
                        {
                            case ModbusDeviceDataType.Int:
                            case ModbusDeviceDataType.UInt:
                                if (ch.Type == typeof(int))     // ch.ModbusFs2InternalType == ModbusFs2InternalType.Int32)
                                {
                                    int v = (int)ch.Value;
                                    adr = BitConverter.GetBytes(v);
                                }
                                if (ch.Type == typeof(uint))     // ch.ModbusFs2InternalType == ModbusFs2InternalType.Int32)
                                {
                                    uint v = (uint)ch.Value;
                                    adr = BitConverter.GetBytes(v);
                                }
                                else if (ch.Type == typeof(double))    // ch.ModbusFs2InternalType == ModbusFs2InternalType.Double)
                                {
                                    double d = (double)ch.Value;
                                    d = (d - ch.D) / ch.K;
                                    if (ch.DeviceDataType == ModbusDeviceDataType.Int)
                                    {
                                        short s = (short)d;
                                        adr = BitConverter.GetBytes(s);
                                    }
                                    else
                                    {
                                        ushort s = (ushort)d;
                                        adr = BitConverter.GetBytes(s);
                                    }
                                }
                                else
                                {
                                    if (self.LoggingLevel >= ModbusLog.logWarnings)
                                        Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrConvert,
                                        self.Name, ch.Name, ch.ModbusFs2InternalType.ToString(), ch.DeviceDataType.ToString()));
                                    conv_ok = false;
                                }
                                if (conv_ok)
                                {
                                    if (ch.ConversionType == ModbusConversionType.SwapBytes)
                                    {
                                        byte tmp = adr[0]; adr[0] = adr[1]; adr[1] = tmp;
                                    }
                                    master.WriteSingleRegister(ch.SlaveId, ch.ModbusDataAddress, BitConverter.ToUInt16(adr, 0));
                                }
                                break;
                            case ModbusDeviceDataType.DInt:
                            case ModbusDeviceDataType.DUInt:
                            case ModbusDeviceDataType.Float:
                                if (ch.Type == typeof(int))     // ch.ModbusFs2InternalType == ModbusFs2InternalType.Int32)
                                {
                                    int v = (int)ch.Value;
                                    adr = BitConverter.GetBytes(v);
                                }
                                if (ch.Type == typeof(uint))     // ch.ModbusFs2InternalType == ModbusFs2InternalType.Int32)
                                {
                                    uint v = (uint)ch.Value;
                                    adr = BitConverter.GetBytes(v);
                                }
                                else if (ch.Type == typeof(double))    // ch.ModbusFs2InternalType == ModbusFs2InternalType.Double)
                                {
                                    double d = (double)ch.Value;
                                    d = (d - ch.D) / ch.K;
                                    if (ch.DeviceDataType == ModbusDeviceDataType.DInt)
                                    {
                                        int s = (int)d;
                                        adr = BitConverter.GetBytes(s);
                                    }
                                    else if (ch.DeviceDataType == ModbusDeviceDataType.DUInt)
                                    {
                                        uint s = (uint)d;
                                        adr = BitConverter.GetBytes(s);
                                    }
                                    else
                                    {
                                        //float
                                        float s = (float)d;
                                        adr = BitConverter.GetBytes(s);
                                    }
                                }
                                else
                                {
                                    if (self.LoggingLevel >= ModbusLog.logWarnings)
                                        Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrConvert,
                                        self.Name, ch.Name, ch.ModbusFs2InternalType.ToString(), ch.DeviceDataType.ToString()));
                                    conv_ok = false;
                                }
                                if (conv_ok)
                                {
                                    SwapBytesOut(adr, out adr0, out adr1, ch.ConversionType);
                                    registers = new ushort[] { BitConverter.ToUInt16(adr0, 0), BitConverter.ToUInt16(adr1, 0) };
                                    master.WriteMultipleRegisters(ch.SlaveId, ch.ModbusDataAddress, registers);
                                }
                                break;
                            case ModbusDeviceDataType.String:
                            case ModbusDeviceDataType.Bool:
                                if (self.LoggingLevel >= ModbusLog.logWarnings)
                                    Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrConvertImpl,
                                        self.Name, ch.Name, ch.ModbusFs2InternalType.ToString(), ch.DeviceDataType.ToString()));
                                break;
                        }
                        break;
                    case ModbusDataTypeEx.Coil:
                        if (ch.Type == typeof(bool))
                        {
                            master.WriteSingleCoil(ch.SlaveId, ch.ModbusDataAddress, (bool)ch.Value);
                        }
                        else
                        {
                            if (self.LoggingLevel >= ModbusLog.logWarnings)
                                Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrConvert,
                                        self.Name, ch.Name, ch.ModbusFs2InternalType.ToString(), ch.DeviceDataType.ToString()));
                        }
                        break;
                }
            }
            catch (Modbus.SlaveException e)
            {
                if (self.LoggingLevel >= ModbusLog.logWarnings)
                    Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrException,
                        self.Name, e.Message));
            }
            catch (OverflowException e)
            {
                if (self.LoggingLevel >= ModbusLog.logWarnings)
                    Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrException,
                        self.Name, e.Message));
            }
            catch (TimeoutException e)
            {
                if (self.LoggingLevel >= ModbusLog.logWarnings)
                    Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrException,
                        self.Name, e.Message));
            }
            catch (InvalidCastException e)
            {
                if (self.LoggingLevel >= ModbusLog.logWarnings)
                    Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrException,
                        self.Name, e.Message));
            }
        }

        protected class ModbusBuffer
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
