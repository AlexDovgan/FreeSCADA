using System;
using FreeSCADA.Schema.ShortProperties;

namespace FreeSCADA.Designer.Views
{
    class PropertyBrowserView : ToolWindow
    {
        private System.Windows.Forms.PropertyGrid propertyGrid;

        public PropertyBrowserView()
		{
			TabText = "Property Browser";
            InitializeComponent();
		}

        private void InitializeComponent()
        {
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(292, 273);
            this.propertyGrid.TabIndex = 0;
            // 
            // PropertyBrowserView
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.propertyGrid);
            this.Name = "PropertyBrowserView";
            this.ResumeLayout(false);

        }
        public void ShowProperties(object obj)
        {
            if (propertyGrid.SelectedObject is IDisposable)
                (propertyGrid.SelectedObject as IDisposable).Dispose();

            propertyGrid.SelectedObject = obj;
            if(obj is CommonShortProp)
                (obj as CommonShortProp).PropertiesChanged += new CommonShortProp.PropertiesChangedDelegate(PropertyBrowserView_PropertiesChanged);
        }

		delegate void InvokeDelegate();
        void PropertyBrowserView_PropertiesChanged()
        {
			propertyGrid.BeginInvoke(new InvokeDelegate(delegate() { propertyGrid.Refresh(); }));
        }
	}

}
