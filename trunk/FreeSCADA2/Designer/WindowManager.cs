﻿using System;
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
            propertyBrowserView.Show(dockPanel, DockState.DockLeft);

			//Connect Windows Manager to heleper events
			dockPanel.ActiveDocumentChanged += new EventHandler(OnActiveDocumentChanged);
		}

		public void ForceWindowsClose()
		{
			foreach (DocumentView doc in documentViews)
				doc.Close();
			projectContentView.Close();
			toolBoxView.Close();
			propertyBrowserView.Close();
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
			documentViews.Add(view);
			view.Show(dockPanel, DockState.Document);
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

			Env.Current.Project.Load(fd.FileName);
			return true;
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
			}

			if (unsaved_documents.Count > 0)
			{
				SaveDocumentsDialog dlg = new SaveDocumentsDialog(unsaved_documents);
				System.Windows.Forms.DialogResult res = dlg.ShowDialog(Env.Current.MainWindow);
				if (res == System.Windows.Forms.DialogResult.No)
				{
					foreach (DocumentView doc in documentViews)
					{
						doc.HandleModifiedOnClose = false;
						doc.Close();
					}
					documentViews.Clear();
					return true;
				}
				if (res == System.Windows.Forms.DialogResult.Cancel)
					return false;
				if (res == System.Windows.Forms.DialogResult.Yes)
				{
					if (SaveProject() == false)
						return false;

					foreach (DocumentView doc in documentViews)
						doc.Close();
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
                toolBoxView.Clean(); 
                currentDocument.OnActivated();
				toolBoxView.ToolActivated += currentDocument.OnToolActivated;
    			currentDocument.ObjectSelected += propertyBrowserView.ShowProperties;
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
		}
    }
}
