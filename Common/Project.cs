using System.IO;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

namespace FreeSCADA.Common
{
	public class Project
	{
		Dictionary<string, MemoryStream> streams = new Dictionary<string,MemoryStream>();

		public event System.EventHandler LoadEvent;

		~Project()
		{
			Clear();
		}

		public void Load(string fileName)
		{
			Clear();

			using (FileStream zipFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
			using (ZipInputStream zipInput = new ZipInputStream(zipFileStream))
			{
				ZipEntry entry;
				byte[] data = new byte[2048];
				while ((entry = zipInput.GetNextEntry()) != null)
				{
					MemoryStream ms;
					if (entry.Size > 0)
						ms = new MemoryStream((int)entry.Size);
					else
						ms = new MemoryStream();
					ICSharpCode.SharpZipLib.Core.StreamUtils.Copy(zipInput, ms, data);
					streams.Add(entry.Name, ms);
				}
			}
			if (LoadEvent != null)
				LoadEvent(this, new System.EventArgs());
		}

		private void Clear()
		{
			foreach (KeyValuePair<string, MemoryStream> pair in streams)
			{
				pair.Value.Close();
				pair.Value.Dispose();
			}
			streams.Clear();
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
				foreach (KeyValuePair<string, MemoryStream> pair in streams)
				{
					ZipEntry entry = new ZipEntry(pair.Key);
					entry.DateTime = System.DateTime.Now;
					entry.Size = pair.Value.Length;

					crc.Reset();
					crc.Update(pair.Value.ToArray());

					entry.Crc = crc.Value;
					zipOutput.PutNextEntry(entry);
					zipOutput.Write(pair.Value.ToArray(), 0, (int)pair.Value.Length);
				}
				zipOutput.Finish();
				zipOutput.Flush();
				zipOutput.Close();
			}
		}

		public MemoryStream this[string name]
		{
			get
			{
				name = name.ToLower();
				if (!streams.ContainsKey(name))
					streams.Add(name, new MemoryStream());

				streams[name].Flush();
				streams[name].Seek(0, SeekOrigin.Begin);
				return streams[name];
			}
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
