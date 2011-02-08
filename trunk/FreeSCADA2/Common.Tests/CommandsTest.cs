using FreeSCADA.CommonUI;
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

			ICommandContext context = commands.GetContext(FreeSCADA.Interfaces.PredefinedContexts.Communication);
			Assert.IsNotNull(context);

			context.AddCommand( testCommand1);
            context.AddCommand( testCommand2);

            Assert.AreEqual(2, context.GetCommands().Count);

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

			ICommandContext context = commands.GetContext(FreeSCADA.Interfaces.PredefinedContexts.GlobalMenu);
            context.AddCommand( testCommand);

            foreach (ICommand cmd in context.GetCommands())
				cmd.Execute();

			Assert.IsTrue(testCommand.isExecuted);

			testCommand.isExecuted = false;
			ToolStripItem menuItem = null;
			foreach(ToolStripItem item in menu.Items)
			{
				if(item.Text == testCommand.Name)
					menuItem = item;
			}
			Assert.IsNotNull(menuItem);
			menuItem.PerformClick();
			Assert.IsTrue(testCommand.isExecuted);

			testCommand.isExecuted = false;
			toolbar.Items[0].PerformClick();
			Assert.IsTrue(testCommand.isExecuted);
		}
	}
}
