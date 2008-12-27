using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FreeSCADA.Common;

namespace FreeSCADA.Communication.Timers
{
	public partial class SettingsForm : Form
	{
		Dictionary<string, string> variableTypeNames = new Dictionary<string,string>();
		List<string> channelNames = new List<string>();
		Plugin plugin;

		public SettingsForm(Plugin plugin)
		{
			InitializeComponent();
			this.plugin = plugin;

            variableTypeNames[typeof(RelativeTimerChannel).FullName] = "Relative timer";
            //variableTypeNames[typeof(AbsoluteTimerChannel).FullName] = "Absolute timer";

            foreach (KeyValuePair<string, string> pair in variableTypeNames)
				channelNames.Add(pair.Value);

			grid.SelectionMode = SourceGrid.GridSelectionMode.Row;
			grid.Selection.EnableMultiSelection = false;

			DevAge.Drawing.RectangleBorder b = grid.Selection.Border;
			b.SetWidth(0);
			grid.Selection.Border = b;
			grid.Selection.FocusBackColor = grid.Selection.BackColor;

			grid.ColumnsCount = 3;
			grid.RowsCount = 1;
			grid[0, 0] = new SourceGrid.Cells.ColumnHeader("Timer name");
			grid[0, 1] = new SourceGrid.Cells.ColumnHeader("Type");
			grid[0, 2] = new SourceGrid.Cells.ColumnHeader("Interval [ms]");

			LoadChannels();

			grid.AutoStretchColumnsToFitWidth = true;
			grid.AutoSizeCells();
		}

		private void OnAddRow(object sender, EventArgs e)
		{
			string variableName = GetUniqueVariableName();
			AddVariable(variableName, typeof(RelativeTimerChannel).FullName, 1000.0);
		}

		private void AddVariable(string variableName, string type, double interval)
		{
			int row = grid.RowsCount;
			grid.RowsCount++;

			grid[row, 0] = new SourceGrid.Cells.Cell(variableName, typeof(string));
			
			SourceGrid.Cells.Editors.ComboBox combo = new SourceGrid.Cells.Editors.ComboBox(typeof(string), channelNames, true);
			grid[row, 1] = new SourceGrid.Cells.Cell(variableTypeNames[type], combo);
			grid[row, 2] = new SourceGrid.Cells.Cell(interval, typeof(double));

			grid.Selection.ResetSelection(true);
			grid.Selection.SelectRow(row, true);
		}

		private void OnRemoveRow(object sender, EventArgs e)
		{
			foreach(int row in grid.Selection.GetSelectionRegion().GetRowsIndex())
				grid.Rows.Remove(row);
		}

		private string GetUniqueVariableName()
		{
			string baseName = "timer_";
			int baseNumber = 1;
			for(;;)
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

		private void OnOkClick(object sender, EventArgs e)
		{
			SaveChannels();
			Close();
		}

		private void OnCancelClick(object sender, EventArgs e)
		{
			Close();
		}

		private void LoadChannels()
		{
			foreach (RelativeTimerChannel channel in plugin.Channels)
				AddVariable(channel.Name, channel.GetType().FullName, channel.Interval);
		}

		private void SaveChannels()
		{
			Interfaces.IChannel[] channels = new Interfaces.IChannel[grid.RowsCount - 1];
			for (int i = 1; i < grid.RowsCount; i++)
			{
				string name = grid[i, 0].DisplayText;
                double interval = (double)(grid[i, 2]).Value;

				string type = null;
				foreach (KeyValuePair<string, string> pair in variableTypeNames)
				{
					if(grid[i, 1].DisplayText == pair.Value)
					{
						type = pair.Key;
						break;
					}
				}
				if (type == null)
					continue;

				channels[i - 1] = ChannelFactory.CreateChannel(type, name, plugin, interval);
			}
			plugin.Channels = channels;
			plugin.SaveSettings();
		}
	}
}
