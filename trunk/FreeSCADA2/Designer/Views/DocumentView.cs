using System;
using WeifenLuo.WinFormsUI.Docking;

namespace FreeSCADA.Designer.Views
{
	abstract class DocumentView : DockContent
	{
        public delegate void ObjectSelectedDelegate(object sender);
        public event ObjectSelectedDelegate ObjectSelected;

		public DocumentView()
		{
			DockAreas = DockAreas.Float | DockAreas.Document;
			TabText = "Document";
		}

		public virtual bool IsModified
		{
			get { return false; }
		}

		public virtual string ProjectEntityName
		{
			get { return TabText; }
			set { TabText = value; }
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

		public virtual void OnSave()
		{
		}

        public void RaiseObjectSelected(object sender )
        {
			if(ObjectSelected != null)
				ObjectSelected(sender);
        }

	}
}
