using FreeSCADA.Interfaces;

namespace FreeSCADA.Designer
{
	static class CommandManager
	{
		static public ICommandContext documentContext = null;
		static public ICommandContext viewContext = null;
		static public ICommandContext fileContext = null;
	}
}
