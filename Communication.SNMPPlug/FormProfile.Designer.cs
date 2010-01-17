namespace FreeSCADA.Communication.SNMPPlug
{
    partial class FormProfile
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSet = new System.Windows.Forms.TextBox();
            this.txtGet = new System.Windows.Forms.TextBox();
            this.cbVersionCode = new System.Windows.Forms.ComboBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.txtPort = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.loggingComboBox = new System.Windows.Forms.ComboBox();
            this.failedNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.NuberNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.TimeoutNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.PauseNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.failedNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NuberNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimeoutNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PauseNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP";
            // 
            // txtIP
            // 
            this.txtIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIP.Location = new System.Drawing.Point(93, 77);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(250, 20);
            this.txtIP.TabIndex = 1;
            this.txtIP.Text = "127.0.0.1";
            this.txtIP.Validated += new System.EventHandler(this.txtIP_Validated);
            this.txtIP.Validating += new System.ComponentModel.CancelEventHandler(this.txtIP_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "SNMP";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtSet);
            this.groupBox1.Controls.Add(this.txtGet);
            this.groupBox1.Location = new System.Drawing.Point(12, 292);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(350, 76);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Community Names";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "SET";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "GET";
            // 
            // txtSet
            // 
            this.txtSet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSet.Location = new System.Drawing.Point(81, 45);
            this.txtSet.Name = "txtSet";
            this.txtSet.Size = new System.Drawing.Size(250, 20);
            this.txtSet.TabIndex = 1;
            this.txtSet.Text = "private";
            this.txtSet.Validated += new System.EventHandler(this.txtSet_Validated);
            this.txtSet.Validating += new System.ComponentModel.CancelEventHandler(this.txtSet_Validating);
            // 
            // txtGet
            // 
            this.txtGet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGet.Location = new System.Drawing.Point(81, 19);
            this.txtGet.Name = "txtGet";
            this.txtGet.Size = new System.Drawing.Size(250, 20);
            this.txtGet.TabIndex = 0;
            this.txtGet.Text = "public";
            this.txtGet.Validated += new System.EventHandler(this.txtGet_Validated);
            this.txtGet.Validating += new System.ComponentModel.CancelEventHandler(this.txtGet_Validating);
            // 
            // cbVersionCode
            // 
            this.cbVersionCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbVersionCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVersionCode.FormattingEnabled = true;
            this.cbVersionCode.Items.AddRange(new object[] {
            "v1",
            "v2c",
            "v3"});
            this.cbVersionCode.Location = new System.Drawing.Point(93, 12);
            this.cbVersionCode.Name = "cbVersionCode";
            this.cbVersionCode.Size = new System.Drawing.Size(250, 21);
            this.cbVersionCode.TabIndex = 0;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(238, 112);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(105, 20);
            this.txtPort.TabIndex = 2;
            this.txtPort.Text = "161";
            this.txtPort.Validated += new System.EventHandler(this.txtPort_Validated);
            this.txtPort.Validating += new System.ComponentModel.CancelEventHandler(this.txtPort_Validating);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(24, 374);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOKClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(276, 374);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 25);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(32, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 25);
            this.label5.TabIndex = 7;
            this.label5.Text = "Port";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(32, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(93, 46);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(250, 20);
            this.txtName.TabIndex = 9;
            // 
            // loggingComboBox
            // 
            this.loggingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.loggingComboBox.FormattingEnabled = true;
            this.loggingComboBox.Location = new System.Drawing.Point(238, 262);
            this.loggingComboBox.Name = "loggingComboBox";
            this.loggingComboBox.Size = new System.Drawing.Size(105, 21);
            this.loggingComboBox.TabIndex = 23;
            // 
            // failedNumericUpDown
            // 
            this.failedNumericUpDown.Location = new System.Drawing.Point(238, 220);
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
            this.failedNumericUpDown.TabIndex = 20;
            this.failedNumericUpDown.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // NuberNumericUpDown
            // 
            this.NuberNumericUpDown.Location = new System.Drawing.Point(238, 194);
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
            this.NuberNumericUpDown.TabIndex = 21;
            this.NuberNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(31, 231);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(162, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "communication to a failed device";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(31, 218);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(159, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Number of cycles before retrying";
            // 
            // TimeoutNumericUpDown
            // 
            this.TimeoutNumericUpDown.Location = new System.Drawing.Point(238, 168);
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
            this.TimeoutNumericUpDown.TabIndex = 22;
            this.TimeoutNumericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(32, 265);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(174, 13);
            this.label11.TabIndex = 15;
            this.label11.Text = "Logging Level (0=no,  3=all, 4=deb)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(32, 196);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Number of retries";
            // 
            // PauseNumericUpDown
            // 
            this.PauseNumericUpDown.Location = new System.Drawing.Point(238, 142);
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
            this.PauseNumericUpDown.TabIndex = 19;
            this.PauseNumericUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(32, 170);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(115, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Pause before retry [ms]";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(32, 144);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(136, 13);
            this.label12.TabIndex = 17;
            this.label12.Text = "Pause between cycles [ms]";
            // 
            // FormProfile
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(375, 411);
            this.Controls.Add(this.loggingComboBox);
            this.Controls.Add(this.failedNumericUpDown);
            this.Controls.Add(this.NuberNumericUpDown);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.TimeoutNumericUpDown);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.PauseNumericUpDown);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbVersionCode);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.label1);
            this.Name = "FormProfile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SNMP Agent";
            this.Load += new System.EventHandler(this.FormProfile_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.failedNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NuberNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimeoutNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PauseNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPort;

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSet;
        private System.Windows.Forms.TextBox txtGet;
        private System.Windows.Forms.ComboBox cbVersionCode;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox loggingComboBox;
        private System.Windows.Forms.NumericUpDown failedNumericUpDown;
        private System.Windows.Forms.NumericUpDown NuberNumericUpDown;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown TimeoutNumericUpDown;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown PauseNumericUpDown;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label12;
    }
}