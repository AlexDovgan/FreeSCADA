using System.Collections.Generic;
using FreeSCADA.ShellInterfaces;
using FreeSCADA.ShellInterfaces.Plugins;
using System.Threading;
using System.Xml;

namespace FreeSCADA.Communication.SimulatorPlug
{
	public class Plugin: ICommunicationPlug
	{
		private IEnvironment environment;
		List<Command> commands = new List<Command>();
		List<IChannel> channels = new List<IChannel>();
		Thread channelUpdaterThread;

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

			LoadSettings();

			//channels.Add(new GenericChannel<string>("Text channel 1", false, this));
			//channels.Add(new GenericChannel<string>("Text channel 2", true, this));
			//channels.Add(new GenericChannel<int>("Integer channel 1", false, this));
			//channels.Add(new GenericChannel<float>("Real channel 1", false, this));

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
			get { return channelUpdaterThread != null; }
		}

		public bool Connect()
		{
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
					foreach (ChannelBase ch in self.channels)
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
			System.IO.MemoryStream ms = environment.Project[StringConstants.PluginId + "_channels"];
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
			ms.Flush();
		}

		void LoadSettings()
		{
			channels.Clear();
			System.IO.MemoryStream ms = environment.Project[StringConstants.PluginId + "_channels"];
			if (ms.Length == 0)
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
}
