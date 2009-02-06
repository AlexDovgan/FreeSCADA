using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO.Ports;

namespace FreeSCADA.Communication.MODBUSPlug
{
    public partial class ModifySerialClientStationForm : Form
    {
        bool test = false;
        List<string> forbiddenNames;

        public ModifySerialClientStationForm()
        {
            InitializeComponent();
        }

        public ModifySerialClientStationForm(ModbusSerialClientStation tcs, List<string> forbiddenNames)
        {
            InitializeComponent();
            this.Tag = tcs;
            this.nameTextBox.Text = tcs.Name;
            this.COMtextBox.Text = tcs.ComPort;
            foreach (object o in Enum.GetValues(typeof(ModbusSerialType))) {
                int i = this.serialTypeComboBox.Items.Add(o);
            }
            this.serialTypeComboBox.SelectedItem = tcs.SerialType;

            this.baudRatenumericUpDown.Value = tcs.BaudRate;

            this.dataBitsComboBox.Items.Add(7);
            this.dataBitsComboBox.Items.Add(8);
            this.dataBitsComboBox.SelectedItem = tcs.DataBits;

            foreach (object o in Enum.GetValues(typeof(StopBits)))
            {
                int i = this.stopBitsComboBox.Items.Add(o);
            }
            this.stopBitsComboBox.SelectedItem = tcs.StopBits;

            foreach (object o in Enum.GetValues(typeof(Parity)))
            {
                int i = this.parityComboBox.Items.Add(o);
            }
            this.parityComboBox.SelectedItem = tcs.Parity;

            foreach (object o in Enum.GetValues(typeof(Handshake)))
            {
                int i = this.handshakeComboBox.Items.Add(o);
            }
            this.handshakeComboBox.SelectedItem = tcs.Handshake;

            this.PauseNumericUpDown.Value = tcs.CycleTimeout;
            this.TimeoutNumericUpDown.Value = tcs.RetryTimeout;
            this.NuberNumericUpDown.Value = tcs.RetryCount;
            this.failedNumericUpDown.Value = tcs.FailedCount;
            this.FormClosing += new FormClosingEventHandler(ModifySerialClientStationForm_FormClosing);
            this.forbiddenNames = forbiddenNames;
        }

        void ModifySerialClientStationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((sender as ModifySerialClientStationForm).test)
            {
                ModbusSerialClientStation tcs = (ModbusSerialClientStation)this.Tag;

                if (forbiddenNames != null && forbiddenNames.Contains(tcs.Name))
                {
                    e.Cancel = true;
                    MessageBox.Show(StringConstants.NameAssigned);
                }
                (sender as ModifySerialClientStationForm).test = false;
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            ModbusSerialClientStation tcs = (ModbusSerialClientStation)this.Tag;
            tcs.Name = this.nameTextBox.Text;
            tcs.SerialType = (ModbusSerialType)this.serialTypeComboBox.SelectedItem;
            tcs.BaudRate = (int)this.baudRatenumericUpDown.Value;
            tcs.ComPort = this.COMtextBox.Text;
            tcs.DataBits = (int)this.dataBitsComboBox.SelectedItem;
            tcs.StopBits = (StopBits)this.stopBitsComboBox.SelectedItem;
            tcs.Parity = (Parity)this.parityComboBox.SelectedItem;
            tcs.Handshake = (Handshake)this.handshakeComboBox.SelectedItem;
            tcs.CycleTimeout = (int)this.PauseNumericUpDown.Value;
            tcs.RetryTimeout = (int)this.TimeoutNumericUpDown.Value;
            tcs.RetryCount = (int)this.NuberNumericUpDown.Value;
            tcs.FailedCount = (int)this.failedNumericUpDown.Value;
            test = true;
        }
    }
}
