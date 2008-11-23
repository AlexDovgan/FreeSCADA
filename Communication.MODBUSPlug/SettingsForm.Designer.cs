namespace FreeSCADA.Communication.MODBUSPlug
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
            this.grid = new SourceGrid.Grid();
            this.addButton = new System.Windows.Forms.Button();
            this.removeVarButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.AddVarButton = new System.Windows.Forms.Button();
            this.stationGrid = new SourceGrid.Grid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.removeStatButton = new System.Windows.Forms.Button();
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
            this.grid.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grid.Size = new System.Drawing.Size(530, 189);
            this.grid.TabIndex = 0;
            this.grid.TabStop = true;
            this.grid.ToolTipText = "";
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Location = new System.Drawing.Point(12, 341);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(80, 23);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "Add Station";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.OnAddStation);
            // 
            // removeVarButton
            // 
            this.removeVarButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeVarButton.Location = new System.Drawing.Point(283, 341);
            this.removeVarButton.Name = "removeVarButton";
            this.removeVarButton.Size = new System.Drawing.Size(80, 23);
            this.removeVarButton.TabIndex = 1;
            this.removeVarButton.Text = "Remove Var";
            this.removeVarButton.UseVisualStyleBackColor = true;
            this.removeVarButton.Click += new System.EventHandler(this.OnRemoveVariable);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(379, 341);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(80, 23);
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
            this.cancelButton.Size = new System.Drawing.Size(80, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.OnCancelClick);
            // 
            // AddVarButton
            // 
            this.AddVarButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddVarButton.Location = new System.Drawing.Point(198, 341);
            this.AddVarButton.Name = "AddVarButton";
            this.AddVarButton.Size = new System.Drawing.Size(80, 23);
            this.AddVarButton.TabIndex = 4;
            this.AddVarButton.Text = "Add Variable";
            this.AddVarButton.UseVisualStyleBackColor = true;
            this.AddVarButton.Click += new System.EventHandler(this.OnAddVariable);
            // 
            // stationGrid
            // 
            this.stationGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.stationGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.stationGrid.ColumnsCount = 3;
            this.stationGrid.FixedRows = 1;
            this.stationGrid.Location = new System.Drawing.Point(3, 3);
            this.stationGrid.Name = "stationGrid";
            this.stationGrid.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.stationGrid.RowsCount = 1;
            this.stationGrid.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.stationGrid.Size = new System.Drawing.Size(530, 107);
            this.stationGrid.TabIndex = 5;
            this.stationGrid.TabStop = true;
            this.stationGrid.ToolTipText = "";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.stationGrid);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grid);
            this.splitContainer1.Size = new System.Drawing.Size(536, 312);
            this.splitContainer1.SplitterDistance = 113;
            this.splitContainer1.TabIndex = 6;
            // 
            // removeStatButton
            // 
            this.removeStatButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeStatButton.Location = new System.Drawing.Point(98, 341);
            this.removeStatButton.Name = "removeStatButton";
            this.removeStatButton.Size = new System.Drawing.Size(80, 23);
            this.removeStatButton.TabIndex = 7;
            this.removeStatButton.Text = "Remove Stat";
            this.removeStatButton.UseVisualStyleBackColor = true;
            this.removeStatButton.Click += new System.EventHandler(this.OnRemoveStation);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 385);
            this.Controls.Add(this.removeStatButton);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.AddVarButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.removeVarButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.addButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(568, 412);
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MODBUS Settings";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private SourceGrid.Grid grid;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.Button removeVarButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button AddVarButton;
        private SourceGrid.Grid stationGrid;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button removeStatButton;
	}
}