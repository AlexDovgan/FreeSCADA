using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using System.Reflection;

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
        Archiver,
		Settings
    }

	public class Project
	{
		public const int CurrentVersion = 201;

		Dictionary<string, byte[]> data = new Dictionary<string, byte[]>();
		bool modifiedFlag = false;

		public event EventHandler ProjectLoaded;
		public event EventHandler ProjectClosed;
		public event EventHandler EntitySetChanged;

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
						//Normalize path string
						string resultingName = entry.Name;
						resultingName = resultingName.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

						data.Add(resultingName, ms.ToArray());
					}
				}
			}
            this.fileName = fileName;
            if (Version != CurrentVersion)
                if (System.Windows.Forms.MessageBox.Show("Project version is difer from current version\n\rdo you whant try to convert?"
                    , "Caution!", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    ConvertProject();
                    if (Version != CurrentVersion)
                        System.Windows.Forms.MessageBox.Show("Project version stil difer from current version."
                    , "Conversion Result");
                }
			FireProjectLoaded();
		}

        void ConvertProject()
        {
            List<ProjectConvertor> convertors = new List<ProjectConvertor>();
            Assembly archiverAssembly = this.GetType().Assembly;
            foreach (Type type in archiverAssembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(ProjectConvertor)))
                     convertors.Add(Activator.CreateInstance(type) as ProjectConvertor);
            }
            if (CurrentVersion > Version)
                for (int i = 0; i < convertors.Count; i++)
                {
                    ProjectConvertor conv = convertors[i];
                    if (conv.AcceptedVersion == Version && conv.ResultVersion <= CurrentVersion)
                    {
                        conv.Convert(this);
                        convertors.RemoveAt(i);
                        i = 0;
                    }
                }
            else
                for (int i = 0; i < convertors.Count; i++)
                {
                    ProjectConvertor conv = convertors[i];
                    if (conv.ResultVersion == CurrentVersion && conv.AcceptedVersion>=Version)
                    {
                        conv.ConvertBack(this);
                        convertors.RemoveAt(i);
                        i = 0;
                    }
                }

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
		public bool RemoveEntity(ProjectEntityType type, string name)
		{
			if (ContainsEntity(type, name) == false)
				return false;

			List<string> keysToRemove = new List<string>();

			foreach (string entity in data.Keys)
			{
				string entityStartPath = Path.Combine(GetEntityTypeInternalName(type), name);
				if (entity.StartsWith(entityStartPath, StringComparison.InvariantCultureIgnoreCase))
					keysToRemove.Add(entity);
			}

			if (keysToRemove.Count == 0)
				return false;

			foreach (string key in keysToRemove)
				data.Remove(key);

			modifiedFlag = true;

			if (type == ProjectEntityType.Schema && ContainsEntity(ProjectEntityType.Script, name))
				RemoveEntity(ProjectEntityType.Script, name);

			if (EntitySetChanged != null)
				EntitySetChanged(this, new EventArgs());

			return true;
		}

		/// <summary>
		/// Return read only stream for specified entity
		/// </summary>
		/// <param name="name">Entity name</param>
		/// <returns>Return Stream instance or null if there is no entity</returns>
		public Stream GetData(string name)
		{
			//Normalize path string
			name = name.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

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
            name = name.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
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
				case ProjectEntityType.Script: return "scripts";
				case ProjectEntityType.Settings: return "settings";
				case ProjectEntityType.Schema: return "Schemas";
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

		/// <summary>
		/// Renames entities in the project
		/// </summary>
		/// <param name="type">Entity type</param>
		/// <param name="oldName">Old entity name</param>
		/// <param name="newName">New entity name</param>
		/// <returns>Returns true if successed</returns>
		public bool RenameEntity(ProjectEntityType type, string oldName, string newName)
		{
			if (ContainsEntity(type, oldName) == false)
				return false;
			if (ContainsEntity(type, newName) == true)
				return false;

			if (type == ProjectEntityType.Schema)
			{
				List<string> keysToRemove = new List<string>();
				Dictionary<string, byte[]> keysToAdd = new Dictionary<string, byte[]>();

				foreach (string entity in data.Keys)
				{
					Regex rx = new Regex(@"^schemas[\\/]+(?<name>.*)[\\/]+.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
					Match match = rx.Match(entity);
					if (match.Success)
					{
						if (match.Groups["name"].Value == oldName)
						{
							keysToRemove.Add(entity);
							keysToAdd.Add(entity.Replace(oldName, newName), data[entity]);
						}
					}
				}

				if (keysToRemove.Count != keysToAdd.Count || keysToAdd.Count == 0)
					return false;

				foreach (string key in keysToRemove)
					data.Remove(key);
				foreach (string key in keysToAdd.Keys)
					data[key] = keysToAdd[key];

				if (ContainsEntity(ProjectEntityType.Script, oldName))
					RenameEntity(ProjectEntityType.Script, oldName, newName);

				modifiedFlag = true;

				if (EntitySetChanged != null)
					EntitySetChanged(this, new EventArgs());
				return true;
			}
			else
			{
				List<string> keysToRemove = new List<string>();
				Dictionary<string, byte[]> keysToAdd = new Dictionary<string, byte[]>();

				foreach (string entity in data.Keys)
				{
					string entityStartPath = Path.Combine(GetEntityTypeInternalName(type), oldName);
					if (entity.StartsWith(entityStartPath))
					{
						keysToRemove.Add(entity);
						keysToAdd.Add(entity.Replace(oldName, newName), data[entity]);
					}
				}

				if (keysToRemove.Count != keysToAdd.Count || keysToAdd.Count == 0)
					return false;

				foreach (string key in keysToRemove)
					data.Remove(key);
				foreach (string key in keysToAdd.Keys)
					data[key] = keysToAdd[key];

				modifiedFlag = true;

				if (EntitySetChanged != null)
					EntitySetChanged(this, new EventArgs());

				return true;
			}
		}

		string[] GetSchemas()
		{
			List<string> schemas = new List<string>();
			foreach (string entity in data.Keys)
			{
				Regex rx = new Regex(@"^schemas[\\/]+(?<name>.*)[\\/]+.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
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

		public string GenerateUniqueName(ProjectEntityType type, string prefix)
		{
			int number = 1;
			string newName;

			do
			{
				newName = string.Format("{0}{1}", prefix, number);
				number++;
			}while(ContainsEntity(type, newName) == true);

			return newName;
		}
	}
}
