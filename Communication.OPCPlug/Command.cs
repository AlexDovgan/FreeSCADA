
namespace FreeSCADA.Communication.OPCPlug
{
	abstract class Command
	{
		protected Plugin plugin;
		string name;
		string group;
		int commandId;

		public Command(Plugin plugin, string name, string group)
		{
			this.plugin = plugin;
			this.name = name;
			this.group = group;

			commandId = plugin.Environment.Commands.RegisterCommand(plugin.PluginId, name, group);
		}

		public int CommandId
		{
			get { return commandId; }
			set { commandId = value; }
		}

		public abstract void ProcessCommand();
	}
}
