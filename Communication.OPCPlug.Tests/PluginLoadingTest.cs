using FreeSCADA.Common;
using FreeSCADA.CommonUI;
using FreeSCADA.Communication.OPCPlug;
using FreeSCADA.Interfaces;
using NUnit.Framework;

namespace Communication.OPCPlug.Tests
{
	[TestFixture]
	public class PluginLoadingTest
	{
		[Test]
		public void Initialization()
		{
			System.Windows.Forms.MenuStrip menu = new System.Windows.Forms.MenuStrip();
			Env.Initialize(null, new Commands(menu, new System.Windows.Forms.ToolStrip()), FreeSCADA.Interfaces.EnvironmentMode.Designer);

			Plugin plugin = (Plugin)Env.Current.CommunicationPlugins["opc_connection_plug"];

			ICommandContext context = Env.Current.Commands.GetContext(PredefinedContexts.Communication);
			Assert.IsNotEmpty(context.GetCommands());

			Env.Deinitialize();
		}
	}
}
