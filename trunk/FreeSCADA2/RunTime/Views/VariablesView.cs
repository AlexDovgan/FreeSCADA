using System;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using FreeSCADA.Common;
using FreeSCADA.Interfaces;
using FreeSCADA.RunTime.Dialogs;

namespace FreeSCADA.RunTime.Views
{
	class VariablesView : DocumentView
	{
        private SourceGrid.Grid channelsGrid;
        private SplitContainer splitContainer1;
        private ReadOnlyPropertyGrid propertyGrid;
		//private System.ComponentModel.IContainer components;

        public VariablesView()
		{
			DocumentName = "Variables view [table]";
			InitializeComponent();
            Initialize();
		}

		private void InitializeComponent()
		{
            this.channelsGrid = new SourceGrid.Grid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.propertyGrid = new ReadOnlyPropertyGrid();   //System.Windows.Forms.PropertyGrid();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelsGrid
            // 
            this.channelsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.channelsGrid.AutoStretchColumnsToFitWidth = true;
            this.channelsGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.channelsGrid.Location = new System.Drawing.Point(3, 3);
            this.channelsGrid.Name = "channelsGrid";
            this.channelsGrid.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.channelsGrid.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.channelsGrid.Size = new System.Drawing.Size(567, 394);
            this.channelsGrid.TabIndex = 2;
            this.channelsGrid.TabStop = true;
            this.channelsGrid.ToolTipText = "";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.channelsGrid);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propertyGrid);
            this.splitContainer1.Size = new System.Drawing.Size(744, 400);
            this.splitContainer1.SplitterDistance = 573;
            this.splitContainer1.TabIndex = 3;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            //this.propertyGrid.Enabled = false;
            this.propertyGrid.Location = new System.Drawing.Point(3, 3);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(163, 393);
            this.propertyGrid.TabIndex = 0;
            // 
            // VariablesView
            // 
            this.ClientSize = new System.Drawing.Size(744, 400);
            this.Controls.Add(this.splitContainer1);
            this.Name = "VariablesView";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

        void Initialize()
        {
            DevAge.Drawing.RectangleBorder b = channelsGrid.Selection.Border;
            b.SetWidth(0);
            channelsGrid.Selection.Border = b;
            channelsGrid.Selection.FocusBackColor = channelsGrid.Selection.BackColor;
            channelsGrid.ColumnsCount = 6;

            SourceGrid.Cells.Views.Cell categoryView = GetCategoryCellView();

            channelsGrid.RowsCount++;
            channelsGrid[0, 0] = new SourceGrid.Cells.ColumnHeader("Channel");
            channelsGrid[0, 1] = new SourceGrid.Cells.ColumnHeader("Value");
            channelsGrid[0, 2] = new SourceGrid.Cells.ColumnHeader("Status");
            channelsGrid[0, 3] = new SourceGrid.Cells.ColumnHeader("Time");
            channelsGrid[0, 4] = new SourceGrid.Cells.ColumnHeader("Access");
            channelsGrid[0, 5] = new SourceGrid.Cells.ColumnHeader("Type");

            foreach (string plugId in Env.Current.CommunicationPlugins.PluginIds)
                LoadPlugin(categoryView, plugId);

            channelsGrid.AutoStretchColumnsToFitWidth = true;
            channelsGrid.AutoSizeCells();
            channelsGrid.Selection.EnableMultiSelection = false;

            this.FormClosing += new FormClosingEventHandler(VariablesView_FormClosing);
            this.channelsGrid.Selection.SelectionChanged += new SourceGrid.RangeRegionChangedEventHandler(Selection_SelectionChanged);
            channelsGrid.MouseDoubleClick += new MouseEventHandler(channelsGrid_MouseDoubleClick);

        }

        void Selection_SelectionChanged(object sender, SourceGrid.RangeRegionChangedEventArgs e)
        {
            int [] rows = channelsGrid.Selection.GetSelectionRegion().GetRowsIndex();
            if (rows.Length > 0)
            {
                this.propertyGrid.SelectedObject = this.channelsGrid.Rows[rows[0]].Tag;
                this.propertyGrid.ReadOnly = true;
            }
        }

        private SourceGrid.Cells.Views.Cell GetCategoryCellView()
        {
            SourceGrid.Cells.Views.Cell categoryView = new SourceGrid.Cells.Views.Cell();
            categoryView.Background = new DevAge.Drawing.VisualElements.BackgroundLinearGradient(SystemColors.ActiveCaption, SystemColors.InactiveCaption, 90.0f);
            categoryView.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText);
            categoryView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            categoryView.Border = DevAge.Drawing.RectangleBorder.NoBorder;
            //categoryView.Font = new Font(Font, FontStyle.Bold);
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

            foreach (IChannel ch in plugs[plugId].Channels)
            {
                channelsGrid.RowsCount++;
                curRow++;

                channelsGrid[curRow, 0] = new SourceGrid.Cells.Cell(ch.Name);
                channelsGrid[curRow, 1] = new SourceGrid.Cells.Cell(ch.Value == null ? "{null}" : ch.Value);
                channelsGrid[curRow, 2] = new SourceGrid.Cells.Cell(ch.StatusFlags);
                channelsGrid[curRow, 3] = new SourceGrid.Cells.Cell(ch.ModifyTime);
                channelsGrid[curRow, 4] = new SourceGrid.Cells.Cell(ch.IsReadOnly ? "R" : "RW");
                channelsGrid[curRow, 5] = new SourceGrid.Cells.Cell(ch.Type);
                channelsGrid.Rows[curRow].Tag = ch;
                ch.Tag = curRow;
                ch.ValueChanged += new EventHandler(OnChannelValueChanged);
            }
        }

        private delegate void UpdateChannelDelegate(IChannel channel, int rowIndex);
        private void UpdateChannelFunc(IChannel channel, int rowIndex)
        {
            channelsGrid[rowIndex, 1].Value = (channel.Value == null) ? "{null}" : channel.Value;
            channelsGrid[rowIndex, 2].Value = channel.StatusFlags;
            channelsGrid[rowIndex, 3].Value = channel.ModifyTime;
        }

        delegate void InvokeDelegate();
        void OnChannelValueChanged(object sender, EventArgs e)
        {
            IChannel ch = (IChannel)sender;
            object[] args = { ch, ch.Tag };
            channelsGrid.BeginInvoke(new UpdateChannelDelegate(UpdateChannelFunc), args);
            if (ch == propertyGrid.SelectedObject)
                propertyGrid.BeginInvoke(new InvokeDelegate(delegate() { propertyGrid.Refresh(); }));
        }

        private void VariablesView_FormClosing(object sender, FormClosingEventArgs e)
        {
            CommunationPlugs plugs = Env.Current.CommunicationPlugins;
            foreach (string plugId in Env.Current.CommunicationPlugins.PluginIds)
            {
                foreach (IChannel ch in plugs[plugId].Channels)
                {
                    ch.ValueChanged -= new EventHandler(OnChannelValueChanged);
                }
            }
            foreach (SourceGrid.Grid.GridRow row in channelsGrid.Rows)
            {
                if (row.Tag != null)
                {
                    IChannel ch = (IChannel)row.Tag;
                    ch.Tag = null; //Clear our tags
                }
            }
        }

        void channelsGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int[] rows = channelsGrid.Selection.GetSelectionRegion().GetRowsIndex();
            if (rows.Length > 0)
            {
                IChannel chan = (IChannel)channelsGrid.Rows[rows[0]].Tag;
                if (chan != null)
                {
                    if (!chan.IsReadOnly) 
                    {
                        SetVariableValue svv = new SetVariableValue(chan);
                        svv.ShowDialog();
                    }
                }
            }
        }

    }
}
