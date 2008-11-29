using System;
using NUnit.Framework;
using FreeSCADA.Communication.OPCPlug;

namespace Communication.OPCPlug.Tests
{
	[TestFixture]
	public class PluginLoadingTest
	{
		[Test]
		public void Initialization()
		{
			EnvironmentMock environment = new EnvironmentMock();
			Plugin plugin = new Plugin();
			plugin.Initialize(environment);

			CommandsMock.CommandInfo cmd1 = new CommandsMock.CommandInfo("opc_connection_plug", "OPC properties...", "Communication");
			Assert.Contains(cmd1, environment.commands.registeredCommands);
		}

		[Test]
		public void Initialization2()
		{
			EnvironmentMock environment = new EnvironmentMock();
			Plugin plugin = new Plugin();
			plugin.Initialize(environment);

			CommandsMock.CommandInfo cmd1 = new CommandsMock.CommandInfo("opc_connection_plug", "OPC properties...", "Communication");

			EnvironmentMock environment2 = new EnvironmentMock();
			plugin.Environment = environment2;
			Assert.Contains(cmd1, environment2.commands.registeredCommands);
		}
	}
}
