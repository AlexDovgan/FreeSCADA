using System;
using FreeSCADA.Communication.SimulatorPlug;
using NUnit.Framework;

namespace Communication.SimulatorPlug.Tests
{
	[TestFixture]
	public class PluginTest
	{
		Plugin plugin;
		EnvironmentMock environment;

		[SetUp]
		public void Init()
		{
			environment = new EnvironmentMock();
			plugin = new Plugin();
			plugin.Initialize(environment);
		}
		[TearDown]
		public void DeInit()
		{
			plugin = null;
			environment = null;
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
			Assert.AreEqual(environment, plugin.Environment);
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
