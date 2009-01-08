using FreeSCADA.Interfaces;

namespace FreeSCADA.RunTime
{
	static class CommandManager
	{
		static public ICommandContext viewContext;

		public enum Priorities
		{
			ViewCommands = 100,
			ViewCommandsEnd = 199,
		}
	}
}
