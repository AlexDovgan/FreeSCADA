using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace FreeSCADA.Common
{
	/// <summary>
	/// This class is to support Most Frequently Used (MRU) lists in a menu
	/// </summary>
	public class MRUManager:IDisposable
	{
		const int Size = 4;

		ToolStripItem firstPosition;
		ToolStripItem lastPosition;

		public delegate void ItemClickedDelegate(object sender, string file);
		public event ItemClickedDelegate ItemClicked;

		public MRUManager(ToolStripItem firstPosition, ToolStripItem lastPosition)
		{
			this.firstPosition = firstPosition;
			this.lastPosition = lastPosition;

			firstPosition.Visible = false;
			lastPosition.Visible = false;

			List<string> strings = new List<string>();
			try
			{
				string cfgFile = ConfigurationManager.GetUserConfigFile("MRUlist");
				using (Stream stream = new FileStream(cfgFile, FileMode.Open, FileAccess.Read, FileShare.Read))
				using (TextReader reader = new StreamReader(stream))
				{
					XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
					strings = (List<string>)serializer.Deserialize(reader);
				}
			}
			catch
			{
			}
			CreateItems(strings);
		}

		public void Add(string file)
		{
			ToolStrip holder = firstPosition.Owner;
			List<string> strings = new List<string>();
			strings.Add(file);
			int first = -1;
			int last = -1;
			for (int i = 0; i < holder.Items.Count; i++)
			{
				if (holder.Items[i] == firstPosition)
					first = i;
				if (holder.Items[i] == lastPosition)
					last = i;
			}

			for (int i = first + 1; i < last; i++)
			{
				if (strings.Count < Size)
				{
					if(strings.Contains(holder.Items[i].Text) == false)
						strings.Add(holder.Items[i].Text);
				}
			}

			try
			{
				string cfgFile = ConfigurationManager.GetUserConfigFile("MRUlist");
				if (!Directory.Exists(Path.GetDirectoryName(cfgFile)))
					Directory.CreateDirectory(Path.GetDirectoryName(cfgFile));

				using (Stream stream = new FileStream(cfgFile, FileMode.Create, FileAccess.Write, FileShare.Read))
				using (TextWriter writer = new StreamWriter(stream))
				{
					XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
					serializer.Serialize(writer, strings);
				}
			}
			catch(Exception e)
			{
				Env.Current.Logger.LogError(string.Format("Cannot save MRU list ({0})", e.Message));
			}

			CreateItems(strings);
		}

		void CreateItems(List<string> strings)
		{
			ToolStrip holder = firstPosition.Owner;
			int first = -1;
			int last = -1;
			for(int i=0;i<holder.Items.Count;i++)
			{
				if (holder.Items[i] == firstPosition)
					first = i;
				if (holder.Items[i] == lastPosition)
					last = i;
			}

			List<ToolStripItem> removalList = new List<ToolStripItem>();
			for (int i = first + 1; i < last; i++)
				removalList.Add(holder.Items[i]);

			foreach(ToolStripItem item in removalList)
			{
				item.Click -= new EventHandler(OnItemClick);
				holder.Items.Remove(item);
			}

			int pos = first + 1;
			foreach (string str in strings)
			{
				ToolStripItem item = new ToolStripMenuItem();
				item.Text = str;
				item.Enabled = true;
				item.Click += new EventHandler(OnItemClick);

				holder.Items.Insert(pos, item);
				pos++;
			}

			lastPosition.Visible = (strings.Count > 0);
		}

		void OnItemClick(object sender, EventArgs e)
		{
			if (ItemClicked != null)
				ItemClicked(this, (sender as ToolStripItem).Text);
		}

		#region IDisposable Members

		public void Dispose()
		{
			CreateItems(new List<string>());
		}

		#endregion
	}
}
