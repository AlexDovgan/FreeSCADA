using FreeSCADA.Interfaces;

namespace FreeSCADA.Designer
{
	static class CommandManager
	{
		static public ICommandContext documentContext = null;
		static public ICommandContext viewContext = null;
		static public ICommandContext fileContext = null;

		public enum Priorities
		{
			FileCommands = 0,
			FileCommandsEnd = 10,

			ViewCommands = 100,
			ViewCommandsEnd = 199,

			EditCommands = 300,
			EditCommandsEnd = 399
		}
	}
}
