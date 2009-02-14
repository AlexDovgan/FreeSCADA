using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using FreeSCADA.Interfaces;
using Modbus.Data;
using Modbus.Device;
using System.IO.Ports;
using FreeSCADA.Common;

namespace FreeSCADA.Communication.MODBUSPlug
{
    public class ModbusSerialClientStation : ModbusBaseClientStation, IModbusStation
    {
        ModbusSerialType serialType = ModbusSerialType.RTU;
        private string comPort;
        private int baudRate = 9600;
        private int dataBits = 8;
        private StopBits stopBits = StopBits.One;
        private Parity parity = Parity.None;
        private Handshake handshake = Handshake.None;
        private Thread channelUpdaterThread;

        public ModbusSerialClientStation(string name, Plugin plugin, string comPort, int cycleTimeout, int retryTimeout, int retryCount, int failedCount)
            : base(name, plugin, cycleTimeout, retryTimeout, retryCount, failedCount)
        {
            this.comPort = comPort;
            //StationActive = false;
        }

        public string ComPort
        {
            get { return comPort; }
            set { comPort = value; }
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

        public new int Start()
        {
            if (base.Start() == 0)
            {
                //// Run Thread
                channelUpdaterThread = new Thread(new ParameterizedThreadStart(ChannelUpdaterThreadProc));
                channelUpdaterThread.Start(this);
                return 0;
            }
            else
                return 1;
        }

        public new void Stop()
        {
            base.Stop();
            if (channelUpdaterThread != null)
            {
                //channelUpdaterThread.Abort();
                sendQueueEndWaitEvent.Set();
                channelUpdaterThread.Join();
                channelUpdaterThread = null;
            }
        }

        private static void ChannelUpdaterThreadProc(object obj)
        {
            try
            {
                SerialPort sport = null;
                ModbusSerialClientStation self = (ModbusSerialClientStation)obj;
                self.runThread = true;

                while (self.runThread)
                {
                    try
                    {
                        foreach (ModbusBuffer buf in self.buffers)
                        {
                            buf.pauseCounter = 0;
                        }
                        if (self.LoggingLevel >= ModbusLog.logInfos)
                            Env.Current.Logger.LogInfo(string.Format(StringConstants.InfoSerialStarting,
                                self.Name, self.comPort, self.baudRate, self.parity, self.dataBits, self.stopBits));
                        //using (SerialPort sport = new SerialPort(self.comPort, self.baudRate, self.parity, self.dataBits, self.stopBits))
                        sport = new SerialPort(self.comPort, self.baudRate, self.parity, self.dataBits, self.stopBits);
                        {
                            sport.Handshake = self.handshake;
                            sport.Open();

                            if (self.LoggingLevel >= ModbusLog.logInfos)
                                Env.Current.Logger.LogInfo(string.Format(StringConstants.InfoSerialStarted,
                                    self.Name, self.comPort, self.baudRate, self.parity, self.dataBits, self.stopBits));
                            ModbusSerialMaster master;
                            if (self.serialType == ModbusSerialType.ASCII)
                                master = ModbusSerialMaster.CreateAscii(sport);
                            else
                                master = ModbusSerialMaster.CreateRtu(sport);
                            master.Transport.Retries = self.retryCount;
                            master.Transport.WaitToRetryMilliseconds = self.retryTimeout;

                            while (self.runThread)
                            {
                                // READING
                                foreach (ModbusBuffer buf in self.buffers)
                                {
                                    // Read an actual Buffer first
                                    self.ReadBuffer(self, master, buf);
                                }   // Foreach buffer

                                // WRITING
                                // This implementation causes new reading cycle after writing
                                // anything to MODBUS
                                // The sending strategy should be also considered and enhanced, but:
                                // As I think for now ... it does not matter...
                                self.sendQueueEndWaitEvent.WaitOne(self.cycleTimeout);
                                self.sendQueueEndWaitEvent.Reset();

                                // fast action first - copy from the queue content to my own buffer
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
                                        self.WriteChannel(self, master, ch);
                                    }
                            }   // for endless
                        }   // Using SerialPort
                    }   // Try
                    catch (Exception e)
                    {
                        self.sendQueueEndWaitEvent.Reset();

                        foreach (byte b in self.failures.Keys)
                        {
                            // All devices in failure
                            self.failures[b].Value = true;
                        }
                        if (self.LoggingLevel >= ModbusLog.logWarnings)
                            Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrException, self.Name, e.Message));
                        if (e is ThreadAbortException)
                            throw e;
                        // if (e is )   // Communication timeout to a device
                    }
                    finally
                    {
                        if (sport != null)
                        {
                            sport.Close();
                            sport.Dispose();
                        }
                        // safety Sleep()
                        Thread.Sleep(5000);
                    }
                }
                if (sport != null)
                {
                    sport.Close();
                    sport.Dispose();
                }
            }
            catch (ThreadAbortException e)
            {
                if (((ModbusSerialClientStation)obj).LoggingLevel >= ModbusLog.logErrors)
                    Env.Current.Logger.LogError(string.Format(StringConstants.ErrException, ((ModbusSerialClientStation)obj).Name, e.Message));
            }
        }
    }
}
