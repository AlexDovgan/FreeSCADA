using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using FreeSCADA.Interfaces;
using FreeSCADA.Interfaces.Plugins;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace FreeSCADA.Communication.SNMPPlug
{
    public class Plugin : ICommunicationPlug
    {
        private IEnvironment environment;
        List<IChannel> channels = new List<IChannel>();
        List<SNMPAgent> agents = new List<SNMPAgent>();

        bool connectedFlag = false;

        /*private static IUnityContainer container;

        internal static IUnityContainer Container
        {
            get { return container; }
        }*/

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

        public SNMPAgent[] Agents
        {
            get { return agents.ToArray(); }
            set
            {
                agents.Clear();
                agents.AddRange(value);
                agents.RemoveAll(delegate(SNMPAgent st) { return st == null; });
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

            /*container = new UnityContainer();
            UnityConfigurationSection section
              = (UnityConfigurationSection)System.Configuration.ConfigurationManager.GetSection("unity");
            //section.Containers.Default.Configure(Container);*/

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
                foreach (SNMPAgent sag in agents)
                    sag.Start();
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
            stopDelegate[] s = new stopDelegate[agents.Count];
            IAsyncResult[] res = new IAsyncResult[agents.Count];
            for (int i = 0; i < agents.Count; i++)
            {
                s[i] = new stopDelegate(agents[i].Stop);
                res[i] = s[i].BeginInvoke(null,null);
                Thread.Sleep(0);
            }
            // Waiting for end of threads
            for (int i = 0; i < agents.Count; i++)
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
                foreach (SNMPAgent agent in agents)
                {
                    XmlElement elem = doc.CreateElement("agent");
                    AgentFactory.SaveAgent(elem, agent);
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
            agents.Clear();
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
                XmlNodeList snodes = doc.GetElementsByTagName("agent");
                foreach (XmlElement snode in snodes)
                    agents.Add(AgentFactory.CreateAgent(snode, this));
                XmlNodeList nodes = doc.GetElementsByTagName("channel");
                foreach (XmlElement node in nodes)
                    channels.Add(ChannelFactory.CreateChannel(node, this));

                foreach (SNMPAgent agent in agents)
                {
                    agent.ClearChannels();
                    foreach (SNMPChannelImp chan in channels)
                        if (chan.AgentName == agent.Name)
                        {
                            agent.AddChannel(chan);
                            chan.MyAgent = agent;
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
