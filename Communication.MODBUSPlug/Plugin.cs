using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using FreeSCADA.Interfaces;
using FreeSCADA.Interfaces.Plugins;

namespace FreeSCADA.Communication.MODBUSPlug
{
    public class Plugin : ICommunicationPlug
    {
        private IEnvironment environment;
        List<IChannel> channels = new List<IChannel>();
        List<IModbusStation> stations = new List<IModbusStation>();

        bool connectedFlag = false;

        ~Plugin()
        {
            if (IsConnected)
                Disconnect();
        }

        #region ICommunicationPlug Members
        public event EventHandler ChannelsChanged;

        public string Name
        {
            get { return StringConstants.PluginName; }
        }

        public IChannel[] Channels
        {
            get { return channels.ToArray(); }
            set
            {
                channels.Clear();
                channels.AddRange(value);
                channels.RemoveAll(delegate(IChannel ch) { return ch == null; });
                FireChannelChangedEvent();
            }
        }

        public IModbusStation[] Stations
        {
            get { return stations.ToArray(); }
            set
            {
                stations.Clear();
                stations.AddRange(value);
                stations.RemoveAll(delegate(IModbusStation st) { return st == null; });
            }
        }

        public string PluginId
        {
            get { return StringConstants.PluginId; }
        }

        public void Initialize(IEnvironment environment)
        {
            this.environment = environment;
            environment.Project.ProjectLoaded += new System.EventHandler(OnProjectLoad);

            LoadSettings();

            if (environment.Mode == EnvironmentMode.Designer)
            {
                ICommandContext context = environment.Commands.GetPredefinedContext(PredefinedContexts.Communication);
                environment.Commands.AddCommand(context, new PropertyCommand(this));
            }
        }

        public bool IsConnected
        {
            get { return connectedFlag; }
        }

        public bool Connect()
        {
            if (IsConnected)
                return false;

            System.GC.Collect();

            if (channels.Count > 0)
            {
                foreach (IModbusStation stat in stations)
                    stat.Start();
            }

            connectedFlag = true;
            return IsConnected;
        }

        delegate void stopDelegate();

        public void Disconnect()
        {
            if (!IsConnected)
                return;
            // Parallel stopping of threads
            stopDelegate[] s = new stopDelegate[stations.Count];
            IAsyncResult[] res = new IAsyncResult[stations.Count];
            for (int i = 0; i < stations.Count; i++)
            {
                s[i] = new stopDelegate(stations[i].Stop);
                res[i] = s[i].BeginInvoke(null,null);
                Thread.Sleep(0);
            }
            // Waiting for end of threads
            for (int i = 0; i < stations.Count; i++)
            {
                s[i].EndInvoke(res[i]);
            }
            System.GC.Collect();
            connectedFlag = false;
        }

        #endregion

        public IEnvironment Environment
        {
            get { return environment; }
            set { Initialize(value); }
        }

        public void SaveSettings()
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                if (ms.Length != 0)
                {
                    ms.SetLength(0);
                    ms.Seek(0, System.IO.SeekOrigin.Begin);
                }

                XmlDocument doc = new System.Xml.XmlDocument();
                XmlElement root_elem = doc.CreateElement("root");
                foreach (IModbusStation stat in stations)
                {
                    XmlElement elem = doc.CreateElement("station");
                    StationFactory.SaveStation(elem, stat);
                    root_elem.AppendChild(elem);
                }

                foreach (IChannel ch in channels)
                {
                    XmlElement elem = doc.CreateElement("channel");
                    ChannelFactory.SaveChannel(elem, ch);
                    root_elem.AppendChild(elem);
                }
                doc.AppendChild(root_elem);
                doc.Save(ms);
                environment.Project.SetData("settings/" + StringConstants.PluginId + "_channels", ms);
            }
        }

        void LoadSettings()
        {
            channels.Clear();
            stations.Clear();
            using (System.IO.Stream ms = environment.Project["settings/" + StringConstants.PluginId + "_channels"])
            {
                if (ms == null || ms.Length == 0)
                    return;
                XmlDocument doc = new System.Xml.XmlDocument();
                try
                {
                    doc.Load(ms);
                }
                catch
                {
                    return;
                }
                XmlNodeList snodes = doc.GetElementsByTagName("station");
                foreach (XmlElement snode in snodes)
                    stations.Add(StationFactory.CreateStation(snode, this));
                XmlNodeList nodes = doc.GetElementsByTagName("channel");
                foreach (XmlElement node in nodes)
                    channels.Add(ChannelFactory.CreateChannel(node, this));

                foreach (IModbusStation stat in stations)
                {
                    stat.ClearChannels();
                    foreach (ModbusChannelImp chan in channels)
                        if (chan.ModbusStation == stat.Name)
                        {
                            stat.AddChannel(chan);
                            chan.MyStation = (ModbusBaseClientStation)stat;
                        }
                }
            }
            FireChannelChangedEvent();
        }

        void OnProjectLoad(object sender, System.EventArgs e)
        {
            LoadSettings();
        }

        void FireChannelChangedEvent()
        {
            if (ChannelsChanged != null)
                ChannelsChanged(this, new EventArgs());
        }
    }
}
