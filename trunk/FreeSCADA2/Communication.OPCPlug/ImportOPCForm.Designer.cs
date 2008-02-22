namespace FreeSCADA.Communication.OPCPlug
{
	partial class ImportOPCForm
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
			this.button3 = new System.Windows.Forms.Button();
			this.serversComboBox = new System.Windows.Forms.ComboBox();
			this.serverTextBox = new System.Windows.Forms.TextBox();
			this.remoteServerButton = new System.Windows.Forms.RadioButton();
			this.localServerButton = new System.Windows.Forms.RadioButton();
			this.channelsTree = new System.Windows.Forms.TreeView();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// connectButton
			// 
			this.connectButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.connectButton.Enabled = false;
			this.connectButton.Location = new System.Drawing.Point(12, 100);
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
			this.groupBox1.Controls.Add(this.button3);
			this.groupBox1.Controls.Add(this.serversComboBox);
			this.groupBox1.Controls.Add(this.serverTextBox);
			this.groupBox1.Controls.Add(this.remoteServerButton);
			this.groupBox1.Controls.Add(this.localServerButton);
			this.groupBox1.Location = new System.Drawing.Point(13, 13);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(453, 81);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Server";
			// 
			// button3
			// 
			this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button3.Image = global::FreeSCADA.Communication.OPCPlug.Properties.Resources.refresh;
			this.button3.Location = new System.Drawing.Point(419, 47);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(28, 21);
			this.button3.TabIndex = 4;
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.OnRefreshServersClick);
			// 
			// serversComboBox
			// 
			this.serversComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.serversComboBox.FormattingEnabled = true;
			this.serversComboBox.Location = new System.Drawing.Point(7, 47);
			this.serversComboBox.Name = "serversComboBox";
			this.serversComboBox.Size = new System.Drawing.Size(406, 21);
			this.serversComboBox.TabIndex = 3;
			this.serversComboBox.TextUpdate += new System.EventHandler(this.OnServerChanged);
			this.serversComboBox.TextChanged += new System.EventHandler(this.OnServerChanged);
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
			this.channelsTree.Location = new System.Drawing.Point(12, 130);
			this.channelsTree.Name = "channelsTree";
			this.channelsTree.PathSeparator = ".";
			this.channelsTree.Size = new System.Drawing.Size(454, 271);
			this.channelsTree.TabIndex = 3;
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
			// ImportOPCForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(478, 442);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.channelsTree);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.connectButton);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(494, 478);
			this.Name = "ImportOPCForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ImportOPCForm";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button connectButton;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox serverTextBox;
		private System.Windows.Forms.RadioButton remoteServerButton;
		private System.Windows.Forms.RadioButton localServerButton;
		private System.Windows.Forms.ComboBox serversComboBox;
		private System.Windows.Forms.TreeView channelsTree;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button button3;
	}
}