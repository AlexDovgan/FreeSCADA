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
	}
}
