using FreeSCADA.Communication.SimulatorPlug;
using NUnit.Framework;

namespace Communication.SimulatorPlug.Tests
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

			CommandsMock.CommandInfo cmd1 = new CommandsMock.CommandInfo("data_simulator_plug","Simulator properties...","Communication");
			Assert.Contains(cmd1, environment.commands.registeredCommands);
		}

		[Test]
		public void Initialization2()
		{
			EnvironmentMock environment = new EnvironmentMock();
			Plugin plugin = new Plugin();
			plugin.Initialize(environment);

			CommandsMock.CommandInfo cmd1 = new CommandsMock.CommandInfo("data_simulator_plug", "Simulator properties...", "Communication");

			EnvironmentMock environment2 = new EnvironmentMock();
			plugin.Environment = environment2;
			Assert.Contains(cmd1, environment2.commands.registeredCommands);
		}
	}
}
