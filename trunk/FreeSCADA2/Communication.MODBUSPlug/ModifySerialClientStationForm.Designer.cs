namespace FreeSCADA.Communication.MODBUSPlug
{
    partial class ModifySerialClientStationForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.PauseNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.TimeoutNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.NuberNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.failedNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.COMtextBox = new System.Windows.Forms.TextBox();
            this.baudRatenumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.dataBitsComboBox = new System.Windows.Forms.ComboBox();
            this.stopBitsComboBox = new System.Windows.Forms.ComboBox();
            this.parityComboBox = new System.Windows.Forms.ComboBox();
            this.handshakeComboBox = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.serialTypeComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.PauseNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimeoutNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NuberNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.failedNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.baudRatenumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // OKButton
            // 
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKButton.Location = new System.Drawing.Point(53, 381);
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
            this.cancelButton.Location = new System.Drawing.Point(206, 381);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(87, 34);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "COM Port Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Baud Rate";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 263);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Pause between cycles [ms]";
            // 
            // PauseNumericUpDown
            // 
            this.PauseNumericUpDown.Location = new System.Drawing.Point(206, 263);
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
            this.PauseNumericUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 289);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Pause before retry [ms]";
            // 
            // TimeoutNumericUpDown
            // 
            this.TimeoutNumericUpDown.Location = new System.Drawing.Point(206, 289);
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
            this.TimeoutNumericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 315);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Number of retries";
            // 
            // NuberNumericUpDown
            // 
            this.NuberNumericUpDown.Location = new System.Drawing.Point(206, 315);
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
            this.NuberNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(33, 50);
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
            // 
            // failedNumericUpDown
            // 
            this.failedNumericUpDown.Location = new System.Drawing.Point(206, 341);
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
            this.failedNumericUpDown.TabIndex = 13;
            this.failedNumericUpDown.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(32, 352);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(162, 13);
            this.label10.TabIndex = 12;
            this.label10.Text = "communication to a failed device";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(32, 339);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(159, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Number of cycles before retrying";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(33, 154);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Data Bits";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(33, 208);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Stop Bits";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(33, 181);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(33, 13);
            this.label11.TabIndex = 16;
            this.label11.Text = "Parity";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(32, 235);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(62, 13);
            this.label12.TabIndex = 17;
            this.label12.Text = "Handshake";
            // 
            // COMtextBox
            // 
            this.COMtextBox.Location = new System.Drawing.Point(208, 98);
            this.COMtextBox.Name = "COMtextBox";
            this.COMtextBox.Size = new System.Drawing.Size(104, 20);
            this.COMtextBox.TabIndex = 18;
            this.COMtextBox.Text = "COM1";
            // 
            // baudRatenumericUpDown
            // 
            this.baudRatenumericUpDown.Location = new System.Drawing.Point(207, 125);
            this.baudRatenumericUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.baudRatenumericUpDown.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.baudRatenumericUpDown.Name = "baudRatenumericUpDown";
            this.baudRatenumericUpDown.Size = new System.Drawing.Size(105, 20);
            this.baudRatenumericUpDown.TabIndex = 19;
            this.baudRatenumericUpDown.Value = new decimal(new int[] {
            9600,
            0,
            0,
            0});
            // 
            // dataBitsComboBox
            // 
            this.dataBitsComboBox.FormattingEnabled = true;
            this.dataBitsComboBox.Location = new System.Drawing.Point(206, 151);
            this.dataBitsComboBox.Name = "dataBitsComboBox";
            this.dataBitsComboBox.Size = new System.Drawing.Size(106, 21);
            this.dataBitsComboBox.TabIndex = 20;
            // 
            // stopBitsComboBox
            // 
            this.stopBitsComboBox.FormattingEnabled = true;
            this.stopBitsComboBox.Location = new System.Drawing.Point(206, 205);
            this.stopBitsComboBox.Name = "stopBitsComboBox";
            this.stopBitsComboBox.Size = new System.Drawing.Size(106, 21);
            this.stopBitsComboBox.TabIndex = 21;
            // 
            // parityComboBox
            // 
            this.parityComboBox.FormattingEnabled = true;
            this.parityComboBox.Location = new System.Drawing.Point(207, 178);
            this.parityComboBox.Name = "parityComboBox";
            this.parityComboBox.Size = new System.Drawing.Size(106, 21);
            this.parityComboBox.TabIndex = 22;
            // 
            // handshakeComboBox
            // 
            this.handshakeComboBox.FormattingEnabled = true;
            this.handshakeComboBox.Location = new System.Drawing.Point(206, 232);
            this.handshakeComboBox.Name = "handshakeComboBox";
            this.handshakeComboBox.Size = new System.Drawing.Size(106, 21);
            this.handshakeComboBox.TabIndex = 23;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 23);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(266, 13);
            this.label13.TabIndex = 25;
            this.label13.Text = "Free-SCADA computer is MASTER in this conversation";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(12, 6);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(232, 13);
            this.label14.TabIndex = 24;
            this.label14.Text = "Set MODBUS Serial Communication parameters";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(33, 75);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(110, 13);
            this.label15.TabIndex = 26;
            this.label15.Text = "MODBUS Serial Type";
            // 
            // serialTypeComboBox
            // 
            this.serialTypeComboBox.FormattingEnabled = true;
            this.serialTypeComboBox.Location = new System.Drawing.Point(207, 72);
            this.serialTypeComboBox.Name = "serialTypeComboBox";
            this.serialTypeComboBox.Size = new System.Drawing.Size(106, 21);
            this.serialTypeComboBox.TabIndex = 27;
            // 
            // ModifySerialClientStationForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 436);
            this.ControlBox = false;
            this.Controls.Add(this.serialTypeComboBox);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.handshakeComboBox);
            this.Controls.Add(this.parityComboBox);
            this.Controls.Add(this.stopBitsComboBox);
            this.Controls.Add(this.dataBitsComboBox);
            this.Controls.Add(this.baudRatenumericUpDown);
            this.Controls.Add(this.COMtextBox);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.failedNumericUpDown);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.NuberNumericUpDown);
            this.Controls.Add(this.TimeoutNumericUpDown);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.PauseNumericUpDown);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.OKButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModifySerialClientStationForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MODBUS Serial Master";
            ((System.ComponentModel.ISupportInitialize)(this.PauseNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimeoutNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NuberNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.failedNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.baudRatenumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown PauseNumericUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown TimeoutNumericUpDown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown NuberNumericUpDown;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.NumericUpDown failedNumericUpDown;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox COMtextBox;
        private System.Windows.Forms.NumericUpDown baudRatenumericUpDown;
        private System.Windows.Forms.ComboBox dataBitsComboBox;
        private System.Windows.Forms.ComboBox stopBitsComboBox;
        private System.Windows.Forms.ComboBox parityComboBox;
        private System.Windows.Forms.ComboBox handshakeComboBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox serialTypeComboBox;
    }
}