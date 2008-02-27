namespace FreeSCADA.Designer.Dialogs
{
	partial class VariablesDialog
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
			this.channelsGrid = new SourceGrid.Grid();
			this.button1 = new System.Windows.Forms.Button();
			this.connectCheckBox = new System.Windows.Forms.CheckBox();
			this.connectionStatusLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// channelsGrid
			// 
			this.channelsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.channelsGrid.AutoStretchColumnsToFitWidth = true;
			this.channelsGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.channelsGrid.Location = new System.Drawing.Point(12, 12);
			this.channelsGrid.Name = "channelsGrid";
			this.channelsGrid.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
			this.channelsGrid.SelectionMode = SourceGrid.GridSelectionMode.Row;
			this.channelsGrid.Size = new System.Drawing.Size(607, 288);
			this.channelsGrid.TabIndex = 0;
			this.channelsGrid.TabStop = true;
			this.channelsGrid.ToolTipText = "";
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(544, 329);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "Close";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.OnCloseButton);
			// 
			// connectCheckBox
			// 
			this.connectCheckBox.AutoSize = true;
			this.connectCheckBox.Location = new System.Drawing.Point(12, 306);
			this.connectCheckBox.Name = "autorefreshCheckBox";
			this.connectCheckBox.Size = new System.Drawing.Size(84, 17);
			this.connectCheckBox.TabIndex = 2;
			this.connectCheckBox.Text = "Autorefresh";
			this.connectCheckBox.UseVisualStyleBackColor = true;
			this.connectCheckBox.CheckedChanged += new System.EventHandler(this.OnConnectChanged);
			// 
			// connectionStatusLabel
			// 
			this.connectionStatusLabel.AutoSize = true;
			this.connectionStatusLabel.Location = new System.Drawing.Point(12, 334);
			this.connectionStatusLabel.Name = "connectionStatusLabel";
			this.connectionStatusLabel.Size = new System.Drawing.Size(94, 13);
			this.connectionStatusLabel.TabIndex = 3;
			this.connectionStatusLabel.Text = "Connection status";
			// 
			// VariablesDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(631, 364);
			this.Controls.Add(this.connectionStatusLabel);
			this.Controls.Add(this.connectCheckBox);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.channelsGrid);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "VariablesForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "VariablesForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VariablesForm_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private SourceGrid.Grid channelsGrid;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.CheckBox connectCheckBox;
		private System.Windows.Forms.Label connectionStatusLabel;

	}
}