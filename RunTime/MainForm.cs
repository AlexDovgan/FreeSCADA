using System.Text.RegularExpressions;
using System.Windows.Forms;
using FreeSCADA.Common;


namespace FreeSCADA.RunTime
{
	public partial class MainForm : Form
	{
		WindowManager windowManager;

		public MainForm()
		{
			InitializeComponent();
			Env.Initialize(this, mainMenu, FreeSCADA.ShellInterfaces.EnvironmentMode.Runtime);
			windowManager = new WindowManager(dockPanel);
			UpdateCaption();
		}

        public MainForm(string fileToLoad)
        {
            InitializeComponent();
            Env.Initialize(this, mainMenu, FreeSCADA.ShellInterfaces.EnvironmentMode.Runtime);
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
                runButton.Enabled = false;
                refreshButton.Enabled = false;
                stopButton.Enabled = true;
			}
		}

		private void OnStopClick(object sender, System.EventArgs e)
		{
			Env.Current.CommunicationPlugins.Disconnect();
			runButton.Enabled = true;
            refreshButton.Enabled = true;
            stopButton.Enabled = false;
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			Env.Current.CommunicationPlugins.Disconnect();
		}
        private void zoomOutButton_Click(object sender, System.EventArgs e)
        {
            windowManager.zoom_out();
            windowManager.SetCurrentDocumentFocus();
        }

        private void zoomInButton_Click(object sender, System.EventArgs e)
        {
            windowManager.zoom_in();
            windowManager.SetCurrentDocumentFocus();
        }

        private void zoomLevelComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int percentage;
            string txt = ((ToolStripComboBox)sender).SelectedItem.ToString();
            //MessageBox.Show(txt);

            try
            {
                MatchCollection matches = Regex.Matches(txt, @"\d+");
                percentage = int.Parse(matches[0].Value);
                windowManager.zoom_level((double)percentage / 100.0);
            }
            catch
            {
                // do nothing
            }
            windowManager.SetCurrentDocumentFocus();
        }

        private void zoomLevelComboBox_KeyUp(object sender, KeyEventArgs e)
        {
            int percentage;
            string txt = ((ToolStripComboBox)sender).Text;
            //MessageBox.Show(e.KeyCode.ToString());
            if (e.KeyCode == Keys.Return)
            {
                try
                {
                    //MessageBox.Show(txt);
                    MatchCollection matches = Regex.Matches(txt, @"\d+");
                    percentage = int.Parse(matches[0].Value);
                    windowManager.zoom_level((double)percentage / 100.0);
                }
                catch
                {
                    // do nothing
                }
                windowManager.SetCurrentDocumentFocus();
            }
        }
        /// <summary>
        /// Zoom level visualuzation
        /// </summary>
        public void zoomLevelComboBox_SetZoomLevelTxt(double level)
        {
            int percentage = (int)(level * 100);
            string txt = "Zoom " + percentage.ToString() + "%";
            zoomLevelComboBox.Text = txt;
        }

        private void refreshButton_Click(object sender, System.EventArgs e)
        {
            windowManager.LoadProject(Env.Current.Project.FileName);
            UpdateCaption();
        }
    }
}
