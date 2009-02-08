namespace FreeSCADA.Designer.SchemaEditor.PropertiesUtils
{
	partial class CommonBindingDialog
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
			this.propertyList = new System.Windows.Forms.ListBox();
			this.button1 = new System.Windows.Forms.Button();
			this.channelsTree = new System.Windows.Forms.TreeView();
			this.enableInDesignerCheckbox = new System.Windows.Forms.CheckBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.CreateAssociationButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// propertyList
			// 
			this.propertyList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.propertyList.FormattingEnabled = true;
			this.propertyList.Location = new System.Drawing.Point(12, 12);
			this.propertyList.Name = "propertyList";
			this.propertyList.Size = new System.Drawing.Size(120, 420);
			this.propertyList.TabIndex = 0;
			this.propertyList.SelectedIndexChanged += new System.EventHandler(this.propertyList_SelectedIndexChanged);
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.Location = new System.Drawing.Point(726, 440);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "Ok";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// channelsTree
			// 
			this.channelsTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.channelsTree.FullRowSelect = true;
			this.channelsTree.HideSelection = false;
			this.channelsTree.Location = new System.Drawing.Point(597, 13);
			this.channelsTree.Name = "channelsTree";
			this.channelsTree.Size = new System.Drawing.Size(204, 419);
			this.channelsTree.TabIndex = 2;
			this.channelsTree.DoubleClick += new System.EventHandler(this.channelsTree_DoubleClick);
			// 
			// enableInDesignerCheckbox
			// 
			this.enableInDesignerCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.enableInDesignerCheckbox.AutoSize = true;
			this.enableInDesignerCheckbox.Location = new System.Drawing.Point(12, 444);
			this.enableInDesignerCheckbox.Name = "enableInDesignerCheckbox";
			this.enableInDesignerCheckbox.Size = new System.Drawing.Size(188, 17);
			this.enableInDesignerCheckbox.TabIndex = 3;
			this.enableInDesignerCheckbox.Text = "Enable property in Designer mode";
			this.enableInDesignerCheckbox.UseVisualStyleBackColor = true;
			this.enableInDesignerCheckbox.CheckedChanged += new System.EventHandler(this.enableInDesignerCheckbox_CheckedChanged);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Location = new System.Drawing.Point(138, 41);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(453, 391);
			this.panel1.TabIndex = 6;
			// 
			// CreateAssociationButton
			// 
			this.CreateAssociationButton.Location = new System.Drawing.Point(139, 13);
			this.CreateAssociationButton.Name = "CreateAssociationButton";
			this.CreateAssociationButton.Size = new System.Drawing.Size(452, 23);
			this.CreateAssociationButton.TabIndex = 7;
			this.CreateAssociationButton.Text = "Create association";
			this.CreateAssociationButton.UseVisualStyleBackColor = true;
			this.CreateAssociationButton.Click += new System.EventHandler(this.CreateAssociationButton_Click);
			// 
			// CommonBindingDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(813, 475);
			this.Controls.Add(this.CreateAssociationButton);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.enableInDesignerCheckbox);
			this.Controls.Add(this.channelsTree);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.propertyList);
			this.Name = "CommonBindingDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Associate element with data";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CommonBindingDialog_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox propertyList;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TreeView channelsTree;
		private System.Windows.Forms.CheckBox enableInDesignerCheckbox;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button CreateAssociationButton;
	}
}