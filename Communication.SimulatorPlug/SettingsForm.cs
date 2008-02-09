using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace FreeSCADA.Communication.SimulatorPlug
{
	public partial class SettingsForm : Form
	{
		string[] variableTypeNames = { "Current time", "Random integer", "Simple integer", "Simple string", "Simple float"};
		enum VariableTypes { CurrentTime, RandomInteger, SimpleInteger, SimpleString, SimpleFloat};
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

			grid.ColumnsCount = 3;
			grid.RowsCount = 1;
			grid[0, 0] = new SourceGrid.Cells.ColumnHeader("Channel name");
			grid[0, 1] = new SourceGrid.Cells.ColumnHeader("Type");
			grid[0, 2] = new SourceGrid.Cells.ColumnHeader("Read only");

			LoadChannels();

			grid.AutoStretchColumnsToFitWidth = true;
			grid.AutoSizeCells();
		}

		private void OnAddRow(object sender, EventArgs e)
		{
			string variableName = GetUniqueVariableName();
			AddVariable(variableName, VariableTypes.SimpleInteger, false);
		}

		private void AddVariable(string variableName, VariableTypes type, bool readOnly)
		{
			int row = grid.RowsCount;
			grid.RowsCount++;

			grid[row, 0] = new SourceGrid.Cells.Cell(variableName, typeof(string));
			SourceGrid.Cells.Editors.ComboBox combo = new SourceGrid.Cells.Editors.ComboBox(typeof(string), variableTypeNames, true);
			grid[row, 1] = new SourceGrid.Cells.Cell(variableTypeNames[(int)type], combo);
			SourceGrid.Cells.CheckBox check = new SourceGrid.Cells.CheckBox();
			check.Checked = readOnly;
			grid[row, 2] = check;

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
			string baseName = "variable_";
			int baseNumber = 1;
			for(;;)
			{
				string newName = string.Format("{0}{1}", baseName, baseNumber);
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
			foreach (ChannelBase channel in plugin.Channels)
			{
				if (channel.InternalType == ChannelBase.InternalChannelType.SimpleGeneric)
				{
					if (channel.Type == typeof(string).Name)
						AddVariable(channel.Name, VariableTypes.SimpleString, channel.IsReadOnly);
					else if (channel.Type == typeof(int).Name)
						AddVariable(channel.Name, VariableTypes.SimpleInteger, channel.IsReadOnly);
					else if (channel.Type == typeof(float).Name)
						AddVariable(channel.Name, VariableTypes.SimpleFloat, channel.IsReadOnly);
				}
				else if (channel.InternalType == ChannelBase.InternalChannelType.RandomInteger)
					AddVariable(channel.Name, VariableTypes.RandomInteger, channel.IsReadOnly);
			}
		}

		private void SaveChannels()
		{
			ShellInterfaces.IChannel[] channels = new ShellInterfaces.IChannel[grid.RowsCount - 1];
			for (int i = 1; i < grid.RowsCount; i++)
			{
				string name = grid[i, 0].DisplayText;
				bool readOnly = (bool)((SourceGrid.Cells.CheckBox)grid[i, 2]).Checked;
				if (grid[i, 1].DisplayText == variableTypeNames[(int)VariableTypes.SimpleString])
					channels[i - 1] = new GenericChannel<string>(name, readOnly, plugin);
				else if (grid[i, 1].DisplayText == variableTypeNames[(int)VariableTypes.SimpleInteger])
					channels[i - 1] = new GenericChannel<int>(name, readOnly, plugin);
				else if (grid[i, 1].DisplayText == variableTypeNames[(int)VariableTypes.SimpleFloat])
					channels[i - 1] = new GenericChannel<float>(name, readOnly, plugin);
				else if (grid[i, 1].DisplayText == variableTypeNames[(int)VariableTypes.RandomInteger])
					channels[i - 1] = new RandomIntegerChannel(name, plugin);
			}
			plugin.Channels = channels;
			plugin.SaveSettings();
		}
	}
}
