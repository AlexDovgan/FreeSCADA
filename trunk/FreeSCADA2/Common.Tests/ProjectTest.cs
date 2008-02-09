using System.IO;
using System.Text;
using NUnit.Framework;

namespace FreeSCADA.Common.Tests
{
	[TestFixture]
	public class ProjectTest
	{
		string projectFile;

		[SetUp]
		public void Init()
		{
			projectFile = System.IO.Path.GetTempFileName();

			if (System.IO.File.Exists(projectFile))
				System.IO.File.Delete(projectFile);
		}
		[TearDown]
		public void DeInit()
		{
			System.IO.File.Delete(projectFile);
		}

		[Test]
		public void StreamContent()
		{
			Project p = new Project();
			Assert.AreSame(p["test1"], p["test1"]);

			MemoryStream stream = p["test1"];
			Assert.IsNotNull(stream);
			Assert.IsTrue(stream.CanRead);
			Assert.IsTrue(stream.CanWrite);

			StreamWriter writer = new StreamWriter(stream);
			writer.WriteLine("test 1234567890 test");
			writer.WriteLine("line 2");
			writer.Flush();
			writer = null;

			stream = p["test1"];
			StreamReader reader = new StreamReader(stream);
			Assert.AreEqual(reader.ReadLine(), "test 1234567890 test");
			Assert.AreEqual(reader.ReadLine(), "line 2");
		}

		[Test]
		public void SaveLoad()
		{
			Project p = new Project();
			StreamWriter writer = new StreamWriter(p["test1"]);
			writer.WriteLine("line 1");
			writer.WriteLine("line 2");
			writer.Flush();
			writer = null;

			Assert.IsFalse(System.IO.File.Exists(projectFile));
			p.Save(projectFile);
			Assert.IsTrue(System.IO.File.Exists(projectFile));

			p = new Project();
			p.Load(projectFile);
			StreamReader reader = new StreamReader(p["test1"]);
			Assert.AreEqual(reader.ReadLine(), "line 1");
			Assert.AreEqual(reader.ReadLine(), "line 2");
			
			//Test clearing method
			p.Load(projectFile);
			reader = new StreamReader(p["test1"]);
			Assert.AreEqual(reader.ReadLine(), "line 1");
		}

		[Test]
		public void SaveLoadBigDataSets()
		{
			const int files = 10;
			const int lines = 100000;

			Project p = new Project();

			for (int i = 0; i < files; i++)
			{
				StreamWriter writer = new StreamWriter(p[string.Format("file {0}", i)]);
				for (int j = 0; j < lines; j++)
					writer.WriteLine(string.Format("Line {0}", j));
				writer.Flush();
				writer = null;
			}
			p.Save(projectFile);

			p = new Project();
			p.Load(projectFile);
			for (int i = 0; i < files; i++)
			{
				StreamReader reader = new StreamReader(p[string.Format("file {0}", i)]);
				for (int j = 0; j < lines; j++)
					Assert.AreEqual(reader.ReadLine(), string.Format("Line {0}", j));
			}
		}
	}
}
