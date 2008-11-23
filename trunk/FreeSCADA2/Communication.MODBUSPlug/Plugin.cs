﻿using System.Collections.Generic;
using System.Xml;
using FreeSCADA.ShellInterfaces;
using FreeSCADA.ShellInterfaces.Plugins;

namespace FreeSCADA.Communication.MODBUSPlug
{
	public class Plugin: ICommunicationPlug
	{
		private IEnvironment environment;
		List<Command> commands = new List<Command>();
		List<IChannel> channels = new List<IChannel>();
        List<ModbusStation> stations = new List<ModbusStation>();

		bool connectedFlag = false;

		~Plugin()
		{
			if (IsConnected)
				Disconnect();
		}

		#region ICommunicationPlug Members

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
            }
        }

        public ModbusStation[] Stations
        {
            get { return stations.ToArray(); }
            set
            {
                stations.Clear();
                stations.AddRange(value);
                stations.RemoveAll(delegate(ModbusStation st) { return st == null; });
            }
        }

		public string PluginId
		{
			get { return StringConstants.PluginId; }
		}

		public void Initialize(IEnvironment environment)
		{
			this.environment = environment;
			environment.Project.LoadEvent += new System.EventHandler(OnProjectLoad);

			LoadSettings();

			if(environment.Mode == EnvironmentMode.Designer)
				commands.Add(new PropertyCommand(this));
		}

		public void ProcessCommand(int commandId)
		{
			foreach (Command cmd in commands)
			{
				if (cmd.CommandId == commandId)
					cmd.ProcessCommand();
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
                foreach (ModbusStation stat in stations)
                    stat.Start();
            }

			connectedFlag = true;
			return IsConnected;
		}

		public void Disconnect()
		{
            if (!IsConnected)
                return;
            connectedFlag = false;
            foreach (ModbusStation stat in stations)
                stat.Stop();
			System.GC.Collect();
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
                foreach (ModbusStation stat in stations)
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
				environment.Project.SetData(StringConstants.PluginId + "_channels", ms);
			}
		}

		void LoadSettings()
		{
			channels.Clear();
			using (System.IO.Stream ms = environment.Project[StringConstants.PluginId + "_channels"])
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

                foreach (ModbusStation stat in stations)
                {
                    stat.ClearChannels();
                    foreach (ModbusChannelImp chan in channels)
                        if (chan.ModbusStation == stat.Name)
                            stat.AddChannel(chan);
                }
            }
		}

		void OnProjectLoad(object sender, System.EventArgs e)
		{
			LoadSettings();
		}
	}
}
