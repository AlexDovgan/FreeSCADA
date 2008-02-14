using System;
using System.Xml;

namespace FreeSCADA.Common
{
	public sealed class ConfigurationManager
	{
		static string productName = "FreeSCADA2";

		public static string GetConfigFile(string config)
		{
			string path = GetUserConfigFile(config);
			if (!System.IO.File.Exists(path))
			{
				path = GetDefaulConfigFile(config);
				if (!System.IO.File.Exists(path))
					return string.Empty;
			}
			return path;
		}

		public static string UserConfigFolder
		{
			get
			{
				string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
				path = System.IO.Path.Combine(path, productName);
				return path;
			}
		}

		public static string DefaultConfigFolder
		{
			get
			{
				string path = AppDomain.CurrentDomain.BaseDirectory;
				path = System.IO.Path.Combine(path, @"..\config");
				path = System.IO.Path.GetFullPath(path);
				return path;
			}
		}

		public static string GetUserConfigFile(string config)
		{
			config = System.IO.Path.ChangeExtension(config, ".xml");

			string path = System.IO.Path.Combine(UserConfigFolder, productName);
			path = System.IO.Path.Combine(path, config);
			return path;
		}

		public static string GetDefaulConfigFile(string config)
		{
			config = System.IO.Path.ChangeExtension(config, ".xml");

			string path = System.IO.Path.Combine(DefaultConfigFolder, productName);
			path = System.IO.Path.Combine(path, config);
			return path;
		}
	}
}
