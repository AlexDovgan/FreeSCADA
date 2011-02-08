using FreeSCADA.Interfaces;

namespace FreeSCADA.CommonUI
{
   public static class CommandManager
	{
		/*static public ICommandContext documentContext;
		static public ICommandContext viewContext;
		static public ICommandContext fileContext;
		static public ICommandContext documentMenuContext;
        static public ICommandContext toolboxContext;
		static public ICommandContext helpContext;
       */
		public enum Priorities
		{
			FileCommands = 0,
			FileCommandsEnd = 9,

            MruCommands = 10,
            MruCommandsEnd = 19,

			ViewCommands = 100,
			ViewCommandsEnd = 199,

			HelpCommands = 200,
			HelpCommandsEnd = 299,

			EditCommands = 300,
			EditCommandsEnd = 399
		}
	}
}
