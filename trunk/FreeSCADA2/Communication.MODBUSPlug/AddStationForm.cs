using System;
using System.Windows.Forms;

namespace FreeSCADA.Communication.MODBUSPlug
{
    public partial class AddStationForm : Form
    {
        public AddStationForm()
        {
            InitializeComponent();
            stationTypeComboBox.Items.Add(ModbusStationType.TCPMaster);
            stationTypeComboBox.Items.Add(ModbusStationType.TCPSlave);
            stationTypeComboBox.Items.Add(ModbusStationType.SerialMaster);
            stationTypeComboBox.Items.Add(ModbusStationType.SerialSlave);
            stationTypeComboBox.SelectedItem = ModbusStationType.TCPMaster;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {

        }
    }
}
