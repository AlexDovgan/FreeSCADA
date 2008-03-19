using System.Collections.Generic;
using System.Windows.Forms;
using FreeSCADA.Common;
using WeifenLuo.WinFormsUI.Docking;

namespace FreeSCADA.RunTime
{
    class WindowManager
    {
		WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;

		List<DockContent> documentViews = new List<DockContent>();
		ProjectContentView projectContentView;

		public WindowManager(DockPanel dockPanel)
		{
			this.dockPanel = dockPanel;

			//Create toolwindows
			projectContentView = new ProjectContentView();
			projectContentView.Show(dockPanel, DockState.DockLeft);
			projectContentView.OpenEntity += new ProjectContentView.OpenEntityHandler(OnOpenProjectEntity);
		}

		public void Close()
		{
			while (documentViews.Count > 0)
				documentViews[0].Close();
			documentViews.Clear();

			projectContentView.Close();
		}

		void OnDocumentWindowClosing(object sender, FormClosingEventArgs e)
		{
			DockContent doc = (DockContent)sender;

            doc.FormClosing -= new FormClosingEventHandler(OnDocumentWindowClosing);
			documentViews.Remove(doc);           
		}

		public void OnOpenProjectEntity(string name)
		{
			// Open your schema and other document types here by entity name
			SchemaView view = new SchemaView();
			if (view.LoadDocument(name) == false)
			{
			    System.Windows.MessageBox.Show(DialogMessages.CannotLoadSchema,
			                                    DialogMessages.ErrorCaption,
			                                    System.Windows.MessageBoxButton.OK,
			                                    System.Windows.MessageBoxImage.Error);
			    return;
			}

			documentViews.Add(view);
			view.Show(dockPanel, DockState.Document);
		}

		/// <summary>
		/// Load a project. Asks user for a file.
		/// </summary>
		/// <returns>Returns true if project was successfully loaded</returns>
		public bool LoadProject()
		{
			OpenFileDialog fd = new OpenFileDialog();

			fd.Filter = StringResources.FileDialogFilter;
			fd.FilterIndex = 0;
			fd.RestoreDirectory = true;

			if (fd.ShowDialog() != DialogResult.OK)
				return false;

			Env.Current.Project.Load(fd.FileName);
			return true;
		}
    }
}
