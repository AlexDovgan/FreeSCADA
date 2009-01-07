using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Reflection;


namespace FreeSCADA.Archiver
{
	public static class DatabaseFactory
	{
		public const string SQLiteName = "System.Data.SQLite";

		enum ProcessorType
		{
			x86,
			x64,
			IPF
		}

		public static DbProviderFactory Get(string invariantName)
		{
			if (invariantName == SQLiteName)
			{
				string sqlightAssembly = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SQLite");
				switch (GetProcessorArch())
				{
					case ProcessorType.x86:
						sqlightAssembly = Path.Combine(sqlightAssembly, "x32");
						break;
					case ProcessorType.x64:
						sqlightAssembly = Path.Combine(sqlightAssembly, "x64");
						break;
					case ProcessorType.IPF:
						sqlightAssembly = Path.Combine(sqlightAssembly, "Itanium");
						break;
				}
				sqlightAssembly = Path.Combine(sqlightAssembly, "system.data.sqlite.dll");

				Assembly lib = Assembly.LoadFrom(sqlightAssembly);
				foreach (Type t in lib.GetExportedTypes())
				{
					if(t.FullName == "System.Data.SQLite.SQLiteFactory")
						return (DbProviderFactory)Activator.CreateInstance(t);
				}

				return null;
			}
			else
			{
				DataTable dt = GetAvailableDB();
				DataRow[] rows = dt.Select(string.Format("InvariantName = '{0}'", invariantName));

				if (rows.Length == 0)
					return null;

				return DbProviderFactories.GetFactory(rows[0]);
			}
		}

		public static DataTable GetAvailableDB()
		{
			DataTable dt = DbProviderFactories.GetFactoryClasses();
			dt.Rows.Add(new object[] { "SQLite Data Provider", ".Net Framework Data Provider for SQLite", SQLiteName, "System.Data.SQLite.SQLiteFactory, System.Data.SQLite" });
			return dt;
		}

		private static ProcessorType GetProcessorArch()
		{
			using (System.Management.ManagementClass processors = new System.Management.ManagementClass("Win32_Processor"))
			{
				foreach (System.Management.ManagementObject processor in processors.GetInstances())
				{
					int AddressWidth = int.Parse(processor["AddressWidth"].ToString());
					int Architecture = int.Parse(processor["Architecture"].ToString());

					//See Win32_Processor Class for details
					if (AddressWidth == 32)
						return ProcessorType.x86;
					if (AddressWidth == 64 && Architecture == 9)
						return ProcessorType.x64;
					if (AddressWidth == 64 && Architecture == 6)
						return ProcessorType.IPF;
				}
			}

			throw new NotSupportedException();
		}
	}
}
