using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using FreeSCADA.Interfaces;
using Modbus.Data;
using Modbus.Device;
using System.IO.Ports;

namespace FreeSCADA.Communication.MODBUSPlug
{
    public class ModbusSerialClientStation : IModbusStation
    {
        private string name;
        private SerialPort serport;
        private Plugin plugin;
        private int cycleTimeout;
        private int retryTimeout;
        private int retryCount;
        private int failedCount;
        private List<ModbusChannelImp> channels = new List<ModbusChannelImp>();
        private List<ModbusBuffer> buffers = new List<ModbusBuffer>();
        Thread channelUpdaterThread;
        ModbusSerialType serialType = ModbusSerialType.RTU;
        private string comPort;
        private int baudRate = 9600;
        private int dataBits = 8;
        private StopBits stopBits = StopBits.One;
        private Parity parity = Parity.None;
        private Handshake handshake = Handshake.None;

        public ModbusSerialClientStation(string name, Plugin plugin, string comPort, int cycleTimeout, int retryTimeout, int retryCount, int failedCount)
        {
            this.name = name;
            this.plugin = plugin;
            this.comPort = comPort;
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
        public string ComPort
        {
            get { return comPort; }
            set { comPort = value; }
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

        public ModbusSerialType SerialType
        {
            get { return serialType; }
            set { serialType = value; }
        }

        public int BaudRate {
            get { return baudRate; }
            set { baudRate = value; }
        }
        public int DataBits {
            get { return dataBits; }
            set { dataBits = value; }
        }
        public StopBits StopBits {
            get { return stopBits; }
            set { stopBits = value; }
        }
        public Parity Parity {
            get { return parity; }
            set { parity = value; }
        }
        public Handshake Handshake
        {
            get { return handshake; }
            set { handshake = value; }
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

            if (channels.Count > 0)
            {
                //// Data Space analyse
                channels.Sort(channels[0].Compare);

                foreach (ModbusChannelImp ch in channels)
                {
                    bool found = false;
                    foreach (ModbusBuffer buf in buffers)
                    {
                        if (ch.ModbusDataType == buf.ModbusDataType)
                        {
                            int mult;
                            if (ch.ModbusDataType == ModbusDataTypeEx.Input || ch.ModbusDataType == ModbusDataTypeEx.Coil)
                                mult = 16;
                            else
                                mult = 1;
                            if (((ch.ModbusDataAddress - buf.lastAddress) < 4 * mult) && (buf.numInputs < 110 * mult))   // Optimization - "holes" in address space less than 4 words
                            // will be included in one read until max frame 2*110+8 bytes is reached
                            {
                                buf.lastAddress = ch.ModbusDataAddress;
                                buf.numInputs = (ushort)(buf.lastAddress - buf.startAddress + 1);
                                buf.channels.Add(ch);
                                found = true;
                            }
                        }
                    }
                    if (!found)
                    {
                        // Set up a new buffer
                        ModbusBuffer buf = new ModbusBuffer();
                        buf.numInputs = 1;
                        buf.startAddress = buf.lastAddress = ch.ModbusDataAddress;
                        buf.ModbusDataType = ch.ModbusDataType;
                        buf.channels.Add(ch);
                        buffers.Add(buf);
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
                ModbusSerialClientStation self = (ModbusSerialClientStation)obj;
                for (; ; )
                {
                    try
                    {
                        /*using (TcpClient client = new TcpClient(self.comPort, self.tcpPort))
                        {
                            ModbusIpMaster master = ModbusIpMaster.CreateIp(client);
                            master.Transport.Retries = self.retryCount;
                            master.Transport.WaitToRetryMilliseconds = self.retryTimeout;

                            for (; ; )
                            {
                                foreach (ModbusBuffer buf in self.buffers)
                                {
                                    ushort startAddress = buf.startAddress;
                                    ushort numInputs = buf.numInputs;
                                    switch (buf.ModbusDataType)
                                    {
                                        case ModbusDataTypeEx.InputRegister:
                                            ushort[] registers = master.ReadInputRegisters(startAddress, numInputs);
                                            DateTime dt = DateTime.Now;
                                            foreach (ModbusChannelImp ch in buf.channels)
                                            {
                                                if (ch.ModbusInternalType == ModbusFs2InternalType.Int32)
                                                {
                                                    ch.DoUpdate((int)registers[ch.ModbusDataAddress - buf.startAddress], dt, ChannelStatusFlags.Good);
                                                }
                                                if (ch.ModbusInternalType == ModbusFs2InternalType.UInt32)
                                                {
                                                    ch.DoUpdate((uint)registers[ch.ModbusDataAddress - buf.startAddress], dt, ChannelStatusFlags.Good);
                                                }
                                                if (ch.ModbusInternalType == ModbusFs2InternalType.Float)
                                                {
                                                    ch.DoUpdate((float)registers[ch.ModbusDataAddress - buf.startAddress], dt, ChannelStatusFlags.Good);
                                                }
                                            }
                                            break;
                                        case ModbusDataTypeEx.Coil:
                                            bool[] inputs = master.ReadCoils(startAddress, numInputs);
                                            dt = DateTime.Now;
                                            foreach (ModbusChannelImp ch in buf.channels)
                                            {
                                                if (ch.ModbusInternalType == ModbusFs2InternalType.Int32)
                                                {
                                                    int val = inputs[ch.ModbusDataAddress - buf.startAddress] ? 1 : 0;
                                                    ch.DoUpdate(val, dt, ChannelStatusFlags.Good);
                                                }
                                                if (ch.ModbusInternalType == ModbusFs2InternalType.UInt32)
                                                {
                                                    uint val = (uint)(inputs[ch.ModbusDataAddress - buf.startAddress] ? 1 : 0);
                                                    ch.DoUpdate(val, dt, ChannelStatusFlags.Good);
                                                }
                                                if (ch.ModbusInternalType == ModbusFs2InternalType.Float)
                                                {
                                                    float val = inputs[ch.ModbusDataAddress - buf.startAddress] ? 1 : 0;
                                                    ch.DoUpdate(val, dt, ChannelStatusFlags.Good);
                                                }
                                            }
                                            break;
                                        case ModbusDataTypeEx.Input:
                                            inputs = master.ReadInputs(startAddress, numInputs);
                                            dt = DateTime.Now;
                                            foreach (ModbusChannelImp ch in buf.channels)
                                            {
                                                if (ch.ModbusInternalType == ModbusFs2InternalType.Int32)
                                                {
                                                    int val = inputs[ch.ModbusDataAddress - buf.startAddress] ? 1 : 0;
                                                    ch.DoUpdate(val, dt, ChannelStatusFlags.Good);
                                                }
                                                if (ch.ModbusInternalType == ModbusFs2InternalType.UInt32)
                                                {
                                                    uint val = (uint)(inputs[ch.ModbusDataAddress - buf.startAddress] ? 1 : 0);
                                                    ch.DoUpdate(val, dt, ChannelStatusFlags.Good);
                                                }
                                                if (ch.ModbusInternalType == ModbusFs2InternalType.Float)
                                                {
                                                    float val = inputs[ch.ModbusDataAddress - buf.startAddress] ? 1 : 0;
                                                    ch.DoUpdate(val, dt, ChannelStatusFlags.Good);
                                                }
                                            }
                                            break;
                                        case ModbusDataTypeEx.HoldingRegister:
                                            registers = master.ReadHoldingRegisters(startAddress, numInputs);
                                            dt = DateTime.Now;
                                            foreach (ModbusChannelImp ch in buf.channels)
                                            {
                                                if (ch.ModbusInternalType == ModbusFs2InternalType.Int32)
                                                {
                                                    ch.DoUpdate((int)registers[ch.ModbusDataAddress - buf.startAddress], dt, ChannelStatusFlags.Good);
                                                }
                                                if (ch.ModbusInternalType == ModbusFs2InternalType.UInt32)
                                                {
                                                    ch.DoUpdate((uint)registers[ch.ModbusDataAddress - buf.startAddress], dt, ChannelStatusFlags.Good);
                                                }
                                                if (ch.ModbusInternalType == ModbusFs2InternalType.Float)
                                                {
                                                    ch.DoUpdate((float)registers[ch.ModbusDataAddress - buf.startAddress], dt, ChannelStatusFlags.Good);
                                                }
                                            }
                                            break;
                                    }
                                }
                                Thread.Sleep(self.cycleTimeout);
                            }
                        }*/
                    }
                    catch (Exception e)
                    {
                        if (e is ThreadAbortException)
                            throw e;
                        // if (e is )   // Communication timeout to a device
                    }
                    Thread.Sleep(1000);
                }
            }
            catch (ThreadAbortException)
            {
            }
        }

        private class ModbusBuffer
        {
            public ModbusDataTypeEx ModbusDataType;
            public ushort startAddress;
            public ushort lastAddress;
            public ushort numInputs;
            public List<ModbusChannelImp> channels = new List<ModbusChannelImp>();
        }
    }
}
