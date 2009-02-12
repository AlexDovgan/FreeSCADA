namespace FreeSCADA.Communication.MODBUSPlug
{
    partial class ModifyTCPClientStationForm
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
            this.components = new System.ComponentModel.Container();
            this.OKButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.IpMaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TcpPortNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.PauseNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.TimeoutNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.NuberNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.failedNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.loggingComboBox = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.TcpPortNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PauseNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimeoutNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NuberNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.failedNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // OKButton
            // 
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKButton.Location = new System.Drawing.Point(55, 285);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(87, 34);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(208, 285);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(87, 34);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Slave IP Address / Hostname";
            // 
            // IpMaskedTextBox
            // 
            this.IpMaskedTextBox.Location = new System.Drawing.Point(208, 73);
            this.IpMaskedTextBox.Name = "IpMaskedTextBox";
            this.IpMaskedTextBox.Size = new System.Drawing.Size(105, 20);
            this.IpMaskedTextBox.TabIndex = 4;
            this.toolTip1.SetToolTip(this.IpMaskedTextBox, "IP Address or Hostname of the slave (server) station");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Slave TCP Port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Pause between cycles [ms]";
            // 
            // TcpPortNumericUpDown
            // 
            this.TcpPortNumericUpDown.Location = new System.Drawing.Point(208, 99);
            this.TcpPortNumericUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.TcpPortNumericUpDown.Name = "TcpPortNumericUpDown";
            this.TcpPortNumericUpDown.Size = new System.Drawing.Size(105, 20);
            this.TcpPortNumericUpDown.TabIndex = 7;
            this.toolTip1.SetToolTip(this.TcpPortNumericUpDown, "TCP Port of the slave (server) station, default = 502");
            this.TcpPortNumericUpDown.Value = new decimal(new int[] {
            502,
            0,
            0,
            0});
            // 
            // PauseNumericUpDown
            // 
            this.PauseNumericUpDown.Location = new System.Drawing.Point(208, 125);
            this.PauseNumericUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.PauseNumericUpDown.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.PauseNumericUpDown.Name = "PauseNumericUpDown";
            this.PauseNumericUpDown.Size = new System.Drawing.Size(105, 20);
            this.PauseNumericUpDown.TabIndex = 8;
            this.toolTip1.SetToolTip(this.PauseNumericUpDown, "Pause between reading cycles in ms");
            this.PauseNumericUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 153);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Pause before retry [ms]";
            // 
            // TimeoutNumericUpDown
            // 
            this.TimeoutNumericUpDown.Location = new System.Drawing.Point(208, 151);
            this.TimeoutNumericUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.TimeoutNumericUpDown.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.TimeoutNumericUpDown.Name = "TimeoutNumericUpDown";
            this.TimeoutNumericUpDown.Size = new System.Drawing.Size(105, 20);
            this.TimeoutNumericUpDown.TabIndex = 8;
            this.toolTip1.SetToolTip(this.TimeoutNumericUpDown, "Pause before retry of an unsuccessfull communication in ms");
            this.TimeoutNumericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(36, 179);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Number of retries";
            // 
            // NuberNumericUpDown
            // 
            this.NuberNumericUpDown.Location = new System.Drawing.Point(208, 177);
            this.NuberNumericUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NuberNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NuberNumericUpDown.Name = "NuberNumericUpDown";
            this.NuberNumericUpDown.Size = new System.Drawing.Size(105, 20);
            this.NuberNumericUpDown.TabIndex = 8;
            this.toolTip1.SetToolTip(this.NuberNumericUpDown, "Number of retries of an unsuccessfull communication in ms");
            this.NuberNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(36, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Station Name";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(208, 47);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(104, 20);
            this.nameTextBox.TabIndex = 10;
            this.toolTip1.SetToolTip(this.nameTextBox, "Name (identification in the FreeSCADA system) of this MODBUS communication.");
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 5);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(227, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Set MODBUS TCP Communication parameters";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(343, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "Free-SCADA computer is MASTER (=TCP CLIENT) in this conversation";
            // 
            // failedNumericUpDown
            // 
            this.failedNumericUpDown.Location = new System.Drawing.Point(207, 203);
            this.failedNumericUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.failedNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.failedNumericUpDown.Name = "failedNumericUpDown";
            this.failedNumericUpDown.Size = new System.Drawing.Size(105, 20);
            this.failedNumericUpDown.TabIndex = 8;
            this.toolTip1.SetToolTip(this.failedNumericUpDown, "Number of communication cycles before a new communication to a previously failed " +
                    "device");
            this.failedNumericUpDown.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(35, 201);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(159, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Number of cycles before retrying";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(35, 214);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(162, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "communication to a failed device";
            // 
            // loggingComboBox
            // 
            this.loggingComboBox.FormattingEnabled = true;
            this.loggingComboBox.Location = new System.Drawing.Point(207, 245);
            this.loggingComboBox.Name = "loggingComboBox";
            this.loggingComboBox.Size = new System.Drawing.Size(104, 21);
            this.loggingComboBox.TabIndex = 12;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(36, 248);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(174, 13);
            this.label11.TabIndex = 6;
            this.label11.Text = "Logging Level (0=no,  3=all, 4=deb)";
            // 
            // ModifyTCPClientStationForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 338);
            this.ControlBox = false;
            this.Controls.Add(this.loggingComboBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.failedNumericUpDown);
            this.Controls.Add(this.NuberNumericUpDown);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.TimeoutNumericUpDown);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.PauseNumericUpDown);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TcpPortNumericUpDown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.IpMaskedTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.OKButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModifyTCPClientStationForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MODBUS TCP Master";
            ((System.ComponentModel.ISupportInitialize)(this.TcpPortNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PauseNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimeoutNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NuberNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.failedNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox IpMaskedTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown TcpPortNumericUpDown;
        private System.Windows.Forms.NumericUpDown PauseNumericUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown TimeoutNumericUpDown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown NuberNumericUpDown;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown failedNumericUpDown;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox loggingComboBox;
        private System.Windows.Forms.Label label11;
    }
}