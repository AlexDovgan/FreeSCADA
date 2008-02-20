using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using FreeSCADA.Common;
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
		}

		private void OnMenuVariables(object sender, System.EventArgs e)
		{
			VariablesForm frm = new VariablesForm();
			frm.ShowDialog(this);
		}

		private void OnMenuExitClick(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void OnSchemaItemClick(object sender, System.EventArgs e)
		{
			windowManager.CreateNewSchema();
		}

		private void OnEventsItemClick(object sender, System.EventArgs e)
		{
			EventsView view = new EventsView();
			view.Show(dockPanel, DockState.Document);
		}

		private void OnMenuSaveClick(object sender, System.EventArgs e)
		{
			SaveFileDialog fd = new SaveFileDialog();

			fd.Filter = "FreeSCADA2 files (*.fs2)|*.fs2|All files (*.*)|*.*";
			fd.FilterIndex = 0;
			fd.RestoreDirectory = true;

			if (fd.ShowDialog() == DialogResult.OK)
				Env.Current.Project.Save(fd.FileName);
		}

		private void OnMenuLoadClick(object sender, System.EventArgs e)
		{
			OpenFileDialog fd = new OpenFileDialog();

			fd.Filter = "FreeSCADA2 files (*.fs2)|*.fs2|All files (*.*)|*.*";
			fd.FilterIndex = 0;
			fd.RestoreDirectory = true;

			if (fd.ShowDialog() == DialogResult.OK)
				Env.Current.Project.Load(fd.FileName);
		}
        
	}
}
