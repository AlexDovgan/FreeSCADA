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
		FreeSCADA.Common.Project project = new FreeSCADA.Common.Project();

		#region Initialization and singleton implementation

		public static void Initialize(Control mainWindow, MenuStrip mainMenu)
		{
			environmentInstance = new Env();

			environmentInstance.commands = new Commands(mainMenu);
			environmentInstance.mainWindow = mainWindow;
			environmentInstance.communicationPlugins = new CommunationPlugs();

			environmentInstance.communicationPlugins.Load();
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

		#endregion

		public CommunationPlugs CommunicationPlugins
		{
			get { return communicationPlugins; }
		}
	}
}
