﻿using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace FreeSCADA.Designer
{
	public partial class MainForm : Form
	{
		ProjectContentView projectContentView = new ProjectContentView();

		public MainForm()
		{
			InitializeComponent();

			Environment.Initialize(this, mainMenu);

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
			SchemaView view = new SchemaView();
			view.Show(dockPanel, DockState.Document);
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
				Environment.Current.Project.Save(fd.FileName);
		}

		private void OnMenuLoadClick(object sender, System.EventArgs e)
		{
			OpenFileDialog fd = new OpenFileDialog();

			fd.Filter = "FreeSCADA2 files (*.fs2)|*.fs2|All files (*.*)|*.*";
			fd.FilterIndex = 0;
			fd.RestoreDirectory = true;

			if (fd.ShowDialog() == DialogResult.OK)
				Environment.Current.Project.Load(fd.FileName);
		}
	}
}
