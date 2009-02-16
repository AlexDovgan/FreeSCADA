using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;

namespace FreeSCADA.Common
{
    public enum ProjectEntityType
    {
        Schema,
        Channel,
        Image,
        Script,
        Trend,
        Report,
        EventList,
        AlarmList,
        VariableList,
        Archiver
    }

	public class Project
	{
		public const int CurrentVersion = 200;

		Dictionary<string, byte[]> data = new Dictionary<string, byte[]>();
		bool modifiedFlag = false;

		public event EventHandler ProjectLoaded;
		public event EventHandler ProjectClosed;

        string fileName = "";

		internal Project()
		{
		}

		public bool IsModified
		{
			get { return modifiedFlag; }
		}

        public string FileName
        {
            get { return fileName; }
        }

        public string SaveAsFileName
        {
            set { fileName = value; }
        }

		public int Version
		{
			get
			{
				using (System.IO.Stream ms = Env.Current.Project["version.info"])
				{
					if (ms == null || ms.Length == 0)
						return CurrentVersion;

					XmlSerializer serializer = new XmlSerializer(typeof(int));
					return (int)serializer.Deserialize(ms);
				}
			}
			internal set
			{
				using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
				{
					XmlSerializer serializer = new XmlSerializer(typeof(int));
					serializer.Serialize(ms, CurrentVersion);

					Env.Current.Project.SetData("version.info", ms);
				}
			}
		}

   		public void Load(string fileName)
        {
			Clear();

			using (FileStream zipFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
			using (ZipInputStream zipInput = new ZipInputStream(zipFileStream))
			{
				ZipEntry entry;
				byte[] tmp_buff = new byte[2048];
				while ((entry = zipInput.GetNextEntry()) != null)
				{
					using (MemoryStream ms = (entry.Size > 0) ? new MemoryStream((int)entry.Size) : new MemoryStream())
					{
						ICSharpCode.SharpZipLib.Core.StreamUtils.Copy(zipInput, ms, tmp_buff);
						ms.Flush();
						data.Add(entry.Name, ms.ToArray());
					}
				}
			}
            this.fileName = fileName;
			FireProjectLoaded();
		}

		internal void Clear()
		{
			if (ProjectClosed != null)
				ProjectClosed(this, new EventArgs());

			data.Clear();
			System.GC.Collect();
			modifiedFlag = false;
			fileName = "";

			FireProjectLoaded();
		}

		public void Save(string fileName)
		{
			if (System.IO.File.Exists(fileName))
				System.IO.File.Delete(fileName);

			Version = CurrentVersion; //Save version info

			using (FileStream zipFileStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
			using (ZipOutputStream zipOutput = new ZipOutputStream(zipFileStream))
			{
				zipOutput.SetLevel(9);
				Crc32 crc = new Crc32();
				foreach (KeyValuePair<string, byte[]> pair in data)
				{
					ZipEntry entry = new ZipEntry(pair.Key);
					entry.DateTime = System.DateTime.Now;
					entry.Size = pair.Value.LongLength;

					crc.Reset();
					crc.Update(pair.Value);

					entry.Crc = crc.Value;
					zipOutput.PutNextEntry(entry);
					zipOutput.Write(pair.Value, 0, pair.Value.Length);
				}
				zipOutput.Finish();
				zipOutput.Flush();
				zipOutput.Close();
			}
			modifiedFlag = false;
            this.fileName = fileName;
		}

		/// <summary>
		/// Return given entity from the project
		/// </summary>
		/// <param name="type">Entity type</param>
		/// <param name="name">Entity name</param>
		public void RemoveEntity(ProjectEntityType type, string name)
		{
			if(data.ContainsKey(GetFullEntityName(type,name)))
			{
				data.Remove(GetFullEntityName(type,name));
				modifiedFlag = true;
			}
		}
		/// <summary>
		/// Return read only stream for specified entity
		/// </summary>
		/// <param name="name">Entity name</param>
		/// <returns>Return Stream instance or null if there is no entity</returns>
		public Stream GetData(string name)
		{
			if (!data.ContainsKey(name))
				return null;

			return new MemoryStream(data[name], false);
		}

		/// <summary>
		/// Return read only stream for specified entity
		/// </summary>
		/// <param name="type">Entity type</param>
		/// <param name="name">Entity name</param>
		/// <returns>Return Stream instance or null if there is no entity</returns>
		public Stream GetData(ProjectEntityType type, string name)
		{
			return GetData(GetFullEntityName(type, name));
		}

		/// <summary>
		/// Writes a tmp_buff into the project. Automatically sets IsModified property.
		/// </summary>
		/// <param name="name">Entity name</param>
		/// <param name="data_block">Data block for saving</param>
		public void SetData(string name, Stream data_block)
		{
			data_block.Flush();
			data_block.Seek(0, SeekOrigin.Begin);
			byte[] bytes = new byte[data_block.Length];
			data_block.Read(bytes, 0, (int)data_block.Length);

			data[name] = bytes;
			modifiedFlag = true;
		}

		/// <summary>
		/// Writes a tmp_buff into the project. Automatically sets IsModified property.
		/// </summary>
		/// <param name="type">Entity type</param>
		/// <param name="name">Entity name</param>
		/// <param name="data_block">Data block for saving</param>
		public void SetData(ProjectEntityType type, string name, Stream data_block)
		{
			SetData(GetFullEntityName(type,name), data_block);
		}

		/// <summary>
		/// Returns full internal name of given entity
		/// </summary>
		public string GetFullEntityName(ProjectEntityType type, string name)
		{
			return Path.Combine(GetEntityTypeInternalName(type), name);
		}

		/// <summary>
		/// Returns internal name of given entity type
		/// </summary>
		public string GetEntityTypeInternalName(ProjectEntityType type)
		{
			switch (type)
			{
				case ProjectEntityType.Image: return "images";
				default:
					throw new NotImplementedException();
			}
		}

		/// <summary>
		/// Return all available entities
		/// </summary>
		/// <returns>return array of entities</returns>
		public string[] GetEntities()
		{
			string[] entities = new string[data.Keys.Count];
			data.Keys.CopyTo(entities, 0);
			return entities;
		}

		/// <summary>
		/// Return all entities of given type
		/// </summary>
		/// <param name="type">Entity type</param>
		/// <returns>return array of entities</returns>
		public string[] GetEntities(ProjectEntityType type)
		{
			if (type == ProjectEntityType.Schema)
				return GetSchemas();
			else
			{
				List<string> entities = new List<string>();
				foreach (string key in data.Keys)
				{
					if (key.StartsWith(GetEntityTypeInternalName(type)))
					{
						string tmp = key.Remove(0, GetEntityTypeInternalName(type).Length + 1);
						entities.Add(tmp);
					}
				}
				return entities.ToArray();
			}
		}

		/// <summary>
		/// Checks if the project contains given entity
		/// </summary>
		/// <param name="type">Entity type</param>
		/// <param name="name">Entity name</param>
		/// <returns>return array of entities</returns>
		public bool ContainsEntity(ProjectEntityType type, string name)
		{
			if(type == ProjectEntityType.Schema)
				return System.Array.IndexOf(GetEntities(type), name) >= 0;
			else
				return data.ContainsKey(GetFullEntityName(type, name));
		}

		string[] GetSchemas()
		{
			List<string> schemas = new List<string>();
			foreach (string entity in data.Keys)
			{
				Regex rx = new Regex(@"^schemas[\/]+(?<name>.*)[\/]+.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
				Match match = rx.Match(entity);
				if (match.Success)
				{
					if (schemas.IndexOf(match.Groups["name"].Value) < 0)
						schemas.Add(match.Groups["name"].Value);
				}
			}
			return schemas.ToArray();
		}

		public Stream this[string name]
		{
			get{ return GetData(name); }
			set { SetData(name, value); }
		}

		private void FireProjectLoaded()
		{
			if (ProjectLoaded != null)
				ProjectLoaded(this, new System.EventArgs());
		}
	}
}
