using FreeSCADA.Common;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Archiver
{
	public class ArchiverMain
	{
		ChannelsSettings channelSettings = new ChannelsSettings();

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

		void OnProjectClosed(object sender, System.EventArgs e)
		{
			channelSettings.Clear();
		}

		void OnProjectLoaded(object sender, System.EventArgs e)
		{
			channelSettings.Load();
		}

		#endregion

		public ChannelsSettings ChannelsSettings
		{
			get
			{
				return channelSettings;
			}
		}
	}
}
