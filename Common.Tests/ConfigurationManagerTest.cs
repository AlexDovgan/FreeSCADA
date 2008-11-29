using NUnit.Framework;

namespace FreeSCADA.Common.Tests
{
	[TestFixture]
	public class ConfigurationManagerTest
	{
		[Test]
		public void ConfigPaths()
		{
			Assert.IsNotEmpty(ConfigurationManager.UserConfigFolder);
			Assert.IsNotEmpty(ConfigurationManager.DefaultConfigFolder);
		}

		[Test]
		public void ConfigFile()
		{
			Assert.IsEmpty(ConfigurationManager.GetConfigFile("test")); //File not exist. should be empty

			Assert.IsNotEmpty(ConfigurationManager.GetDefaulConfigFile("test"));
			Assert.IsNotEmpty(ConfigurationManager.GetUserConfigFile("test"));
		}
	}
}
