using System.Windows.Forms;
using FreeSCADA.Interfaces;
using NUnit.Framework;

namespace FreeSCADA.Common.Tests
{
	[TestFixture]
	public class CommandsTest
	{
		[Test]
		public void RegisterCommand()
		{
			MenuStrip menu = new MenuStrip();
			ToolStrip toolbar = new ToolStrip();
			Commands commands = new Commands(menu, toolbar);

			CommandMock testCommand1 = new CommandMock("Test command 1");
			CommandMock testCommand2 = new CommandMock("Test command 2");

			ICommandContext context = commands.GetPredefinedContext(FreeSCADA.Interfaces.PredefinedContexts.Communication);
			Assert.IsNotNull(context);

			commands.AddCommand(context, testCommand1);
			commands.AddCommand(context, testCommand2);

			Assert.AreEqual(2, commands.GetCommands(context).Count);

			ToolStripMenuItem group = (ToolStripMenuItem)menu.Items[0];
			Assert.AreEqual("Test command 1", group.DropDown.Items[0].Text);
			Assert.AreEqual("Test command 2", group.DropDown.Items[1].Text);
		}

		[Test]
		public void HandlingEvents()
		{
			MenuStrip menu = new MenuStrip();
			ToolStrip toolbar = new ToolStrip();
			Commands commands = new Commands(menu, toolbar);

			CommandMock testCommand = new CommandMock("Test command 1");

			ICommandContext context = commands.GetPredefinedContext(FreeSCADA.Interfaces.PredefinedContexts.Global);
			commands.AddCommand(context, testCommand);

			foreach (ICommand cmd in commands.GetCommands(context))
				cmd.Execute();

			Assert.IsTrue(testCommand.isExecuted);

			testCommand.isExecuted = false;
			menu.Items[1].PerformClick();
			Assert.IsTrue(testCommand.isExecuted);

			testCommand.isExecuted = false;
			toolbar.Items[0].PerformClick();
			Assert.IsTrue(testCommand.isExecuted);
		}
	}
}
