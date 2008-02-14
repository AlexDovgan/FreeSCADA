using NUnit.Framework;
using System.Windows.Forms;

namespace FreeSCADA.Common.Tests
{
	[TestFixture]
	public class CommandsTest
	{
		[Test]
		public void RegisterCommand()
		{
			MenuStrip menu = new MenuStrip();
			Commands commands = new Commands(menu);

			//There is only one command. RegisterCommand should return "0" then "1"
			Assert.AreEqual(0, commands.RegisterCommand("test_id", "Test command 1", "Test group"));
			Assert.AreEqual(1, commands.RegisterCommand("test_id", "Test command 2", "Test group"));

			Assert.AreEqual("Test group", menu.Items[0].Text);
			Assert.IsTrue(menu.Items[0] is ToolStripMenuItem);

			ToolStripMenuItem group = (ToolStripMenuItem)menu.Items[0];
			Assert.AreEqual("Test command 1", group.DropDown.Items[0].Text);
			Assert.AreEqual("Test command 2", group.DropDown.Items[1].Text);
		}

		class PluginEventReceiver
		{
			public object sender = null;
			public int id = -1;
			public string pluginId = null;
			public bool gotEvent = false;

			public void Handler(object sender, int id, string pluginId)
			{
				gotEvent = true;
				this.sender = sender;
				this.id = id;
				this.pluginId = pluginId;
			}
		}

		[Test]
		public void PluginCommandEvent()
		{
			MenuStrip menu = new MenuStrip();
			Commands commands = new Commands(menu);

			int cmdId = commands.RegisterCommand("test_id", "Test command 1", "Test group");

			PluginEventReceiver receiver = new PluginEventReceiver();
			commands.PluginCommand += new Commands.PluginCommandHandler(receiver.Handler);
			
			Assert.IsFalse(receiver.gotEvent);
			ToolStripMenuItem group = (ToolStripMenuItem)menu.Items[0];
			group.DropDown.Items[0].PerformClick();

			Assert.IsTrue(receiver.gotEvent);
			Assert.AreEqual(commands, receiver.sender);
			Assert.AreEqual(cmdId, receiver.id);
			Assert.AreEqual("test_id",receiver.pluginId);			
		}
	}
}
