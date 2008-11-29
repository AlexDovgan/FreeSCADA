using System;
using System.Windows.Forms;

namespace FreeSCADA.Communication.MODBUSPlug
{
	public partial class SettingsForm : Form
	{
		Plugin plugin;

		public SettingsForm(Plugin plugin)
		{
			InitializeComponent();
			this.plugin = plugin;

			grid.SelectionMode = SourceGrid.GridSelectionMode.Row;
			grid.Selection.EnableMultiSelection = false;

			DevAge.Drawing.RectangleBorder b = grid.Selection.Border;
			b.SetWidth(0);
            grid.Selection.Border = b;
            grid.Selection.FocusBackColor = grid.Selection.BackColor;

			grid.ColumnsCount = 5;
			grid.RowsCount = 1;
            grid[0, 0] = new SourceGrid.Cells.ColumnHeader("Channel name");
            grid[0, 1] = new SourceGrid.Cells.ColumnHeader("Channel Type");
            grid[0, 2] = new SourceGrid.Cells.ColumnHeader("MODBUS Station");
            grid[0, 3] = new SourceGrid.Cells.ColumnHeader("MODBUS Type");
            grid[0, 4] = new SourceGrid.Cells.ColumnHeader("MODBUS Address");

            stationGrid.SelectionMode = SourceGrid.GridSelectionMode.Row;
			stationGrid.Selection.EnableMultiSelection = false;

            stationGrid.Selection.Border = b;
            stationGrid.Selection.FocusBackColor = stationGrid.Selection.BackColor;

			stationGrid.ColumnsCount = 3;
			stationGrid.RowsCount = 1;
            stationGrid[0, 0] = new SourceGrid.Cells.ColumnHeader("Station name");
            stationGrid[0, 1] = new SourceGrid.Cells.ColumnHeader("IP Address");
            stationGrid[0, 2] = new SourceGrid.Cells.ColumnHeader("TCP Port");

			LoadStations();
			LoadChannels();

			stationGrid.AutoStretchColumnsToFitWidth = true;
			stationGrid.AutoSizeCells();
 			grid.AutoStretchColumnsToFitWidth = true;
			grid.AutoSizeCells();
       }

        private void AddStation(string name, string ipAddress, int tcpPort)
        {
            int row = stationGrid.RowsCount;
            stationGrid.RowsCount++;
            stationGrid[row, 0] = new SourceGrid.Cells.Cell(name, typeof(string));
            stationGrid[row, 1] = new SourceGrid.Cells.Cell(ipAddress, typeof(string));
            stationGrid[row, 2] = new SourceGrid.Cells.Cell(tcpPort, typeof(int));
        }

		private void OnAddStation(object sender, EventArgs e)
		{
            string var = GetUniqueStationName();
            int row = stationGrid.RowsCount;
            stationGrid.RowsCount++;
            string ip = "127.0.0.1";
            string po = "502";

            stationGrid[row, 0] = new SourceGrid.Cells.Cell(var, typeof(string));
            stationGrid[row, 1] = new SourceGrid.Cells.Cell(ip, typeof(string));
            stationGrid[row, 2] = new SourceGrid.Cells.Cell(po, typeof(int));

            stationGrid.Selection.ResetSelection(true);
            stationGrid.Selection.SelectRow(row, true);
        }

        private void OnRemoveStation(object sender, EventArgs e)
        {
            foreach (int row in stationGrid.Selection.GetSelectionRegion().GetRowsIndex())
            {
                bool exist = false;
                for (int i = 1; i < grid.RowsCount; i++)
                {
                    if (grid[i, 2].DisplayText == stationGrid[row, 0].DisplayText)
                    {
                        exist = true;
                        break;
                    }
                }
                if (exist)
                    MessageBox.Show("Variables of this station exist! Cannot be deleted!", "Error");
                else
                    stationGrid.Rows.Remove(row);
            }
        }
        
        private void LoadStations()
        {
            foreach (ModbusStation stat in plugin.Stations)
                AddStation(stat.Name, stat.IPAddress, stat.TCPPort);
        }

        private void AddVariable(string name, string type, string modbusStation, string modbusType, string modbusAddress)
        {
            int row = grid.RowsCount;
            grid.RowsCount++;
            grid[row, 0] = new SourceGrid.Cells.Cell(name, typeof(string));
            grid[row, 1] = new SourceGrid.Cells.Cell(type, typeof(ModbusInternalType));
            grid[row, 2] = new SourceGrid.Cells.Cell(modbusStation, typeof(string));
            grid[row, 3] = new SourceGrid.Cells.Cell(modbusType, typeof(Modbus.Data.ModbusDataType));
            grid[row, 4] = new SourceGrid.Cells.Cell(modbusAddress, typeof(int));
        }

        private void OnAddVariable(object sender, EventArgs e)
        {
            if (stationGrid.RowsCount > 1)
            {

                string var = GetUniqueVariableName();
                int row = grid.RowsCount;
                grid.RowsCount++;
                ModbusInternalType type = ModbusInternalType.Integer;
                Modbus.Data.ModbusDataType mt = Modbus.Data.ModbusDataType.HoldingRegister;
                string statname;
                int [] sel = stationGrid.Selection.GetSelectionRegion().GetRowsIndex();
                if (sel.GetLength(0) > 0)
                    if (sel[0] > 1)
                        statname = stationGrid[sel[0], 0].DisplayText;
                    else
                        statname = stationGrid[1, 0].DisplayText;
                else
                    statname = stationGrid[1, 0].DisplayText;

                grid[row, 0] = new SourceGrid.Cells.Cell(var, typeof(string));
                grid[row, 1] = new SourceGrid.Cells.Cell(type, typeof(ModbusInternalType));
                grid[row, 2] = new SourceGrid.Cells.Cell(statname, typeof(string));
                grid[row, 3] = new SourceGrid.Cells.Cell(mt, typeof(Modbus.Data.ModbusDataType));
                grid[row, 4] = new SourceGrid.Cells.Cell(1, typeof(int));

                grid.Selection.ResetSelection(true);
                grid.Selection.SelectRow(row, true);
            }
            else
                MessageBox.Show("Cannot create variable - no station definition exists", "Error");
        }
        
        private void OnRemoveVariable(object sender, EventArgs e)
		{
			foreach(int row in grid.Selection.GetSelectionRegion().GetRowsIndex())
				grid.Rows.Remove(row);
		}

        private void LoadChannels()
        {
            foreach (ModbusChannelImp channel in plugin.Channels)
                AddVariable(channel.Name, channel.Type, channel.ModbusStation, channel.ModbusType, channel.ModbusAddress);
        }

        private string GetUniqueVariableName()
        {
            string baseName = "variable_";
            int baseNumber = 1;
            for (; ; )
            {
                string newName = string.Format(System.Globalization.CultureInfo.CurrentUICulture, "{0}{1}", baseName, baseNumber);
                bool exists = false;
                for (int i = 1; i < grid.RowsCount; i++)
                {
                    if (grid[i, 0].DisplayText == newName)
                    {
                        exists = true;
                        break;
                    }
                }
                if (exists == false)
                    return newName;

                baseNumber++;
            }
        }

        private string GetUniqueStationName()
        {
            string baseName = "station_";
            int baseNumber = 1;
            for (; ; )
            {
                string newName = string.Format(System.Globalization.CultureInfo.CurrentUICulture, "{0}{1}", baseName, baseNumber);
                bool exists = false;
                for (int i = 1; i < stationGrid.RowsCount; i++)
                {
                    if (stationGrid[i, 0].DisplayText == newName)
                    {
                        exists = true;
                        break;
                    }
                }
                if (exists == false)
                    return newName;

                baseNumber++;
            }
        }

		private void OnOkClick(object sender, EventArgs e)
		{
			SaveSettings();
			Close();
		}

		private void OnCancelClick(object sender, EventArgs e)
		{
			Close();
		}

		private void SaveSettings()
		{
			ShellInterfaces.IChannel[] channels = new ShellInterfaces.IChannel[grid.RowsCount - 1];
			for (int i = 1; i < grid.RowsCount; i++)
			{
				channels[i-1] = ChannelFactory.CreateChannel(	grid[i, 0].DisplayText, 
																plugin, 
																grid[i, 1].DisplayText, 
																grid[i, 2].DisplayText,
                                                                grid[i, 3].DisplayText,
            													grid[i, 4].DisplayText);
            }
			plugin.Channels = channels;

            ModbusStation[] stations = new ModbusStation[stationGrid.RowsCount - 1];
            for (int i = 1; i < stationGrid.RowsCount; i++)
            {
                stations[i - 1] = StationFactory.CreateStation(stationGrid[i, 0].DisplayText,
                                                                plugin,
                                                                stationGrid[i, 1].DisplayText,
                                                                int.Parse(stationGrid[i, 2].DisplayText));
            }
			plugin.Stations = stations;

            foreach (ModbusStation stat in stations)
            {
                stat.ClearChannels();
                foreach (ModbusChannelImp chan in channels)
                    if (chan.ModbusStation == stat.Name)
                        stat.AddChannel(chan);
            }

			plugin.SaveSettings();
		}

	}
}
