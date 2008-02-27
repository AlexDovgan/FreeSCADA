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
		string projectFileName;

		/// <summary>
		/// Constructor
		/// </summary>
		public MainForm()
		{
			InitializeComponent();
			Env.Initialize(this, mainMenu);
			windowManager = new WindowManager(dockPanel);            
		}

		private void OnMenuVariables(object sender, System.EventArgs e)
		{
			VariablesDialog frm = new VariablesDialog();
			frm.ShowDialog(this);
		}

		private void OnMenuExitClick(object sender, System.EventArgs e)
		{
			if (windowManager.Close())
				this.Close();
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
			if (projectFileName == "")
			{
				SaveFileDialog fd = new SaveFileDialog();

				fd.Filter = "FreeSCADA2 files (*.fs2)|*.fs2|All files (*.*)|*.*";
				fd.FilterIndex = 0;
				fd.RestoreDirectory = true;

				if (fd.ShowDialog() == DialogResult.OK)
					projectFileName = fd.FileName;
				else
					return;
			}
			windowManager.SaveAll();
			Env.Current.Project.Save(projectFileName);
		}

		private void OnLoadProjectClick(object sender, System.EventArgs e)
		{
			OpenFileDialog fd = new OpenFileDialog();

			fd.Filter = "FreeSCADA2 files (*.fs2)|*.fs2|All files (*.*)|*.*";
			fd.FilterIndex = 0;
			fd.RestoreDirectory = true;

			if (fd.ShowDialog() == DialogResult.OK)
			{
				Env.Current.Project.Load(fd.FileName);
				projectFileName = fd.FileName;
			}
		}

		private void OnSaveFileClick(object sender, System.EventArgs e)
		{
			windowManager.Save();
		}
        
	}
}
