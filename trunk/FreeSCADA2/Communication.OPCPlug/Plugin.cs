﻿using System.Collections.Generic;
using System.Xml;
using FreeSCADA.ShellInterfaces;
using FreeSCADA.ShellInterfaces.Plugins;

namespace FreeSCADA.Communication.OPCPlug
{
	public class Plugin: ICommunicationPlug
	{
		private IEnvironment environment;
		List<Command> commands = new List<Command>();
		List<IChannel> channels = new List<IChannel>();
		List<ConnectionGroup> connectionGroups = new List<ConnectionGroup>();
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
				channels.RemoveAll( delegate(IChannel ch) { return ch == null; } );
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

			connectionGroups.Clear();
			System.GC.Collect();

			if (channels.Count > 0)
			{
				List<IChannel> originalChannels = new List<IChannel>();
				originalChannels.AddRange(channels);
				do
				{
					List<OPCBaseChannel> groupChannels = new List<OPCBaseChannel>();
					OPCBaseChannel lhc = (OPCBaseChannel)originalChannels[0];
					groupChannels.Add(lhc);
					originalChannels.RemoveAt(0);
					for (int i = originalChannels.Count - 1; i >= 0; i--)
					{
						OPCBaseChannel rhc = (OPCBaseChannel)originalChannels[i];
						if (lhc.OpcServer == rhc.OpcServer && lhc.OpcHost == rhc.OpcHost)
						{
							groupChannels.Add(rhc);
							originalChannels.RemoveAt(i);
						}
					}
					connectionGroups.Add(new ConnectionGroup(lhc.OpcServer, lhc.OpcHost, groupChannels));

				} while (originalChannels.Count > 0);
			}

			connectedFlag = true;
			return IsConnected;
		}

		public void Disconnect()
		{
			connectedFlag = false;

			foreach (OPCBaseChannel ch in channels)
				ch.Disconnect();
			
			connectionGroups.Clear();

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
				XmlNodeList nodes = doc.GetElementsByTagName("channel");
				foreach (XmlElement node in nodes)
					channels.Add(ChannelFactory.CreateChannel(node, this));
			}
		}

		void OnProjectLoad(object sender, System.EventArgs e)
		{
			LoadSettings();
		}
	}
}
