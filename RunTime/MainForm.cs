using System.Windows.Forms;
using FreeSCADA.Archiver;
using FreeSCADA.Common;


namespace FreeSCADA.RunTime
{
	public partial class MainForm : Form
	{
		WindowManager windowManager;

		public MainForm()
		{
			InitializeComponent();
			Env.Initialize(this, mainMenu, mainToolbar, FreeSCADA.Interfaces.EnvironmentMode.Runtime);
			ArchiverMain.Initialize();

			CommandManager.viewContext = new BaseCommandContext(viewSubMenu.DropDown, mainToolbar);

			windowManager = new WindowManager(dockPanel);
			UpdateCaption();
		}

        public MainForm(string fileToLoad)
        {
            InitializeComponent();
			Env.Initialize(this, mainMenu, mainToolbar, FreeSCADA.Interfaces.EnvironmentMode.Runtime);
			ArchiverMain.Initialize();

			CommandManager.viewContext = new BaseCommandContext(viewSubMenu.DropDown, mainToolbar);

            windowManager = new WindowManager(dockPanel);
            if (fileToLoad != "")
                windowManager.LoadProject(fileToLoad);
            UpdateCaption();
        }

		void UpdateCaption()
		{
			if (Env.Current.Project.FileName == "")
				Text = StringResources.MainWindowName;
			else
				Text = string.Format(StringResources.MainWindowNameEx, Env.Current.Project.FileName);

			showTableButton.Enabled = ArchiverMain.Current.DatabaseSettings.EnableArchiving;
		}

		private void OnLoadProjectClick(object sender, System.EventArgs e)
		{
			windowManager.LoadProject();
			UpdateCaption();
		}

		private void OnMenuExitClick(object sender, System.EventArgs e)
		{
			Close();
		}

		private void OnRunClick(object sender, System.EventArgs e)
		{
			if (Env.Current.CommunicationPlugins.Connect())
			{
				if (ArchiverMain.Current.DatabaseSettings.EnableArchiving)
				{
					if (ArchiverMain.Current.Start() == false)
					{
						Env.Current.CommunicationPlugins.Disconnect();
						return;
					}
				}
				runButton.Enabled = false;
				refreshButton.Enabled = false;
				stopButton.Enabled = true;
			}
		}

		private void OnStopClick(object sender, System.EventArgs e)
		{
			if (ArchiverMain.Current.DatabaseSettings.EnableArchiving)
				ArchiverMain.Current.Stop();

			Env.Current.CommunicationPlugins.Disconnect();
			runButton.Enabled = true;
            refreshButton.Enabled = true;
            stopButton.Enabled = false;
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			ArchiverMain.Current.Stop();
			Env.Current.CommunicationPlugins.Disconnect();
		}

        private void refreshButton_Click(object sender, System.EventArgs e)
        {
            windowManager.LoadProject(Env.Current.Project.FileName);
            UpdateCaption();
        }

		private void showTableButton_Click(object sender, System.EventArgs e)
		{
			windowManager.ShowQueryView();
		}
    }
}
