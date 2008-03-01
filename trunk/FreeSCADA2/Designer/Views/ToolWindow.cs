using WeifenLuo.WinFormsUI.Docking;

namespace FreeSCADA.Designer.Views
{
	class ToolWindow : DockContent
    {
        public ToolWindow()
		{
			DockAreas = DockAreas.Float | DockAreas.DockBottom | DockAreas.DockLeft | DockAreas.DockRight;
			TabText = "ToolWindow";
		}
	}
}
