using System;
using System.Diagnostics;
using System.Windows.Forms;
using FreeSCADA.Archiver;
using FreeSCADA.Common;
using FreeSCADA.CommonUI;
using FreeSCADA.Designer.Dialogs;
using FreeSCADA.Interfaces;

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
			Env.Initialize(this, new Common.Commands(mainMenu, mainToolbar),FreeSCADA.Interfaces.EnvironmentMode.Designer);
			ArchiverMain.Initialize();
            

            Env.Current.Commands.RegisterContext("FileContext", new MenuCommandContext(fileToolStripMenuItem.DropDown));
            Env.Current.Commands.RegisterContext("ViewContext",new MenuCommandContext(viewSubMenu.DropDown));
			Env.Current.Commands.RegisterContext("DocumentContext" ,new MenuCommandContext(editSubMenu.DropDown));
            

           	ToolStripMenuItem newItem = new ToolStripMenuItem(StringResources.CommandContextHelp);
			mainMenu.Items.Add(newItem);
			Env.Current.Commands.RegisterContext("HelpContext",new MenuCommandContext(newItem.DropDown));
			Env.Current.Commands.GetContext("HelpContext").AddCommand(new CheckForUpdatesCommand());

            windowManager = new WindowManager(dockPanel);
            Env.Current.Project.ProjectLoaded += new EventHandler(OnProjectLoaded);
			UpdateCaptionAndCommands();
		}

		void OnProjectLoaded(object sender, EventArgs e)
		{
			UpdateCaptionAndCommands();
		}

		private void OnMenuVariables(object sender, System.EventArgs e)
		{
			VariablesDialog frm = new VariablesDialog();
			frm.ShowDialog(this);
		}

		private void OnMenuMediaContent(object sender, EventArgs e)
		{
			ProjectMediaDialog frm = new ProjectMediaDialog();
			frm.ShowDialog(this);
		}


		

		private void OnEventsItemClick(object sender, System.EventArgs e)
		{
			//windowManager.ShowEvents();
		}

		private void OnVariablesSettingsClick(object sender, EventArgs e)
		{
            //windowManager.ShowVariablesView();
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
			if (string.IsNullOrEmpty(Env.Current.Project.FileName))
				Text = StringResources.MainWindowName;
			else
				Text = string.Format(StringResources.MainWindowNameEx, Env.Current.Project.FileName);

		}

		

      
        /*
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.FileName = Env.Current.Project.FileName;
			sfd.Filter = StringResources.FileOpenDialogFilter;
			sfd.FilterIndex = 0;
			sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Env.Current.Project.SaveAsFileName = sfd.FileName;
                windowManager.SaveProject();
                UpdateCaptionAndCommands();
            }
        }
        */
        private void OnArchiverSettingsClick(object sender, EventArgs e)
        {
            //windowManager.ShowArchiverSettings();
        }

		private void OnAddNewScriptClick(object sender, EventArgs e)
		{
			string newScriptName = Env.Current.Project.GenerateUniqueName(ProjectEntityType.Script, StringResources.UntitledScript);
			Env.Current.ScriptManager.CreateNewScript(newScriptName);
		}

        private void OnVariablesViewClick(object sender, EventArgs e)
        {
            //\windowManager.ShowVariablesView();
        }
    }
}
