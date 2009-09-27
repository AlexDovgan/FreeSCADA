namespace FreeSCADA.Communication.CLServer
{
	partial class ImportChannelsForm
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
			this.connectButton = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.portTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.serverTextBox = new System.Windows.Forms.TextBox();
			this.remoteServerButton = new System.Windows.Forms.RadioButton();
			this.localServerButton = new System.Windows.Forms.RadioButton();
			this.channelsTree = new System.Windows.Forms.TreeView();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.statusTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// connectButton
			// 
			this.connectButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.connectButton.Enabled = false;
			this.connectButton.Location = new System.Drawing.Point(12, 96);
			this.connectButton.Name = "connectButton";
			this.connectButton.Size = new System.Drawing.Size(454, 23);
			this.connectButton.TabIndex = 1;
			this.connectButton.Text = "Connect";
			this.connectButton.UseVisualStyleBackColor = true;
			this.connectButton.Click += new System.EventHandler(this.OnConnect);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.portTextBox);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.serverTextBox);
			this.groupBox1.Controls.Add(this.remoteServerButton);
			this.groupBox1.Controls.Add(this.localServerButton);
			this.groupBox1.Location = new System.Drawing.Point(13, 13);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(453, 77);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Server";
			// 
			// portTextBox
			// 
			this.portTextBox.Location = new System.Drawing.Point(132, 46);
			this.portTextBox.Name = "portTextBox";
			this.portTextBox.Size = new System.Drawing.Size(100, 20);
			this.portTextBox.TabIndex = 4;
			this.portTextBox.Text = "8080";
			this.portTextBox.TextChanged += new System.EventHandler(this.portTextBox_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(90, 49);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(31, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Port:";
			// 
			// serverTextBox
			// 
			this.serverTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.serverTextBox.Enabled = false;
			this.serverTextBox.Location = new System.Drawing.Point(132, 20);
			this.serverTextBox.Name = "serverTextBox";
			this.serverTextBox.Size = new System.Drawing.Size(315, 20);
			this.serverTextBox.TabIndex = 2;
			this.serverTextBox.Text = "localhost";
			this.serverTextBox.TextChanged += new System.EventHandler(this.serverTextBox_TextChanged);
			// 
			// remoteServerButton
			// 
			this.remoteServerButton.AutoSize = true;
			this.remoteServerButton.Location = new System.Drawing.Point(63, 20);
			this.remoteServerButton.Name = "remoteServerButton";
			this.remoteServerButton.Size = new System.Drawing.Size(62, 17);
			this.remoteServerButton.TabIndex = 1;
			this.remoteServerButton.Text = "Remote";
			this.remoteServerButton.UseVisualStyleBackColor = true;
			this.remoteServerButton.CheckedChanged += new System.EventHandler(this.OnServerClick);
			// 
			// localServerButton
			// 
			this.localServerButton.AutoSize = true;
			this.localServerButton.Checked = true;
			this.localServerButton.Location = new System.Drawing.Point(7, 20);
			this.localServerButton.Name = "localServerButton";
			this.localServerButton.Size = new System.Drawing.Size(49, 17);
			this.localServerButton.TabIndex = 0;
			this.localServerButton.TabStop = true;
			this.localServerButton.Text = "Local";
			this.localServerButton.UseVisualStyleBackColor = true;
			this.localServerButton.CheckedChanged += new System.EventHandler(this.OnServerClick);
			// 
			// channelsTree
			// 
			this.channelsTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.channelsTree.CheckBoxes = true;
			this.channelsTree.Location = new System.Drawing.Point(12, 125);
			this.channelsTree.Name = "channelsTree";
			this.channelsTree.PathSeparator = ".";
			this.channelsTree.Size = new System.Drawing.Size(454, 186);
			this.channelsTree.TabIndex = 3;
			this.channelsTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.channelsTree_AfterCheck);
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.Location = new System.Drawing.Point(310, 407);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 4;
			this.okButton.Text = "Ok";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.OnOkClick);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.Location = new System.Drawing.Point(391, 407);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 5;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.OnCancelClick);
			// 
			// statusTextBox
			// 
			this.statusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.statusTextBox.BackColor = System.Drawing.SystemColors.Window;
			this.statusTextBox.Location = new System.Drawing.Point(12, 334);
			this.statusTextBox.Multiline = true;
			this.statusTextBox.Name = "statusTextBox";
			this.statusTextBox.ReadOnly = true;
			this.statusTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.statusTextBox.Size = new System.Drawing.Size(454, 67);
			this.statusTextBox.TabIndex = 6;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 318);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(42, 13);
			this.label2.TabIndex = 7;
			this.label2.Text = "Status:";
			// 
			// ImportChannelsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(478, 442);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.statusTextBox);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.channelsTree);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.connectButton);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(494, 478);
			this.Name = "ImportChannelsForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Import channel definition from remote server";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button connectButton;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox serverTextBox;
		private System.Windows.Forms.RadioButton remoteServerButton;
		private System.Windows.Forms.RadioButton localServerButton;
		private System.Windows.Forms.TreeView channelsTree;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox portTextBox;
		private System.Windows.Forms.TextBox statusTextBox;
		private System.Windows.Forms.Label label2;
	}
}