using System.IO;
using System.Windows.Forms;
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
			Env.Initialize(new Control(), new MenuStrip(), null, FreeSCADA.Interfaces.EnvironmentMode.Designer);

			projectFile = System.IO.Path.GetTempFileName();

			if (System.IO.File.Exists(projectFile))
				System.IO.File.Delete(projectFile);
		}
		[TearDown]
		public void DeInit()
		{
			Env.Deinitialize();
			System.IO.File.Delete(projectFile);
		}

		[Test]
		public void StreamContent()
		{
			Project p = Env.Current.Project;
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
			Project p = Env.Current.Project;
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
			Assert.AreEqual(projectFile, p.FileName);
			Assert.IsTrue(System.IO.File.Exists(projectFile));
			Assert.IsFalse(p.IsModified);

			Env.Deinitialize();
			Env.Initialize(new Control(), new MenuStrip(), null, FreeSCADA.Interfaces.EnvironmentMode.Designer);
			p = Env.Current.Project;

			bool loadEventCalled = false;
			p.ProjectLoaded += new System.EventHandler(new System.EventHandler( delegate(object obj, System.EventArgs args)
																					{ loadEventCalled = true; Assert.AreSame(p, obj); }));
			p.Load(projectFile);
			Assert.IsTrue(loadEventCalled);
			Assert.AreEqual(projectFile, p.FileName);
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

			Project p = Env.Current.Project;

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

			Env.Deinitialize();
			Env.Initialize(new Control(), new MenuStrip(), null, FreeSCADA.Interfaces.EnvironmentMode.Designer);
			p = Env.Current.Project;
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

		[Test]
		public void Entries()
		{
			Project p = Env.Current.Project;
			string[] test_entries = {	"file 1",
										"file 2",
										"dir 1/file 3",
										"dir 2\\file 4",
										"Schemas/Schema 1/xaml",
										"Schemas/Schema 1/actions",
										"Schemas/Schema 1/triggers",
										"Schemas/Schema 2/xaml",
										"Schemas/Schema 2/actions",
										"Schemas/Schema 2/triggers"
									};
			for (int i = 0; i < test_entries.Length; i++)
			{
				using (MemoryStream stream = new MemoryStream())
				using (StreamWriter writer = new StreamWriter(stream))
				{
					p[test_entries[i]] = stream;
				}
			}

			for (int i = 0; i < test_entries.Length; i++)
				Assert.Contains(test_entries[i], p.GetEntities());

			Assert.AreEqual(2, p.GetSchemas().Length);
			Assert.Contains("Schema 1", p.GetSchemas());
			Assert.Contains("Schema 2", p.GetSchemas());

			Assert.IsFalse(p.IsSchemaNameUnique("Schema 2"));
			Assert.IsTrue(p.IsSchemaNameUnique("Schema 3"));
		}
	}
}
