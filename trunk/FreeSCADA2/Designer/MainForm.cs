using System;
using System.Diagnostics;
using System.Windows.Forms;
using FreeSCADA.Archiver;
using FreeSCADA.Common;
using FreeSCADA.Designer.Dialogs;

namespace FreeSCADA.Designer
{
    /// <summary>
	/// Application main window
	/// </summary>
	public partial class MainForm : Form
	{
        WindowManager windowManager;

		/// <summary>
		/// Constructor
		/// </summary>
		public MainForm()
		{
			InitializeComponent();
			Env.Initialize(this, mainMenu, mainToolbar, FreeSCADA.Interfaces.EnvironmentMode.Designer);
			ArchiverMain.Initialize();

			CommandManager.fileContext = new BaseCommandContext(fileToolStripMenuItem.DropDown, mainToolbar);
			CommandManager.viewContext = new BaseCommandContext(viewSubMenu.DropDown, mainToolbar);
			CommandManager.documentContext = new BaseCommandContext(editSubMenu.DropDown, mainToolbar);
			
			windowManager = new WindowManager(dockPanel);
			
			UpdateCaptionAndCommands();
		}

		private void OnMenuVariables(object sender, System.EventArgs e)
		{
			VariablesDialog frm = new VariablesDialog();
			frm.ShowDialog(this);
		}

		private void OnMenuExitClick(object sender, System.EventArgs e)
		{
			Close();
		}

		private void OnSchemaItemClick(object sender, System.EventArgs e)
		{
			windowManager.CreateNewSchema();
		}

		private void OnEventsItemClick(object sender, System.EventArgs e)
		{
			windowManager.ShowEvents();
		}

		private void OnArchiverSettingsClick(object sender, EventArgs e)
		{
			windowManager.ShowArhiverSettings();
		}

		private void OnSaveProjectClick(object sender, System.EventArgs e)
		{
			windowManager.SaveProject();
			UpdateCaptionAndCommands();
		}

		private void OnLoadProjectClick(object sender, System.EventArgs e)
		{
			windowManager.LoadProject();
			UpdateCaptionAndCommands();
		}

		private void OnSaveFileClick(object sender, System.EventArgs e)
		{
			//windowManager.SaveDocument();
            windowManager.SaveProject();
            UpdateCaptionAndCommands();
        }

		private void OnFormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = !windowManager.Close();
		}

		private void OnFormClosed(object sender, FormClosedEventArgs e)
		{
			Env.Deinitialize();
		}

		void UpdateCaptionAndCommands()
		{
			if (Env.Current.Project.FileName == "")
				Text = StringResources.MainWindowName;
			else
				Text = string.Format(StringResources.MainWindowNameEx, Env.Current.Project.FileName);

			if (Env.Current.Project.FileName == "")
				runButton.Enabled = false;
			else
				runButton.Enabled = !Env.Current.Project.IsModified;
		}

		private void OnNewProjectClick(object sender, System.EventArgs e)
		{
			if (windowManager.Close())
			{
				windowManager.ForceWindowsClose();
				Env.Current.CreateNewProject();
				windowManager = new WindowManager(dockPanel);
				Env.Current.CreateNewProject();
                UpdateCaptionAndCommands();
				System.GC.Collect();
			}
		}

        private void importSchemaToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            windowManager.ImportSchema();
        }

        private void runButton_Click(object sender, EventArgs e)
        {

            ProcessStartInfo psi = new ProcessStartInfo(Application.StartupPath + @"\\RunTime.exe");
            psi.Arguments = "\""+Env.Current.Project.FileName+"\"";
            Process.Start(psi);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.FileName = Env.Current.Project.FileName;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Env.Current.Project.SaveAsFileName = sfd.FileName;
                windowManager.SaveProject();
                UpdateCaptionAndCommands();
            }
        }
    }
}
