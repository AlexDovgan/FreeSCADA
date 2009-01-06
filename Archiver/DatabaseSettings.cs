using System;
using System.Xml.Serialization;
using FreeSCADA.Common;

namespace FreeSCADA.Archiver
{
	[Serializable]
	public class DatabaseSettings
	{
		string dbProvider = DatabaseFactory.SQLiteName;
		string dbFile = "";
		string dbSource = "";
		string dbCatalog = "";
		string dbUser = "";
		string dbPassword = "";
		string dbConnectionString = "";

		public string DbProvider
		{
			get { return dbProvider; }
			set { dbProvider = value; }
		}

		public string DbFile
		{
			get { return dbFile; }
			set { dbFile = value; }
		}

		public string DbSource
		{
			get { return dbSource; }
			set { dbSource = value; }
		}

		public string DbCatalog
		{
			get { return dbCatalog; }
			set { dbCatalog = value; }
		}

		public string DbUser
		{
			get { return dbUser; }
			set { dbUser = value; }
		}

		public string DbPassword
		{
			get { return dbPassword; }
			set { dbPassword = value; }
		}

		public string DbConnectionString
		{
			get { return dbConnectionString; }
			set { dbConnectionString = value; }
		}

		public void Load()
		{
			using (System.IO.Stream ms = Env.Current.Project["settings/archiver/database.cfg"])
			{
				if (ms == null || ms.Length == 0)
					return;

				XmlSerializer serializer = new XmlSerializer(typeof(DatabaseSettings));
				DatabaseSettings tmp = (DatabaseSettings)serializer.Deserialize(ms);

				dbProvider = tmp.dbProvider;
				dbFile = tmp.dbFile;
				dbSource = tmp.dbSource;
				dbCatalog = tmp.dbCatalog;
				dbUser = tmp.dbUser;
				dbPassword = tmp.dbPassword;
				dbConnectionString = tmp.dbConnectionString;
			}
		}

		public void Save()
		{
			using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
			{
				XmlSerializer serializer = new XmlSerializer(typeof(DatabaseSettings));
				serializer.Serialize(ms, this);

				Env.Current.Project.SetData("settings/archiver/database.cfg", ms);
			}
		}
	}
}
