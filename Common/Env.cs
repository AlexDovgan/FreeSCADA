using System.Windows.Forms;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Common
{
	public class Env : IEnvironment
	{
		Commands commands;
		Control mainWindow;
        CommunationPlugs communicationPlugins;
        VisualControlsPlugs visualPlugins;
        FreeSCADA.Common.Project project;
		EnvironmentMode mode;
		Logger logger;

		#region Initialization and singleton implementation
		static Env environmentInstance = null;
		public static void Initialize(Control mainWindow, MenuStrip mainMenu, ToolStrip mainToolbar,  EnvironmentMode mode)
		{
			if (environmentInstance == null)
			{
				environmentInstance = new Env();

				environmentInstance.mode = mode;
				environmentInstance.logger = new Logger();
				environmentInstance.CreateNewProject();
				environmentInstance.commands = new Commands(mainMenu, mainToolbar);
				environmentInstance.mainWindow = mainWindow;
                environmentInstance.communicationPlugins = new CommunationPlugs();
                environmentInstance.visualPlugins = new VisualControlsPlugs();

				environmentInstance.communicationPlugins.Load();
                environmentInstance.visualPlugins.Load();
			}
		}
        
		public static void Deinitialize()
		{
			environmentInstance = null;
		}

		public static Env Current
		{
			get
			{
				if (environmentInstance == null)
					throw new System.NullReferenceException();

				return environmentInstance;
			}
		}

		Env() { }

		#endregion

		#region IEnvironment Members

		public ICommands Commands
		{
			get { return commands; }
		}

		public Control MainWindow
		{
			get	{ return mainWindow;	}
		}

		public FreeSCADA.Common.Project Project
		{
			get { return project; }
		}

		public EnvironmentMode Mode
		{
			get { return mode; }
		}

		#endregion

		public CommunationPlugs CommunicationPlugins
		{
			get { return communicationPlugins; }
		}

		public VisualControlsPlugs VisualPlugins
		{
			get { return visualPlugins; }
		}

		public void CreateNewProject()
		{
			if (project != null)
				project.Clear();
			else
				project = new FreeSCADA.Common.Project();
		}

		public Logger Logger
		{
			get { return logger; }
			set { logger = value; }
		}
	}
}
