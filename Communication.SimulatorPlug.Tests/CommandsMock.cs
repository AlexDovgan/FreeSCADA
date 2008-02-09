using System;
using System.Collections.Generic;
using FreeSCADA.ShellInterfaces;

namespace Communication.SimulatorPlug.Tests
{
	class CommandsMock:ICommands
	{
		public struct CommandInfo
		{
			public string PluginId;
			public string Name;
			public string Group;

			public CommandInfo(string pluginId, string name, string group)
			{
				PluginId = pluginId;
				Name = name;
				Group = group;
			}
		}
		public List<CommandInfo> registeredCommands = new List<CommandInfo>();

		#region ICommands Members

		public int RegisterCommand(string pluginId, string name, string group)
		{
			registeredCommands.Add(new CommandInfo(pluginId,name,group));
			return registeredCommands.Count - 1;
		}

		#endregion
	}
}
