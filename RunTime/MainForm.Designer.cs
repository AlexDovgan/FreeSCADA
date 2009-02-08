namespace FreeSCADA.RunTime
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.ToolStripButton toolStripButton1;
			this.mainToolbar = new System.Windows.Forms.ToolStrip();
			this.refreshButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.runButton = new System.Windows.Forms.ToolStripButton();
			this.stopButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.showTableButton = new System.Windows.Forms.ToolStripButton();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.mainMenu = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewSubMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
			this.mRUstartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.mainToolbar.SuspendLayout();
			this.mainMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStripButton1
			// 
			toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			toolStripButton1.Image = global::FreeSCADA.RunTime.Properties.Resources.open_file;
			toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			toolStripButton1.Name = "toolStripButton1";
			toolStripButton1.Size = new System.Drawing.Size(23, 22);
			toolStripButton1.Text = "Open Project";
			toolStripButton1.Click += new System.EventHandler(this.OnLoadProjectClick);
			// 
			// mainToolbar
			// 
			this.mainToolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.mainToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            toolStripButton1,
            this.refreshButton,
            this.toolStripSeparator1,
            this.runButton,
            this.stopButton,
            this.toolStripSeparator4,
            this.showTableButton});
			this.mainToolbar.Location = new System.Drawing.Point(0, 24);
			this.mainToolbar.Name = "mainToolbar";
			this.mainToolbar.Size = new System.Drawing.Size(513, 25);
			this.mainToolbar.TabIndex = 9;
			this.mainToolbar.Text = "mainToolbar";
			// 
			// refreshButton
			// 
			this.refreshButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.refreshButton.Image = global::FreeSCADA.RunTime.Properties.Resources.refresh;
			this.refreshButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.refreshButton.Name = "refreshButton";
			this.refreshButton.Size = new System.Drawing.Size(23, 22);
			this.refreshButton.Text = "Refresh";
			this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// runButton
			// 
			this.runButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.runButton.Image = global::FreeSCADA.RunTime.Properties.Resources.run;
			this.runButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.runButton.Name = "runButton";
			this.runButton.Size = new System.Drawing.Size(23, 22);
			this.runButton.Text = "Start Project";
			this.runButton.Click += new System.EventHandler(this.OnRunClick);
			// 
			// stopButton
			// 
			this.stopButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.stopButton.Enabled = false;
			this.stopButton.Image = global::FreeSCADA.RunTime.Properties.Resources.stop;
			this.stopButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.stopButton.Name = "stopButton";
			this.stopButton.Size = new System.Drawing.Size(23, 22);
			this.stopButton.Text = "Stop Project";
			this.stopButton.Click += new System.EventHandler(this.OnStopClick);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
			// 
			// showTableButton
			// 
			this.showTableButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.showTableButton.Image = global::FreeSCADA.RunTime.Properties.Resources.db_table;
			this.showTableButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.showTableButton.Name = "showTableButton";
			this.showTableButton.Size = new System.Drawing.Size(23, 22);
			this.showTableButton.Text = "toolStripButton2";
			this.showTableButton.Click += new System.EventHandler(this.showTableButton_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Location = new System.Drawing.Point(0, 324);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(513, 22);
			this.statusStrip1.TabIndex = 6;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// mainMenu
			// 
			this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewSubMenu});
			this.mainMenu.Location = new System.Drawing.Point(0, 0);
			this.mainMenu.Name = "mainMenu";
			this.mainMenu.Size = new System.Drawing.Size(513, 24);
			this.mainMenu.TabIndex = 5;
			this.mainMenu.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.toolStripSeparator2,
            this.mRUstartToolStripMenuItem,
            this.toolStripSeparator3,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// loadToolStripMenuItem
			// 
			this.loadToolStripMenuItem.Image = global::FreeSCADA.RunTime.Properties.Resources.open_file;
			this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
			this.loadToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.loadToolStripMenuItem.Text = "Load project...";
			this.loadToolStripMenuItem.Click += new System.EventHandler(this.OnLoadProjectClick);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.OnMenuExitClick);
			// 
			// viewSubMenu
			// 
			this.viewSubMenu.Name = "viewSubMenu";
			this.viewSubMenu.Size = new System.Drawing.Size(44, 20);
			this.viewSubMenu.Text = "View";
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
			this.dockPanel.Size = new System.Drawing.Size(513, 275);
			this.dockPanel.TabIndex = 10;
			// 
			// mRUstartToolStripMenuItem
			// 
			this.mRUstartToolStripMenuItem.Name = "mRUstartToolStripMenuItem";
			this.mRUstartToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.mRUstartToolStripMenuItem.Text = "MRU_start";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(149, 6);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(513, 346);
			this.Controls.Add(this.dockPanel);
			this.Controls.Add(this.mainToolbar);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.mainMenu);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
			this.Text = "RunTime";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
			this.mainToolbar.ResumeLayout(false);
			this.mainToolbar.PerformLayout();
			this.mainMenu.ResumeLayout(false);
			this.mainMenu.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.MenuStrip mainMenu;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
		private System.Windows.Forms.ToolStripButton runButton;
		private System.Windows.Forms.ToolStripButton stopButton;
        private System.Windows.Forms.ToolStripButton refreshButton;
		private System.Windows.Forms.ToolStrip mainToolbar;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripButton showTableButton;
		private System.Windows.Forms.ToolStripMenuItem viewSubMenu;
		private System.Windows.Forms.ToolStripMenuItem mRUstartToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
	}
}

