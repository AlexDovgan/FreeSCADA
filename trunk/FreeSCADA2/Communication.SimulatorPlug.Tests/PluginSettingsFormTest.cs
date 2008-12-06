using FreeSCADA.Common;
using FreeSCADA.Communication.SimulatorPlug;
using FreeSCADA.Interfaces;
using NUnit.Extensions.Forms;
using NUnit.Framework;

namespace Communication.SimulatorPlug.Tests
{
	[TestFixture]
	public class PluginSettingsFormTest : NUnitFormTest
	{
		Plugin plugin;
		int[] channelchangedNotification;
		string[] ChannelTypes = { "Current time", "Random integer", "Simple integer", "Simple string", "Simple float" };
		string projectFile;

		public override void Setup()
		{
			System.Windows.Forms.MenuStrip menu = new System.Windows.Forms.MenuStrip();
			Env.Initialize(null, menu, null, FreeSCADA.Interfaces.EnvironmentMode.Designer);
			plugin = (Plugin)Env.Current.CommunicationPlugins["data_simulator_plug"];

			projectFile = System.IO.Path.GetTempFileName();

			if (System.IO.File.Exists(projectFile))
				System.IO.File.Delete(projectFile);
		}

		public override void TearDown()
		{
			plugin = null;
			Env.Deinitialize();

			System.IO.File.Delete(projectFile);

			System.GC.Collect();
		}

		[Test]
		public void AddingNewChannels()
		{
			ExpectModal("SettingsForm", "AddingNewChannelsHandler");
			
			ICommandContext context = Env.Current.Commands.GetPredefinedContext(PredefinedContexts.Communication);
			Env.Current.Commands.FindCommandByName(context, StringConstants.PropertyCommandName).Execute();

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

			ICommandContext context = Env.Current.Commands.GetPredefinedContext(PredefinedContexts.Communication);
			Env.Current.Commands.FindCommandByName(context, StringConstants.PropertyCommandName).Execute();

			Assert.AreEqual(3, plugin.Channels.Length);
			Assert.IsFalse(plugin.IsConnected);
			channelchangedNotification = new int[3];
			for (int i = 0; i < 3; i++)
				channelchangedNotification[i] = 0;

			int channelNo = 0;
			foreach (FreeSCADA.Interfaces.IChannel ch in plugin.Channels)
			{
				ch.Tag = channelNo;
				Assert.AreEqual(channelNo, ch.Tag);
				channelNo++;
			}

			foreach (FreeSCADA.Interfaces.IChannel ch in plugin.Channels)
			{
				ch.ValueChanged += new System.EventHandler(OnChannelValueChanged);
			}

			plugin.Connect();
			System.Threading.Thread.Sleep(500);
			foreach (FreeSCADA.Interfaces.IChannel ch in plugin.Channels)
				Assert.IsNotNull(ch.Value);

			for (int i = 0; i < 3; i++)
				Assert.Greater(channelchangedNotification[i], 0);

			plugin.Disconnect();
		}

		void OnChannelValueChanged(object sender, System.EventArgs e)
		{
			FreeSCADA.Interfaces.IChannel ch = (FreeSCADA.Interfaces.IChannel)sender;
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

		[Test]
		public void LoadSaveChannels()
		{
			ExpectModal("SettingsForm", "CreateTestChannels");

			//Plugin should save its channels in the project
			ICommandContext context = Env.Current.Commands.GetPredefinedContext(PredefinedContexts.Communication);
			Env.Current.Commands.FindCommandByName(context, StringConstants.PropertyCommandName).Execute();

			Assert.AreEqual(ChannelTypes.Length, plugin.Channels.Length);

			//Plugin should load its channels from the project
			plugin = new Plugin();
			plugin.Initialize(Env.Current);

			Assert.AreEqual(ChannelTypes.Length, plugin.Channels.Length);
			for(int i=0;i<plugin.Channels.Length;i++)
			{
				FreeSCADA.Interfaces.IChannel ch = plugin.Channels[i];
				Assert.IsNotNull(ch);
				Assert.AreEqual(string.Format("variable_{0}", i + 1), ch.Name);
			}
		}

		private void CreateTestChannels()
		{
			GridTester grid = new GridTester("grid", "SettingsForm");
			ButtonTester addButton = new ButtonTester("addButton", "SettingsForm");
			ButtonTester okButton = new ButtonTester("okButton", "SettingsForm");

			for (int i = 0; i < ChannelTypes.Length; i++)
			{
				addButton.Click();
				grid.SetChannelType(i, ChannelTypes[i]);
				grid.SetChannelName(i, string.Format("variable_{0}", i + 1));
			}

			okButton.Click();
		}

		[Test]
		public void LoadSaveFileChannels()
		{
			ExpectModal("SettingsForm", "CreateTestChannels");

			//Plugin should save its channels in the project
			ICommandContext context = Env.Current.Commands.GetPredefinedContext(PredefinedContexts.Communication);
			Env.Current.Commands.FindCommandByName(context, StringConstants.PropertyCommandName).Execute();

			Assert.AreEqual(ChannelTypes.Length, plugin.Channels.Length);
			Env.Current.Project.Save(projectFile);

			//Plugin should load its channels from the project on its loading
			Env.Deinitialize();
			System.Windows.Forms.MenuStrip menu = new System.Windows.Forms.MenuStrip();
			Env.Initialize(null, menu, null, FreeSCADA.Interfaces.EnvironmentMode.Designer);
			plugin = (Plugin)Env.Current.CommunicationPlugins["data_simulator_plug"];
			Env.Current.Project.Load(projectFile); 

			Assert.AreEqual(ChannelTypes.Length, plugin.Channels.Length);
			for (int i = 0; i < plugin.Channels.Length; i++)
			{
				FreeSCADA.Interfaces.IChannel ch = plugin.Channels[i];
				Assert.IsNotNull(ch);
				Assert.AreEqual(string.Format("variable_{0}", i + 1), ch.Name);
			}
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

		public void SetChannelName(int channelNo, string name)
		{
			Control[channelNo + 1, 0].Value = name;
		}
	}
}
