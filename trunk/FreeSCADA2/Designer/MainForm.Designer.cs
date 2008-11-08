namespace FreeSCADA.Designer
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
        }

		#region Windows Form FreeSCADA.Designer generated code

		/// <summary>
		/// Required method for FreeSCADA.Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.ToolStripButton newProjectButton;
			System.Windows.Forms.ToolStripButton openProjectButton;
			System.Windows.Forms.ToolStripButton toolStripButton3;
			System.Windows.Forms.ToolStripButton toolStripButtonNewSchema;
			System.Windows.Forms.ToolStripButton toolStripButton5;
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.runButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.zoomLevelComboBox = new System.Windows.Forms.ToolStripComboBox();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.importSchemaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.projectContentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.variablesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mainMenu = new System.Windows.Forms.MenuStrip();
			newProjectButton = new System.Windows.Forms.ToolStripButton();
			openProjectButton = new System.Windows.Forms.ToolStripButton();
			toolStripButton3 = new System.Windows.Forms.ToolStripButton();
			toolStripButtonNewSchema = new System.Windows.Forms.ToolStripButton();
			toolStripButton5 = new System.Windows.Forms.ToolStripButton();
			this.toolStrip1.SuspendLayout();
			this.mainMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// newProjectButton
			// 
			newProjectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			newProjectButton.Image = global::FreeSCADA.Designer.Properties.Resources.new_file;
			newProjectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			newProjectButton.Name = "newProjectButton";
			newProjectButton.Size = new System.Drawing.Size(23, 22);
			newProjectButton.Text = "New Project";
			newProjectButton.Click += new System.EventHandler(this.OnNewProjectClick);
			// 
			// openProjectButton
			// 
			openProjectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			openProjectButton.Image = global::FreeSCADA.Designer.Properties.Resources.open_file;
			openProjectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			openProjectButton.Name = "openProjectButton";
			openProjectButton.Size = new System.Drawing.Size(23, 22);
			openProjectButton.Text = "Open Project";
			openProjectButton.Click += new System.EventHandler(this.OnLoadProjectClick);
			// 
			// toolStripButton3
			// 
			toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			toolStripButton3.Image = global::FreeSCADA.Designer.Properties.Resources.disk;
			toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
			toolStripButton3.Name = "toolStripButton3";
			toolStripButton3.Size = new System.Drawing.Size(23, 22);
			toolStripButton3.Text = "Save Project";
			toolStripButton3.Click += new System.EventHandler(this.OnSaveFileClick);
			// 
			// toolStripButtonNewSchema
			// 
			toolStripButtonNewSchema.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			toolStripButtonNewSchema.Image = global::FreeSCADA.Designer.Properties.Resources.page_white_add;
			toolStripButtonNewSchema.ImageTransparentColor = System.Drawing.Color.Magenta;
			toolStripButtonNewSchema.Name = "toolStripButtonNewSchema";
			toolStripButtonNewSchema.Size = new System.Drawing.Size(23, 22);
			toolStripButtonNewSchema.Text = "New Schema";
			toolStripButtonNewSchema.Click += new System.EventHandler(this.OnSchemaItemClick);
			// 
			// toolStripButton5
			// 
			toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			toolStripButton5.Image = global::FreeSCADA.Designer.Properties.Resources.table_add;
			toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
			toolStripButton5.Name = "toolStripButton5";
			toolStripButton5.Size = new System.Drawing.Size(23, 22);
			toolStripButton5.Text = "New Event List";
			toolStripButton5.Click += new System.EventHandler(this.OnEventsItemClick);
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            newProjectButton,
            openProjectButton,
            toolStripButton3,
            this.toolStripSeparator1,
            toolStripButtonNewSchema,
            toolStripButton5,
            this.runButton,
            this.toolStripSeparator4,
            this.zoomLevelComboBox});
			this.toolStrip1.Location = new System.Drawing.Point(0, 24);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(599, 25);
			this.toolStrip1.TabIndex = 3;
			this.toolStrip1.Text = "mainToolbar";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// runButton
			// 
			this.runButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.runButton.Image = global::FreeSCADA.Designer.Properties.Resources.run;
			this.runButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.runButton.Name = "runButton";
			this.runButton.Size = new System.Drawing.Size(23, 22);
			this.runButton.Text = "Run Runtime";
			this.runButton.Click += new System.EventHandler(this.runButton_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
			// 
			// zoomLevelComboBox
			// 
			this.zoomLevelComboBox.Items.AddRange(new object[] {
            "Home (100%)",
            "50%",
            "150%"});
			this.zoomLevelComboBox.Name = "zoomLevelComboBox";
			this.zoomLevelComboBox.Size = new System.Drawing.Size(121, 25);
			this.zoomLevelComboBox.ToolTipText = "Zoom Level";
			this.zoomLevelComboBox.SelectedIndexChanged += new System.EventHandler(this.zoomLevelComboBox_SelectedIndexChanged);
			this.zoomLevelComboBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.zoomLevelComboBox_KeyUp);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Location = new System.Drawing.Point(0, 362);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(599, 22);
			this.statusStrip1.TabIndex = 1;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// dockPanel
			// 
			this.dockPanel.ActiveAutoHideContent = null;
			this.dockPanel.DefaultFloatWindowSize = new System.Drawing.Size(200, 300);
			this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dockPanel.DockBottomPortion = 0.15;
			this.dockPanel.DockLeftPortion = 0.15;
			this.dockPanel.DockRightPortion = 0.15;
			this.dockPanel.DockTopPortion = 0.15;
			this.dockPanel.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
			this.dockPanel.Location = new System.Drawing.Point(0, 49);
			this.dockPanel.Name = "dockPanel";
			this.dockPanel.Size = new System.Drawing.Size(599, 313);
			this.dockPanel.TabIndex = 4;
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.toolStripSeparator2,
            this.importSchemaToolStripMenuItem,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Image = global::FreeSCADA.Designer.Properties.Resources.save_file;
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
			this.saveToolStripMenuItem.Text = "Save project";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.OnSaveProjectClick);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Image = global::FreeSCADA.Designer.Properties.Resources.save_file;
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
			this.saveAsToolStripMenuItem.Text = "Save project As";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
			// 
			// loadToolStripMenuItem
			// 
			this.loadToolStripMenuItem.Image = global::FreeSCADA.Designer.Properties.Resources.open_file;
			this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
			this.loadToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
			this.loadToolStripMenuItem.Text = "Load project...";
			this.loadToolStripMenuItem.Click += new System.EventHandler(this.OnLoadProjectClick);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(156, 6);
			// 
			// importSchemaToolStripMenuItem
			// 
			this.importSchemaToolStripMenuItem.Name = "importSchemaToolStripMenuItem";
			this.importSchemaToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
			this.importSchemaToolStripMenuItem.Text = "Import Graphics";
			this.importSchemaToolStripMenuItem.Click += new System.EventHandler(this.importSchemaToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.OnMenuExitClick);
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.projectContentToolStripMenuItem});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.viewToolStripMenuItem.Text = "View";
			// 
			// projectContentToolStripMenuItem
			// 
			this.projectContentToolStripMenuItem.Name = "projectContentToolStripMenuItem";
			this.projectContentToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
			this.projectContentToolStripMenuItem.Text = "Project Content";
			// 
			// projectToolStripMenuItem
			// 
			this.projectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.variablesToolStripMenuItem});
			this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
			this.projectToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
			this.projectToolStripMenuItem.Text = "Project";
			// 
			// variablesToolStripMenuItem
			// 
			this.variablesToolStripMenuItem.Name = "variablesToolStripMenuItem";
			this.variablesToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
			this.variablesToolStripMenuItem.Text = "Variables...";
			this.variablesToolStripMenuItem.Click += new System.EventHandler(this.OnMenuVariables);
			// 
			// mainMenu
			// 
			this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.projectToolStripMenuItem});
			this.mainMenu.Location = new System.Drawing.Point(0, 0);
			this.mainMenu.Name = "mainMenu";
			this.mainMenu.Size = new System.Drawing.Size(599, 24);
			this.mainMenu.TabIndex = 0;
			this.mainMenu.Text = "menuStrip1";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(599, 384);
			this.Controls.Add(this.dockPanel);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.mainMenu);
			this.MainMenuStrip = this.mainMenu;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
			this.Text = "Designer";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.mainMenu.ResumeLayout(false);
			this.mainMenu.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem projectContentToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem variablesToolStripMenuItem;
		private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem importSchemaToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox zoomLevelComboBox;
        private System.Windows.Forms.ToolStripButton runButton;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStrip toolStrip1;
	}
}
