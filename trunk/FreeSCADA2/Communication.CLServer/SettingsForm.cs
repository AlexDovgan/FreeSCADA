using System;
using System.Windows.Forms;

namespace FreeSCADA.Communication.CLServer
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

			grid.ColumnsCount = 4;
			grid.RowsCount = 1;
			grid[0, 0] = new SourceGrid.Cells.ColumnHeader("Channel name");
			grid[0, 1] = new SourceGrid.Cells.ColumnHeader("Remote channel");
			grid[0, 2] = new SourceGrid.Cells.ColumnHeader("Server");
			grid[0, 3] = new SourceGrid.Cells.ColumnHeader("Port");

			LoadChannels();

			grid.AutoStretchColumnsToFitWidth = true;
			grid.AutoSizeCells();
		}

		private void OnImport(object sender, EventArgs e)
		{
			ImportChannelsForm form = new ImportChannelsForm();
			form.ShowDialog(this);

			int varNumber = GetVariableInitialNumber();
			int row = grid.RowsCount;
			grid.RowsCount += form.Channels.Count;
			foreach (ImportChannelsForm.RemoteChannelInfo ch in form.Channels)
			{
				string variableName = GetVariableName(varNumber);
				varNumber++;

				grid[row, 0] = new SourceGrid.Cells.Cell(variableName, typeof(string));
				grid[row, 1] = new SourceGrid.Cells.Cell(ch.channelFullId, typeof(string));
				grid[row, 2] = new SourceGrid.Cells.Cell(ch.server, typeof(string));
				grid[row, 3] = new SourceGrid.Cells.Cell(ch.port, typeof(int));
				grid.Rows[row].Tag = ch.type;

				row++;
			}
			grid.Selection.ResetSelection(true);
			grid.Selection.SelectRow(row-1, true);
		}

		private void AddVariable(string variableName, string server, string fullId, int port, Type type)
		{
			int row = grid.RowsCount;
			grid.RowsCount++;

			grid[row, 0] = new SourceGrid.Cells.Cell(variableName, typeof(string));
			grid[row, 1] = new SourceGrid.Cells.Cell(fullId, typeof(string));
			grid[row, 2] = new SourceGrid.Cells.Cell(server, typeof(string));
			grid[row, 3] = new SourceGrid.Cells.Cell(port, typeof(int));
			grid.Rows[row].Tag = type;

			grid.Selection.ResetSelection(true);
			grid.Selection.SelectRow(row, true);
		}

		private void OnRemoveRow(object sender, EventArgs e)
		{
			foreach(int row in grid.Selection.GetSelectionRegion().GetRowsIndex())
				grid.Rows.Remove(row);
		}

		private string GetVariableName(int baseNumber)
		{
			string baseName = "variable_";
			return string.Format(System.Globalization.CultureInfo.CurrentUICulture, "{0}{1}", baseName, baseNumber);
		}

		private int GetVariableInitialNumber()
		{
			string baseName = "variable_";
			int baseNumber = 1;
			for (int i = 1; i < grid.RowsCount; i++)
			{
				string gridText = grid[i, 0].DisplayText;
				if (gridText.StartsWith(baseName))
				{
					string number = gridText.Substring(baseName.Length);
					int tmp;
					if (int.TryParse(number, out tmp))
						baseNumber = Math.Max(baseNumber, tmp + 1);
				}
			}
			return baseNumber;
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
			foreach (RemoutingChannel channel in plugin.Channels)
				AddVariable(channel.Name, channel.Server, channel.ServerFullId, channel.Port, channel.Type);
		}

		private void SaveChannels()
		{
			Interfaces.IChannel[] channels = new Interfaces.IChannel[grid.RowsCount - 1];
			for (int i = 1; i < grid.RowsCount; i++)
			{
				Type type = typeof(object);
				if (grid.Rows[i].Tag != null && grid.Rows[i].Tag is Type)
					type = grid.Rows[i].Tag as Type;

				channels[i - 1] = ChannelFactory.CreateChannel(	grid[i, 0].DisplayText,
																plugin,
																grid[i, 2].DisplayText,
																grid[i, 1].DisplayText,
																(int)grid[i, 3].Value,
																type);
			}
			plugin.Channels = channels;
			plugin.SaveSettings();
		}
	}
}
