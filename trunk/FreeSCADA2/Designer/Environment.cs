﻿using FreeSCADA.ShellInterfaces;
using System.Windows.Forms;

namespace FreeSCADA.Designer
{
	public class Environment : IEnvironment
	{
		static Environment environmentInstance = null;

		Commands commands;
		Control mainWindow;
		CommunationPlugs communicationPlugins;
		FreeSCADA.Common.Project project = new FreeSCADA.Common.Project();

		#region Initialization and singleton implementation

		public static void Initialize(Control mainWindow, MenuStrip mainMenu)
		{
			environmentInstance = new Environment();

			environmentInstance.commands = new Commands(mainMenu);
			environmentInstance.mainWindow = mainWindow;
			environmentInstance.communicationPlugins = new CommunationPlugs();

			environmentInstance.communicationPlugins.Load();
		}

		public static Environment Current
		{
			get
			{
				if (environmentInstance == null)
					throw new System.NullReferenceException();

				return environmentInstance;
			}
		}

		Environment() { }

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
