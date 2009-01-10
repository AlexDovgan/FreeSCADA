﻿using System;
using System.Windows.Forms;

namespace FreeSCADA.Communication.MODBUSPlug
{
    public partial class ModifyTCPClientStationForm : Form
    {
        bool test = false;

        public ModifyTCPClientStationForm()
        {
            InitializeComponent();
        }

        public ModifyTCPClientStationForm(ModbusTCPClientStation tcs)
        {
            InitializeComponent();
            this.Tag = tcs;
            this.nameTextBox.Text = tcs.Name;
            this.IpMaskedTextBox.Text = tcs.IPAddress;
            this.TcpPortNumericUpDown.Value = tcs.TCPPort;
            this.PauseNumericUpDown.Value = tcs.CycleTimeout;
            this.TimeoutNumericUpDown.Value = tcs.RetryTimeout;
            this.NuberNumericUpDown.Value = tcs.RetryCount;
            this.failedNumericUpDown.Value = tcs.FailedCount;
            this.FormClosing += new FormClosingEventHandler(ModifyTCPClientStationForm_FormClosing);
        }

        void ModifyTCPClientStationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((sender as ModifyTCPClientStationForm).test)
            {
                if (/* Some validation is */ false)
                {
                    e.Cancel = true;
                }
                (sender as ModifyTCPClientStationForm).test = false;
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            ModbusTCPClientStation tcs = (ModbusTCPClientStation)this.Tag;
            tcs.Name = this.nameTextBox.Text;
            tcs.IPAddress = this.IpMaskedTextBox.Text;
            tcs.TCPPort = (int)this.TcpPortNumericUpDown.Value;
            tcs.CycleTimeout = (int)this.PauseNumericUpDown.Value;
            tcs.RetryTimeout = (int)this.TimeoutNumericUpDown.Value;
            tcs.RetryCount = (int)this.NuberNumericUpDown.Value;
            tcs.FailedCount = (int)this.failedNumericUpDown.Value;
            test = true;
        }
    }
}
