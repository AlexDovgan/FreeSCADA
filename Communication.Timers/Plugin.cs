using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using FreeSCADA.Common;
using FreeSCADA.Interfaces;
using FreeSCADA.Interfaces.Plugins;

namespace FreeSCADA.Communication.Timers
{
	public class Plugin: ICommunicationPlug
	{
		private IEnvironment environment;
		List<IChannel> channels = new List<IChannel>();
        bool isConnected;

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
				ICommandContext context = environment.Commands.GetContext(PredefinedContexts.Communication);
				context.AddCommand(new PropertyCommand(this));
			}
		}

		public bool IsConnected
		{
            get { return isConnected; }
		}

		public bool Connect()
		{
            isConnected = true;
            foreach (IChannel ch in channels)
            {
                if (ch is RelativeTimerChannel)
                    (ch as RelativeTimerChannel).Enable = true;
                //else if (ch is AbsoluteTimerChannel)
                //    ;
            }
			return IsConnected;
		}

		public void Disconnect()
		{
            isConnected = false;
            foreach (IChannel ch in channels)
            {
                if (ch is RelativeTimerChannel)
                    (ch as RelativeTimerChannel).Enable = false;
                //else if (ch is AbsoluteTimerChannel)
                //    ;
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
					XmlElement elem = doc.CreateElement("timer");
					ChannelFactory.SaveChannel(elem, ch);
					root_elem.AppendChild(elem);
				}
				doc.AppendChild(root_elem);
				doc.Save(ms);
				environment.Project[StringConstants.PluginId + "_channels"] = ms;
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
				XmlNodeList nodes = doc.GetElementsByTagName("timer");
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
