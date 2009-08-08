using System;
using System.Collections.Generic;
using System.Xml;
using FreeSCADA.Interfaces;
using FreeSCADA.Interfaces.Plugins;

namespace FreeSCADA.Communication.CLServer
{
	public class Plugin: ICommunicationPlug
	{
		private IEnvironment environment;
		List<IChannel> channels = new List<IChannel>();
		List<ConnectionGroup> connectionGroups = new List<ConnectionGroup>();
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
				channels.RemoveAll( delegate(IChannel ch) { return ch == null; } );
				FireChannelChangedEvent();
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

			connectedFlag = true;

			foreach (ConnectionGroup group in connectionGroups)
				group.Dispose();
			connectionGroups.Clear();
			System.GC.Collect();

			if (channels.Count > 0)
			{
				try
				{
					List<IChannel> originalChannels = new List<IChannel>();
					originalChannels.AddRange(channels);
					do
					{
						List<RemoutingChannel> groupChannels = new List<RemoutingChannel>();
						RemoutingChannel lhc = (RemoutingChannel)originalChannels[0];
						groupChannels.Add(lhc);
						originalChannels.RemoveAt(0);
						for (int i = originalChannels.Count - 1; i >= 0; i--)
						{
							RemoutingChannel rhc = (RemoutingChannel)originalChannels[i];
							if (lhc.Server == rhc.Server && lhc.Port == rhc.Port)
							{
								groupChannels.Add(rhc);
								originalChannels.RemoveAt(i);
							}
						}
						connectionGroups.Add(new ConnectionGroup(lhc.Server, lhc.Port, groupChannels));

					} while (originalChannels.Count > 0);
				}
				catch (Exception e)
				{
					connectedFlag = false;
					FreeSCADA.Common.Env.Current.Logger.LogError(string.Format("Problem with connection to remote server: {0}", e.Message));
				}
			}
			
			return IsConnected;
		}

		public void Disconnect()
		{
			connectedFlag = false;

			foreach (ConnectionGroup group in connectionGroups)
				group.Dispose();
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
				environment.Project.SetData("settings/" + StringConstants.PluginId + "_channels", ms);
			}
		}

		void LoadSettings()
		{
			channels.Clear();
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
				XmlNodeList nodes = doc.GetElementsByTagName("channel");
				foreach (XmlElement node in nodes)
					channels.Add(ChannelFactory.CreateChannel(node, this));
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
