using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FreeSCADA.Common;
using FreeSCADA.Designer.Dialogs;
using FreeSCADA.Designer.Views;
using WeifenLuo.WinFormsUI.Docking;

namespace FreeSCADA.Designer
{
	class WindowManager 
    {
		WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;

        List<DocumentView> documentViews = new List<DocumentView>();
        DocumentView currentDocument;

		ProjectContentView projectContentView;
        PropertyBrowserView propertyBrowserView;
		ToolBoxView toolBoxView;

		public WindowManager(DockPanel dockPanel)
		{
			this.dockPanel = dockPanel;

			//Create toolwindows
			projectContentView = new ProjectContentView();
			projectContentView.Show(dockPanel, DockState.DockLeft);
			projectContentView.OpenEntity += new ProjectContentView.OpenEntityHandler(OnOpenProjectEntity);

			toolBoxView = new ToolBoxView();
			toolBoxView.Show(dockPanel, DockState.DockRight);
            
            propertyBrowserView = new PropertyBrowserView();
			propertyBrowserView.Show(toolBoxView.Pane, DockAlignment.Bottom, 0.6);

			//Connect Windows Manager to heleper events
			dockPanel.ActiveDocumentChanged += new EventHandler(OnActiveDocumentChanged);
		}

		public void ForceWindowsClose()
		{
            while (documentViews.Count > 0)
            {
                DocumentView doc = documentViews[0];
                //doc.HandleModifiedOnClose = false;
                doc.Close(); //this window should be removed from documentViews on closing
                documentViews.Remove(doc);
            }
            documentViews.Clear();

			projectContentView.Close();
			toolBoxView.Close();
			propertyBrowserView.Close();

			dockPanel.ActiveDocumentChanged -= new EventHandler(OnActiveDocumentChanged);
		}

		public void CreateNewSchema()
		{
			SchemaView view = new SchemaView();
			if(view.CreateNewDocument() == false)
			{
				System.Windows.MessageBox.Show( DialogMessages.CannotCreateSchema,
												DialogMessages.ErrorCaption,
												System.Windows.MessageBoxButton.OK,
												System.Windows.MessageBoxImage.Error);
				return;
			}

			//Generate unique name
			for (int i = 1; i < 1000; i++)
			{
				string newName = string.Format("{0} {1}", StringResources.UntitledSchema, i);
				bool hasTheSameDocument = false;
				foreach (DocumentView doc in documentViews)
				{
					if (doc is SchemaView && doc.DocumentName == newName)
					{
						hasTheSameDocument = true;
						break;
					}
				}
				if (hasTheSameDocument == false && Env.Current.Project.IsSchemaNameUnique(newName))
				{
					view.DocumentName = newName;
					break;
				}
			}
            
			view.ToolsCollectionChanged += toolBoxView.OnToolsCollectionChanged;
            view.SetCurrentTool += toolBoxView.OnSetCurrentTool;
            view.FormClosing += new FormClosingEventHandler(OnDocumentWindowClosing);
			documentViews.Add(view);
			view.Show(dockPanel, DockState.Document);
		}

		void OnDocumentWindowClosing(object sender, FormClosingEventArgs e)
		{
			DocumentView doc = (DocumentView)sender;
			if (doc.HandleModifiedOnClose && doc.IsModified)
			{
				System.Windows.MessageBoxResult res = System.Windows.MessageBox.Show(	DialogMessages.NotSavedDocument,
																						DialogMessages.SaveDocumentCaption,
																						System.Windows.MessageBoxButton.YesNoCancel,
																						System.Windows.MessageBoxImage.Warning);
				if (res == System.Windows.MessageBoxResult.Yes)
					doc.SaveDocument();
				if (res == System.Windows.MessageBoxResult.Cancel)
				{
					e.Cancel = true;
					return;
				}
			}
            //doc.ToolsCollectionChanged -= toolBoxView.OnToolsCollectionChanged;
            doc.FormClosing -= new FormClosingEventHandler(OnDocumentWindowClosing);
			documentViews.Remove(doc);
            propertyBrowserView.ShowProperties(new Object());
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
            view.ToolsCollectionChanged += toolBoxView.OnToolsCollectionChanged;
            view.SetCurrentTool += toolBoxView.OnSetCurrentTool;
            documentViews.Add(view);
            view.Show(dockPanel, DockState.Document);
		}

		/// <summary>
		/// Create or active existing "Events" view.
		/// </summary>
		public void ShowEvents()
		{
			foreach (DocumentView doc in documentViews)
			{
				if (doc is EventsView)
				{
					doc.Activate();
					return;
				}
			}

			EventsView view = new EventsView();
			view.Show(dockPanel, DockState.Document);

			view.FormClosing += new FormClosingEventHandler(OnDocumentWindowClosing);
			documentViews.Add(view);
		}

		/// <summary>
		/// Create or active existing "Archiver Settings" view.
		/// </summary>
		public void ShowArchiverSettings()
		{
			foreach (DocumentView doc in documentViews)
			{
				if (doc is ArchiverSettingsView)
				{
					doc.Activate();
					return;
				}
			}

			ArchiverSettingsView view = new ArchiverSettingsView();
			view.Show(dockPanel, DockState.Document);

			view.FormClosing += new FormClosingEventHandler(OnDocumentWindowClosing);
			documentViews.Add(view);
		}

		/// <summary>
		/// SaveDocument current document
		/// </summary>
		public void SaveDocument()
		{
			if (currentDocument != null && currentDocument.IsModified)
				currentDocument.SaveDocument();
			projectContentView.RefreshContent(Env.Current.Project);
		}

		/// <summary>
		/// SaveDocument all opened documents
		/// </summary>
		public void SaveAllDocuments()
		{
			foreach(DocumentView documentWindow in documentViews)
				if (documentWindow != null && documentWindow.IsModified)
					documentWindow.SaveDocument();
			projectContentView.RefreshContent(Env.Current.Project);
		}

		/// <summary>
		/// Save current project. Asks user for a file if needed.
		/// </summary>
		/// <returns>Returns true if project was successfully saved</returns>
		public bool SaveProject()
		{
			string projectFileName = Env.Current.Project.FileName;
			if (projectFileName == "")
			{
				SaveFileDialog fd = new SaveFileDialog();

				fd.Filter = StringResources.FileDialogFilter;
				fd.FilterIndex = 0;
				fd.RestoreDirectory = true;

				if (fd.ShowDialog() == DialogResult.OK)
					projectFileName = fd.FileName;
				else
					return false;
			}

			SaveAllDocuments();
			Env.Current.Project.Save(projectFileName);
			projectContentView.RefreshContent(Env.Current.Project);
			return true;
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
        public bool ImportSchema()
        {
            if(!(currentDocument is SchemaView)) 
                return false;
            OpenFileDialog fd = new OpenFileDialog();

            fd.Filter = StringResources.FileImportDialogFilter;
            fd.FilterIndex = 0;
            fd.RestoreDirectory = true;

            if (fd.ShowDialog() != DialogResult.OK )
                return false;
            return  (currentDocument as SchemaView).ImportFile(fd.FileName);
            
        }

		/// <summary>
		/// Close project. If there are unsaved open documents, an appropriate dialog will be show. User will be able 
		/// to save these documents or cancel cloasure. Return false if closing should be canceled.
		/// </summary>
		/// <returns>Return true if all views successfully close. Otherwise returns false which 
		/// should prevent application closing.</returns>
		public bool Close()
		{
            List<string> unsaved_documents = new List<string>();
            List<DocumentView> other_documents = new List<DocumentView>();

            if (Env.Current.Project.IsModified)
			{
				if(Env.Current.Project.FileName == "")
					unsaved_documents.Add(StringResources.UnsavedProjectName);
				else
					unsaved_documents.Add(Env.Current.Project.FileName);
			}

			foreach (DocumentView documentWindow in documentViews)
			{
                if (documentWindow != null && documentWindow.IsModified)
                    unsaved_documents.Add(documentWindow.DocumentName);
                else
                    other_documents.Add(documentWindow);
			}

            foreach (DocumentView documentWindow in other_documents)
            {
                documentWindow.Close();
                documentViews.Remove(documentWindow);
            }

			if (unsaved_documents.Count > 0)
			{
				SaveDocumentsDialog dlg = new SaveDocumentsDialog(unsaved_documents);
				System.Windows.Forms.DialogResult res = dlg.ShowDialog(Env.Current.MainWindow);
				if (res == System.Windows.Forms.DialogResult.No)
				{
					while (documentViews.Count > 0)
					{
						DocumentView doc = documentViews[0];
						doc.HandleModifiedOnClose = false;
						doc.Close(); //this window should be removed from documentViews on closing
                        documentViews.Remove(doc);
					}
					return true;
				}
				if (res == System.Windows.Forms.DialogResult.Cancel)
					return false;
				if (res == System.Windows.Forms.DialogResult.Yes)
				{
					if (SaveProject() == false)
						return false;

                    while (documentViews.Count > 0)
                    {
                        DocumentView doc = documentViews[0];
                        doc.HandleModifiedOnClose = false;
                        doc.Close();
                        documentViews.Remove(doc);
                    }
                    documentViews.Clear();
				}
			}
		return true;
		}

		void OnActiveDocumentChanged(object sender, EventArgs e)
		{
			DeactivatingDocument();
			currentDocument = (DocumentView)dockPanel.ActiveDocument;
			ActivatingDocument();
		}

		private void ActivatingDocument()
		{
			//Notify and subscribe document to appropriate tool windows
			if (currentDocument != null)
			{
                //toolBoxView.Clean(); 
                currentDocument.OnActivated();
				toolBoxView.ToolActivated += currentDocument.OnToolActivated;
    			currentDocument.ObjectSelected += propertyBrowserView.ShowProperties;
                //(Env.Current.MainWindow as MainForm).undoButton.Tag = currentDocument;
                //(Env.Current.MainWindow as MainForm).redoButton.Tag = currentDocument;
            }
		}

		private void DeactivatingDocument()
		{
			//Notify and unsubscribe document from all tool windows
			if (currentDocument != null)
			{
				currentDocument.OnDeactivated();
				toolBoxView.ToolActivated -= currentDocument.OnToolActivated;
                currentDocument.ObjectSelected -= propertyBrowserView.ShowProperties;
            }
            propertyBrowserView.ShowProperties(new Object());
        }

        public void SetCurrentDocumentFocus()
        {
            if (currentDocument != null) currentDocument.Focus();
        }

      

        public void ExecuteCommand(FreeSCADA.Interfaces.ICommand command, Object param)
        {
			SchemaEditor.SchemaCommands.SchemaCommand cmd = (SchemaEditor.SchemaCommands.SchemaCommand)command;
			if(cmd != null)
				cmd.ControlledObject = (param == null) ? currentDocument : param;

			command.Execute();
        }
  
    }
}
