namespace FreeSCADA.Archiver
{
	partial class DatabaseSettingsForm
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
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.dbType1 = new System.Windows.Forms.RadioButton();
			this.dbType2 = new System.Windows.Forms.RadioButton();
			this.fileNameLabel = new System.Windows.Forms.Label();
			this.fileNameBox = new System.Windows.Forms.TextBox();
			this.providerLabel1 = new System.Windows.Forms.Label();
			this.providerCombo1 = new System.Windows.Forms.ComboBox();
			this.serverLabel = new System.Windows.Forms.Label();
			this.serverBox = new System.Windows.Forms.TextBox();
			this.dbNameLabel = new System.Windows.Forms.Label();
			this.dbNameBox = new System.Windows.Forms.TextBox();
			this.userLabel = new System.Windows.Forms.Label();
			this.userBox = new System.Windows.Forms.TextBox();
			this.passwordLabel = new System.Windows.Forms.Label();
			this.passwordBox = new System.Windows.Forms.TextBox();
			this.dbType3 = new System.Windows.Forms.RadioButton();
			this.providerLabel2 = new System.Windows.Forms.Label();
			this.providerCombo2 = new System.Windows.Forms.ComboBox();
			this.connectionStringLabel = new System.Windows.Forms.Label();
			this.connectionStringBox = new System.Windows.Forms.TextBox();
			this.browseFileButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.Location = new System.Drawing.Point(512, 407);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 20;
			this.okButton.Text = "Ok";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.OnOkClick);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.Location = new System.Drawing.Point(593, 407);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 21;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.OnCancelClick);
			// 
			// dbType1
			// 
			this.dbType1.AutoSize = true;
			this.dbType1.Checked = true;
			this.dbType1.Location = new System.Drawing.Point(13, 13);
			this.dbType1.Name = "dbType1";
			this.dbType1.Size = new System.Drawing.Size(165, 17);
			this.dbType1.TabIndex = 0;
			this.dbType1.TabStop = true;
			this.dbType1.Text = "Embedded database (SQLite)";
			this.dbType1.UseVisualStyleBackColor = true;
			this.dbType1.CheckedChanged += new System.EventHandler(this.OnDbTypeUpdated);
			// 
			// dbType2
			// 
			this.dbType2.AutoSize = true;
			this.dbType2.Location = new System.Drawing.Point(13, 80);
			this.dbType2.Name = "dbType2";
			this.dbType2.Size = new System.Drawing.Size(113, 17);
			this.dbType2.TabIndex = 1;
			this.dbType2.Text = "External database";
			this.dbType2.UseVisualStyleBackColor = true;
			this.dbType2.CheckedChanged += new System.EventHandler(this.OnDbTypeUpdated);
			// 
			// fileNameLabel
			// 
			this.fileNameLabel.AutoSize = true;
			this.fileNameLabel.Location = new System.Drawing.Point(32, 37);
			this.fileNameLabel.Name = "fileNameLabel";
			this.fileNameLabel.Size = new System.Drawing.Size(52, 13);
			this.fileNameLabel.TabIndex = 3;
			this.fileNameLabel.Text = "File name";
			// 
			// fileNameBox
			// 
			this.fileNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.fileNameBox.Location = new System.Drawing.Point(35, 54);
			this.fileNameBox.Name = "fileNameBox";
			this.fileNameBox.Size = new System.Drawing.Size(604, 20);
			this.fileNameBox.TabIndex = 4;
			// 
			// providerLabel1
			// 
			this.providerLabel1.AutoSize = true;
			this.providerLabel1.Location = new System.Drawing.Point(35, 104);
			this.providerLabel1.Name = "providerLabel1";
			this.providerLabel1.Size = new System.Drawing.Size(47, 13);
			this.providerLabel1.TabIndex = 6;
			this.providerLabel1.Text = "Provider";
			// 
			// providerCombo1
			// 
			this.providerCombo1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.providerCombo1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.providerCombo1.FormattingEnabled = true;
			this.providerCombo1.Location = new System.Drawing.Point(38, 121);
			this.providerCombo1.Name = "providerCombo1";
			this.providerCombo1.Size = new System.Drawing.Size(630, 21);
			this.providerCombo1.TabIndex = 7;
			// 
			// serverLabel
			// 
			this.serverLabel.AutoSize = true;
			this.serverLabel.Location = new System.Drawing.Point(38, 149);
			this.serverLabel.Name = "serverLabel";
			this.serverLabel.Size = new System.Drawing.Size(39, 13);
			this.serverLabel.TabIndex = 8;
			this.serverLabel.Text = "Server";
			// 
			// serverBox
			// 
			this.serverBox.Location = new System.Drawing.Point(41, 166);
			this.serverBox.Name = "serverBox";
			this.serverBox.Size = new System.Drawing.Size(137, 20);
			this.serverBox.TabIndex = 9;
			// 
			// dbNameLabel
			// 
			this.dbNameLabel.AutoSize = true;
			this.dbNameLabel.Location = new System.Drawing.Point(38, 189);
			this.dbNameLabel.Name = "dbNameLabel";
			this.dbNameLabel.Size = new System.Drawing.Size(82, 13);
			this.dbNameLabel.TabIndex = 10;
			this.dbNameLabel.Text = "Database name";
			// 
			// dbNameBox
			// 
			this.dbNameBox.Location = new System.Drawing.Point(41, 205);
			this.dbNameBox.Name = "dbNameBox";
			this.dbNameBox.Size = new System.Drawing.Size(137, 20);
			this.dbNameBox.TabIndex = 11;
			// 
			// userLabel
			// 
			this.userLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.userLabel.AutoSize = true;
			this.userLabel.Location = new System.Drawing.Point(378, 149);
			this.userLabel.Name = "userLabel";
			this.userLabel.Size = new System.Drawing.Size(29, 13);
			this.userLabel.TabIndex = 12;
			this.userLabel.Text = "User";
			// 
			// userBox
			// 
			this.userBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.userBox.Location = new System.Drawing.Point(381, 166);
			this.userBox.Name = "userBox";
			this.userBox.Size = new System.Drawing.Size(100, 20);
			this.userBox.TabIndex = 13;
			// 
			// passwordLabel
			// 
			this.passwordLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.passwordLabel.AutoSize = true;
			this.passwordLabel.Location = new System.Drawing.Point(378, 189);
			this.passwordLabel.Name = "passwordLabel";
			this.passwordLabel.Size = new System.Drawing.Size(53, 13);
			this.passwordLabel.TabIndex = 14;
			this.passwordLabel.Text = "Password";
			// 
			// passwordBox
			// 
			this.passwordBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.passwordBox.Location = new System.Drawing.Point(381, 205);
			this.passwordBox.Name = "passwordBox";
			this.passwordBox.Size = new System.Drawing.Size(100, 20);
			this.passwordBox.TabIndex = 15;
			// 
			// dbType3
			// 
			this.dbType3.AutoSize = true;
			this.dbType3.Location = new System.Drawing.Point(13, 231);
			this.dbType3.Name = "dbType3";
			this.dbType3.Size = new System.Drawing.Size(172, 17);
			this.dbType3.TabIndex = 2;
			this.dbType3.Text = "External database (Advanced)";
			this.dbType3.UseVisualStyleBackColor = true;
			this.dbType3.CheckedChanged += new System.EventHandler(this.OnDbTypeUpdated);
			// 
			// providerLabel2
			// 
			this.providerLabel2.AutoSize = true;
			this.providerLabel2.Location = new System.Drawing.Point(35, 255);
			this.providerLabel2.Name = "providerLabel2";
			this.providerLabel2.Size = new System.Drawing.Size(47, 13);
			this.providerLabel2.TabIndex = 16;
			this.providerLabel2.Text = "Provider";
			// 
			// providerCombo2
			// 
			this.providerCombo2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.providerCombo2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.providerCombo2.FormattingEnabled = true;
			this.providerCombo2.Location = new System.Drawing.Point(38, 272);
			this.providerCombo2.Name = "providerCombo2";
			this.providerCombo2.Size = new System.Drawing.Size(630, 21);
			this.providerCombo2.TabIndex = 17;
			// 
			// connectionStringLabel
			// 
			this.connectionStringLabel.AutoSize = true;
			this.connectionStringLabel.Location = new System.Drawing.Point(38, 300);
			this.connectionStringLabel.Name = "connectionStringLabel";
			this.connectionStringLabel.Size = new System.Drawing.Size(91, 13);
			this.connectionStringLabel.TabIndex = 18;
			this.connectionStringLabel.Text = "Connection string";
			// 
			// connectionStringBox
			// 
			this.connectionStringBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.connectionStringBox.Location = new System.Drawing.Point(38, 317);
			this.connectionStringBox.Multiline = true;
			this.connectionStringBox.Name = "connectionStringBox";
			this.connectionStringBox.Size = new System.Drawing.Size(630, 74);
			this.connectionStringBox.TabIndex = 19;
			// 
			// browseFileButton
			// 
			this.browseFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.browseFileButton.Location = new System.Drawing.Point(641, 54);
			this.browseFileButton.Name = "browseFileButton";
			this.browseFileButton.Size = new System.Drawing.Size(27, 20);
			this.browseFileButton.TabIndex = 5;
			this.browseFileButton.Text = "...";
			this.browseFileButton.UseVisualStyleBackColor = true;
			// 
			// DatabaseSettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(680, 442);
			this.Controls.Add(this.browseFileButton);
			this.Controls.Add(this.passwordBox);
			this.Controls.Add(this.passwordLabel);
			this.Controls.Add(this.dbNameBox);
			this.Controls.Add(this.userBox);
			this.Controls.Add(this.dbNameLabel);
			this.Controls.Add(this.userLabel);
			this.Controls.Add(this.connectionStringBox);
			this.Controls.Add(this.serverBox);
			this.Controls.Add(this.connectionStringLabel);
			this.Controls.Add(this.serverLabel);
			this.Controls.Add(this.providerCombo2);
			this.Controls.Add(this.providerCombo1);
			this.Controls.Add(this.providerLabel2);
			this.Controls.Add(this.providerLabel1);
			this.Controls.Add(this.fileNameBox);
			this.Controls.Add(this.fileNameLabel);
			this.Controls.Add(this.dbType3);
			this.Controls.Add(this.dbType2);
			this.Controls.Add(this.dbType1);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(696, 478);
			this.Name = "DatabaseSettingsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Database connection settings";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.RadioButton dbType1;
		private System.Windows.Forms.RadioButton dbType2;
		private System.Windows.Forms.Label fileNameLabel;
		private System.Windows.Forms.TextBox fileNameBox;
		private System.Windows.Forms.Label providerLabel1;
		private System.Windows.Forms.ComboBox providerCombo1;
		private System.Windows.Forms.Label serverLabel;
		private System.Windows.Forms.TextBox serverBox;
		private System.Windows.Forms.Label dbNameLabel;
		private System.Windows.Forms.TextBox dbNameBox;
		private System.Windows.Forms.Label userLabel;
		private System.Windows.Forms.TextBox userBox;
		private System.Windows.Forms.Label passwordLabel;
		private System.Windows.Forms.TextBox passwordBox;
		private System.Windows.Forms.RadioButton dbType3;
		private System.Windows.Forms.Label providerLabel2;
		private System.Windows.Forms.ComboBox providerCombo2;
		private System.Windows.Forms.Label connectionStringLabel;
		private System.Windows.Forms.TextBox connectionStringBox;
		private System.Windows.Forms.Button browseFileButton;
	}
}