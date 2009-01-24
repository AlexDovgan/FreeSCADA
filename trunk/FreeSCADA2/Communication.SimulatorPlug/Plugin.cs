using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using FreeSCADA.Common;
using FreeSCADA.Interfaces;
using FreeSCADA.Interfaces.Plugins;

namespace FreeSCADA.Communication.SimulatorPlug
{
	public class Plugin: ICommunicationPlug
	{
		private IEnvironment environment;
		List<IChannel> channels = new List<IChannel>();
		Thread channelUpdaterThread;

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
			get { return channelUpdaterThread != null; }
		}

		public bool Connect()
		{
			foreach (IChannel channel in channels)
			{
				if (channel is BaseChannel)
					(channel as BaseChannel).Reset();
			}
			channelUpdaterThread = new Thread(new ParameterizedThreadStart(ChannelUpdaterThreadProc));
			channelUpdaterThread.Start(this);
			return IsConnected;
		}

		public void Disconnect()
		{
			if (channelUpdaterThread != null)
			{
				channelUpdaterThread.Abort();
				channelUpdaterThread.Join();
				channelUpdaterThread = null;
			}
		}

		#endregion

		private static void ChannelUpdaterThreadProc(object obj)
		{
			try
			{
				Plugin self = (Plugin)obj;
				for (; ; )
				{
					//System.Console.WriteLine("{0} ChannelUpdaterThreadProc: Start loop", System.DateTime.Now);
					foreach (BaseChannel ch in self.channels)
						ch.DoUpdate();
					Thread.Sleep(100);
				}
			}
			catch (ThreadAbortException)
			{
			}
		}

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
				environment.Project["settings/" + StringConstants.PluginId + "_channels"] = ms;
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
