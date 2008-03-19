using System;
using FreeSCADA.ShellInterfaces;

namespace Communication.SimulatorPlug.Tests
{
	class EnvironmentMock:IEnvironment
	{
		public CommandsMock commands = new CommandsMock();
		public FreeSCADA.Common.Project project = new FreeSCADA.Common.Project();

		public ICommands Commands
		{
			get { return commands; }
		}

		public System.Windows.Forms.Control MainWindow
		{
			get { return null; }
		}

		public FreeSCADA.Common.Project Project
		{
			get { return project; }
		}

		public EnvironmentMode Mode
		{
			get { return EnvironmentMode.Designer; }
		}
	}
}
