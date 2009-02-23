namespace FreeSCADA.Communication.SimulatorPlug
{
	partial class SettingsForm
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
            this.components = new System.ComponentModel.Container();
            this.grid = new SourceGrid.Grid();
            this.addButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.expressionEditBox = new Alsing.Windows.Forms.SyntaxBoxControl();
            this.syntaxDocument1 = new Alsing.SourceCode.SyntaxDocument(this.components);
            this.expressionContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.triggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.counterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.codeTemplateButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tipLabel = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.expressionContextMenu.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grid
            // 
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.grid.ColumnsCount = 3;
            this.grid.FixedRows = 1;
            this.grid.Location = new System.Drawing.Point(3, 3);
            this.grid.Name = "grid";
            this.grid.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grid.RowsCount = 1;
            this.grid.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.grid.Size = new System.Drawing.Size(609, 177);
            this.grid.TabIndex = 0;
            this.grid.TabStop = true;
            this.grid.ToolTipText = "";
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Location = new System.Drawing.Point(8, 158);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.OnAddRow);
            // 
            // removeButton
            // 
            this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeButton.Location = new System.Drawing.Point(89, 158);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(75, 23);
            this.removeButton.TabIndex = 1;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.OnRemoveRow);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(436, 158);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OnOkClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(517, 158);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.OnCancelClick);
            // 
            // expressionEditBox
            // 
            this.expressionEditBox.ActiveView = Alsing.Windows.Forms.ActiveView.BottomRight;
            this.expressionEditBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.expressionEditBox.AutoListPosition = null;
            this.expressionEditBox.AutoListSelectedText = "a123";
            this.expressionEditBox.AutoListVisible = false;
            this.expressionEditBox.BackColor = System.Drawing.Color.White;
            this.expressionEditBox.BorderStyle = Alsing.Windows.Forms.BorderStyle.None;
            this.expressionEditBox.CopyAsRTF = false;
            this.expressionEditBox.Document = this.syntaxDocument1;
            this.expressionEditBox.FontName = "Courier new";
            this.expressionEditBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.expressionEditBox.InfoTipCount = 1;
            this.expressionEditBox.InfoTipPosition = null;
            this.expressionEditBox.InfoTipSelectedIndex = 1;
            this.expressionEditBox.InfoTipVisible = false;
            this.expressionEditBox.Location = new System.Drawing.Point(0, 30);
            this.expressionEditBox.LockCursorUpdate = false;
            this.expressionEditBox.Name = "expressionEditBox";
            this.expressionEditBox.ShowScopeIndicator = false;
            this.expressionEditBox.Size = new System.Drawing.Size(615, 124);
            this.expressionEditBox.SmoothScroll = false;
            this.expressionEditBox.SplitviewH = -4;
            this.expressionEditBox.SplitviewV = -4;
            this.expressionEditBox.TabGuideColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(219)))), ((int)(((byte)(214)))));
            this.expressionEditBox.TabIndex = 5;
            this.expressionEditBox.WhitespaceColor = System.Drawing.SystemColors.ControlDark;
            this.expressionEditBox.TextChanged += new System.EventHandler(this.expressionEditBox_TextChanged);
            // 
            // syntaxDocument1
            // 
            this.syntaxDocument1.Lines = new string[] {
        ""};
            this.syntaxDocument1.MaxUndoBufferSize = 1000;
            this.syntaxDocument1.Modified = false;
            this.syntaxDocument1.UndoStep = 0;
            // 
            // expressionContextMenu
            // 
            this.expressionContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.triggerToolStripMenuItem,
            this.counterToolStripMenuItem});
            this.expressionContextMenu.Name = "expressionContextMenu";
            this.expressionContextMenu.Size = new System.Drawing.Size(125, 48);
            // 
            // triggerToolStripMenuItem
            // 
            this.triggerToolStripMenuItem.Name = "triggerToolStripMenuItem";
            this.triggerToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.triggerToolStripMenuItem.Text = "Trigger";
            this.triggerToolStripMenuItem.Click += new System.EventHandler(this.triggerToolStripMenuItem_Click);
            // 
            // counterToolStripMenuItem
            // 
            this.counterToolStripMenuItem.Name = "counterToolStripMenuItem";
            this.counterToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.counterToolStripMenuItem.Text = "Counter";
            this.counterToolStripMenuItem.Click += new System.EventHandler(this.counterToolStripMenuItem_Click);
            // 
            // codeTemplateButton
            // 
            this.codeTemplateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.codeTemplateButton.Enabled = false;
            this.codeTemplateButton.Location = new System.Drawing.Point(433, 4);
            this.codeTemplateButton.Name = "codeTemplateButton";
            this.codeTemplateButton.Size = new System.Drawing.Size(156, 23);
            this.codeTemplateButton.TabIndex = 7;
            this.codeTemplateButton.Text = "Insert code template";
            this.codeTemplateButton.UseVisualStyleBackColor = true;
            this.codeTemplateButton.Click += new System.EventHandler(this.codeTemplateButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Expression:";
            // 
            // tipLabel
            // 
            this.tipLabel.AutoSize = true;
            this.tipLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.tipLabel.ForeColor = System.Drawing.SystemColors.Desktop;
            this.tipLabel.Location = new System.Drawing.Point(114, 9);
            this.tipLabel.Name = "tipLabel";
            this.tipLabel.Size = new System.Drawing.Size(22, 13);
            this.tipLabel.TabIndex = 8;
            this.tipLabel.Text = "Tip";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(1, 1);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.grid);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.tipLabel);
            this.splitContainer1.Panel2.Controls.Add(this.addButton);
            this.splitContainer1.Panel2.Controls.Add(this.okButton);
            this.splitContainer1.Panel2.Controls.Add(this.expressionEditBox);
            this.splitContainer1.Panel2.Controls.Add(this.codeTemplateButton);
            this.splitContainer1.Panel2.Controls.Add(this.cancelButton);
            this.splitContainer1.Panel2.Controls.Add(this.removeButton);
            this.splitContainer1.Size = new System.Drawing.Size(615, 376);
            this.splitContainer1.SplitterDistance = 183;
            this.splitContainer1.TabIndex = 9;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 389);
            this.Controls.Add(this.splitContainer1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(568, 412);
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Simulator settings";
            this.expressionContextMenu.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private SourceGrid.Grid grid;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
        private Alsing.Windows.Forms.SyntaxBoxControl expressionEditBox;
		private System.Windows.Forms.ContextMenuStrip expressionContextMenu;
        private Alsing.SourceCode.SyntaxDocument syntaxDocument1;
		private System.Windows.Forms.Button codeTemplateButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ToolStripMenuItem triggerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem counterToolStripMenuItem;
        private System.Windows.Forms.Label tipLabel;
        private System.Windows.Forms.SplitContainer splitContainer1;
	}
}