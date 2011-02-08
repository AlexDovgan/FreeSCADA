using System.Collections.Generic;

namespace FreeSCADA.Interfaces
{
	public interface ICommandContext
	{
		void AddCommand(ICommand cmd);
		void RemoveCommand(ICommand cmd);
        List<ICommand> GetCommands();
	}
}
 