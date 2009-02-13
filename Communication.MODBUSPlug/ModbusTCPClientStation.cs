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
    public class ModbusTCPClientStation : ModbusBaseClientStation, IModbusStation
    {
        private string ipAddress;
        private int tcpPort;
        private Thread channelUpdaterThread;

        public ModbusTCPClientStation(string name, Plugin plugin, string ipAddress, int tcpPort, int cycleTimeout, int retryTimeout, int retryCount, int failedCount)
            :base(name, plugin, cycleTimeout, retryTimeout, retryCount, failedCount)
        {
            this.ipAddress = ipAddress;
            this.tcpPort = tcpPort;
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
                channelUpdaterThread.Abort();
                channelUpdaterThread.Join();
                channelUpdaterThread = null;
            }
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

                            for (; ; )
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
                                if (self.channelsToSend.Count > 0) {
                                    for (int i = self.channelsToSend.Count; i > 0 ; i--)
                                    {
                                        ModbusChannelImp ch = self.channelsToSend[i - 1];
                                        // One try ONLY
                                        self.channelsToSend.RemoveAt(i - 1);
                                        self.WriteChannel(self, master, ch);
                                    }
                                }
                            }   // for endless
                        }   // Using TCP client
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
    }
}
