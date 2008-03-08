using System.Windows.Forms;
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
			Env.Initialize(this, mainMenu);
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
			windowManager.SaveDocument();
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
	}
}
