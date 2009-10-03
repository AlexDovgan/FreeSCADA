using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace FreeSCADA.Communication.SNMPPlug
{
    public partial class ModifyChannelForm : Form
    {
        bool test = false;
        List<string> forbiddenNames;
        List<string> stations;
        string selectedStation;

        public ModifyChannelForm(SNMPChannelImp ch, List<string> forbiddenNames, List<string> stations, string selectedStation)
        {
            InitializeComponent();
            InitializeTooltips();
            this.Tag = ch;
            this.forbiddenNames = forbiddenNames;
            this.stations = stations;
            this.selectedStation = selectedStation;
            this.FormClosing += new FormClosingEventHandler(ModifyChannelForm_FormClosing);

            nameTextBox.Text = ch.Name;

            foreach (string s in stations)
            {
                agentComboBox.Items.Add(s);
            }
            agentComboBox.SelectedItem = ch.AgentName;

            MakeControlsValidation(ch);
        }

        void InitializeTooltips()
        {
            ToolTip expressionTooltip = new ToolTip();
            expressionTooltip.AutomaticDelay = 180000;
            expressionTooltip.InitialDelay = 100;
            expressionTooltip.ShowAlways = true;

            string tip = "";
            //expressionTooltip.SetToolTip(modbusDataAddressNumericUpDown, tip);
        }

        void ModifyChannelForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((sender as ModifyChannelForm).test)
            {
                SNMPChannelImp ch = (SNMPChannelImp)this.Tag;

                if (forbiddenNames != null && forbiddenNames.Contains(ch.Name))
                {
                    e.Cancel = true;
                    MessageBox.Show(StringConstants.NameAssigned);
                }
                (sender as ModifyChannelForm).test = false;
            }
        }

        void MakeControlsValidation(SNMPChannelImp ch)
        {
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            test = true;
        }

        public SNMPChannelImp DoShow()
        {
            if (this.ShowDialog() == DialogResult.OK)
            {
                // renaming - special treatment
                if (this.nameTextBox.Text != (this.Tag as SNMPChannelImp ).Name)
                {
                    this.Tag = new SNMPChannelImp(this.nameTextBox.Text, (SNMPChannelImp)this.Tag);
                }
                return (SNMPChannelImp)this.Tag;
            }
            else
                return null;
        }
    }
}
