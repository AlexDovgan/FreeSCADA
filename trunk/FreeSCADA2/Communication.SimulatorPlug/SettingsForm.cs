using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FreeSCADA.Common;

namespace FreeSCADA.Communication.SimulatorPlug
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

			variableTypeNames[typeof(CurrentTimeChannel).FullName]		= "Current time";
            variableTypeNames[typeof(RandomIntegerChannel).FullName] = "Random integer";
            variableTypeNames[typeof(SawIntegerChannel).FullName] = "Saw (integer -100 .. 100)";
            variableTypeNames[typeof(RampIntegerChannel).FullName] = "Ramp (integer 0 .. 100)";
            variableTypeNames[typeof(SinusDoubleChannel).FullName] = "Sinus (double -1 .. 1)";
            variableTypeNames[typeof(GenericChannel<int>).FullName] = "Simple integer";
            variableTypeNames[typeof(GenericChannel<string>).FullName] = "Simple string";
			variableTypeNames[typeof(GenericChannel<float>).FullName]	= "Simple float";
			variableTypeNames[typeof(ComputableChannel).FullName] = "Computable";
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
			grid[0, 0] = new SourceGrid.Cells.ColumnHeader("Channel name");
			grid[0, 1] = new SourceGrid.Cells.ColumnHeader("Type");
			grid[0, 2] = new SourceGrid.Cells.ColumnHeader("Read only");

			grid.Selection.SelectionChanged += new SourceGrid.RangeRegionChangedEventHandler(OnSelectionChanged);

			LoadChannels();

			grid.AutoStretchColumnsToFitWidth = true;
			grid.AutoSizeCells();

			InitializeTooltips();
		}

		void InitializeTooltips()
		{
			ToolTip expressionTooltip = new ToolTip();
			expressionTooltip.AutomaticDelay = 180000;
			expressionTooltip.InitialDelay = 100;
			expressionTooltip.ShowAlways = true;

			string tip = "Channel processing script.\n\n" +
						"The script is Python based (please refer to Python documentation for the syntax).\n"+
						"At the end of your script, you must assing some value to 'result' variable. This is\n"+
						"internal value used by FreeSCADA. You could also refer to other variables withing Simulator\n"+
						"plugin just by typing it names like a variable.\n\nExample:\n"+
						"   if variable_1 > 5:\n"+
						"      result = 1\n"+
						"   else:\n"+
						"      result = 0";
			expressionTooltip.SetToolTip(expressionEditBox, tip);
		}

		void OnSelectionChanged(object sender, SourceGrid.RangeRegionChangedEventArgs e)
		{
			UpdateExpressionField();
		}

		private void UpdateExpressionField()
		{
			if (grid.Selection.GetSelectionRegion().Count == 0)
				expressionEditBox.Enabled = false;

			foreach (int row in grid.Selection.GetSelectionRegion().GetRowsIndex())
			{
				UpdateExpressionFieldForRow(row, grid[row, 1].DisplayText);
			}
		}

		private void UpdateExpressionFieldForRow(int row, string typeDisplayName)
		{
			string type = null;
			foreach (KeyValuePair<string, string> pair in variableTypeNames)
			{
				if (typeDisplayName == pair.Value)
				{
					type = pair.Key;
					break;
				}
			}

			if (type == typeof(ComputableChannel).FullName)
			{
				expressionEditBox.Enabled = true;
				if (grid.Rows[row].Tag != null)
					expressionEditBox.Text = (string)grid.Rows[row].Tag;
				else
					expressionEditBox.Text = "";
			}
			else
			{
				expressionEditBox.Enabled = false;
				expressionEditBox.Text = "";
			}
		}

		private void OnAddRow(object sender, EventArgs e)
		{
			string variableName = GetUniqueVariableName();
			AddVariable(variableName, typeof(GenericChannel<int>).FullName, false);
		}

		private void AddVariable(string variableName, string type, bool readOnly)
		{
			int row = grid.RowsCount;
			grid.RowsCount++;

			grid[row, 0] = new SourceGrid.Cells.Cell(variableName, typeof(string));
			
			SourceGrid.Cells.Editors.ComboBox combo = new SourceGrid.Cells.Editors.ComboBox(typeof(string), channelNames, true);
			combo.Control.SelectionChangeCommitted += new EventHandler(OnChannelTypeChanged);
			grid[row, 1] = new SourceGrid.Cells.Cell(variableTypeNames[type], combo);
			SourceGrid.Cells.CheckBox check = new SourceGrid.Cells.CheckBox();
			check.Checked = readOnly;
			grid[row, 2] = check;

			grid.Selection.ResetSelection(true);
			grid.Selection.SelectRow(row, true);
		}

		void OnChannelTypeChanged(object sender, EventArgs e)
		{
			DevAge.Windows.Forms.DevAgeComboBox combo = (DevAge.Windows.Forms.DevAgeComboBox)sender;
			foreach (int row in grid.Selection.GetSelectionRegion().GetRowsIndex())
			{
				UpdateExpressionFieldForRow(row, (string)combo.Value);
			}
		}

		private void OnRemoveRow(object sender, EventArgs e)
		{
			foreach (int row in grid.Selection.GetSelectionRegion().GetRowsIndex())
			{
				if(grid[row, 1].Editor is SourceGrid.Cells.Editors.ComboBox)
				{
					SourceGrid.Cells.Editors.ComboBox combo = grid[row, 1].Editor as SourceGrid.Cells.Editors.ComboBox;
					combo.Control.SelectionChangeCommitted -= new EventHandler(OnChannelTypeChanged);
				}
				grid.Rows.Remove(row);
			}
		}

		private string GetUniqueVariableName()
		{
			string baseName = "variable_";
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
			foreach (BaseChannel channel in plugin.Channels)
			{
				AddVariable(channel.Name, channel.GetType().FullName, channel.IsReadOnly);
				if (channel is ComputableChannel)
					grid.Rows[grid.RowsCount - 1].Tag = (channel as ComputableChannel).Expression;
			}
		}

		private void SaveChannels()
		{
			Interfaces.IChannel[] channels = new Interfaces.IChannel[grid.RowsCount - 1];
			for (int i = 1; i < grid.RowsCount; i++)
			{
				string name = grid[i, 0].DisplayText;
				bool readOnly = (bool)((SourceGrid.Cells.CheckBox)grid[i, 2]).Checked;

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

				if (type == typeof(ComputableChannel).FullName)
				{
					string expression = "";
					if(grid.Rows[i].Tag != null)
						expression = (string)grid.Rows[i].Tag;
					channels[i - 1] = new ComputableChannel(name, plugin, expression);
				}
				else
					channels[i - 1] = ChannelFactory.CreateChannel(type, name, readOnly, plugin);
			}
			plugin.Channels = channels;
			plugin.SaveSettings();
		}

		private void expressionEditBox_TextChanged(object sender, EventArgs e)
		{
			foreach (int row in grid.Selection.GetSelectionRegion().GetRowsIndex())
			{
				grid.Rows[row].Tag = expressionEditBox.Text;
			}
		}
	}
}
