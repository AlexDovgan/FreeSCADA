using System.Windows.Forms;
using NUnit.Framework;

namespace FreeSCADA.Common.Tests
{
	[TestFixture]
	public class CommunationPlugsTest
	{
		MenuStrip menu;

		[SetUp]
		public void Init()
		{
			menu = new MenuStrip();
			Env.Initialize(new Control(), menu, null, FreeSCADA.Interfaces.EnvironmentMode.Designer);
		}
		[TearDown]
		public void DeInit()
		{
			Env.Deinitialize();
			menu = null;

			System.GC.Collect();
		}

		[Test]
		public void Load()
		{
			Assert.IsNotNull(Env.Current);
			Assert.IsNotEmpty(Env.Current.CommunicationPlugins.PluginIds);
		}

		[Test]
		public void Indexer()
		{
			foreach (string plug in Env.Current.CommunicationPlugins.PluginIds)
			{
				Assert.IsNotEmpty(plug);
				Assert.IsNotNull(Env.Current.CommunicationPlugins[plug]);
			}
		}

		[Test]
		public void Connection()
		{
			CommunationPlugs plugs = Env.Current.CommunicationPlugins;
			Assert.IsFalse(plugs.IsConnected);
			Assert.IsTrue(plugs.Connect());
			Assert.IsTrue(plugs.IsConnected);
			plugs.Disconnect();
			Assert.IsFalse(plugs.IsConnected);
		}
	}
}
