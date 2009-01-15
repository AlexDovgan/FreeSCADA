using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using FreeSCADA.Interfaces;
using Modbus.Data;
using Modbus.Device;

namespace FreeSCADA.Communication.MODBUSPlug
{
    public class ModbusSerialClientStation : IModbusStation
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
        Thread channelUpdaterThread;

        public ModbusSerialClientStation(string name, Plugin plugin, string ipAddress, int tcpPort, int cycleTimeout, int retryTimeout, int retryCount, int failedCount)
        {
            this.name = name;
            this.plugin = plugin;
            this.ipAddress = ipAddress;
            this.tcpPort = tcpPort;
            this.cycleTimeout = Math.Max(cycleTimeout, 10);
            this.retryTimeout = Math.Max(retryTimeout, 100);
            this.retryCount = Math.Max(retryCount, 1);
            this.failedCount = Math.Max(failedCount, 1);
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
        public int SerialPort
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
                        using (TcpClient client = new TcpClient(self.ipAddress, self.tcpPort))
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
                        }
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
