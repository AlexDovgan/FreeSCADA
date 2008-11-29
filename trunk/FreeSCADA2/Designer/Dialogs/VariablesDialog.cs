using System;
using System.Drawing;
using System.Windows.Forms;
using FreeSCADA.Common;
using FreeSCADA.ShellInterfaces;

namespace FreeSCADA.Designer.Dialogs
{
	/// <summary>
	/// The form displays a complete list of variables from current Project.
	/// </summary>
	public partial class VariablesDialog : Form
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public VariablesDialog()
		{
			InitializeComponent();

			DevAge.Drawing.RectangleBorder b = channelsGrid.Selection.Border;
			b.SetWidth(0);
			channelsGrid.Selection.Border = b;
			channelsGrid.Selection.FocusBackColor = channelsGrid.Selection.BackColor;
			channelsGrid.ColumnsCount = 4;

			SourceGrid.Cells.Views.Cell categoryView = GetCategoryCellView();

			foreach (string plugId in Env.Current.CommunicationPlugins.PluginIds)
				LoadPlugin(categoryView, plugId);

			channelsGrid.AutoStretchColumnsToFitWidth = true;
			channelsGrid.AutoSizeCells();

			Connect(connectCheckBox.Checked);
		}

		private SourceGrid.Cells.Views.Cell GetCategoryCellView()
		{
			SourceGrid.Cells.Views.Cell categoryView = new SourceGrid.Cells.Views.Cell();
			categoryView.Background = new DevAge.Drawing.VisualElements.BackgroundLinearGradient(SystemColors.ActiveCaption, SystemColors.InactiveCaption, 90.0f);
			categoryView.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
			categoryView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
			categoryView.Border = DevAge.Drawing.RectangleBorder.NoBorder;
			categoryView.Font = new Font(Font, FontStyle.Bold);
			return categoryView;
		}

		private void LoadPlugin(SourceGrid.Cells.Views.Cell categoryView, string plugId)
		{
			int curRow = channelsGrid.RowsCount;
			channelsGrid.RowsCount += 2;
			CommunationPlugs plugs = Env.Current.CommunicationPlugins;

			channelsGrid[curRow, 0] = new SourceGrid.Cells.Cell(plugs[plugId].Name);
			channelsGrid[curRow, 0].ColumnSpan = channelsGrid.ColumnsCount;
			channelsGrid[curRow, 0].View = categoryView;
			channelsGrid[curRow, 0].AddController(new SourceGrid.Cells.Controllers.Unselectable());
			curRow++;

			channelsGrid[curRow, 0] = new SourceGrid.Cells.ColumnHeader("Channel");
			channelsGrid[curRow, 1] = new SourceGrid.Cells.ColumnHeader("Value");
			channelsGrid[curRow, 2] = new SourceGrid.Cells.ColumnHeader("Access");
			channelsGrid[curRow, 3] = new SourceGrid.Cells.ColumnHeader("Type");

			foreach(IChannel ch in plugs[plugId].Channels)
			{
				channelsGrid.RowsCount++;
				curRow++;

				channelsGrid[curRow, 0] = new SourceGrid.Cells.Cell(ch.Name);
				channelsGrid[curRow, 1] = new SourceGrid.Cells.Cell(ch.Value == null?"{null}":ch.Value);
				channelsGrid[curRow, 2] = new SourceGrid.Cells.Cell(ch.IsReadOnly?"R":"RW");
				channelsGrid[curRow, 3] = new SourceGrid.Cells.Cell(ch.Type);
				channelsGrid.Rows[curRow].Tag = ch;
				ch.Tag = curRow;
				ch.ValueChanged += new EventHandler(OnChannelValueChanged);
			}
		}

		private void OnCloseButton(object sender, EventArgs e)
		{
            CommunationPlugs plugs = Env.Current.CommunicationPlugins;
            foreach (string plugId in Env.Current.CommunicationPlugins.PluginIds)
                foreach (IChannel ch in plugs[plugId].Channels)
                {
                    ch.ValueChanged -= new EventHandler(OnChannelValueChanged);
                }
            Close();
		}

		private void OnConnectChanged(object sender, EventArgs e)
		{
			Connect(connectCheckBox.Checked);
		}

		private void Connect(bool enable)
		{
			if (enable)
			{
				if (Env.Current.CommunicationPlugins.Connect() == true)
					connectionStatusLabel.Text = "Connection status: connected";
			}
			else
			{
				Env.Current.CommunicationPlugins.Disconnect();
				connectionStatusLabel.Text = "Connection status: disconnected";
			}
		}

		private delegate void UpdateChannelDelegate(IChannel channel, int rowIndex);
		private void UpdateChannelFunc(IChannel channel, int rowIndex)
		{
			channelsGrid[rowIndex, 1].Value = (channel.Value == null) ? "{null}" : channel.Value;
			//Console.WriteLine("{0} UpdateChannelFunc",System.DateTime.Now);
		}

		void OnChannelValueChanged(object sender, EventArgs e)
		{
			IChannel ch = (IChannel)sender;
			object[] args = { ch, ch.Tag };
			channelsGrid.Invoke(new UpdateChannelDelegate(UpdateChannelFunc), args);
		}

		private void VariablesForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			Connect(false);
			foreach (SourceGrid.Grid.GridRow row in channelsGrid.Rows)
			{
				if (row.Tag != null)
				{
					IChannel ch = (IChannel)row.Tag;
					ch.Tag = null; //Clear our tags
				}
			}
		}
	}
}
