using FreeSCADA.ShellInterfaces;
using System.Windows.Forms;

namespace FreeSCADA.Common
{
	public class Env : IEnvironment
	{
		static Env environmentInstance = null;

		Commands commands;
		Control mainWindow;
		CommunationPlugs communicationPlugins;
		FreeSCADA.Common.Project project;
		EnvironmentMode mode;

		#region Initialization and singleton implementation

		public static void Initialize(Control mainWindow, MenuStrip mainMenu, EnvironmentMode mode)
		{
			if (environmentInstance == null)
			{
				environmentInstance = new Env();

				environmentInstance.mode = mode;
				environmentInstance.CreateNewProject();
				environmentInstance.commands = new Commands(mainMenu);
				environmentInstance.mainWindow = mainWindow;
				environmentInstance.communicationPlugins = new CommunationPlugs();

				environmentInstance.communicationPlugins.Load();
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

		public void CreateNewProject()
		{
			project = new FreeSCADA.Common.Project();
		}
	}
}
