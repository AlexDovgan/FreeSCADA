using System;
using System.Windows.Forms;

namespace FreeSCADA.Communication.MODBUSPlug
{
    public partial class ModifySerialClientStationForm : Form
    {
        bool test = false;

        public ModifySerialClientStationForm()
        {
            InitializeComponent();
        }

        public ModifySerialClientStationForm(ModbusSerialClientStation tcs)
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
                if (/* Some validation is */ false)
                {
                    e.Cancel = true;
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
