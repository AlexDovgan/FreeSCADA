using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FreeSCADA.Common;
using FreeSCADA.CommonUI;
using FreeSCADA.Common.Scripting;
using FreeSCADA.Designer.Dialogs;
using FreeSCADA.Designer.Views;
using WeifenLuo.WinFormsUI.Docking;
using FreeSCADA.CommonUI.Interfaces;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Designer
{
	class WindowManager: IDisposable, FreeSCADA.CommonUI.Interfaces.IWindowManager
    {
		WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
		MRUManager mruManager;

        List<DocumentView> documentViews = new List<DocumentView>();
        DocumentView currentDocument;

		ProjectContentView projectContentView;
        PropertyBrowserView propertyBrowserView;
		ToolBoxView toolBoxView;

		public WindowManager(DockPanel dockPanel)
		{
			this.dockPanel = dockPanel;
			this.mruManager =  new MRUManager(Env.Current.Commands.GetContext("FileContext"),this);

			//Create toolwindows
			projectContentView = new ProjectContentView(this);
			projectContentView.Show(dockPanel, DockState.DockLeft);
			projectContentView.OpenEntity += new ProjectContentView.OpenEntityHandler(ActivateDocument);
			projectContentView.SelectNode += new ProjectContentView.SelectNodeHandler(OnSelectProjectNode);

			toolBoxView = new ToolBoxView();
			toolBoxView.Show(dockPanel, DockState.DockRight);
            
            propertyBrowserView = new PropertyBrowserView();
			propertyBrowserView.Show(toolBoxView.Pane, DockAlignment.Bottom, 0.6);

			//Connect Windows Manager to heleper events
			dockPanel.ActiveDocumentChanged += new EventHandler(OnActiveDocumentChanged);
            Env.Current.Commands.RegisterContext("ToolboxContext",new ToolboxContext(toolBoxView));
            Env.Current.Commands.GetContext("FileContext").AddCommand(new CommonUI.GlobalCommands.LoadProjectCommand(mruManager, this));
            Env.Current.Commands.GetContext("FileContext").AddCommand(new Commands.SaveProjectCommand(mruManager, this));
            Env.Current.Commands.GetContext("FileContext").AddCommand(new CommonUI.GlobalCommands.NullCommand((int)CommandManager.Priorities.FileCommandsEnd));
            Env.Current.Commands.GetContext("FileContext").AddCommand(new CommonUI.GlobalCommands.NullCommand((int)CommandManager.Priorities.MruCommandsEnd));
            Env.Current.Commands.GetContext("FileContext").AddCommand(new CommonUI.GlobalCommands.ExitCommand());
            Env.Current.Commands.GetContext(PredefinedContexts.GlobalToolbar).AddCommand(new Commands.NewProjectCommand(this));
            Env.Current.Commands.GetContext(PredefinedContexts.GlobalToolbar).AddCommand(new Commands.NewSchemaCommand(this));
            Env.Current.Commands.GetContext(PredefinedContexts.GlobalToolbar).AddCommand(new Commands.RunProjectCommand());
        
			//Env.Current.ScriptManager.NewScriptCreated += new ScriptManager.NewScriptCreatedHandler(OnOpenScript);
		}

		public void ForceWindowsClose()
		{
            while (documentViews.Count > 0)
            {
                DocumentView doc = documentViews[0];
                doc.Close(); //this window should be removed from documentViews on closing
                documentViews.Remove(doc);
            }
            documentViews.Clear();

			projectContentView.Close();
			toolBoxView.Close();
			propertyBrowserView.Close();

			dockPanel.ActiveDocumentChanged -= new EventHandler(OnActiveDocumentChanged);
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

        public void ActivateDocument(IDocumentView view)
        {
            if (view == null)
                return;
            foreach (DocumentView d in documentViews)
            {
                if (d.GetType() ==view.GetType())
                {
                    if (d.DocumentName == view.DocumentName)
                    {
                        d.Activate();
                        return;
                    }
                }
            }
            DocumentView doc = view as DocumentView;
            if (doc != null)
            {
                doc.FormClosing += new FormClosingEventHandler(OnDocumentWindowClosing);
                documentViews.Add(doc);
                doc.Show(dockPanel, DockState.Document);
                Refresh();
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
                    (new Commands.SaveProjectCommand(mruManager, this)).Execute();

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
    			currentDocument.SelectionManager.SelectionChanged += propertyBrowserView.ShowProperties;
            }
		}

		private void DeactivatingDocument()
		{
			//Notify and unsubscribe document from all tool windows
			if (currentDocument != null)
			{
				currentDocument.OnDeactivated();
                currentDocument.SelectionManager.SelectionChanged -= propertyBrowserView.ShowProperties;
            }
            propertyBrowserView.ShowProperties(new Object());
        }

        public void SetCurrentDocumentFocus()
        {
            if (currentDocument != null) 
                currentDocument.Focus();
        }

      


		#region IDisposable Members

		public void Dispose()
		{
			ForceWindowsClose();
			projectContentView.OpenEntity -= new ProjectContentView.OpenEntityHandler(ActivateDocument);
			projectContentView.SelectNode -= new ProjectContentView.SelectNodeHandler(OnSelectProjectNode);
			dockPanel.ActiveDocumentChanged -= new EventHandler(OnActiveDocumentChanged);
			//Env.Current.ScriptManager.NewScriptCreated -= new ScriptManager.NewScriptCreatedHandler(OnOpenScript);

			//Create toolwindows
			projectContentView.Dispose();
			toolBoxView.Dispose();
			propertyBrowserView.Dispose();

			mruManager.Dispose();
		}

		#endregion

        #region IWindowManager Members


        
        public void Refresh()
        {
            projectContentView.RefreshContent(Env.Current.Project);
        }

        #endregion
    }

}
