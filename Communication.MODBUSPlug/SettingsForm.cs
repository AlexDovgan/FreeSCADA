using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;

namespace FreeSCADA.Communication.MODBUSPlug
{
	public partial class SettingsForm : Form
	{
		Plugin plugin;
        const int gridColName = 0;
        const int gridColFSType = 1;
        const int gridColStation = 2;
        const int gridColDevice = 3;
        const int gridColMODType = 4;
        const int gridColDevDataType = 5;
        const int gridColAddress = 6;
        const int gridColDevDataLen = 7;
        const int gridColConversion = 8;
        const int gridColReadWrite = 9;

        const int stationGridColName = 0;
        const int stationGridColAddr = 1;
        const int stationGridColPara = 2;

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

			grid.ColumnsCount = 10;
			grid.RowsCount = 1;
            grid[0, gridColName] = new SourceGrid.Cells.ColumnHeader("Channel name");
            grid[0, gridColFSType] = new SourceGrid.Cells.ColumnHeader("FS2 Channel Type");
            grid[0, gridColStation] = new SourceGrid.Cells.ColumnHeader("Station Name");
            grid[0, gridColMODType] = new SourceGrid.Cells.ColumnHeader("MOD Register Type");
            grid[0, gridColAddress] = new SourceGrid.Cells.ColumnHeader("MOD Address");
            grid[0, gridColDevice] = new SourceGrid.Cells.ColumnHeader("MOD Device Index");
            grid[0, gridColDevDataType] = new SourceGrid.Cells.ColumnHeader("MOD Data Type");
            grid[0, gridColDevDataLen] = new SourceGrid.Cells.ColumnHeader("MOD Data Length");
            grid[0, gridColConversion] = new SourceGrid.Cells.ColumnHeader("MOD Byte Swap");
            grid[0, gridColReadWrite] = new SourceGrid.Cells.ColumnHeader("MOD R/W");
            grid.MouseDoubleClick += new MouseEventHandler(grid_MouseDoubleClick);

            //grid.Controller.AddController(new ValueChangedEvent(grid));

            stationGrid.SelectionMode = SourceGrid.GridSelectionMode.Row;
			stationGrid.Selection.EnableMultiSelection = false;

            stationGrid.Selection.Border = b;
            stationGrid.Selection.FocusBackColor = stationGrid.Selection.BackColor;

			stationGrid.ColumnsCount = 3;
			stationGrid.RowsCount = 1;
            stationGrid[0, stationGridColName] = new SourceGrid.Cells.ColumnHeader("Station name");
            stationGrid[0, stationGridColAddr] = new SourceGrid.Cells.ColumnHeader("Communication");
            stationGrid[0, stationGridColPara] = new SourceGrid.Cells.ColumnHeader("Parameters");
            stationGrid.MouseDoubleClick += new MouseEventHandler(stationGrid_MouseDoubleClick);
			LoadStations();
			LoadChannels();

			stationGrid.AutoStretchColumnsToFitWidth = true;
			stationGrid.AutoSizeCells();
 			grid.AutoStretchColumnsToFitWidth = true;
			grid.AutoSizeCells();
       }

        void stationGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SourceGrid.Grid stationGrid = (SourceGrid.Grid)sender;
            int [] rows = stationGrid.Selection.GetSelectionRegion().GetRowsIndex();
            IModbusStation stat = (IModbusStation)stationGrid[rows[0], stationGridColName].Tag;
            string oldname = stat.Name;
            List<string> forbiddenNames = new List<string>();
            for (int i = 1; i < stationGrid.RowsCount; i++)
            {
                if (i != rows[0])
                    forbiddenNames.Add(stationGrid[i, stationGridColName].DisplayText);
            }
            if (stat is ModbusTCPClientStation)
            {
                ModifyTCPClientStationForm mtc = new ModifyTCPClientStationForm((ModbusTCPClientStation)stat, forbiddenNames);
                if (mtc.ShowDialog() == DialogResult.OK)
                {
                    stationGrid[rows[0], stationGridColName].Value = (stat as ModbusTCPClientStation).Name;
                    stationGrid[rows[0], stationGridColAddr].Value = "MODBUS/TCP, " + (stat as ModbusTCPClientStation).IPAddress;
                    stationGrid[rows[0], stationGridColPara].Value = "TCPport = " + (stat as ModbusTCPClientStation).TCPPort;
                    stationGrid.Invalidate();
                }
                else return;
            }
            if (stat is ModbusSerialClientStation)
            {
                ModifySerialClientStationForm msc = new ModifySerialClientStationForm((ModbusSerialClientStation)stat, forbiddenNames);
                if (msc.ShowDialog() == DialogResult.OK)
                {
                    stationGrid[rows[0], stationGridColName].Value = (stat as ModbusSerialClientStation).Name;
                    stationGrid[rows[0], stationGridColAddr].Value = 
                        "MODBUS/" + (stat as ModbusSerialClientStation).SerialType.ToString() + ", "
                        + (stat as ModbusSerialClientStation).ComPort;
                    stationGrid[rows[0], stationGridColPara].Value = 
                        (stat as ModbusSerialClientStation).BaudRate.ToString()+","
                        + (stat as ModbusSerialClientStation).DataBits.ToString() + ","
                        + (stat as ModbusSerialClientStation).Parity.ToString() + ","
                        + (stat as ModbusSerialClientStation).StopBits.ToString();
                    stationGrid.Invalidate();
                }
                else return;
            }
            if (oldname != stat.Name)
            {
                //MessageBox.Show("renaming");
                for (int i = 1; i < grid.RowsCount; i++)
                {
                    if (grid[i, gridColStation].DisplayText == oldname)
                    {
                        (grid[i, gridColName].Tag as ModbusChannelImp).ModbusStation = stat.Name;
                        grid[i, gridColStation].Value = stat.Name;
                        grid.Invalidate();
                    }
                }
            }
            //updateStationsInVariables();
        }

        private void AddTCPClientStation(ModbusTCPClientStation stat)
        {
            int row = stationGrid.RowsCount;
            stationGrid.RowsCount++;
            stationGrid[row, stationGridColName] = new SourceGrid.Cells.Cell(stat.Name);
            stationGrid[row, stationGridColName].Tag = stat;
            stationGrid[row, stationGridColName].Editor = null;
            stationGrid[row, stationGridColAddr] = new SourceGrid.Cells.Cell("MODBUS/TCP, " + stat.IPAddress, typeof(string));
            stationGrid[row, stationGridColPara] = new SourceGrid.Cells.Cell("TCPport = " + stat.TCPPort, typeof(int));
            stationGrid[row, stationGridColName].Editor = null;
            stationGrid[row, stationGridColAddr].Editor = null;
            stationGrid[row, stationGridColPara].Editor = null;
        }

        private void AddSerialClientStation(ModbusSerialClientStation stat)
        {
            int row = stationGrid.RowsCount;
            stationGrid.RowsCount++;
            stationGrid[row, stationGridColName] = new SourceGrid.Cells.Cell(stat.Name);
            stationGrid[row, stationGridColName].Tag = stat;
            stationGrid[row, stationGridColName].Editor = null;
            stationGrid[row, stationGridColAddr] = new SourceGrid.Cells.Cell("MODBUS/" + stat.SerialType.ToString() + ", " + stat.ComPort, typeof(string));
            stationGrid[row, stationGridColPara] = new SourceGrid.Cells.Cell(stat.BaudRate.ToString()+","+stat.DataBits.ToString()+","+stat.Parity.ToString()+","+stat.StopBits.ToString(), typeof(string));
            stationGrid[row, stationGridColName].Editor = null;
            stationGrid[row, stationGridColAddr].Editor = null;
            stationGrid[row, stationGridColPara].Editor = null;
        }

        private void OnAddStation(object sender, EventArgs e)
		{
            string var = GetUniqueStationName();
            int row = stationGrid.RowsCount;
            string ip = "127.0.0.1";
            int po = 502;
            string com = "COM1";

            AddStationForm asf = new AddStationForm();
            if (asf.ShowDialog() == DialogResult.OK)
            {
                if (asf.stationTypeComboBox.SelectedItem != null)
                switch ((ModbusStationType)asf.stationTypeComboBox.SelectedItem)
                {
                    case ModbusStationType.TCPMaster:
                        ModbusTCPClientStation stat = new ModbusTCPClientStation(var, plugin, ip, po, 100, 1000, 3, 20);
                        ModifyTCPClientStationForm mtc = new ModifyTCPClientStationForm(stat, null);
                        if (mtc.ShowDialog() == DialogResult.OK)
                        {
                            AddTCPClientStation(stat);
                            stationGrid.Selection.ResetSelection(true);
                            stationGrid.Selection.SelectRow(row, true);
                        }
                        break;
                    case ModbusStationType.SerialMaster:
                        ModbusSerialClientStation stat2 = new ModbusSerialClientStation(var, plugin, com, 100, 1000, 3, 20);
                        ModifySerialClientStationForm msc = new ModifySerialClientStationForm(stat2, null);
                        if (msc.ShowDialog() == DialogResult.OK)
                        {
                            AddSerialClientStation(stat2);
                            stationGrid.Selection.ResetSelection(true);
                            stationGrid.Selection.SelectRow(row, true);
                        }
                        break;
                }
            }
            //updateStationsInVariables();
        }

        private void OnRemoveStation(object sender, EventArgs e)
        {
            foreach (int row in stationGrid.Selection.GetSelectionRegion().GetRowsIndex())
            {
                bool exist = false;
                for (int i = 1; i < grid.RowsCount; i++)
                {
                    if (grid[i, gridColStation].DisplayText == stationGrid[row, stationGridColName].DisplayText)
                    {
                        exist = true;
                        break;
                    }
                }
                if (exist)
                    MessageBox.Show(StringConstants.VariablesExist, StringConstants.Error);
                else
                    stationGrid.Rows.Remove(row);
            }
            //updateStationsInVariables();
        }
        
        private void LoadStations()
        {
            foreach (IModbusStation stat in plugin.Stations)
            {
                if (stat is ModbusTCPClientStation)
                {
                    ModbusTCPClientStation tcpstat = (ModbusTCPClientStation)stat;
                    AddTCPClientStation(tcpstat);
                }
                else if (stat is ModbusSerialClientStation)
                {
                    ModbusSerialClientStation tcpstat = (ModbusSerialClientStation)stat;
                    AddSerialClientStation(tcpstat);
                }
            }
        }

        void grid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int[] rows = grid.Selection.GetSelectionRegion().GetRowsIndex();
            ModbusChannelImp chan = (ModbusChannelImp)grid[rows[0], gridColName].Tag;
            string oldname = chan.Name;
            List<string> forbiddenNames = new List<string>();
            for (int i = 1; i < grid.RowsCount; i++)
            {
                if (i != rows[0])
                    forbiddenNames.Add(grid[i, gridColName].DisplayText);
            }
            List<string> stations = new List<string>();
            for (int i = 1; i < stationGrid.RowsCount; i++)
            {
                stations.Add(((IModbusStation)stationGrid[i, stationGridColName].Tag).Name);
            }
            ModifyChannelForm mcc = new ModifyChannelForm(chan, forbiddenNames, stations, null);
            if ((chan = mcc.DoShow()) != null)
            {
                showChannel(rows[0], chan);
                grid.Invalidate();
            }
            else return;
        }

        private void AddVariable(ModbusChannelImp channel)
        {
            int row = grid.RowsCount;
            grid.RowsCount++;

            showChannel(row, channel);

            grid.Selection.ResetSelection(true);
            grid.Selection.SelectRow(row, true);
        }

        private void showChannel(int row, ModbusChannelImp channel)
        {
            grid[row, gridColName] = new SourceGrid.Cells.Cell(channel.Name);
            grid[row, gridColName].Tag = channel;
            grid[row, gridColName].Editor = null;

            grid[row, gridColFSType] = new SourceGrid.Cells.Cell(channel.ModbusFs2InternalType, typeof(ModbusFs2InternalType));
            grid[row, gridColFSType].Editor = null;

            grid[row, gridColStation] = new SourceGrid.Cells.Cell(channel.ModbusStation);
            grid[row, gridColStation].Editor = null;

            grid[row, gridColMODType] = new SourceGrid.Cells.Cell(channel.ModbusDataType);
            grid[row, gridColMODType].Editor = null;
            grid[row, gridColAddress] = new SourceGrid.Cells.Cell(channel.ModbusDataAddress);
            grid[row, gridColAddress].Editor = null;
            grid[row, gridColDevice] = new SourceGrid.Cells.Cell(channel.SlaveId);
            grid[row, gridColDevice].Editor = null;
            grid[row, gridColDevDataType] = new SourceGrid.Cells.Cell(channel.DeviceDataType);
            grid[row, gridColDevDataType].Editor = null;
            grid[row, gridColDevDataLen] = new SourceGrid.Cells.Cell(channel.DeviceDataLen);
            grid[row, gridColDevDataLen].Editor = null;
            grid[row, gridColConversion] = new SourceGrid.Cells.Cell(channel.ConversionType);
            grid[row, gridColConversion].Editor = null;
            grid[row, gridColReadWrite] = new SourceGrid.Cells.Cell(channel.ModbusReadWrite);
            grid[row, gridColReadWrite].Editor = null;
        }

        private void OnAddVariable(object sender, EventArgs e)
        {
            if (stationGrid.RowsCount > 1)
            {

                string var = GetUniqueVariableName();
                string statname;
                int [] sel = stationGrid.Selection.GetSelectionRegion().GetRowsIndex();
                if (sel.GetLength(0) > 0)
                    if (sel[0] > 1)
                        statname = stationGrid[sel[0], stationGridColName].DisplayText;
                    else
                        statname = stationGrid[1, stationGridColName].DisplayText;
                else
                    statname = stationGrid[1, stationGridColName].DisplayText;

                int[] rows = grid.Selection.GetSelectionRegion().GetRowsIndex();
                List<string> forbiddenNames = new List<string>();
                for (int i = 1; i < grid.RowsCount; i++)
                {
                    if (rows.Length > 0)
                    {
                        if (i != rows[0])
                            forbiddenNames.Add(grid[i, gridColName].DisplayText);
                    }
                    else
                        forbiddenNames.Add(grid[i, gridColName].DisplayText);
                }
                List<string> stations = new List<string>();
                for (int i = 1; i < stationGrid.RowsCount; i++)
                {
                    stations.Add(((IModbusStation)stationGrid[i, stationGridColName].Tag).Name);
                }

                ModbusChannelImp ch = (ModbusChannelImp)ChannelFactory.CreateChannel(var, plugin, typeof(int), statname, ModbusDataTypeEx.HoldingRegister, 1, 0,
                                                                                     ModbusDeviceDataType.Int, 1, ModbusConversionType.SwapNone, 0);
                ModifyChannelForm mcc = new ModifyChannelForm(ch, forbiddenNames, stations, statname);
                if ((ch = mcc.DoShow()) != null)
                {
                    AddVariable(ch);
                }
            }
            else
                MessageBox.Show(StringConstants.CannotCreateVariable, StringConstants.Error);
        }

        private void OnRemoveVariable(object sender, EventArgs e)
		{
			foreach(int row in grid.Selection.GetSelectionRegion().GetRowsIndex())
				grid.Rows.Remove(row);
		}

        private void LoadChannels()
        {
            foreach (ModbusChannelImp channel in plugin.Channels)
                AddVariable(channel);
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
                    if (grid[i, gridColName].DisplayText == newName)
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
                    if (stationGrid[i, stationGridColName].DisplayText == newName)
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
			Interfaces.IChannel[] channels = new Interfaces.IChannel[grid.RowsCount - 1];
			for (int i = 1; i < grid.RowsCount; i++)
			{
                channels[i - 1] = (Interfaces.IChannel)grid[i, gridColName].Tag;
            }
            plugin.Channels = channels;

            IModbusStation[] stations = new IModbusStation[stationGrid.RowsCount - 1];
            for (int i = 1; i < stationGrid.RowsCount; i++)
            {
                stations[i - 1] = (IModbusStation)stationGrid[i, stationGridColName].Tag;
            }
			plugin.Stations = stations;

            foreach (IModbusStation stat in stations)
            {
                stat.ClearChannels();
                foreach (ModbusChannelImp chan in channels)
                    if (chan.ModbusStation == stat.Name)
                    {
                        stat.AddChannel(chan);
                        chan.MyStation = (ModbusBaseClientStation)stat;
                    }
            }

			plugin.SaveSettings();
		}
    }
}
