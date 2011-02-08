using FreeSCADA.Common;
using FreeSCADA.Communication.SimulatorPlug;
using FreeSCADA.Interfaces;
using NUnit.Framework;

namespace Communication.SimulatorPlug.Tests
{
	[TestFixture]
	public class PluginLoadingTest
	{
		[Test]
		public void Initialization()
		{
            
			System.Windows.Forms.MenuStrip menu = new System.Windows.Forms.MenuStrip();
			Env.Initialize(null, new Commands(menu, null), FreeSCADA.Interfaces.EnvironmentMode.Designer);

			Plugin plugin = (Plugin)Env.Current.CommunicationPlugins["data_simulator_plug"];

			ICommandContext context = Env.Current.Commands.GetContext(PredefinedContexts.Communication);
            Assert.IsNotEmpty(context.GetCommands());

			Env.Deinitialize();
		}
	}
}
