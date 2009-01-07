using FreeSCADA.Interfaces;

namespace FreeSCADA.Designer
{
	static class CommandManager
	{
		static public ICommandContext documentContext;
		static public ICommandContext viewContext;
		static public ICommandContext fileContext;

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
