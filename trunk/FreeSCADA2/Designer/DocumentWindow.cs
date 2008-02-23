using System;
using WeifenLuo.WinFormsUI.Docking;

namespace FreeSCADA.Designer
{
	abstract class DocumentWindow : DockContent
	{
        public delegate void ObjectSelectedDelegate(object sender);
        public event ObjectSelectedDelegate ObjectSelected;

		public DocumentWindow()
		{
			DockAreas = DockAreas.Float | DockAreas.Document;
			TabText = "Document";
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
        public void RaiseObjectSelected(object sender )
        {
            ObjectSelected(sender);
        }

	}
}
