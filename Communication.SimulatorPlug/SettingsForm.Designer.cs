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
			this.expressionEditBox = new System.Windows.Forms.TextBox();
			this.expressionContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.codeTemplateButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.triggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.counterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.expressionContextMenu.SuspendLayout();
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
			this.grid.Location = new System.Drawing.Point(12, 12);
			this.grid.Name = "grid";
			this.grid.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
			this.grid.RowsCount = 1;
			this.grid.SelectionMode = SourceGrid.GridSelectionMode.Row;
			this.grid.Size = new System.Drawing.Size(528, 187);
			this.grid.TabIndex = 0;
			this.grid.TabStop = true;
			this.grid.ToolTipText = "";
			// 
			// addButton
			// 
			this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.addButton.Location = new System.Drawing.Point(12, 341);
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
			this.removeButton.Location = new System.Drawing.Point(93, 341);
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
			this.okButton.Location = new System.Drawing.Point(384, 341);
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
			this.cancelButton.Location = new System.Drawing.Point(465, 341);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 3;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.OnCancelClick);
			// 
			// expressionEditBox
			// 
			this.expressionEditBox.AcceptsReturn = true;
			this.expressionEditBox.AcceptsTab = true;
			this.expressionEditBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.expressionEditBox.Enabled = false;
			this.expressionEditBox.Location = new System.Drawing.Point(12, 234);
			this.expressionEditBox.Multiline = true;
			this.expressionEditBox.Name = "expressionEditBox";
			this.expressionEditBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.expressionEditBox.Size = new System.Drawing.Size(528, 101);
			this.expressionEditBox.TabIndex = 5;
			this.expressionEditBox.WordWrap = false;
			this.expressionEditBox.TextChanged += new System.EventHandler(this.expressionEditBox_TextChanged);
			// 
			// expressionContextMenu
			// 
			this.expressionContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.triggerToolStripMenuItem,
            this.counterToolStripMenuItem});
			this.expressionContextMenu.Name = "expressionContextMenu";
			this.expressionContextMenu.Size = new System.Drawing.Size(118, 48);
			// 
			// codeTemplateButton
			// 
			this.codeTemplateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.codeTemplateButton.Enabled = false;
			this.codeTemplateButton.Location = new System.Drawing.Point(384, 205);
			this.codeTemplateButton.Name = "codeTemplateButton";
			this.codeTemplateButton.Size = new System.Drawing.Size(156, 23);
			this.codeTemplateButton.TabIndex = 7;
			this.codeTemplateButton.Text = "Insert code template";
			this.codeTemplateButton.UseVisualStyleBackColor = true;
			this.codeTemplateButton.Click += new System.EventHandler(this.codeTemplateButton_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 215);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(63, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "Expression:";
			// 
			// triggerToolStripMenuItem
			// 
			this.triggerToolStripMenuItem.Name = "triggerToolStripMenuItem";
			this.triggerToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
			this.triggerToolStripMenuItem.Text = "Trigger";
			this.triggerToolStripMenuItem.Click += new System.EventHandler(this.triggerToolStripMenuItem_Click);
			// 
			// counterToolStripMenuItem
			// 
			this.counterToolStripMenuItem.Name = "counterToolStripMenuItem";
			this.counterToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
			this.counterToolStripMenuItem.Text = "Counter";
			this.counterToolStripMenuItem.Click += new System.EventHandler(this.counterToolStripMenuItem_Click);
			// 
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(552, 376);
			this.Controls.Add(this.expressionEditBox);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.removeButton);
			this.Controls.Add(this.codeTemplateButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.addButton);
			this.Controls.Add(this.grid);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(568, 412);
			this.Name = "SettingsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Simulator settings";
			this.expressionContextMenu.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private SourceGrid.Grid grid;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.TextBox expressionEditBox;
		private System.Windows.Forms.ContextMenuStrip expressionContextMenu;
		private System.Windows.Forms.Button codeTemplateButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ToolStripMenuItem triggerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem counterToolStripMenuItem;
	}
}