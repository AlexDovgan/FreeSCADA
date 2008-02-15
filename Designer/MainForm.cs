using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using FreeSCADA.Common;
using FreeSCADA.Scheme;
namespace FreeSCADA.Designer
{
	public partial class MainForm : Form
	{
		ProjectContentView projectContentView = new ProjectContentView();
        SchemaView currentScheme;
		public MainForm()
		{
			InitializeComponent();

			Env.Initialize(this, mainMenu);

			projectContentView.Show(dockPanel, DockState.DockLeft);
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
			SchemaView view = new SchemaView(new FSSchemeEditor(FSSchemeDocument.CreateNewScheme()));
			view.Show(dockPanel, DockState.Document);
            currentScheme = view;
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

        private void toolStripButton6_Click(object sender, System.EventArgs e)
        {
            
            if(currentScheme!=null)
                currentScheme.schemeEditor.SelectTool(FreeSCADA.Scheme.Tools.ToolTypes.Select);
            
        }

        private void toolStripButton7_Click(object sender, System.EventArgs e)
        {
            if (currentScheme != null)
                currentScheme.schemeEditor.SelectTool(FreeSCADA.Scheme.Tools.ToolTypes.Rectangle);

        }

        private void toolStripButton8_Click(object sender, System.EventArgs e)
        {
            if (currentScheme != null)
                currentScheme.schemeEditor.SelectTool(FreeSCADA.Scheme.Tools.ToolTypes.Ellipse);
        }
	}
}
