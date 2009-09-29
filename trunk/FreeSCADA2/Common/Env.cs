﻿using System.Windows.Forms;
using FreeSCADA.Common.Scripting;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Common
{
	public class Env : IEnvironment
	{
		const string version = "2.0.0.9";

		Commands commands;
		Control mainWindow;
        CommunationPlugs communicationPlugins;
        VisualControlsPlugs visualPlugins;
        FreeSCADA.Common.Project project;
		EnvironmentMode mode;
		Logger logger;
		ScriptManager scriptManager;


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
                environmentInstance.scriptManager = new ScriptManager();
                environmentInstance.visualPlugins = new VisualControlsPlugs();

				environmentInstance.communicationPlugins.Load();
                environmentInstance.visualPlugins.Load();

				//Should be called after all plugins
				environmentInstance.scriptManager.Initialize();
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

		public string Version
		{
			get { return version; }
		}

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
		
		public ScriptManager ScriptManager
		{
			get { return scriptManager; }
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
