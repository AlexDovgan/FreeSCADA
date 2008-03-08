using System;
using WeifenLuo.WinFormsUI.Docking;

namespace FreeSCADA.Designer.Views
{
	class DocumentView : DockContent
	{
        public delegate void ObjectSelectedDelegate(object sender);
        public event ObjectSelectedDelegate ObjectSelected;
		string documentName="";
		bool modifiedFlag = false;
		bool handleModifiedFlagOnClose = true;

		public DocumentView()
		{
			DockAreas = DockAreas.Float | DockAreas.Document;
			documentName = "Document";
			UpdateCaption();
		}

		public string DocumentName
		{
			get { return documentName; }
			set { documentName = value; UpdateCaption();}
		}

		/// <summary>
		/// This property should be set to "true" for new documents and set to "false" after saving the document.
		/// </summary>
		public virtual bool IsModified
		{
			get { return modifiedFlag; }
			set { modifiedFlag = value; UpdateCaption(); }
		}

		public bool HandleModifiedOnClose
		{
			get { return handleModifiedFlagOnClose; }
			set { handleModifiedFlagOnClose = value; }
		}

        public virtual void OnToolActivated(object sender, Type tool)
        {
        }

		public virtual void OnActivated()
		{
		}

		public virtual void OnDeactivated()
		{
		}

		public virtual bool SaveDocument()
		{
			return false;
		}

		public virtual bool LoadDocument(string name)
        {
			return false;
        }

        public virtual bool CreateNewDocument()
        {
			return false;
        }

        public void RaiseObjectSelected(object sender )
        {
			if(ObjectSelected != null)
				ObjectSelected(sender);
        }

		private void UpdateCaption()
		{
			TabText = DocumentName;
			if (IsModified)
				TabText += " *";
		}
	}
}
