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
			this.checkBox1 = new System.Windows.Forms.CheckBox();
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
			this.channelsTree.Location = new System.Drawing.Point(597, 13);
			this.channelsTree.Name = "channelsTree";
			this.channelsTree.Size = new System.Drawing.Size(204, 419);
			this.channelsTree.TabIndex = 2;
			// 
			// checkBox1
			// 
			this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(12, 444);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(188, 17);
			this.checkBox1.TabIndex = 3;
			this.checkBox1.Text = "Enable property in Designer mode";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// CommonBindingDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(813, 475);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.channelsTree);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.propertyList);
			this.Name = "CommonBindingDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Associate element with data";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox propertyList;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TreeView channelsTree;
		private System.Windows.Forms.CheckBox checkBox1;
	}
}