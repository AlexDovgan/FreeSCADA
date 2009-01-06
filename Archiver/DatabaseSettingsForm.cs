using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FreeSCADA.Archiver
{
	public partial class DatabaseSettingsForm : Form
	{
		List<Control> controlGroup1 = new List<Control>();
		List<Control> controlGroup2 = new List<Control>();
		List<Control> controlGroup3 = new List<Control>();
		List<String> dbProvidersInvariantNames = new List<string>();

		public DatabaseSettingsForm()
		{
			InitializeComponent();

			controlGroup1.Add(fileNameLabel);
			controlGroup1.Add(fileNameBox);
			controlGroup1.Add(browseFileButton);

			controlGroup2.Add(providerLabel1);
			controlGroup2.Add(providerCombo1);
			controlGroup2.Add(serverLabel);
			controlGroup2.Add(serverBox);
			controlGroup2.Add(dbNameLabel);
			controlGroup2.Add(dbNameBox);
			controlGroup2.Add(userLabel);
			controlGroup2.Add(userBox);
			controlGroup2.Add(passwordLabel);
			controlGroup2.Add(passwordBox);

			controlGroup3.Add(providerLabel2);
			controlGroup3.Add(providerCombo2);
			controlGroup3.Add(connectionStringLabel);
			controlGroup3.Add(connectionStringBox);

			System.Data.DataTable dt = DatabaseFactory.GetAvailableDB();
			foreach (System.Data.DataRow row in dt.Rows)
			{
				if (row[2].ToString() == DatabaseFactory.SQLiteName)
					continue;

				providerCombo1.Items.Add(string.Format("{0} ({1})", row[1], row[0]));
				providerCombo2.Items.Add(string.Format("{0} ({1})", row[1], row[0]));
				dbProvidersInvariantNames.Add(row[2].ToString());
			}
			if (providerCombo1.Items.Count > 0)
				providerCombo1.SelectedIndex = 0;
			if (providerCombo2.Items.Count > 0)
				providerCombo2.SelectedIndex = 0;

			LoadSettings();
			UpdateControlsAvailability();
		}

		private void OnOkClick(object sender, EventArgs e)
		{
			SaveSettings();
			Close();
		}

		private void OnCancelClick(object sender, EventArgs e)
		{
			Close();
		}

		void UpdateControlsAvailability()
		{
			if (dbType1.Checked)
			{
				UpdateGroup(controlGroup1, true);
				UpdateGroup(controlGroup2, false);
				UpdateGroup(controlGroup3, false);
			}
			else if (dbType2.Checked)
			{
				UpdateGroup(controlGroup1, false);
				UpdateGroup(controlGroup2, true);
				UpdateGroup(controlGroup3, false);
			}
			else if (dbType3.Checked)
			{
				UpdateGroup(controlGroup1, false);
				UpdateGroup(controlGroup2, false);
				UpdateGroup(controlGroup3, true);
			}
		}

		void UpdateGroup(List<Control> group, bool state)
		{
			foreach (Control ctrl in group)
				ctrl.Enabled = state;
		}

		private void OnDbTypeUpdated(object sender, EventArgs e)
		{
			UpdateControlsAvailability();
		}

		void SaveSettings()
		{
			DatabaseSettings settings = new DatabaseSettings();

			settings.DbFile = fileNameBox.Text;
			settings.DbSource = serverBox.Text;
			settings.DbCatalog = dbNameBox.Text;
			settings.DbUser = userBox.Text;
			settings.DbPassword = passwordBox.Text;

			if (dbType1.Checked == true)
				settings.DbProvider = DatabaseFactory.SQLiteName;
			else if (dbType2.Checked == true)
				settings.DbProvider = dbProvidersInvariantNames[providerCombo1.SelectedIndex];
			else
			{
				settings.DbProvider = dbProvidersInvariantNames[providerCombo2.SelectedIndex];
				settings.DbConnectionString = connectionStringBox.Text;
				settings.DbFile = "";
				settings.DbSource = "";
			}

			settings.Save();
		}

		void LoadSettings()
		{
			DatabaseSettings settings = new DatabaseSettings();
			settings.Load();

			if (settings.DbProvider == DatabaseFactory.SQLiteName)
			{
				dbType1.Checked = true;
				fileNameBox.Text = settings.DbFile;
			}
			else if (settings.DbFile.Length == 0 && settings.DbSource.Length == 0)
			{
				dbType3.Checked = true;
				connectionStringBox.Text = settings.DbConnectionString;
			}
			else
			{
				dbType2.Checked = true;
				serverBox.Text = settings.DbSource;
				dbNameBox.Text = settings.DbCatalog;
				userBox.Text = settings.DbUser;
				passwordBox.Text = settings.DbPassword;
			}

			int provider = dbProvidersInvariantNames.IndexOf(settings.DbProvider);
			if (provider >= 0)
			{
				providerCombo1.SelectedIndex = provider;
				providerCombo2.SelectedIndex = provider;
			}
		}
	}
}
