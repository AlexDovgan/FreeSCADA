using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FreeSCADA.ShellInterfaces;
using FreeSCADA.ShellInterfaces.Plugins;

namespace FreeSCADA.Common
{
	public class CommunationPlugs
	{
		private List<ICommunicationPlug> commPlugs = new List<ICommunicationPlug>();

		public void Load()
		{
			DirectoryInfo di = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
			foreach (FileInfo fi in di.GetFiles("Communication.*.dll"))
			{
				Assembly lib = Assembly.LoadFrom(fi.FullName);
				foreach (Type t in lib.GetExportedTypes())
				{
					if (t.GetInterface(typeof(ICommunicationPlug).FullName) != null)
					{
						ICommunicationPlug plug = (ICommunicationPlug)Activator.CreateInstance(t);
						InitializePlugin(Env.Current, plug);
					}
				}
			}
			(Env.Current.Commands as Commands).PluginCommand += new Commands.PluginCommandHandler(OnCommand);
		}

		void OnCommand(object sender, int id, string pluginId)
		{
			foreach(ICommunicationPlug plug in commPlugs)
				if(plug.PluginId == pluginId)
					plug.ProcessCommand(id);
		}

		private void InitializePlugin(IEnvironment env, ICommunicationPlug plug)
		{
			plug.Initialize(env);
			commPlugs.Add(plug);
		}

		public List<string> PluginIds
		{
			get
			{
				List<string> list = new List<string>();
				foreach (ICommunicationPlug plug in commPlugs)
					list.Add(plug.PluginId);
				return list;
			}
		}

		public ICommunicationPlug this[string pluginId]
		{
			get
			{
				foreach (ICommunicationPlug plug in commPlugs)
					if (plug.PluginId == pluginId)
						return plug;
				return null;
			}
		}

		public bool Connect()
		{
			foreach (ICommunicationPlug plug in commPlugs)
			{
				if (plug.Connect() == false)
				{
					Disconnect();
					return false;
				}
			}
			return true;
		}

		public void Disconnect()
		{
			foreach (ICommunicationPlug plug in commPlugs)
				plug.Disconnect();
		}

		public bool IsConnected
		{
			get
			{
				foreach (ICommunicationPlug plug in commPlugs)
				{
					if (plug.IsConnected == false)
						return false;
				}
				return true;
			}
		}
        
        public IChannel GetChannel(string name)
        {
                
            string[] splited = name.Split('.');
            if(splited.Length>1)
            {
                ICommunicationPlug plug;
                if( (plug=this[splited[0]])!=null)
                {
                    foreach (IChannel chanel in plug.Channels)
                    {
                        if(chanel.Name==splited[1])
                            return chanel;
                    }
                } return null;
            } return null;
            
        }
	}
}
