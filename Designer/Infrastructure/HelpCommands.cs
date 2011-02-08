using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using System.Windows.Forms;
using FreeSCADA.Common;
using FreeSCADA.CommonUI;

namespace FreeSCADA.Designer
{
	class CheckForUpdatesCommand : BaseCommand
	{
		public CheckForUpdatesCommand()
		{
			Priority = (int)CommandManager.Priorities.HelpCommands;
			CanExecute = true;
		}

		public override string Description
		{
			get
			{
				return StringResources.CommandCheckForUpdatesDescription;
			}
		}

		public override string Name
		{
			get
			{
				return StringResources.CommandCheckForUpdatesName;
			}
		}
		public override void Execute()
		{
			bool newVersionAvailable = false;
			string url = "";

			try
			{
				XmlDocument versionDescription = new XmlDocument();
				versionDescription.Load("http://www.free-scada.org/pad_description.xml");

				string[] webVersion = null;
				string[] localVersion = Env.Current.Version.Split(new char[] { '.' });
				foreach (XmlElement node in versionDescription.GetElementsByTagName("Program_Version"))
					webVersion = node.InnerText.Split(new char[] { '.' });

				foreach (XmlElement node in versionDescription.GetElementsByTagName("Primary_Download_URL"))
					url = node.InnerText;

				if (webVersion != null && localVersion != null)
				{
					int partsCount = Math.Min(webVersion.Length, localVersion.Length);
					for (int i = 0; i < partsCount; i++)
					{
						int web = int.Parse(webVersion[i], System.Globalization.CultureInfo.InvariantCulture);
						int local = int.Parse(localVersion[i], System.Globalization.CultureInfo.InvariantCulture);
						if (web > local)
						{
							newVersionAvailable = true;
							break;
						}
					}
				}
			}
			catch (Exception e)
			{
				string caption = StringResources.CommandCheckForUpdatesName;
				string text = String.Format(StringResources.UpdatesFailToCheckForNewVersion, e.Message);
				MessageBox.Show(Env.Current.MainWindow, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (newVersionAvailable)
			{
				string caption = StringResources.CommandCheckForUpdatesName;
				string text = StringResources.UpdatesNewVersionAvailable;
				DialogResult res = MessageBox.Show(Env.Current.MainWindow, text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
				if (res == DialogResult.Yes)
					System.Diagnostics.Process.Start(url);
			}
			else
			{
				string caption = StringResources.CommandCheckForUpdatesName;
				string text = StringResources.UpdatesHaveTheLatestVersion;
				MessageBox.Show(Env.Current.MainWindow, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}
	}
}
