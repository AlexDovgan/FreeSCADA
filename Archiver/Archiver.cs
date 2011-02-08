using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using FreeSCADA.Common;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Archiver
{
	public class ArchiverMain
	{
		ChannelsSettings channelSettings = new ChannelsSettings();
		DatabaseSettings databaseSettings = new DatabaseSettings();
		DbWriter dbWriter;
        DbReader dbReader;
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
				ICommandContext context = Env.Current.Commands.GetContext(PredefinedContexts.Project);
                context.AddCommand(new PropertyCommand());
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

            dbReader = new DbReader();
            if (dbReader.Open() == false)
                return false;

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
            if (dbReader != null)
                dbReader.Close();
		}

		
		public DataTable GetChannelData(DateTime from, DateTime to, List<ChannelInfo> channels)
		{
            if (!IsRunning)
                new DataTable();

			string datePattern = "yyyy-MM-dd HH:mm:ss";
			string query = "SELECT ChannelName, Time, Value FROM Channels WHERE ";
			query += string.Format("Time >= '{0}' AND Time <= '{1}' ", from.ToString(datePattern), to.ToString(datePattern));
			query += "AND (";
			for(int i=0;i<channels.Count;i++)
			{
				ChannelInfo ch = channels[i];
				query += string.Format("(PluginId='{0}' AND ChannelName='{1}')", ch.PluginId, ch.ChannelName);
				if (i != channels.Count - 1)
				{
					query += " OR ";
				}
			}
			query += ") ORDER BY Time;";

			return dbReader.ExeсCommand(query);
		}
        public DateTime GetChannelsOlderDate(List<ChannelInfo> channels)
        {

            if (!IsRunning)
                return DateTime.Now;

            string query = "SELECT min(Time) FROM Channels WHERE ";
            query += "(";
            for (int i = 0; i < channels.Count; i++)
            {
                ChannelInfo ch = channels[i];
                query += string.Format("(PluginId='{0}' AND ChannelName='{1}')", ch.PluginId, ch.ChannelName);
                if (i != channels.Count - 1)
                {
                    query += " OR ";
                }
            }
            query += ")";
            DataTable dt = dbReader.ExeсCommand(query);
            DateTime date=new DateTime();
            if(dt.Rows.Count>0)
                DateTime.TryParse(dt.Rows[0].ItemArray[0].ToString(),out date);
            return date;

        }

	}
}
