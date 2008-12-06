using System.Collections.Generic;

namespace FreeSCADA.Interfaces
{
	public enum PredefinedContexts
	{
		Global,
		Communication
	}
	public interface ICommands
	{
		void AddCommand(ICommandContext context, ICommand cmd);
		void RemoveCommand(ICommand cmd);

		ICommandContext GetPredefinedContext(PredefinedContexts type);

		List<ICommand> GetCommands(ICommandContext context);

		/// <summary>
		/// This method is only for testing and should not be used normally
		/// </summary>
		ICommand FindCommandByName(ICommandContext context, string name);
	}
}
