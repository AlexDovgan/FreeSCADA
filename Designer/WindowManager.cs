using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FreeSCADA.Common;
using FreeSCADA.Common.Scripting;
using FreeSCADA.Designer.Dialogs;
using FreeSCADA.Designer.Views;
using WeifenLuo.WinFormsUI.Docking;

namespace FreeSCADA.Designer
{
	class WindowManager: IDisposable
    {
		WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
		MRUManager mruManager;

        List<DocumentView> documentViews = new List<DocumentView>();
        DocumentView currentDocument;

		ProjectContentView projectContentView;
        PropertyBrowserView propertyBrowserView;
		ToolBoxView toolBoxView;

		public WindowManager(DockPanel dockPanel, MRUManager mruManager)
		{
			this.dockPanel = dockPanel;
			this.mruManager = mruManager;

			mruManager.ItemClicked += new MRUManager.ItemClickedDelegate(OnMRUItemClicked);

			//Create toolwindows
			projectContentView = new ProjectContentView();
			projectContentView.Show(dockPanel, DockState.DockLeft);
			projectContentView.OpenEntity += new ProjectContentView.OpenEntityHandler(OnOpenProjectEntity);
			projectContentView.SelectNode += new ProjectContentView.SelectNodeHandler(OnSelectProjectNode);

			toolBoxView = new ToolBoxView();
			toolBoxView.Show(dockPanel, DockState.DockRight);
            
            propertyBrowserView = new PropertyBrowserView();
			propertyBrowserView.Show(toolBoxView.Pane, DockAlignment.Bottom, 0.6);

			//Connect Windows Manager to heleper events
			dockPanel.ActiveDocumentChanged += new EventHandler(OnActiveDocumentChanged);
            CommandManager.toolboxContext = new ToolboxContext(toolBoxView);
			Env.Current.ScriptManager.NewScriptCreated += new ScriptManager.NewScriptCreatedHandler(OnOpenScript);
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
            
            string name;
            var hasTheSameDocument = false;
            name = Env.Current.Project.GenerateUniqueName(ProjectEntityType.Schema, "Untitled_");
            do
            {
                hasTheSameDocument = false;
                
                foreach (DocumentView doc in documentViews)
                {
                    if (doc is SchemaView && doc.DocumentName == name)
                    {
                        hasTheSameDocument = true;
                        name = Env.Current.Project.GenerateUniqueName(ProjectEntityType.Schema, name);
                        break;
                    }
                }
                
            }while (hasTheSameDocument);
	

			var view = new SchemaView(name);

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
            
            doc.FormClosing -= new FormClosingEventHandler(OnDocumentWindowClosing);
			documentViews.Remove(doc);
            propertyBrowserView.ShowProperties(new Object());
        }

        public void OnOpenProjectEntity(ProjectEntityType type, string name)
		{
			// Open your schema and other document types here by entity name
            DocumentView view = null;
            switch (type)
            {
                case ProjectEntityType.Schema:
                    foreach (DocumentView doc in documentViews)
                    {
                        if (doc is SchemaView)
                        {
                            if (doc.DocumentName == name)
                            {
                                doc.Activate();
                                return;
                            }
                        }
                    }
                    view = new SchemaView(name);
                    
                    view.FormClosing += new FormClosingEventHandler(OnDocumentWindowClosing);
                    documentViews.Add(view);
                    view.Show(dockPanel, DockState.Document);
                    break;
                case ProjectEntityType.Archiver:
                    ShowArchiverSettings();
                    break;
				case ProjectEntityType.Script:
					OnOpenScript(this, name);
					break;
                // etc....
                default:
                    break;
            }
		}

		void OnSelectProjectNode(FreeSCADA.Designer.Views.ProjectNodes.BaseNode node)
		{
			if (node is FreeSCADA.Designer.Views.ProjectNodes.ChannelNode)
			{
				FreeSCADA.Interfaces.IChannel ch = (node as FreeSCADA.Designer.Views.ProjectNodes.ChannelNode).Channel;
				propertyBrowserView.ShowProperties(ch);
			}
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

			var view = new EventsView();
			view.Show(dockPanel, DockState.Document);

			view.FormClosing += new FormClosingEventHandler(OnDocumentWindowClosing);
			documentViews.Add(view);
		}

        /// <summary>
        /// Create or active existing "Archiver Settings" view.
        /// </summary>
        public void ShowArchiverSettings()
        {
            foreach (var doc in documentViews)
            {
                if (doc is ArchiverSettingsView)
                {
                    doc.Activate();
                    return;
                }
            }

            var view = new ArchiverSettingsView();
            view.Show(dockPanel, DockState.Document);

            view.FormClosing += new FormClosingEventHandler(OnDocumentWindowClosing);
            documentViews.Add(view);
        }

        /// <summary>
        /// Create or active existing "Variables" view.
        /// </summary>
        public void ShowVariablesView()
        {
            foreach (var doc in documentViews)
            {
                if (doc is VariablesView)
                {
                    doc.Activate();
                    return;
                }
            }

            var view = new VariablesView();
            view.Show(dockPanel, DockState.Document);

            view.FormClosing += new FormClosingEventHandler(OnDocumentWindowClosing);
            view.SelectChannel += new VariablesView.SelectChannelHandler(OnSelectChannel);
            documentViews.Add(view);
        }

        void OnSelectChannel(object channel)
        {
            if (propertyBrowserView != null)
                propertyBrowserView.ShowProperties(channel);
        }

        void OnOpenScript(object sender, string scriptName)
		{
            
			foreach (var doc in documentViews)
			{
				if (doc is ScriptView && doc.DocumentName == scriptName)
				{
					doc.Activate();
					return;
				}
			}

			var view = new ScriptView(scriptName);
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
			foreach(var documentWindow in documentViews)
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
				var fd = new SaveFileDialog
				             {
				                 Filter = StringResources.FileOpenDialogFilter,
				                 FilterIndex = 0,
				                 RestoreDirectory = true
				             };

			    if (fd.ShowDialog() == DialogResult.OK)
					projectFileName = fd.FileName;
				else
					return false;
			}

			SaveAllDocuments();
			Env.Current.Project.Save(projectFileName);
			projectContentView.RefreshContent(Env.Current.Project);
			mruManager.Add(projectFileName);
			return true;
		}

		/// <summary>
		/// Load a project. Asks user for a file.
		/// </summary>
		/// <returns>Returns true if project was successfully loaded</returns>
		public bool LoadProject()
		{
			var  fd = new OpenFileDialog
			              {
			                  Filter = StringResources.FileOpenDialogFilter,
			                  FilterIndex = 0,
			                  RestoreDirectory = true
			              };

		    if (fd.ShowDialog() != DialogResult.OK)
				return false;

            Close();
			Env.Current.Project.Load(fd.FileName);
			mruManager.Add(fd.FileName);
			return true;
		}

      
		void OnMRUItemClicked(object sender, string file)
		{
			Close();
			Env.Current.Project.Load(file);
			mruManager.Add(file);
		}

		/// <summary>
		/// Close project. If there are unsaved open documents, an appropriate dialog will be show. User will be able 
		/// to save these documents or cancel cloasure. Return false if closing should be canceled.
		/// </summary>
		/// <returns>Return true if all views successfully close. Otherwise returns false which 
		/// should prevent application closing.</returns>
		public bool Close()
		{
            var unsavedDocuments = new List<string>();
            var otherDocuments = new List<DocumentView>();

            if (Env.Current.Project.IsModified)
			{
				if(Env.Current.Project.FileName == "")
					unsavedDocuments.Add(StringResources.UnsavedProjectName);
				else
					unsavedDocuments.Add(Env.Current.Project.FileName);
			}

			foreach (var documentWindow in documentViews)
			{
                if (documentWindow != null && documentWindow.IsModified)
                    unsavedDocuments.Add(documentWindow.DocumentName);
                else
                    otherDocuments.Add(documentWindow);
			}

            foreach (var documentWindow in otherDocuments)
            {
                documentWindow.Close();
                documentViews.Remove(documentWindow);
            }

			if (unsavedDocuments.Count > 0)
			{
				var dlg = new SaveDocumentsDialog(unsavedDocuments);
				System.Windows.Forms.DialogResult res = dlg.ShowDialog(Env.Current.MainWindow);
				if (res == System.Windows.Forms.DialogResult.No)
				{
					while (documentViews.Count > 0)
					{
						var doc = documentViews[0];
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
                        var doc = documentViews[0];
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
    			currentDocument.ObjectSelected += propertyBrowserView.ShowProperties;
            }
		}

		private void DeactivatingDocument()
		{
			//Notify and unsubscribe document from all tool windows
			if (currentDocument != null)
			{
				currentDocument.OnDeactivated();
				currentDocument.ObjectSelected -= propertyBrowserView.ShowProperties;
            }
            propertyBrowserView.ShowProperties(new Object());
        }

        public void SetCurrentDocumentFocus()
        {
            if (currentDocument != null) currentDocument.Focus();
        }

      


		#region IDisposable Members

		public void Dispose()
		{
			ForceWindowsClose();

			mruManager.ItemClicked -= new MRUManager.ItemClickedDelegate(OnMRUItemClicked);
			projectContentView.OpenEntity -= new ProjectContentView.OpenEntityHandler(OnOpenProjectEntity);
			projectContentView.SelectNode -= new ProjectContentView.SelectNodeHandler(OnSelectProjectNode);
			dockPanel.ActiveDocumentChanged -= new EventHandler(OnActiveDocumentChanged);
			Env.Current.ScriptManager.NewScriptCreated -= new ScriptManager.NewScriptCreatedHandler(OnOpenScript);

			//Create toolwindows
			projectContentView.Dispose();
			toolBoxView.Dispose();
			propertyBrowserView.Dispose();

			mruManager.Dispose();
		}

		#endregion
	}
}
