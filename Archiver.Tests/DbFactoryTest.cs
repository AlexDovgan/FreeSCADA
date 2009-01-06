using System.Data;
using System.Data.Common;
using NUnit.Framework;

namespace FreeSCADA.Archiver.Tests
{
	[TestFixture]
	public class DbFactoryTest
	{
		[SetUp]
		public void Init()
		{
		}
		[TearDown]
		public void DeInit()
		{
		}

		[Test]
		public void ProvidersList()
		{
			DataTable dt = DatabaseFactory.GetAvailableDB();
			Assert.IsNotNull(dt);
			Assert.IsNotEmpty(dt.Rows);

			DataRow[] rows = dt.Select("InvariantName = 'System.Data.SQLite'");
			Assert.IsNotEmpty(rows);
		}

		[Test]
		public void CreateProvider()
		{
			DataTable dt = DatabaseFactory.GetAvailableDB();
			foreach (DataRow row in dt.Rows)
			{
				DbProviderFactory factory = DatabaseFactory.Get(row["InvariantName"].ToString());
				Assert.IsNotNull(dt);
			}
		}
	}
}
