using System.Windows.Forms;
using FreeSCADA.Common;
using FreeSCADA.Communication.OPCPlug;
using FreeSCADA.Interfaces;
using NUnit.Extensions.Forms;
using NUnit.Framework;

namespace Communication.OPCPlug.Tests
{
	[TestFixture]
	public class ReadWriteChannelsTest : NUnitFormTest
	{
		Plugin plugin;
		string projectFile;

		public override void Setup()
		{
			System.Windows.Forms.MenuStrip menu = new System.Windows.Forms.MenuStrip();
			System.Windows.Forms.ToolStrip toolbar = new System.Windows.Forms.ToolStrip();
			Env.Initialize(null, menu, toolbar, FreeSCADA.Interfaces.EnvironmentMode.Designer);
			plugin = (Plugin)Env.Current.CommunicationPlugins["opc_connection_plug"];

			projectFile = System.IO.Path.GetTempFileName();

			if (System.IO.File.Exists(projectFile))
				System.IO.File.Delete(projectFile);
		}

		public override void TearDown()
		{
			plugin = null;
			Env.Deinitialize();
			System.IO.File.Delete(projectFile);

			System.GC.Collect();
		}

		bool importAllChannelsSettingsFormHandlerProcessing = false;

		private void ImportAllChannelsSettingsFormHandler()
		{
			if (importAllChannelsSettingsFormHandlerProcessing)
				return;
			importAllChannelsSettingsFormHandlerProcessing = true;

			AnyControlTester gridTester = new AnyControlTester("grid", "SettingsForm");
			SourceGrid.Grid grid = (SourceGrid.Grid)gridTester.Control;

			ButtonTester addButton = new ButtonTester("addButton", "SettingsForm");
			ButtonTester removeButton = new ButtonTester("removeButton", "SettingsForm");
			ButtonTester okButton = new ButtonTester("okButton", "SettingsForm");

			Assert.AreEqual(1, grid.RowsCount); //Header row only
			int row_count = grid.RowsCount;

			ExpectModal("ImportOPCForm", "ImportAllChannelsImportFormHandler");

			addButton.Click();

			Assert.Greater(grid.RowsCount, row_count);
			row_count = grid.RowsCount;

			removeButton.Click();

			Assert.AreEqual(row_count-1, grid.RowsCount); //Remove one row

			okButton.Click();

			importAllChannelsSettingsFormHandlerProcessing = false;
		}

		private void ImportAllChannelsImportFormHandler()
		{
			ButtonTester okButton = new ButtonTester("okButton", "ImportOPCForm");
			ButtonTester connectButton = new ButtonTester("connectButton", "ImportOPCForm");
			ButtonTester cancelButton = new ButtonTester("cancelButton", "ImportOPCForm");

			AnyControlTester serversComboBoxTester = new AnyControlTester("serversComboBox", "ImportOPCForm");
			ComboBox serversComboBox = (ComboBox)serversComboBoxTester.Control;

			AnyControlTester channelsTreeTester = new AnyControlTester("channelsTree", "ImportOPCForm");
			TreeView channelsTree = (TreeView)channelsTreeTester.Control;

			serversComboBox.Text = "ICONICS.SimulatorOPCDA.2";
			connectButton.Click();

			Assert.IsNotEmpty(channelsTree.Nodes);
			
			foreach (TreeNode n in channelsTree.Nodes)
				SetNodeCheck(n, true);

			okButton.Click();
		}

		private void SetNodeCheck(TreeNode root, bool state)
		{
			root.Checked = state;
			foreach (TreeNode n in root.Nodes)
				SetNodeCheck(n, state);
		}

		[Test]
		public void ImportAllChannels()
		{
			ExpectModal("SettingsForm", "ImportAllChannelsSettingsFormHandler");

			ICommandContext context = Env.Current.Commands.GetPredefinedContext(PredefinedContexts.Communication);
			Env.Current.Commands.FindCommandByName(context, StringConstants.PropertyCommandName).Execute();

			Assert.Greater(plugin.Channels.Length, 0);
		}

		[Test]
		public void ReadAllChannels()
		{
			ExpectModal("SettingsForm", "ImportAllChannelsSettingsFormHandler");
			ICommandContext context = Env.Current.Commands.GetPredefinedContext(PredefinedContexts.Communication);
			Env.Current.Commands.FindCommandByName(context, StringConstants.PropertyCommandName).Execute();


			Assert.AreEqual(true, plugin.Connect());

			//Wait for some time until all channels get filled by OPC
			bool[] channelStates = new bool[plugin.Channels.Length];
			for (int i = 0; i < plugin.Channels.Length * 2; i++)
			{
				for(int j=0;j<plugin.Channels.Length;j++)
				{
					IChannel ch = plugin.Channels[j];
					System.Console.WriteLine("Channel value: {0}", ch.Value);

					if(ch.Value != null && ch.Value.ToString() != "")
						channelStates[j] = true;
					else
						channelStates[j] = false;
				}

				bool stop=true;
				foreach (bool val in channelStates)
				{
					if (val == false)
					{
						stop = false;
						break;
					}
				}

				if (stop == true)
					break;
				System.Threading.Thread.Sleep(100);
			}

			foreach (bool val in channelStates)
				Assert.IsTrue(val);

			plugin.Disconnect();
		}

		[Test]
		public void WriteToAllChannels()
		{
			ExpectModal("SettingsForm", "ImportAllChannelsSettingsFormHandler");
			ICommandContext context = Env.Current.Commands.GetPredefinedContext(PredefinedContexts.Communication);
			Env.Current.Commands.FindCommandByName(context, StringConstants.PropertyCommandName).Execute();


			Assert.AreEqual(true, plugin.Connect());

			//Wait for some time until all channels get filled by OPC
			System.Threading.Thread.Sleep(1000);
			
			//TODO: How to check that the value is set on OPC server side?
			foreach (IChannel ch in plugin.Channels)
			{
				object val = ch.Value;
				ch.Value = val;
			}

			plugin.Disconnect();
		}

		[Test]
		public void WriteToAllChannelsAsync()
		{
			ExpectModal("SettingsForm", "ImportAllChannelsSettingsFormHandler");
			ICommandContext context = Env.Current.Commands.GetPredefinedContext(PredefinedContexts.Communication);
			Env.Current.Commands.FindCommandByName(context, StringConstants.PropertyCommandName).Execute();


			Assert.IsTrue(plugin.Connect());

			//Wait for some time until all channels get filled by OPC
			System.Threading.Thread.Sleep(1000);

			System.Threading.Thread updateThread = new System.Threading.Thread(() =>
				{
					foreach (IChannel ch in plugin.Channels)
					{
						object val = ch.Value;
						ch.Value = val;
						Assert.AreEqual(ch.Value, val);
					}
				}
			);
			updateThread.Start();
			updateThread.Join();
			plugin.Disconnect();
		}
	}

	class AnyControlTester : ControlTester
	{
		public AnyControlTester(string controlName, string formName)
			: base(controlName, formName)
		{

		}

		new public System.Windows.Forms.Control Control
		{
			get
			{
				return (System.Windows.Forms.Control)base.Control;
			}
		}
	}
}
