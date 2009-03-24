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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mRU1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.importSchemaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewSubMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.projectContentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.variablesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mediaContentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.editSubMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.mainToolbar = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.runButton = new System.Windows.Forms.ToolStripButton();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            newProjectButton = new System.Windows.Forms.ToolStripButton();
            openProjectButton = new System.Windows.Forms.ToolStripButton();
            toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            toolStripButtonNewSchema = new System.Windows.Forms.ToolStripButton();
            toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.mainMenu.SuspendLayout();
            this.mainToolbar.SuspendLayout();
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
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 386);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(699, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.toolStripMenuItem1,
            this.mRU1ToolStripMenuItem,
            this.toolStripSeparator2,
            this.importSchemaToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::FreeSCADA.Designer.Properties.Resources.save_file;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.saveToolStripMenuItem.Text = "Save project";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.OnSaveProjectClick);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Image = global::FreeSCADA.Designer.Properties.Resources.save_file;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.saveAsToolStripMenuItem.Text = "Save project As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Image = global::FreeSCADA.Designer.Properties.Resources.open_file;
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.loadToolStripMenuItem.Text = "Load project...";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.OnLoadProjectClick);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(158, 6);
            // 
            // mRU1ToolStripMenuItem
            // 
            this.mRU1ToolStripMenuItem.Name = "mRU1ToolStripMenuItem";
            this.mRU1ToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.mRU1ToolStripMenuItem.Text = "MRU_start";
            this.mRU1ToolStripMenuItem.Visible = false;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(158, 6);
            // 
            // importSchemaToolStripMenuItem
            // 
            this.importSchemaToolStripMenuItem.Name = "importSchemaToolStripMenuItem";
            this.importSchemaToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.importSchemaToolStripMenuItem.Text = "Import Graphics";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.OnMenuExitClick);
            // 
            // viewSubMenu
            // 
            this.viewSubMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.projectContentToolStripMenuItem});
            this.viewSubMenu.Name = "viewSubMenu";
            this.viewSubMenu.Size = new System.Drawing.Size(41, 20);
            this.viewSubMenu.Text = "View";
            // 
            // projectContentToolStripMenuItem
            // 
            this.projectContentToolStripMenuItem.Name = "projectContentToolStripMenuItem";
            this.projectContentToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.projectContentToolStripMenuItem.Text = "Project Content";
            // 
            // projectToolStripMenuItem
            // 
            this.projectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.variablesToolStripMenuItem,
            this.mediaContentToolStripMenuItem});
            this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
            this.projectToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.projectToolStripMenuItem.Text = "Project";
            // 
            // variablesToolStripMenuItem
            // 
            this.variablesToolStripMenuItem.Name = "variablesToolStripMenuItem";
            this.variablesToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.variablesToolStripMenuItem.Text = "Variables...";
            this.variablesToolStripMenuItem.Click += new System.EventHandler(this.OnMenuVariables);
            // 
            // mediaContentToolStripMenuItem
            // 
            this.mediaContentToolStripMenuItem.Name = "mediaContentToolStripMenuItem";
            this.mediaContentToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.mediaContentToolStripMenuItem.Text = "Media content...";
            this.mediaContentToolStripMenuItem.Click += new System.EventHandler(this.OnMenuMediaContent);
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editSubMenu,
            this.viewSubMenu,
            this.projectToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(699, 24);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "menuStrip1";
            // 
            // editSubMenu
            // 
            this.editSubMenu.Name = "editSubMenu";
            this.editSubMenu.Size = new System.Drawing.Size(37, 20);
            this.editSubMenu.Text = "Edit";
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // TopToolStripPanel
            // 
            this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // mainToolbar
            // 
            this.mainToolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mainToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            newProjectButton,
            openProjectButton,
            toolStripButton3,
            this.toolStripSeparator1,
            toolStripButtonNewSchema,
            toolStripButton5,
            this.toolStripButton1,
            this.toolStripButton4,
            this.toolStripButton2,
            this.runButton});
            this.mainToolbar.Location = new System.Drawing.Point(0, 24);
            this.mainToolbar.Name = "mainToolbar";
            this.mainToolbar.Size = new System.Drawing.Size(699, 25);
            this.mainToolbar.TabIndex = 3;
            this.mainToolbar.Text = "mainToolbar";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::FreeSCADA.Designer.Properties.Resources.db_settings;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Archiver Settings";
            this.toolStripButton1.Click += new System.EventHandler(this.OnArchiverSettingsClick);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::FreeSCADA.Designer.Properties.Resources.script_add;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "Add new script";
            this.toolStripButton2.Click += new System.EventHandler(this.OnAddNewScriptClick);
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
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // ContentPanel
            // 
            this.ContentPanel.Size = new System.Drawing.Size(699, 383);
            // 
            // dockPanel
            // 
            this.dockPanel.ActiveAutoHideContent = null;
            this.dockPanel.AllowDrop = true;
            this.dockPanel.DefaultFloatWindowSize = new System.Drawing.Size(200, 300);
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.DockBottomPortion = 0.15;
            this.dockPanel.DockLeftPortion = 0.15;
            this.dockPanel.DockRightPortion = 0.15;
            this.dockPanel.DockTopPortion = 0.15;
            this.dockPanel.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
            this.dockPanel.Location = new System.Drawing.Point(0, 49);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.Size = new System.Drawing.Size(699, 337);
            this.dockPanel.TabIndex = 4;
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = global::FreeSCADA.Designer.Properties.Resources.tree_channels;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton4.Text = "Channels and Plugins";
            this.toolStripButton4.Click += new System.EventHandler(this.OnVariablesViewClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 408);
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.mainToolbar);
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.statusStrip1);
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "Designer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.mainToolbar.ResumeLayout(false);
            this.mainToolbar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewSubMenu;
		private System.Windows.Forms.ToolStripMenuItem projectContentToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem variablesToolStripMenuItem;
		private System.Windows.Forms.MenuStrip mainMenu;
		private System.Windows.Forms.ToolStripMenuItem importSchemaToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editSubMenu;
		private System.Windows.Forms.ToolStrip mainToolbar;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton runButton;
		private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
		private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
		private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
		private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
		private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
		private System.Windows.Forms.ToolStripContentPanel ContentPanel;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.ToolStripMenuItem mediaContentToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mRU1ToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
	}
}
