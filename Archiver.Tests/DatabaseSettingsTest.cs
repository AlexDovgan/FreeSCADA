using System.Windows.Forms;
using FreeSCADA.Common;
using NUnit.Framework;

namespace FreeSCADA.Archiver.Tests
{
	[TestFixture]
	public class DatabaseSettingsTest
	{
		[SetUp]
		public void Init()
		{
			Env.Initialize(new Control(), new Commands(new MenuStrip(), null), FreeSCADA.Interfaces.EnvironmentMode.Designer);
		}
		[TearDown]
		public void DeInit()
		{
			Env.Deinitialize();
		}

		[Test]
		public void SaveLoad()
		{
			DatabaseSettings settings1 = new DatabaseSettings();
			
			settings1.DbCatalog = "dbCatalog";
			settings1.DbFile = "dbFile";
			settings1.DbPassword = "dbPassword";
			settings1.DbProvider = "dbProvider";
			settings1.DbSource = "dbSource";
			settings1.DbUser = "dbUser";

			settings1.Save();

			DatabaseSettings settings2 = new DatabaseSettings();
			settings2.Load();

			Assert.AreEqual(settings1.DbCatalog, settings2.DbCatalog);
			Assert.AreEqual(settings1.DbFile, settings2.DbFile);
			Assert.AreEqual(settings1.DbPassword, settings2.DbPassword);
			Assert.AreEqual(settings1.DbProvider, settings2.DbProvider);
			Assert.AreEqual(settings1.DbSource, settings2.DbSource);
			Assert.AreEqual(settings1.DbUser, settings2.DbUser);
		}
	}
}
