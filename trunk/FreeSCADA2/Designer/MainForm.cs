using System.Windows.Forms;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using WeifenLuo.WinFormsUI.Docking;
using FreeSCADA;
using FreeSCADA.Common;
using FreeSCADA.Designer.Dialogs;
using FreeSCADA.Designer.SchemaEditor.SchemaCommands;

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
                Env.Current.Project.LoadNew();
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
            windowManager.ExecuteCommand(new ZoomOutCommand(), null);
            windowManager.SetCurrentDocumentFocus();
        }

        private void zoomInButton_Click(object sender, System.EventArgs e)
        {
            windowManager.ExecuteCommand(new ZoomInCommand(), null);
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
                windowManager.ExecuteCommand(new ZoomLevelCommand((double)percentage / 100.0), null);
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
                    windowManager.ExecuteCommand(new ZoomLevelCommand((double)percentage / 100.0), null);
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

        private void runButton_Click(object sender, EventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo(@"RunTime.exe");
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
                UpdateCaption();
            }
        }

        /// <summary>
        /// Change graphics object
        /// </summary>
        public void ChangeGraphicsObject(System.Windows.UIElement old, System.Windows.UIElement el)
        {
            windowManager.ChangeGraphicsObject(old, el);
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            windowManager.ExecuteCommand((new CopyCommand()), (sender as ToolStripItem).Tag);
        }

        private void xamlViewButton_Click(object sender, EventArgs e)
        {
            windowManager.ExecuteCommand((new XamlViewCommand()), (sender as ToolStripItem).Tag);
        }

        private void cutButton_Click(object sender, EventArgs e)
        {
            windowManager.ExecuteCommand((new CutCommand()), (sender as ToolStripItem).Tag);
        }

        private void pasteButton_Click(object sender, EventArgs e)
        {
            windowManager.ExecuteCommand((new PasteCommand()), (sender as ToolStripItem).Tag);
        }

        private void groupButton_Click(object sender, EventArgs e)
        {
            windowManager.ExecuteCommand((new GroupCommand()), (sender as ToolStripItem).Tag);
        }

        private void ungroupButton_Click(object sender, EventArgs e)
        {
            windowManager.ExecuteCommand((new UngroupCommand()), (sender as ToolStripItem).Tag);
        }

    }
}
