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
            this.CreateAssociationButton = new System.Windows.Forms.Button();
            this.bindingTypes = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyList
            // 
            this.propertyList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.propertyList.FormattingEnabled = true;
            this.propertyList.IntegralHeight = false;
            this.propertyList.Location = new System.Drawing.Point(12, 41);
            this.propertyList.Name = "propertyList";
            this.propertyList.Size = new System.Drawing.Size(120, 307);
            this.propertyList.Sorted = true;
            this.propertyList.TabIndex = 0;
            this.propertyList.SelectedIndexChanged += new System.EventHandler(this.propertyList_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(624, 356);
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
            this.channelsTree.Location = new System.Drawing.Point(495, 41);
            this.channelsTree.Name = "channelsTree";
            this.channelsTree.Size = new System.Drawing.Size(204, 307);
            this.channelsTree.TabIndex = 2;
            this.channelsTree.DoubleClick += new System.EventHandler(this.channelsTree_DoubleClick);
            // 
            // enableInDesignerCheckbox
            // 
            this.enableInDesignerCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.enableInDesignerCheckbox.AutoSize = true;
            this.enableInDesignerCheckbox.Location = new System.Drawing.Point(12, 360);
            this.enableInDesignerCheckbox.Name = "enableInDesignerCheckbox";
            this.enableInDesignerCheckbox.Size = new System.Drawing.Size(200, 17);
            this.enableInDesignerCheckbox.TabIndex = 3;
            this.enableInDesignerCheckbox.Text = "Enable association in Designer mode";
            this.enableInDesignerCheckbox.UseVisualStyleBackColor = true;
            this.enableInDesignerCheckbox.CheckedChanged += new System.EventHandler(this.enableInDesignerCheckbox_CheckedChanged);
            // 
            // CreateAssociationButton
            // 
            this.CreateAssociationButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CreateAssociationButton.Location = new System.Drawing.Point(201, 0);
            this.CreateAssociationButton.Name = "CreateAssociationButton";
            this.CreateAssociationButton.Size = new System.Drawing.Size(150, 21);
            this.CreateAssociationButton.TabIndex = 7;
            this.CreateAssociationButton.Text = "Create association";
            this.CreateAssociationButton.UseVisualStyleBackColor = true;
            this.CreateAssociationButton.Click += new System.EventHandler(this.CreateAssociationButton_Click);
            // 
            // bindingTypes
            // 
            this.bindingTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.bindingTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bindingTypes.FormattingEnabled = true;
            this.bindingTypes.Location = new System.Drawing.Point(0, 0);
            this.bindingTypes.Name = "bindingTypes";
            this.bindingTypes.Size = new System.Drawing.Size(195, 21);
            this.bindingTypes.TabIndex = 8;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Location = new System.Drawing.Point(138, 13);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(351, 335);
            this.panel2.TabIndex = 9;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(351, 307);
            this.panel1.TabIndex = 10;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.bindingTypes);
            this.panel3.Controls.Add(this.CreateAssociationButton);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(351, 28);
            this.panel3.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Properties:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(495, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Channels:";
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(543, 356);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 11;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // CommonBindingDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 391);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.enableInDesignerCheckbox);
            this.Controls.Add(this.channelsTree);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.propertyList);
            this.Controls.Add(this.panel2);
            this.MinimumSize = new System.Drawing.Size(727, 394);
            this.Name = "CommonBindingDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Associate element with data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CommonBindingDialog_FormClosing);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox propertyList;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TreeView channelsTree;
		private System.Windows.Forms.CheckBox enableInDesignerCheckbox;
		private System.Windows.Forms.Button CreateAssociationButton;
		private System.Windows.Forms.ComboBox bindingTypes;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button saveButton;
	}
}