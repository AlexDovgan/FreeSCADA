using System.Collections.Generic;
using System.Windows.Forms;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Common
{
	public class Commands : ICommands
	{
		struct CommandInfo
		{
			public ICommandContext context;
			public ICommand cmd;
		};
		List<CommandInfo> registeredCommands = new List<CommandInfo>();
		BaseCommandContext globalContext;
		BaseCommandContext communicationContext;

		#region ICommands implementation

		public Commands(MenuStrip menu, ToolStrip toolbar)
		{
			globalContext = new BaseCommandContext(menu, toolbar);

			ToolStrip communicationMenu = GetGroupItem(menu, StringResources.CommunicationCommandGroupName);
			communicationContext = new BaseCommandContext(communicationMenu, null);
		}

		public void AddCommand(ICommandContext context, ICommand cmd)
		{
			CommandInfo commandInfo = new CommandInfo();
			commandInfo.context = context;
			commandInfo.cmd = cmd;
			registeredCommands.Add(commandInfo);

			commandInfo.context.AddCommand(commandInfo.cmd);
		}

		public void RemoveCommand(ICommand cmd)
		{
			List<CommandInfo> newList = new List<CommandInfo>();
			List<CommandInfo> removalList = new List<CommandInfo>();
			foreach (CommandInfo cmdInfo in registeredCommands)
			{
				if (cmdInfo.cmd == cmd)
					removalList.Add(cmdInfo);
				else
					newList.Add(cmdInfo);
			}

			registeredCommands = newList;

			foreach (CommandInfo cmdInfo in removalList)
				cmdInfo.context.RemoveCommand(cmdInfo.cmd);
		}

		public List<ICommand> GetCommands(ICommandContext context)
		{
			List<ICommand> result = new List<ICommand>();
			foreach (CommandInfo cmdInfo in registeredCommands)
			{
				if (cmdInfo.context == context)
					result.Add(cmdInfo.cmd);
			}
			return result;
		}

		public ICommand FindCommandByName(ICommandContext context, string name)
		{
			foreach (CommandInfo cmdInfo in registeredCommands)
			{
				if (cmdInfo.context == context && cmdInfo.cmd.Name == name)
					return cmdInfo.cmd;
			}
			return null;
		}

		public ICommandContext GetPredefinedContext(PredefinedContexts type)
		{
			if (type == PredefinedContexts.Global)
				return globalContext;
			else if (type == PredefinedContexts.Communication)
				return communicationContext;
			else
				return null;
		}
		#endregion

		private ToolStrip GetGroupItem(ToolStrip root, string name)
		{
			foreach (ToolStripItem item in root.Items)
			{
				ToolStripMenuItem tmp = (ToolStripMenuItem)item;
				if (name == item.Text && tmp != null)
					return tmp.DropDown;
			}
			ToolStripMenuItem newItem = new ToolStripMenuItem(name);
			root.Items.Add(newItem);
			return newItem.DropDown;
		}
	}
}
