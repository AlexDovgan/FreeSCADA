using System;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using FreeSCADA.Common;
using FreeSCADA.Interfaces;

namespace FreeSCADA.RunTime.Views
{
	class VariablesView : DocumentView
	{
		private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private SourceGrid.Grid channelsGrid;
		private System.ComponentModel.IContainer components;

        public VariablesView()
		{
			DocumentName = "Variables view [table]";
			InitializeComponent();
            Initialize();
		}

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.channelsGrid = new SourceGrid.Grid();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.DataSource = this.bindingSource1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(744, 400);
            this.dataGridView1.TabIndex = 1;
            // 
            // channelsGrid
            // 
            this.channelsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.channelsGrid.AutoStretchColumnsToFitWidth = true;
            this.channelsGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.channelsGrid.Location = new System.Drawing.Point(0, 0);
            this.channelsGrid.Name = "channelsGrid";
            this.channelsGrid.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.channelsGrid.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.channelsGrid.Size = new System.Drawing.Size(744, 400);
            this.channelsGrid.TabIndex = 2;
            this.channelsGrid.TabStop = true;
            this.channelsGrid.ToolTipText = "";
            // 
            // ArchiverTableView
            // 
            this.ClientSize = new System.Drawing.Size(744, 400);
            this.Controls.Add(this.channelsGrid);
            this.Controls.Add(this.dataGridView1);
            this.Name = "ArchiverTableView";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
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

            this.FormClosing += new FormClosingEventHandler(VariablesView_FormClosing);
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

        void OnChannelValueChanged(object sender, EventArgs e)
        {
            IChannel ch = (IChannel)sender;
            object[] args = { ch, ch.Tag };
            channelsGrid.BeginInvoke(new UpdateChannelDelegate(UpdateChannelFunc), args);
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
    }
}
