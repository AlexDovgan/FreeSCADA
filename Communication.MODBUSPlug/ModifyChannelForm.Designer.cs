namespace FreeSCADA.Communication.MODBUSPlug
{
    partial class ModifyChannelForm
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
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.modbusFs2InternalTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.stationComboBox = new System.Windows.Forms.ComboBox();
            this.slaveIdUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.modbusDataTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.deviceDataTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.modbusDataAddressNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.deviceDataLenNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.conversionTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.modbusReadWriteComboBox = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.bitIndexNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.kMaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.dMaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.slaveIdUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.modbusDataAddressNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deviceDataLenNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bitIndexNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(41, 26);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(273, 20);
            this.nameTextBox.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(38, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Channel Name";
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(210, 375);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(87, 34);
            this.cancelButton.TabIndex = 12;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // OKButton
            // 
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKButton.Location = new System.Drawing.Point(57, 375);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(87, 34);
            this.OKButton.TabIndex = 11;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // modbusFs2InternalTypeComboBox
            // 
            this.modbusFs2InternalTypeComboBox.FormattingEnabled = true;
            this.modbusFs2InternalTypeComboBox.Location = new System.Drawing.Point(211, 52);
            this.modbusFs2InternalTypeComboBox.Name = "modbusFs2InternalTypeComboBox";
            this.modbusFs2InternalTypeComboBox.Size = new System.Drawing.Size(103, 21);
            this.modbusFs2InternalTypeComboBox.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "FS2 Channel Type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Station Name";
            // 
            // stationComboBox
            // 
            this.stationComboBox.FormattingEnabled = true;
            this.stationComboBox.Location = new System.Drawing.Point(210, 79);
            this.stationComboBox.Name = "stationComboBox";
            this.stationComboBox.Size = new System.Drawing.Size(103, 21);
            this.stationComboBox.TabIndex = 18;
            // 
            // slaveIdUpDown
            // 
            this.slaveIdUpDown.Location = new System.Drawing.Point(210, 106);
            this.slaveIdUpDown.Maximum = new decimal(new int[] {
            254,
            0,
            0,
            0});
            this.slaveIdUpDown.Name = "slaveIdUpDown";
            this.slaveIdUpDown.Size = new System.Drawing.Size(103, 20);
            this.slaveIdUpDown.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(148, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "MOD Device Index (Slave ID)";
            // 
            // modbusDataTypeComboBox
            // 
            this.modbusDataTypeComboBox.FormattingEnabled = true;
            this.modbusDataTypeComboBox.Location = new System.Drawing.Point(210, 132);
            this.modbusDataTypeComboBox.Name = "modbusDataTypeComboBox";
            this.modbusDataTypeComboBox.Size = new System.Drawing.Size(103, 21);
            this.modbusDataTypeComboBox.TabIndex = 22;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(38, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "MOD Register Type";
            // 
            // deviceDataTypeComboBox
            // 
            this.deviceDataTypeComboBox.FormattingEnabled = true;
            this.deviceDataTypeComboBox.Location = new System.Drawing.Point(210, 159);
            this.deviceDataTypeComboBox.Name = "deviceDataTypeComboBox";
            this.deviceDataTypeComboBox.Size = new System.Drawing.Size(103, 21);
            this.deviceDataTypeComboBox.TabIndex = 24;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(38, 162);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "MOD Data Type";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(38, 188);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 13);
            this.label7.TabIndex = 26;
            this.label7.Text = "MOD Address";
            // 
            // modbusDataAddressNumericUpDown
            // 
            this.modbusDataAddressNumericUpDown.Location = new System.Drawing.Point(210, 186);
            this.modbusDataAddressNumericUpDown.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.modbusDataAddressNumericUpDown.Name = "modbusDataAddressNumericUpDown";
            this.modbusDataAddressNumericUpDown.Size = new System.Drawing.Size(103, 20);
            this.modbusDataAddressNumericUpDown.TabIndex = 25;
            this.modbusDataAddressNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(38, 214);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "MOD Data Length";
            // 
            // deviceDataLenNumericUpDown
            // 
            this.deviceDataLenNumericUpDown.Location = new System.Drawing.Point(210, 212);
            this.deviceDataLenNumericUpDown.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.deviceDataLenNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.deviceDataLenNumericUpDown.Name = "deviceDataLenNumericUpDown";
            this.deviceDataLenNumericUpDown.Size = new System.Drawing.Size(103, 20);
            this.deviceDataLenNumericUpDown.TabIndex = 27;
            this.deviceDataLenNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // conversionTypeComboBox
            // 
            this.conversionTypeComboBox.FormattingEnabled = true;
            this.conversionTypeComboBox.Location = new System.Drawing.Point(210, 238);
            this.conversionTypeComboBox.Name = "conversionTypeComboBox";
            this.conversionTypeComboBox.Size = new System.Drawing.Size(103, 21);
            this.conversionTypeComboBox.TabIndex = 30;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(38, 241);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 13);
            this.label9.TabIndex = 29;
            this.label9.Text = "MOD Swap Type";
            // 
            // modbusReadWriteComboBox
            // 
            this.modbusReadWriteComboBox.FormattingEnabled = true;
            this.modbusReadWriteComboBox.Location = new System.Drawing.Point(210, 265);
            this.modbusReadWriteComboBox.Name = "modbusReadWriteComboBox";
            this.modbusReadWriteComboBox.Size = new System.Drawing.Size(103, 21);
            this.modbusReadWriteComboBox.TabIndex = 32;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(38, 268);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(97, 13);
            this.label10.TabIndex = 31;
            this.label10.Text = "MOD Read / Write";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(38, 294);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(76, 13);
            this.label11.TabIndex = 34;
            this.label11.Text = "MOD Bit Index";
            // 
            // bitIndexNumericUpDown
            // 
            this.bitIndexNumericUpDown.Location = new System.Drawing.Point(210, 292);
            this.bitIndexNumericUpDown.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.bitIndexNumericUpDown.Name = "bitIndexNumericUpDown";
            this.bitIndexNumericUpDown.Size = new System.Drawing.Size(103, 20);
            this.bitIndexNumericUpDown.TabIndex = 33;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(38, 320);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(106, 13);
            this.label12.TabIndex = 36;
            this.label12.Text = "k constant of y=kx+d";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(38, 346);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(106, 13);
            this.label13.TabIndex = 38;
            this.label13.Text = "d constant of y=kx+d";
            // 
            // kMaskedTextBox
            // 
            this.kMaskedTextBox.Location = new System.Drawing.Point(210, 317);
            this.kMaskedTextBox.Name = "kMaskedTextBox";
            this.kMaskedTextBox.Size = new System.Drawing.Size(103, 20);
            this.kMaskedTextBox.TabIndex = 39;
            // 
            // dMaskedTextBox
            // 
            this.dMaskedTextBox.Location = new System.Drawing.Point(210, 343);
            this.dMaskedTextBox.Name = "dMaskedTextBox";
            this.dMaskedTextBox.Size = new System.Drawing.Size(103, 20);
            this.dMaskedTextBox.TabIndex = 40;
            // 
            // ModifyChannelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 419);
            this.Controls.Add(this.dMaskedTextBox);
            this.Controls.Add(this.kMaskedTextBox);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.bitIndexNumericUpDown);
            this.Controls.Add(this.modbusReadWriteComboBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.conversionTypeComboBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.deviceDataLenNumericUpDown);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.modbusDataAddressNumericUpDown);
            this.Controls.Add(this.deviceDataTypeComboBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.modbusDataTypeComboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.slaveIdUpDown);
            this.Controls.Add(this.stationComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.modbusFs2InternalTypeComboBox);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.OKButton);
            this.Name = "ModifyChannelForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Modify Channel";
            ((System.ComponentModel.ISupportInitialize)(this.slaveIdUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.modbusDataAddressNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deviceDataLenNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bitIndexNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.ComboBox modbusFs2InternalTypeComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox stationComboBox;
        private System.Windows.Forms.NumericUpDown slaveIdUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox modbusDataTypeComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox deviceDataTypeComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown modbusDataAddressNumericUpDown;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown deviceDataLenNumericUpDown;
        private System.Windows.Forms.ComboBox conversionTypeComboBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox modbusReadWriteComboBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown bitIndexNumericUpDown;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.MaskedTextBox kMaskedTextBox;
        private System.Windows.Forms.MaskedTextBox dMaskedTextBox;
    }
}