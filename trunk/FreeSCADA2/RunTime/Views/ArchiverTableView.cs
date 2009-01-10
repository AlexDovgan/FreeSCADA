using System;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using FreeSCADA.Archiver;

namespace FreeSCADA.RunTime.Views
{
	class ArchiverTableView : DocumentView
	{
		struct ThreadData
		{
			public ArchiverTableView view;
			public QueryInfo query;
		}

		private System.Windows.Forms.BindingSource bindingSource1;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.Label label1;
		private System.ComponentModel.IContainer components;
	
		public ArchiverTableView()
		{
			DocumentName = "Historical view [table]";
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.label1 = new System.Windows.Forms.Label();
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
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(744, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Loading data. Please wait...";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label1.Visible = false;
			// 
			// ArchiverTableView
			// 
			this.ClientSize = new System.Drawing.Size(744, 400);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.dataGridView1);
			this.Name = "ArchiverTableView";
			((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);

		}

		static void DataLoadingThread(Object threadArgs)
		{
			ThreadData data = (ThreadData)threadArgs;

			DataTable dt = ArchiverMain.Current.GetChannelData(data.query.From, data.query.To, data.query.Channels);
            if (dt == null) return;

			object[] args = new object[1];
			args[0] = dt;
			data.view.BeginInvoke(new LoadingFinishedDelegate(data.view.OnLoadingFinished), args);
		}

		public delegate void LoadingFinishedDelegate(DataTable dt);
		public void OnLoadingFinished(DataTable dt)
		{
			bindingSource1.DataSource = dt;

			dataGridView1.Visible = true;
			label1.Visible = false;

			Cursor = Cursors.Default;
		}


		public bool Open(QueryInfo queryInfo)
		{
			dataGridView1.AutoGenerateColumns = true;

			dataGridView1.Visible = false;
			label1.Visible = true;
			Cursor = Cursors.WaitCursor;

			ThreadData args = new ThreadData();
			args.view = this;
			args.query = queryInfo;

			ThreadPool.QueueUserWorkItem(new WaitCallback(DataLoadingThread), args);

			return true;
		}
    }
}
