using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;
using FreeSCADA.Designer.Views;
using FreeSCADA.Designer.Dialogs;
using FreeSCADA.Common;

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
			projectContentView.OpenEntity += new ProjectContentView.OpenEntityHandler(OpenProjectEntity);

			toolBoxView = new ToolBoxView();
			toolBoxView.Show(dockPanel, DockState.DockRightAutoHide);
            
            propertyBrowserView = new PropertyBrowserView();
            propertyBrowserView.Show(dockPanel, DockState.DockLeft);

			//Connect Windows Manager to heleper events
			dockPanel.ActiveDocumentChanged += new EventHandler(OnActiveDocumentChanged);
		}

		public void CreateNewSchema()
		{
            try
            {
                SchemaView view = new SchemaView();
                view.ToolsCollectionChanged += toolBoxView.OnToolsCollectionChanged;
                documentViews.Add(view);
                view.Show(dockPanel, DockState.Document);
            }
            catch (Exception)
            {

            }
		}

		public void OpenProjectEntity(string name)
		{
			// Open your schema and other document types here by entity name
		}

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
		/// Save current document
		/// </summary>
		public void Save()
		{
			if (currentDocument != null && currentDocument.IsModified)
				currentDocument.OnSave();
		}

		/// <summary>
		/// Save all opened documents
		/// </summary>
		public void SaveAll()
		{
			foreach(DocumentView documentWindow in documentViews)
				if (documentWindow != null && documentWindow.IsModified)
					documentWindow.OnSave();
		}

		/// <summary>
		/// Close project. If there are unsaved open documents, an appropriate dialog will be show. User will be able 
		/// to save these documents or cancel cloasure.
		/// </summary>
		/// <returns>Return true if all views successfully close. Otherwise returns false which 
		/// should prevent application closing.</returns>
		public bool Close()
		{
			bool show_dialog = false;
			List<string> unsaved_documents = new List<string>();
			foreach (DocumentView documentWindow in documentViews)
			{
				if (documentWindow != null && documentWindow.IsModified)
				{
					show_dialog = true;
					unsaved_documents.Add(documentWindow.ProjectEntityName);
				}
			}

			if (show_dialog)
			{
				SaveDocumentsDialog dlg = new SaveDocumentsDialog(unsaved_documents);
				System.Windows.Forms.DialogResult res = dlg.ShowDialog(Env.Current.MainWindow);
				if (res == System.Windows.Forms.DialogResult.No)
					return true;
				if (res == System.Windows.Forms.DialogResult.Cancel)
					return false;
				if (res == System.Windows.Forms.DialogResult.Yes)
					SaveAll();
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
                //currentDocument.ObjectSelected+=new DocumentWindow.ObjectSelectedDelegate(propertyBrowserView.ShowProperties);
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
