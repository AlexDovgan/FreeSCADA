using System.Windows.Forms;
using System;
using System.Text.RegularExpressions;
using WeifenLuo.WinFormsUI.Docking;
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
			Env.Initialize(this, mainMenu, FreeSCADA.ShellInterfaces.EnvironmentMode.Designer);
			windowManager = new WindowManager(dockPanel);
			UpdateCaption();
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

		private void OnSaveProjectClick(object sender, System.EventArgs e)
		{
			windowManager.SaveProject();
			UpdateCaption();
		}

		private void OnLoadProjectClick(object sender, System.EventArgs e)
		{
			windowManager.LoadProject();
			UpdateCaption();
		}

		private void OnSaveFileClick(object sender, System.EventArgs e)
		{
			//windowManager.SaveDocument();
            windowManager.SaveProject();
            UpdateCaption();
        }

		private void OnFormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = !windowManager.Close();
		}

		private void OnFormClosed(object sender, FormClosedEventArgs e)
		{
			Env.Deinitialize();
		}

		void UpdateCaption()
		{
			if (Env.Current.Project.FileName == "")
				Text = StringResources.MainWindowName;
			else
				Text = string.Format(StringResources.MainWindowNameEx, Env.Current.Project.FileName);
		}

		private void OnNewProjectClick(object sender, System.EventArgs e)
		{
			if (windowManager.Close())
			{
				windowManager.ForceWindowsClose();
				Env.Current.CreateNewProject();
				windowManager = new WindowManager(dockPanel);
				UpdateCaption();
				System.GC.Collect();
			}
		}

        private void importSchemaToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            windowManager.ImportSchema();
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
            catch (Exception ex)
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
                catch (Exception ex)
                {
                    // do nothing
                }
                windowManager.SetCurrentDocumentFocus();
            }
        }
    }
}
