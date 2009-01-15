using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace FreeSCADA.Communication.MODBUSPlug
{
    public partial class ModifyTCPClientStationForm : Form
    {
        bool test = false;
        List<string> forbiddenNames;

        public ModifyTCPClientStationForm()
        {
            InitializeComponent();
        }

        public ModifyTCPClientStationForm(ModbusTCPClientStation tcs, List<string> forbiddenNames)
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
            this.forbiddenNames = forbiddenNames;
        }

        void ModifyTCPClientStationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((sender as ModifyTCPClientStationForm).test)
            {
                ModbusTCPClientStation tcs = (ModbusTCPClientStation)this.Tag;

                if (forbiddenNames != null && forbiddenNames.Contains(tcs.Name))
                {
                    e.Cancel = true;
                    MessageBox.Show("Name already assigned to another Station!");
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
