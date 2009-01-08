using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FreeSCADA.Common;
using FreeSCADA.RunTime.Views;
using WeifenLuo.WinFormsUI.Docking;

namespace FreeSCADA.RunTime
{
    class WindowManager
    {
		WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;

		List<DocumentView> documentViews = new List<DocumentView>();
		ProjectContentView projectContentView;

		DocumentView currentDocument;

		public WindowManager(DockPanel dockPanel)
		{
			this.dockPanel = dockPanel;

			//Create toolwindows
			projectContentView = new ProjectContentView();
			projectContentView.Show(dockPanel, DockState.DockLeft);
			projectContentView.OpenEntity += new ProjectContentView.OpenEntityHandler(OnOpenProjectEntity);
            //Connect Windows Manager to heleper events
            dockPanel.ActiveDocumentChanged += new EventHandler(OnActiveDocumentChanged);
        }

		public void Close()
		{
            while (documentViews.Count > 0)
            {
				DocumentView doc = documentViews[0];
                doc.Close();
                documentViews.Remove(doc);
            }
			documentViews.Clear();

			//projectContentView.Close();
		}

		void OnDocumentWindowClosing(object sender, FormClosingEventArgs e)
		{
			DocumentView doc = (DocumentView)sender;

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
			currentDocument = (DocumentView)dockPanel.ActiveDocument;
        }

        void OnActiveDocumentChanged(object sender, EventArgs e)
        {
			if (currentDocument != null)
				currentDocument.OnDeactivated();

			currentDocument = (DocumentView)dockPanel.ActiveDocument;

			if (currentDocument != null)
				currentDocument.OnActivated();
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
            Close();
            Env.Current.Project.Load(fd.FileName);
            return true;
        }

        /// <summary>
        /// Load a project, taking filename from command line argument.
        /// </summary>
        /// <returns>Returns true if project was successfully loaded</returns>
        public bool LoadProject(string fileToLoad)
        {
            Close();
            Env.Current.Project.Load(fileToLoad);
            return true;
        }

		public void ShowArchiverTable()
		{
			foreach (DockContent doc in documentViews)
			{
				if (doc is ArchiverTableView)
				{
					doc.Activate();
					return;
				}
			}

			ArchiverTableView view = new ArchiverTableView();
			view.Show(dockPanel, DockState.Document);

			view.FormClosing += new FormClosingEventHandler(OnDocumentWindowClosing);
			documentViews.Add(view);
		}
    }
}
