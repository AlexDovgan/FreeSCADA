using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FreeSCADA.Communication.MODBUSPlug
{
    public partial class ModifyTCPClientStationForm : Form
    {
        bool test = false;
        bool cancel = false;
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
            this.loggingComboBox.Items.Add(0);
            this.loggingComboBox.Items.Add(1);
            this.loggingComboBox.Items.Add(2);
            this.loggingComboBox.Items.Add(3);
            this.loggingComboBox.Items.Add(4);
            this.loggingComboBox.SelectedItem = tcs.LoggingLevel;
        }

        void ModifyTCPClientStationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((sender as ModifyTCPClientStationForm).cancel)
            {
                e.Cancel = true;
                cancel = false;
            }
            if ((sender as ModifyTCPClientStationForm).test)
            {
                ModbusTCPClientStation tcs = (ModbusTCPClientStation)this.Tag;

                if (forbiddenNames != null && forbiddenNames.Contains(tcs.Name))
                {
                    e.Cancel = true;
                    MessageBox.Show(StringConstants.NameAssigned);
                }
                (sender as ModifyTCPClientStationForm).test = false;
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            try
            {
                ModbusTCPClientStation tcs = (ModbusTCPClientStation)this.Tag;
                tcs.Name = this.nameTextBox.Text;
                tcs.IPAddress = this.IpMaskedTextBox.Text;
                tcs.TCPPort = (int)this.TcpPortNumericUpDown.Value;
                tcs.CycleTimeout = (int)this.PauseNumericUpDown.Value;
                tcs.RetryTimeout = (int)this.TimeoutNumericUpDown.Value;
                tcs.RetryCount = (int)this.NuberNumericUpDown.Value;
                tcs.FailedCount = (int)this.failedNumericUpDown.Value;
                tcs.LoggingLevel = (int)this.loggingComboBox.SelectedItem;
                test = true;
            }
            catch
            {
                MessageBox.Show(StringConstants.ReadingValues);
                cancel = true;
            }
        }
    }
}
