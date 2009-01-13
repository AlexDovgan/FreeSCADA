using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FreeSCADA.Common;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Designer.Dialogs
{
	/// <summary>
	/// The form displays a complete list of variables from current Project.
	/// </summary>
	public partial class VariablesDialog : Form
	{
		bool selectMode = false;
		List<IChannel> selectedChannels = new List<IChannel>();

		/// <summary>
		/// Constructor
		/// </summary>
		public VariablesDialog()
		{
			InitializeComponent();
			Initialize();			
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public VariablesDialog(bool selectMode)
		{
			this.selectMode = selectMode;

			InitializeComponent();
			Initialize();
		}

		void Initialize()
		{
			DevAge.Drawing.RectangleBorder b = channelsGrid.Selection.Border;
			b.SetWidth(0);
			channelsGrid.Selection.Border = b;
			channelsGrid.Selection.FocusBackColor = channelsGrid.Selection.BackColor;
			channelsGrid.ColumnsCount = 4;

			SourceGrid.Cells.Views.Cell categoryView = GetCategoryCellView();

			channelsGrid.RowsCount++;
			channelsGrid[0, 0] = new SourceGrid.Cells.ColumnHeader("Channel");
			channelsGrid[0, 1] = new SourceGrid.Cells.ColumnHeader("Value");
			channelsGrid[0, 2] = new SourceGrid.Cells.ColumnHeader("Access");
			channelsGrid[0, 3] = new SourceGrid.Cells.ColumnHeader("Type");

			foreach (string plugId in Env.Current.CommunicationPlugins.PluginIds)
				LoadPlugin(categoryView, plugId);

			channelsGrid.AutoStretchColumnsToFitWidth = true;
			channelsGrid.AutoSizeCells();

			channelsGrid.Selection.SelectionChanged += new SourceGrid.RangeRegionChangedEventHandler(OnSelectionChanged);
			UpdateSelectedChannels();

			Connect(connectCheckBox.Checked);

			selectButton.Visible = selectMode;
			if (selectMode == true)
				closeButton.Text = "Cancel";
			else
				closeButton.Text = "Close";
		}

		/// <summary>
		/// Returns list of selected channels. Available only if selectedMode==true was passed to the constructor.
		/// </summary>
		public List<IChannel> SelectedChannels
		{
			get { return selectedChannels; }
			set { selectedChannels = value; }
		}

		void OnSelectionChanged(object sender, SourceGrid.RangeRegionChangedEventArgs e)
		{
			UpdateSelectedChannels();
		}

		private void UpdateSelectedChannels()
		{
			selectedChannels.Clear();
			foreach (int row in channelsGrid.Selection.GetSelectionRegion().GetRowsIndex())
			{

				if (channelsGrid.Rows[row].Tag != null && channelsGrid.Rows[row].Tag is IChannel)
				{
					IChannel ch = channelsGrid.Rows[row].Tag as IChannel;
					selectedChannels.Add(ch);
				}
			}

			selectButton.Enabled = selectedChannels.Count > 0;
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
			channelsGrid.RowsCount++;
			CommunationPlugs plugs = Env.Current.CommunicationPlugins;

			channelsGrid[curRow, 0] = new SourceGrid.Cells.Cell(plugs[plugId].Name);
			channelsGrid[curRow, 0].ColumnSpan = channelsGrid.ColumnsCount;
			channelsGrid[curRow, 0].View = categoryView;
			channelsGrid[curRow, 0].AddController(new SourceGrid.Cells.Controllers.Unselectable());

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
			{
				foreach (IChannel ch in plugs[plugId].Channels)
				{
					ch.ValueChanged -= new EventHandler(OnChannelValueChanged);
				}
			}

			if (selectMode == true)
				DialogResult = DialogResult.Cancel;
			else
				DialogResult = DialogResult.OK;

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

		private void selectButton_Click(object sender, EventArgs e)
		{
            CommunationPlugs plugs = Env.Current.CommunicationPlugins;
            foreach (string plugId in Env.Current.CommunicationPlugins.PluginIds)
            {
                foreach (IChannel ch in plugs[plugId].Channels)
                {
                    ch.ValueChanged -= new EventHandler(OnChannelValueChanged);
                }
            }

            DialogResult = DialogResult.OK;
			Close();
		}

	}
}
