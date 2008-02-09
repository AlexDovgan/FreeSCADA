using WeifenLuo.WinFormsUI.Docking;

namespace FreeSCADA.Designer
{
	class DocumentWindow : DockContent
	{
		public DocumentWindow()
		{
			DockAreas = DockAreas.Float | DockAreas.Document;
			TabText = "Document";
		}
	}
}
