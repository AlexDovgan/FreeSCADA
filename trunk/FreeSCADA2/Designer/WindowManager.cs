using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;

namespace FreeSCADA.Designer
{
    class WindowManager
    {
		WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;

        List<DocumentWindow> documentWindows = new List<DocumentWindow>();
        DocumentWindow CurrentDocument;

		ProjectContentView projectContentView;
        PropertyBrowserView propertyBrowserView;
		ToolBoxView toolBoxView;

		public WindowManager(DockPanel dockPanel)
		{
			this.dockPanel = dockPanel;

			//Create toolwindows
			projectContentView = new ProjectContentView();
			projectContentView.Show(dockPanel, DockState.DockLeft);

			toolBoxView = new ToolBoxView();
			toolBoxView.Show(dockPanel, DockState.DockRightAutoHide);
            
            propertyBrowserView = new PropertyBrowserView();
            propertyBrowserView.Show(dockPanel, DockState.DockLeft);

			//Connect Windows Manager to heleper events
			dockPanel.ActiveDocumentChanged += new EventHandler(OnActiveDocumentChanged);
			FreeSCADA.Common.Env.Current.Project.LoadEvent += new EventHandler(OnProjectLoad);
		}

		void OnProjectLoad(object sender, EventArgs e)
		{
			//Load schema from project file
		}

		public void CreateNewSchema()
		{
            try
            {
                SchemaView view = new SchemaView();
                view.ToolsCollectionChanged += toolBoxView.OnToolsCollectionChanged;
                documentWindows.Add(view);

                view.Show(dockPanel, DockState.Document);
            }
            catch (Exception ex)
            {

            }
		}

		void OnActiveDocumentChanged(object sender, EventArgs e)
		{
			DeactivatingDocument();
			CurrentDocument = (DocumentWindow)dockPanel.ActiveDocument;
			ActivatingDocument();
		}

		private void ActivatingDocument()
		{
			//Notify and subscribe document to appropriate tool windows
			if (CurrentDocument != null)
			{
				CurrentDocument.OnActivated();
				if (CurrentDocument is SchemaView)
					toolBoxView.ToolActivated += (CurrentDocument as SchemaView).OnToolActivated;
			}
			else
				toolBoxView.Clean();
		}

		private void DeactivatingDocument()
		{
			//Notify and unsubscribe document from all tool windows
			if (CurrentDocument != null)
			{
				CurrentDocument.OnDeactivated();
				if (CurrentDocument is SchemaView)
					toolBoxView.ToolActivated -= (CurrentDocument as SchemaView).OnToolActivated;
			}
		}
    }
}
