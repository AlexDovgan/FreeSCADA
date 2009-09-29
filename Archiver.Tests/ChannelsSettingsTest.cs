using System.Collections.Generic;
using System.Windows.Forms;
using FreeSCADA.Common;
using FreeSCADA.Communication.SimulatorPlug;
using FreeSCADA.Interfaces;
using NUnit.Extensions.Forms;
using NUnit.Framework;


namespace FreeSCADA.Archiver.Tests
{
	[TestFixture]
	public class ChannelsSettingsTest : NUnitFormTest
	{
		public override void Setup()
		{
			Env.Initialize(new Control(), new MenuStrip(), new ToolStrip(), FreeSCADA.Interfaces.EnvironmentMode.Designer);
			ArchiverMain.Initialize();
		}
		public override void TearDown()
		{
			ArchiverMain.Deinitialize();
			Env.Deinitialize();
		}

		[Test]
		public void GetChannelList()
		{
			ArchiverMain archiver = ArchiverMain.Current;

			CreateFewChannels();

			List<ChannelInfo> channels = archiver.ChannelsSettings.Channels;
			Assert.IsNotEmpty(channels);
		}

		[Test]
		public void SaveLoadRules()
		{
			ArchiverMain archiver = ArchiverMain.Current;

			CreateFewChannels();

			Assert.IsEmpty(archiver.ChannelsSettings.Rules);

			Rule newRule = new Rule();
			newRule.Name = "Test rule";
			newRule.AddChannel(archiver.ChannelsSettings.Channels[0]);
			newRule.AddChannel(archiver.ChannelsSettings.Channels[1]);
			newRule.AddCondition(new TimeIntervalCondition(12345));

			archiver.ChannelsSettings.AddRule(newRule);

			Assert.IsNotEmpty(archiver.ChannelsSettings.Rules);

			archiver.ChannelsSettings.Save();

			ArchiverMain.Deinitialize();
			ArchiverMain.Initialize();

			archiver = ArchiverMain.Current;
			archiver.ChannelsSettings.Rules = new List<Rule>();
			archiver.ChannelsSettings.Load();
			Assert.IsNotEmpty(archiver.ChannelsSettings.Rules);

			Assert.AreEqual(newRule.Name, archiver.ChannelsSettings.Rules[0].Name);
			Assert.AreEqual(newRule.Channels[0].ChannelName, archiver.ChannelsSettings.Rules[0].Channels[0].ChannelName);
			Assert.AreEqual(newRule.Channels[1].ChannelName, archiver.ChannelsSettings.Rules[0].Channels[1].ChannelName);
			Assert.AreEqual((newRule.Conditions[0] as TimeIntervalCondition).Interval, (archiver.ChannelsSettings.Rules[0].Conditions[0] as TimeIntervalCondition).Interval);
		}


		void CreateFewChannels()
		{
			ExpectModal("SettingsForm", "CreateRandomChannels");

			ICommandContext context = Env.Current.Commands.GetPredefinedContext(PredefinedContexts.Communication);
			Env.Current.Commands.FindCommandByName(context, StringConstants.PropertyCommandName).Execute();
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
}
