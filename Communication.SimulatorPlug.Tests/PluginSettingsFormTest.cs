using FreeSCADA.Communication.SimulatorPlug;
using NUnit.Framework;
using NUnit.Extensions.Forms;

namespace Communication.SimulatorPlug.Tests
{
	[TestFixture]
	public class PluginSettingsFormTest : NUnitFormTest
	{
		Plugin plugin;
		EnvironmentMock environment;
		int[] channelchangedNotification;

		public override void Setup()
		{
			environment = new EnvironmentMock();
			plugin = new Plugin();
			plugin.Initialize(environment);
		}

		public override void TearDown()
		{
			plugin = null;
			environment = null;
			System.GC.Collect();
		}

		[Test]
		public void AddingNewChannels()
		{
			ExpectModal("SettingsForm", "AddingNewChannelsHandler");
			plugin.ProcessCommand(0);
			Assert.AreEqual(3, plugin.Channels.Length);
		}

		private void AddingNewChannelsHandler()
		{
			GridTester grid = new GridTester("grid", "SettingsForm");
			ButtonTester addButton = new ButtonTester("addButton", "SettingsForm");
			ButtonTester removeButton = new ButtonTester("removeButton", "SettingsForm");
			ButtonTester okButton = new ButtonTester("okButton", "SettingsForm");

			Assert.AreEqual(1, grid.Control.RowsCount); //Header row only

			addButton.Click();
			addButton.Click();
			addButton.Click();
			addButton.Click();

			Assert.AreEqual(5, grid.Control.RowsCount); //Header row + 4 channel rows

			removeButton.Click();

			Assert.AreEqual(4, grid.Control.RowsCount); //Remove one row

			okButton.Click();
		}

		[Test]
		public void ReceiveData()
		{
			ExpectModal("SettingsForm", "CreateRandomChannels");
			plugin.ProcessCommand(0);
			Assert.AreEqual(3, plugin.Channels.Length);
			Assert.IsFalse(plugin.IsConnected);
			channelchangedNotification = new int[3];
			for (int i = 0; i < 3; i++)
				channelchangedNotification[i] = 0;

			int channelNo = 0;
			foreach (FreeSCADA.ShellInterfaces.IChannel ch in plugin.Channels)
			{
				Assert.IsNull(ch.Value);
				ch.Tag = channelNo;
				Assert.AreEqual(channelNo, ch.Tag);
				channelNo++;
			}

			foreach (FreeSCADA.ShellInterfaces.IChannel ch in plugin.Channels)
			{
				Assert.IsNull(ch.Value);
				ch.ValueChanged += new System.EventHandler(OnChannelValueChanged);
			}

			plugin.Connect();
			System.Threading.Thread.Sleep(500);
			foreach (FreeSCADA.ShellInterfaces.IChannel ch in plugin.Channels)
				Assert.IsNotNull(ch.Value);

			for (int i = 0; i < 3; i++)
				Assert.Greater(channelchangedNotification[i], 0);

			plugin.Disconnect();
		}

		void OnChannelValueChanged(object sender, System.EventArgs e)
		{
			FreeSCADA.ShellInterfaces.IChannel ch = (FreeSCADA.ShellInterfaces.IChannel)sender;
			channelchangedNotification[(int)ch.Tag]++;
		}

		private void CreateRandomChannels()
		{
			GridTester grid = new GridTester("grid", "SettingsForm");
			ButtonTester addButton = new ButtonTester("addButton", "SettingsForm");
			ButtonTester okButton = new ButtonTester("okButton", "SettingsForm");

			addButton.Click();
			addButton.Click();
			addButton.Click();

			grid.SetChannelType(0, "Random integer");
			grid.SetChannelType(1, "Random integer");
			grid.SetChannelType(2, "Random integer");

			okButton.Click();
		}
	}

	class GridTester : ControlTester
	{
		public GridTester(string controlName, string formName)
			: base(controlName, formName)
		{
			
		}

		new public SourceGrid.Grid Control
		{
			get
			{
				return (SourceGrid.Grid)base.Control;
			}
		}

		public void SetChannelType(int channelNo, string type)
		{
			Control[channelNo + 1, 1].Value = type;
		}
	}
}
