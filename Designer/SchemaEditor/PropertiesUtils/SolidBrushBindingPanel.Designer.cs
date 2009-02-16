namespace FreeSCADA.Designer.SchemaEditor.PropertiesUtils
{
	partial class SolidBrushBindingPanel
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.maxEdit = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.minEdit = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.endColorButton = new System.Windows.Forms.Button();
            this.startColorButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minEdit)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.maxEdit);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.minEdit);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(168, 77);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Extents";
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
            this.label1.Size = new System.Drawing.Size(24, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Min";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Channel:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(59, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "label4";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.endColorButton);
            this.groupBox2.Controls.Add(this.startColorButton);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(151, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(159, 77);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Color Ranges";
            // 
            // endColorButton
            // 
            this.endColorButton.Location = new System.Drawing.Point(70, 43);
            this.endColorButton.Name = "endColorButton";
            this.endColorButton.Size = new System.Drawing.Size(75, 23);
            this.endColorButton.TabIndex = 1;
            this.endColorButton.UseVisualStyleBackColor = true;
            this.endColorButton.Click += new System.EventHandler(this.button_Click);
            // 
            // startColorButton
            // 
            this.startColorButton.Location = new System.Drawing.Point(70, 17);
            this.startColorButton.Name = "startColorButton";
            this.startColorButton.Size = new System.Drawing.Size(75, 23);
            this.startColorButton.TabIndex = 1;
            this.startColorButton.UseVisualStyleBackColor = true;
            this.startColorButton.Click += new System.EventHandler(this.button_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Stop Color";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Start Color";
            // 
            // SolidBrushBindingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Name = "SolidBrushBindingPanel";
            this.Size = new System.Drawing.Size(324, 113);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minEdit)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.NumericUpDown minEdit;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown maxEdit;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button endColorButton;
        private System.Windows.Forms.Button startColorButton;
	}
}
