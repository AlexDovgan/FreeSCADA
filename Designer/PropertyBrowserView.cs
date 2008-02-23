using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FreeSCADA.Designer
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
            propertyGrid.SelectedObject = obj;
        }
	}

}
