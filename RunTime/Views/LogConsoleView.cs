using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace FreeSCADA.RunTime.Views
{
	class LogConsoleView : DocumentView
	{
		private ListView listView1;
		private ColumnHeader columnHeader1;
		private ImageList imageList1;
		private System.ComponentModel.IContainer components;
		private ColumnHeader columnHeader3;
		private ColumnHeader columnHeader2;
	
		private class CustomLogger: FreeSCADA.Common.Logger
		{
			public delegate void OnMessageHandler(Severity severity, string message);
			public event OnMessageHandler OnMessage;

			public override void Log(Severity severity, string message)
			{
				base.Log(severity, message);

				if (OnMessage != null)
					OnMessage(severity, message);
			}
		}


		public LogConsoleView()
		{
			DockAreas = DockAreas.Float | DockAreas.DockBottom | DockAreas.DockTop;
			DocumentName = "Console";
			InitializeComponent();

			imageList1.Images.Add(global::FreeSCADA.RunTime.Properties.Resources.log_info);
			imageList1.Images.Add(global::FreeSCADA.RunTime.Properties.Resources.log_warning);
			imageList1.Images.Add(global::FreeSCADA.RunTime.Properties.Resources.log_error);

			CustomLogger logger = new CustomLogger();
			logger.OnMessage += new CustomLogger.OnMessageHandler(logger_OnMessage);
			FreeSCADA.Common.Env.Current.Logger = logger;
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// listView1
			// 
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
			this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView1.FullRowSelect = true;
			this.listView1.Location = new System.Drawing.Point(0, 0);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(260, 237);
			this.listView1.SmallImageList = this.imageList1;
			this.listView1.TabIndex = 0;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.Resize += new System.EventHandler(this.listView1_Resize);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Severity";
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Message";
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Time";
			// 
			// imageList1
			// 
			this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imageList1.ImageSize = new System.Drawing.Size(12, 12);
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// LogConsoleView
			// 
			this.ClientSize = new System.Drawing.Size(260, 237);
			this.Controls.Add(this.listView1);
			this.Name = "LogConsoleView";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LogConsoleView_FormClosed);
			this.ResumeLayout(false);

		}

		void logger_OnMessage(FreeSCADA.Common.Logger.Severity severity, string message)
		{
			object[] args = new object[2];
			args[0] = severity;
			args[1] = message;
			BeginInvoke(new CustomLogger.OnMessageHandler(this.OnMessage), args);
		}

		void OnMessage(FreeSCADA.Common.Logger.Severity severity, string message)
		{
			ListViewItem item = listView1.Items.Add("", (int)severity);
			item.SubItems.Add(System.DateTime.Now.ToString("HH:mm:ss"));
			item.SubItems.Add(message);
			if (severity == FreeSCADA.Common.Logger.Severity.Error || severity == FreeSCADA.Common.Logger.Severity.Warning)
				Activate();
		}

		private void listView1_Resize(object sender, System.EventArgs e)
		{
			columnHeader1.Width = imageList1.ImageSize.Width*2;
			columnHeader2.Width = -2;
			columnHeader3.Width = -2;
		}

		private void LogConsoleView_FormClosed(object sender, FormClosedEventArgs e)
		{
			FreeSCADA.Common.Env.Current.Logger = new FreeSCADA.Common.Logger();
		}
    }
}
