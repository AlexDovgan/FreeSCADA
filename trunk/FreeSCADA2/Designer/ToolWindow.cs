using WeifenLuo.WinFormsUI.Docking;

namespace FreeSCADA.Designer
{
	class ToolWindow : DockContent
    {
        public ToolWindow()
		{
			DockAreas = DockAreas.Float | DockAreas.DockBottom | DockAreas.DockLeft | DockAreas.DockRight;
			TabText = "ToolWindow";
            
		}

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ToolWindow
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Name = "ToolWindow";
            this.ResumeLayout(false);

        }
	}
}
