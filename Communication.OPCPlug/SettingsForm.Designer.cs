namespace FreeSCADA.Communication.OPCPlug
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
			this.removeButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// grid
			// 
			this.grid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.grid.ColumnsCount = 3;
			this.grid.FixedRows = 1;
			this.grid.Location = new System.Drawing.Point(12, 12);
			this.grid.Name = "grid";
			this.grid.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
			this.grid.RowsCount = 1;
			this.grid.SelectionMode = SourceGrid.GridSelectionMode.Cell;
			this.grid.Size = new System.Drawing.Size(528, 323);
			this.grid.TabIndex = 0;
			this.grid.TabStop = true;
			this.grid.ToolTipText = "";
			// 
			// addButton
			// 
			this.addButton.Location = new System.Drawing.Point(12, 341);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(75, 23);
			this.addButton.TabIndex = 1;
			this.addButton.Text = "Add";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler(this.OnImport);
			// 
			// removeButton
			// 
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
			this.cancelButton.Location = new System.Drawing.Point(465, 341);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 3;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.OnCancelClick);
			// 
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(552, 376);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.removeButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.addButton);
			this.Controls.Add(this.grid);
			this.Name = "SettingsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "SettingsForm";
			this.ResumeLayout(false);

		}

		#endregion

		private SourceGrid.Grid grid;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
	}
}