using System;
using System.Windows.Forms;
using System.Collections.Generic;

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
            this.PauseNumericUpDown.Value = tcs.CycleTimeout;
            this.TimeoutNumericUpDown.Value = tcs.RetryTimeout;
            this.NuberNumericUpDown.Value = tcs.RetryCount;
            this.FormClosing += new FormClosingEventHandler(ModifySerialClientStationForm_FormClosing);
        }

        void ModifySerialClientStationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((sender as ModifySerialClientStationForm).test)
            {
                ModbusTCPClientStation tcs = (ModbusTCPClientStation)this.Tag;

                if (forbiddenNames != null && forbiddenNames.Contains(tcs.Name))
                {
                    e.Cancel = true;
                    MessageBox.Show("Name already assigned to another Station!");
                }
                (sender as ModifySerialClientStationForm).test = false;
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            ModbusSerialClientStation tcs = (ModbusSerialClientStation)this.Tag;
            tcs.Name = this.nameTextBox.Text;
            tcs.CycleTimeout = (int)this.PauseNumericUpDown.Value;
            tcs.RetryTimeout = (int)this.TimeoutNumericUpDown.Value;
            tcs.RetryCount = (int)this.NuberNumericUpDown.Value;
            test = true;
        }
    }
}
