namespace FreeSCADA.Designer.SchemaEditor.PropertiesUtils
{
	partial class StringBindingPanel
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.expressionEdit = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.channelsGrid = new SourceGrid.Grid();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(4, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(59, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Expression";
			// 
			// expressionEdit
			// 
			this.expressionEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.expressionEdit.Location = new System.Drawing.Point(7, 21);
			this.expressionEdit.Multiline = true;
			this.expressionEdit.Name = "expressionEdit";
			this.expressionEdit.Size = new System.Drawing.Size(434, 75);
			this.expressionEdit.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(4, 99);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(82, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Active channels";
			// 
			// channelsGrid
			// 
			this.channelsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.channelsGrid.AutoStretchColumnsToFitWidth = true;
			this.channelsGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.channelsGrid.Location = new System.Drawing.Point(7, 115);
			this.channelsGrid.Name = "channelsGrid";
			this.channelsGrid.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
			this.channelsGrid.SelectionMode = SourceGrid.GridSelectionMode.Row;
			this.channelsGrid.Size = new System.Drawing.Size(434, 109);
			this.channelsGrid.TabIndex = 2;
			this.channelsGrid.TabStop = true;
			this.channelsGrid.ToolTipText = "";
			// 
			// StringBindingPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.channelsGrid);
			this.Controls.Add(this.expressionEdit);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "StringBindingPanel";
			this.Size = new System.Drawing.Size(444, 227);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox expressionEdit;
		private System.Windows.Forms.Label label2;
		private SourceGrid.Grid channelsGrid;
	}
}
