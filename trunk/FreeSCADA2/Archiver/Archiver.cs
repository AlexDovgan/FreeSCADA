using System.Threading;
using System.Data;
using System.Data.Common;
using FreeSCADA.Common;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Archiver
{
	public class ArchiverMain
	{
		ChannelsSettings channelSettings = new ChannelsSettings();
		DatabaseSettings databaseSettings = new DatabaseSettings();
		DbWriter dbWriter;

		Thread channelUpdaterThread;

		#region Initialization and singleton implementation

		static ArchiverMain instance;

		public static void Initialize()
		{
			if (instance == null)
				instance = new ArchiverMain();
		}
        
		public static void Deinitialize()
		{
			instance = null;
		}

		public static ArchiverMain Current
		{
			get
			{
				if (instance == null)
					throw new System.NullReferenceException();

				return instance;
			}
		}

		ArchiverMain()
		{
			Env.Current.Project.ProjectLoaded += new System.EventHandler(OnProjectLoaded);
			Env.Current.Project.ProjectClosed += new System.EventHandler(OnProjectClosed);

			OnProjectLoaded(Env.Current.Project, new System.EventArgs());

			if (Env.Current.Mode == EnvironmentMode.Designer)
			{
				ICommandContext context = Env.Current.Commands.GetPredefinedContext(PredefinedContexts.Project);
				Env.Current.Commands.AddCommand(context, new PropertyCommand());
			}
		}

		#endregion

		void OnProjectClosed(object sender, System.EventArgs e)
		{
			channelSettings.Clear();
		}

		void OnProjectLoaded(object sender, System.EventArgs e)
		{
			databaseSettings.Load();
			channelSettings.Load();
		}

		public ChannelsSettings ChannelsSettings
		{
			get
			{
				return channelSettings;
			}
		}

		public DatabaseSettings DatabaseSettings
		{
			get
			{
				return databaseSettings;
			}
		}

		public bool IsRunning
		{
			get { return channelUpdaterThread != null; }
		}

		private static void ChannelUpdaterThreadProc(object obj)
		{
			ArchiverMain self = (ArchiverMain)obj;

			try
			{
				for (; ; )
				{
					//System.Console.WriteLine("{0} ChannelUpdaterThreadProc: Start loop", System.DateTime.Now);
					foreach (Rule rule in self.channelSettings.Rules)
					{
						if (rule.Enable)
						{
							foreach (BaseCondition cond in rule.Conditions)
								cond.Process();

							if (rule.Archive)
								self.dbWriter.WriteChannels(rule.Channels);
						}
					}
					Thread.Sleep(100);
				}
			}
			catch (ThreadAbortException)
			{
			}

			if (self.dbWriter != null)
				self.dbWriter.Close();
		}

		public bool Start()
		{
			dbWriter = new DbWriter();
			if (dbWriter.Open() == false)
				return false;

			channelUpdaterThread = new Thread(new ParameterizedThreadStart(ChannelUpdaterThreadProc));
			channelUpdaterThread.Start(this);

			return IsRunning;
		}

		public void Stop()
		{
			if (channelUpdaterThread != null)
			{
				channelUpdaterThread.Abort();
				channelUpdaterThread.Join();
				channelUpdaterThread = null;

				if (dbWriter != null)
					dbWriter.Close();
			}
		}

		public DataTable GetDataTable(string selectCommand)
		{
			DataTable data = new DataTable();
			data.Locale = System.Globalization.CultureInfo.InvariantCulture;

			DbProviderFactory dbProviderFactory = DatabaseFactory.Get(ArchiverMain.Current.DatabaseSettings.DbProvider);
			DbConnection dbConnection = dbProviderFactory.CreateConnection();
			dbConnection.ConnectionString = ArchiverMain.Current.DatabaseSettings.CreateConnectionString();
			try
			{
				dbConnection.Open();
			}
			catch (System.Exception e)
			{
				System.Windows.Forms.MessageBox.Show(e.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				return data;
			}

			DbDataAdapter dataAdapter = dbProviderFactory.CreateDataAdapter();
			DbCommand command = dbConnection.CreateCommand();
			command.CommandText = selectCommand;

			dataAdapter.MissingMappingAction = MissingMappingAction.Passthrough;
			dataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
			dataAdapter.SelectCommand = command;
			dataAdapter.Fill(data);
		
			return data;
		}

	}
}
