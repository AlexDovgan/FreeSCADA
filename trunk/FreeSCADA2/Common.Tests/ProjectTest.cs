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
			Assert.IsNull(p.GetData("test1")); //There is no such entity

			using (MemoryStream stream = new MemoryStream())
			using (StreamWriter writer = new StreamWriter(stream))
			{
				writer.WriteLine("test 1234567890 test");
				writer.WriteLine("line 2");
				writer.Flush();
				p.SetData("test1", stream);
			}

			using (Stream stream = p.GetData("test1"))
			using (StreamReader reader = new StreamReader(stream))
			{
				Assert.AreEqual(reader.ReadLine(), "test 1234567890 test");
				Assert.AreEqual(reader.ReadLine(), "line 2");
			}
		}

		[Test]
		public void SaveLoad()
		{
			Project p = new Project();
			Assert.IsFalse(p.IsModified);

			using (MemoryStream stream = new MemoryStream())
			using (StreamWriter writer = new StreamWriter(stream))
			{
				writer.WriteLine("line 1");
				writer.WriteLine("line 2");
				writer.Flush();
				p.SetData("test1", stream);
			}

			Assert.IsTrue(p.IsModified);
			Assert.IsFalse(System.IO.File.Exists(projectFile));
			p.Save(projectFile);
			Assert.IsTrue(System.IO.File.Exists(projectFile));
			Assert.IsFalse(p.IsModified);

			p = new Project();
			p.Load(projectFile);
			using (Stream stream = p.GetData("test1"))
			using (StreamReader reader = new StreamReader(stream))
			{
				Assert.AreEqual(reader.ReadLine(), "line 1");
				Assert.AreEqual(reader.ReadLine(), "line 2");
			}

			//Test clearing method
			p.Load(projectFile);
			using (Stream stream = p.GetData("test1"))
			using (StreamReader reader = new StreamReader(stream))
			{
				Assert.AreEqual(reader.ReadLine(), "line 1");
				Assert.AreEqual(reader.ReadLine(), "line 2");
			}
		}

		[Test]
		public void SaveLoadBigDataSets()
		{
			const int files = 10;
			const int lines = 100000;

			Project p = new Project();

			for (int i = 0; i < files; i++)
			{
				using (MemoryStream stream = new MemoryStream())
				using (StreamWriter writer = new StreamWriter(stream))
				{
					for (int j = 0; j < lines; j++)
						writer.WriteLine(string.Format("Line {0}", j));
					writer.Flush();
					p.SetData(string.Format("file {0}", i), stream);
				}
			}
			p.Save(projectFile);

			p = new Project();
			p.Load(projectFile);
			for (int i = 0; i < files; i++)
			{
				using (Stream stream = p.GetData(string.Format("file {0}", i)))
				using (StreamReader reader = new StreamReader(stream))
				{
					for (int j = 0; j < lines; j++)
						Assert.AreEqual(reader.ReadLine(), string.Format("Line {0}", j));
				}
			}
		}
	}
}
