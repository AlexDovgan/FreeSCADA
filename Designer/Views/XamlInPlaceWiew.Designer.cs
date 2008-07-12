namespace FreeSCADA.Designer.Views
{
    partial class XamlInPlaceWiew
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
            this.XAMLtextBox = new System.Windows.Forms.TextBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.changeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // XAMLtextBox
            // 
            this.XAMLtextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.XAMLtextBox.Location = new System.Drawing.Point(0, 0);
            this.XAMLtextBox.Multiline = true;
            this.XAMLtextBox.Name = "XAMLtextBox";
            this.XAMLtextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.XAMLtextBox.Size = new System.Drawing.Size(792, 422);
            this.XAMLtextBox.TabIndex = 0;
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(428, 428);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(84, 43);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // changeButton
            // 
            this.changeButton.Location = new System.Drawing.Point(297, 428);
            this.changeButton.Name = "changeButton";
            this.changeButton.Size = new System.Drawing.Size(84, 43);
            this.changeButton.TabIndex = 2;
            this.changeButton.Text = "Change Object";
            this.changeButton.UseVisualStyleBackColor = true;
            this.changeButton.Click += new System.EventHandler(this.changeButton_Click);
            // 
            // XamlInPlaceWiew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 473);
            this.Controls.Add(this.changeButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.XAMLtextBox);
            this.Name = "XamlInPlaceWiew";
            this.ShowInTaskbar = false;
            this.Text = "XamlInPlaceWiew";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Forms.TextBox XAMLtextBox;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button changeButton;

    }
}