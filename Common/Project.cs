using System.IO;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

namespace FreeSCADA.Common
{
	public class Project
	{
		Dictionary<string, byte[]> data = new Dictionary<string, byte[]>();
		bool modifiedFlag = false;

		public event System.EventHandler LoadEvent;

		~Project()
		{
			Clear();
		}

		public bool IsModified
		{
			get { return modifiedFlag; }
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
			if (LoadEvent != null)
				LoadEvent(this, new System.EventArgs());
		}

		private void Clear()
		{
			data.Clear();
			System.GC.Collect();
			modifiedFlag = false;
		}

		public void Save(string fileName)
		{
			if (System.IO.File.Exists(fileName))
				System.IO.File.Delete(fileName);

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
		}

		/// <summary>
		/// Return read only stream for specified entity
		/// </summary>
		/// <param name="name">Entity name</param>
		/// <returns>Return Stream instance or null if there is no entity</returns>
		public Stream GetData(string name)
		{
			name = name.ToLower();
			if (!data.ContainsKey(name))
				return null;

			return new MemoryStream(data[name], false);
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

			data[name.ToLower()] = bytes;
			modifiedFlag = true;
		}

		public string[] GetEntities()
		{
			string[] entities = new string[data.Keys.Count];
			data.Keys.CopyTo(entities, 0);
			return entities;
		}

		public Stream this[string name]
		{
			get{ return GetData(name); }
			set { SetData(name, value); }
		}

		public void SerializeContent(string name, object obj)
		{
			
		}

		public object DeserializeContent(string name)
		{
			return null;
		}
	}
}
