namespace FreeSCADA.Communication.CLServer
{
	partial class ImportProgressForm
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
			this.statusLabel = new System.Windows.Forms.Label();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.SuspendLayout();
			// 
			// statusLabel
			// 
			this.statusLabel.AutoSize = true;
			this.statusLabel.Location = new System.Drawing.Point(13, 13);
			this.statusLabel.Name = "statusLabel";
			this.statusLabel.Size = new System.Drawing.Size(62, 13);
			this.statusLabel.TabIndex = 0;
			this.statusLabel.Text = "statusLabel";
			this.statusLabel.UseWaitCursor = true;
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(13, 30);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(445, 23);
			this.progressBar.TabIndex = 1;
			this.progressBar.UseWaitCursor = true;
			// 
			// ImportProgressForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(470, 68);
			this.ControlBox = false;
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.statusLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "ImportProgressForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Importing channels...";
			this.UseWaitCursor = true;
			this.Load += new System.EventHandler(this.ImportProgressForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label statusLabel;
		private System.Windows.Forms.ProgressBar progressBar;
	}
}