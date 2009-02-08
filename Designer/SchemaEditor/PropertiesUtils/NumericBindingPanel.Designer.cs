namespace FreeSCADA.Designer.SchemaEditor.PropertiesUtils
{
	partial class NumericBindingPanel
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
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.maxEdit = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.minEdit = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.maxEdit)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.minEdit)).BeginInit();
			this.SuspendLayout();
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(4, 4);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(96, 17);
			this.checkBox1.TabIndex = 0;
			this.checkBox1.Text = "Use constaints";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.maxEdit);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.minEdit);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Enabled = false;
			this.groupBox1.Location = new System.Drawing.Point(4, 28);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(288, 77);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Constraints";
			// 
			// maxEdit
			// 
			this.maxEdit.DecimalPlaces = 2;
			this.maxEdit.Location = new System.Drawing.Point(37, 46);
			this.maxEdit.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.maxEdit.Minimum = new decimal(new int[] {
            65535,
            0,
            0,
            -2147483648});
			this.maxEdit.Name = "maxEdit";
			this.maxEdit.Size = new System.Drawing.Size(84, 20);
			this.maxEdit.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(27, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Max";
			// 
			// minEdit
			// 
			this.minEdit.DecimalPlaces = 2;
			this.minEdit.Location = new System.Drawing.Point(37, 20);
			this.minEdit.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.minEdit.Minimum = new decimal(new int[] {
            65535,
            0,
            0,
            -2147483648});
			this.minEdit.Name = "minEdit";
			this.minEdit.Size = new System.Drawing.Size(84, 20);
			this.minEdit.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(23, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Min";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(4, 112);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(50, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Channel:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(60, 112);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(35, 13);
			this.label4.TabIndex = 3;
			this.label4.Text = "label4";
			// 
			// NumericBindingPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.checkBox1);
			this.Name = "NumericBindingPanel";
			this.Size = new System.Drawing.Size(295, 136);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.maxEdit)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.minEdit)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.NumericUpDown minEdit;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown maxEdit;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
	}
}
