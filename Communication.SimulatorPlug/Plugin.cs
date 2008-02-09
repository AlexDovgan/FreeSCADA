using System.Collections.Generic;
using FreeSCADA.ShellInterfaces;
using FreeSCADA.ShellInterfaces.Plugins;
using System.Threading;

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
			//System.IO.MemoryStream ms = plugin.Environment.Project[StringConstants.PluginId + "_channels"];
			////System.Xml.XmlWriter writter = System.Xml.XmlWriter.Create(ms);
			//XmlDocument doc = new System.Xml.XmlDocument();
			//XmlElement root_elem = doc.CreateElement("root");
			//ShellInterfaces.IChannel[] channels = new ShellInterfaces.IChannel[grid.RowsCount - 1];
			//for (int i = 1; i < grid.RowsCount; i++)
			//{
			//    channels[i-1] = new GenericChannel<int>(grid[i, 0].DisplayText, false, plugin);

			//    XmlElement elem = doc.CreateElement("channel");
			//    ChannelFactory.SaveChannel(elem, channels[i - 1]);
			//    root_elem.AppendChild(elem);
			//}
			//doc.Save(ms);

			//System.IO.MemoryStream ms = plugin.Environment.Project[StringConstants.PluginId + "_channels"];
			//System.Xml.XmlWriter writter = System.Xml.XmlWriter.Create(ms);
			//writter.WriteStartElement("root");
			//for (int i = 1; i < grid.RowsCount; i++)
			//{


			//    writter.WriteStartElement("channel");
			//    writter.WriteAttributeString("var_name", grid[i, 0].DisplayText);
			//    writter.WriteAttributeString("var_type", grid[i, 1].DisplayText);
			//    writter.WriteEndElement();
			//}
			//writter.WriteEndElement();
			//writter.Flush();

			//using (BinaryWriter binWriter = new BinaryWriter(File.Open(@"d:\temp.txt", FileMode.Create)))
			//    binWriter.Write(ms.GetBuffer(), 0, (int)ms.Length);
		}

		void LoadSettings()
		{
		}
	}
}
