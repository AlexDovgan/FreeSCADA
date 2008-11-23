﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using Modbus.Device;
using Modbus.Data;

namespace FreeSCADA.Communication.MODBUSPlug
{
    public class ModbusStation
    {
        private string name;
        private string ipAddress;
        private int tcpPort;
        private Plugin plugin;
        private List<ModbusChannelImp> channels = new List<ModbusChannelImp>();
        private List<ModbusBuffer> buffers = new List<ModbusBuffer>();
        Thread channelUpdaterThread;

        public ModbusStation(string name, Plugin plugin, string ipAddress, int tcpPort)
        {
            this.name = name;
            this.plugin = plugin;
            this.ipAddress = ipAddress;
            this.tcpPort = tcpPort;
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public string IPAddress
        {
            get
            {
                return ipAddress;
            }
            set
            {
                ipAddress = value;
            }
        }
        public int TCPPort
        {
            get
            {
                return tcpPort;
            }
            set
            {
                tcpPort = value;
            }
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
                            if (ch.ModbusDataType == ModbusDataType.Input || ch.ModbusDataType == ModbusDataType.Coil)
                                mult = 8;
                            else
                                mult = 1;
                            if (((ch.ModbusDataAddress - buf.lastAddress) < 4 * mult) && (buf.numInputs < 110 * mult))   // Optimization - "holes" in address space less than 4 bytes
                                                                                                       // will be included in one read until max frame 2*110+4 is reached
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
                ModbusStation self = (ModbusStation)obj;
                for (; ; )
                {
                    try
                    {
                        using (TcpClient client = new TcpClient(self.ipAddress, self.tcpPort))
                        {
                            ModbusIpMaster master = ModbusIpMaster.CreateIp(client);
                            for (; ; )
                            {
                                foreach (ModbusBuffer buf in self.buffers)
                                {
                                    ushort startAddress = buf.startAddress;
                                    ushort numInputs = buf.numInputs;
                                    switch (buf.ModbusDataType)
                                    {
                                        case ModbusDataType.InputRegister:
                                            ushort[] registers = master.ReadInputRegisters(startAddress, numInputs);
                                            DateTime dt = DateTime.Now;
                                            foreach (ModbusChannelImp ch in buf.channels)
                                            {
                                                if (ch.Value.GetType() == typeof(int))
                                                {
                                                    ch.DoUpdate((int)registers[ch.ModbusDataAddress - buf.startAddress], dt, 192);
                                                }
                                                if (ch.Value.GetType() == typeof(uint))
                                                {
                                                    ch.DoUpdate((uint)registers[ch.ModbusDataAddress - buf.startAddress], dt, 192);
                                                }
                                                if (ch.Value.GetType() == typeof(float))
                                                {
                                                    ch.DoUpdate((float)registers[ch.ModbusDataAddress - buf.startAddress], dt, 192);
                                                }
                                            }
                                            break;
                                        case ModbusDataType.Coil:
                                            bool[] inputs = master.ReadCoils(startAddress, numInputs);
                                            dt = DateTime.Now;
                                            foreach (ModbusChannelImp ch in buf.channels)
                                            {
                                                if (ch.Value.GetType() == typeof(int))
                                                {
                                                    int val = inputs[ch.ModbusDataAddress - buf.startAddress] ? 1 : 0;
                                                    ch.DoUpdate(val, dt, 192);
                                                }
                                                if (ch.Value.GetType() == typeof(uint))
                                                {
                                                    uint val = (uint)(inputs[ch.ModbusDataAddress - buf.startAddress] ? 1 : 0);
                                                    ch.DoUpdate(val, dt, 192);
                                                }
                                                if (ch.Value.GetType() == typeof(float))
                                                {
                                                    float val = inputs[ch.ModbusDataAddress - buf.startAddress] ? 1 : 0;
                                                    ch.DoUpdate(val, dt, 192);
                                                }
                                            }
                                            break;
                                        case ModbusDataType.Input:
                                            inputs = master.ReadInputs(startAddress, numInputs);
                                            dt = DateTime.Now;
                                            foreach (ModbusChannelImp ch in buf.channels)
                                            {
                                                if (ch.Value.GetType() == typeof(int))
                                                {
                                                    int val = inputs[ch.ModbusDataAddress - buf.startAddress] ? 1 : 0;
                                                    ch.DoUpdate(val, dt, 192);
                                                }
                                                if (ch.Value.GetType() == typeof(uint))
                                                {
                                                    uint val = (uint)(inputs[ch.ModbusDataAddress - buf.startAddress] ? 1 : 0);
                                                    ch.DoUpdate(val, dt, 192);
                                                }
                                                if (ch.Value.GetType() == typeof(float))
                                                {
                                                    float val = inputs[ch.ModbusDataAddress - buf.startAddress] ? 1 : 0;
                                                    ch.DoUpdate(val, dt, 192);
                                                }
                                            }
                                            break;
                                        case ModbusDataType.HoldingRegister:
                                            registers = master.ReadHoldingRegisters(startAddress, numInputs);
                                            dt = DateTime.Now;
                                            foreach (ModbusChannelImp ch in buf.channels)
                                            {
                                                if (ch.Value.GetType() == typeof(int))
                                                {
                                                    ch.DoUpdate((int)registers[ch.ModbusDataAddress - buf.startAddress], dt, 192);
                                                }
                                                if (ch.Value.GetType() == typeof(uint))
                                                {
                                                    ch.DoUpdate((uint)registers[ch.ModbusDataAddress - buf.startAddress], dt, 192);
                                                }
                                                if (ch.Value.GetType() == typeof(float))
                                                {
                                                    ch.DoUpdate((float)registers[ch.ModbusDataAddress - buf.startAddress], dt, 192);
                                                }
                                            }
                                            break;
                                    }
                                }
                                Thread.Sleep(100);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (e is ThreadAbortException)
                            throw e;
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
            public ModbusDataType ModbusDataType;
            public ushort startAddress;
            public ushort lastAddress;
            public ushort numInputs;
            public List<ModbusChannelImp> channels = new List<ModbusChannelImp>();
        }
    }
}
