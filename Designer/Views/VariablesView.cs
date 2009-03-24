using System;
using System.Drawing;
using System.Windows.Forms;
using FreeSCADA.Common;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Designer.Views
{
    class VariablesView : DocumentView
    {
        private SourceGrid.Grid channelsGrid;
        private SplitContainer splitContainer1;
        private SourceGrid.Grid pluginsGrid;
        private Label label2;
        private Label label1;
        int lockedForEditRow = 0;
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
            this.pluginsGrid = new SourceGrid.Grid();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
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
            this.channelsGrid.Location = new System.Drawing.Point(3, 26);
            this.channelsGrid.Name = "channelsGrid";
            this.channelsGrid.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.channelsGrid.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.channelsGrid.Size = new System.Drawing.Size(737, 249);
            this.channelsGrid.TabIndex = 2;
            this.channelsGrid.TabStop = true;
            this.channelsGrid.ToolTipText = "";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(2, 4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pluginsGrid);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.channelsGrid);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(743, 384);
            this.splitContainer1.SplitterDistance = 102;
            this.splitContainer1.TabIndex = 3;
            // 
            // pluginsGrid
            // 
            this.pluginsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pluginsGrid.AutoStretchColumnsToFitWidth = true;
            this.pluginsGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pluginsGrid.Location = new System.Drawing.Point(3, 24);
            this.pluginsGrid.Name = "pluginsGrid";
            this.pluginsGrid.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.pluginsGrid.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.pluginsGrid.Size = new System.Drawing.Size(737, 75);
            this.pluginsGrid.TabIndex = 3;
            this.pluginsGrid.TabStop = true;
            this.pluginsGrid.ToolTipText = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(258, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "Communication Plugins";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, -4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "Channels";
            // 
            // VariablesView
            // 
            this.ClientSize = new System.Drawing.Size(744, 400);
            this.Controls.Add(this.splitContainer1);
            this.Name = "VariablesView";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
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

        public delegate void SelectChannelHandler(object channel);
		/// <summary>Occurs when user clicks on a node from the list</summary>
        public event SelectChannelHandler SelectChannel;

        void Selection_SelectionChanged(object sender, SourceGrid.RangeRegionChangedEventArgs e)
        {
            int[] rows = channelsGrid.Selection.GetSelectionRegion().GetRowsIndex();
            if (rows.Length > 0)
            {
                if (SelectChannel != null)
                    SelectChannel(this.channelsGrid.Rows[rows[0]].Tag);
                //this.propertyGrid.SelectedObject = this.channelsGrid.Rows[rows[0]].Tag;
                //this.propertyGrid.ReadOnly = true;
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
            if (rowIndex != lockedForEditRow)
            {
                channelsGrid[rowIndex, 1].Value = (channel.Value == null) ? "{null}" : channel.Value;
                channelsGrid[rowIndex, 2].Value = channel.StatusFlags;
                channelsGrid[rowIndex, 3].Value = channel.ModifyTime;
            }
        }

        delegate void InvokeDelegate();
        void OnChannelValueChanged(object sender, EventArgs e)
        {
            IChannel ch = (IChannel)sender;
            object[] args = { ch, ch.Tag };
            channelsGrid.BeginInvoke(new UpdateChannelDelegate(UpdateChannelFunc), args);
            //if (SelectChannel != null)
            //    SelectChannel(ch);
            //if (ch == propertyGrid.SelectedObject)
            //    propertyGrid.BeginInvoke(new InvokeDelegate(delegate() { propertyGrid.Refresh(); }));
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
            if (!Env.Current.CommunicationPlugins.IsConnected) return;

            int[] rows = channelsGrid.Selection.GetSelectionRegion().GetRowsIndex();
            if (rows.Length > 0)
            {
                IChannel chan = (IChannel)channelsGrid.Rows[rows[0]].Tag;
                if (chan != null)
                {
                    if (!chan.IsReadOnly)
                    {
                        channelsGrid[rows[0], 1] = new SourceGrid.Cells.Cell(chan.Value == null ? "{null}" : chan.Value, chan.Value.GetType());
                        SourceGrid.CellContext context = new SourceGrid.CellContext(channelsGrid, new SourceGrid.Position(rows[0], 1));
                        context.StartEdit();
                        if (channelsGrid[rows[0], 1].FindController(typeof(myController)) == null)
                            channelsGrid[rows[0], 1].AddController(new myController(this));
                        lockedForEditRow = rows[0];
                    }
                }
            }
        }

        class myController : SourceGrid.Cells.Controllers.ControllerBase
        {
            SourceGrid.Grid channelsGrid;
            VariablesView variablesView;

            public myController(VariablesView variablesView)
            {
                this.channelsGrid = variablesView.channelsGrid;
                this.variablesView = variablesView;
            }

            public override void OnValueChanged(SourceGrid.CellContext sender, EventArgs e)
            {
                base.OnValueChanged(sender, e);

                if (channelsGrid.Rows[sender.Position.Row].Tag != null && variablesView.lockedForEditRow == sender.Position.Row)
                    if ((channelsGrid.Rows[sender.Position.Row].Tag as IChannel).Value != sender.Value)
                        (channelsGrid.Rows[sender.Position.Row].Tag as IChannel).Value = sender.Value;
                sender.EndEdit(false);
            }
            public override void OnFocusLeft(SourceGrid.CellContext sender, EventArgs e)
            {
                variablesView.lockedForEditRow = 0;
                sender.Value = (channelsGrid.Rows[sender.Position.Row].Tag as IChannel).Value;
            }
        }
    }
}
