using System;
using FreeSCADA.Common;
using FreeSCADA.Communication.SimulatorPlug;
using NUnit.Framework;

namespace Communication.SimulatorPlug.Tests
{
	[TestFixture]
	public class PluginTest
	{
		Plugin plugin;

		[SetUp]
		public void Init()
		{
			System.Windows.Forms.MenuStrip menu = new System.Windows.Forms.MenuStrip();
			Env.Initialize(null, new Commands(menu, null), FreeSCADA.Interfaces.EnvironmentMode.Designer);
			plugin = (Plugin)Env.Current.CommunicationPlugins["data_simulator_plug"];
		}
		[TearDown]
		public void DeInit()
		{
			plugin = null;
			Env.Deinitialize();
			GC.Collect();
		}

		[Test]
		public void PublicInfo()
		{
			Assert.AreEqual("Data Simulator", plugin.Name);
			Assert.AreEqual("data_simulator_plug", plugin.PluginId);
		}

		[Test]
		public void Miscellaneous()
		{
			Assert.AreEqual(Env.Current, plugin.Environment);
			Assert.IsNotNull(plugin.Channels);
			Assert.IsEmpty(plugin.Channels); //Should be empty because the project object is empty
			Assert.IsFalse(plugin.IsConnected); //Should be false initially
		}

		[Test]
		public void Connections()
		{
			Assert.IsFalse(plugin.IsConnected); //Should be false initially
			Assert.IsTrue(plugin.Connect());
			Assert.IsTrue(plugin.IsConnected);
			plugin.Disconnect();
			Assert.IsFalse(plugin.IsConnected);
		}
	}
}
