using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FreeSCADA.Communication.MODBUSPlug
{
    public partial class ModifyTCPClientStationForm : Form
    {
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
        }
    }
}
