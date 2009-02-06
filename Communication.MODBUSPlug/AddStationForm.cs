using System;
using System.Windows.Forms;

namespace FreeSCADA.Communication.MODBUSPlug
{
    public partial class AddStationForm : Form
    {
        public AddStationForm()
        {
            InitializeComponent();
            foreach (object o in Enum.GetValues(typeof(ModbusStationType)))
            {
                int i = this.stationTypeComboBox.Items.Add(o);
            }

            stationTypeComboBox.SelectedItem = ModbusStationType.TCPMaster;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {

        }

        private void OKButton_Click(object sender, EventArgs e)
        {

        }
    }
}
