using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using NUnit.Framework;

namespace CLServer.Tests
{
	[TestFixture]
	public class DataRetrieverTest
	{
		DataRetrieverClient client;
		ServerStarter server = new ServerStarter();
		DataUpdatedCallback callback;

		[SetUp]
		public void Init()
		{
			string projectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Samples\bouncing_ball.fs2");
			projectPath = Path.GetFullPath(projectPath);
			Assert.IsTrue(server.Start(projectPath));

			callback = new DataUpdatedCallback();
			EndpointAddress epAddress = new EndpointAddress(server.BaseAddress + "DataRetriever");
			client = new DataRetrieverClient(new InstanceContext(callback), new WSDualHttpBinding(WSDualHttpSecurityMode.None), epAddress);
			client.Open();
		}
		[TearDown]
		public void DeInit()
		{
			client.Close();
			server.Stop();
		}

		[Test]
		public void SubscribeToEvents()
		{
			client.SetChannelValue("data_simulator_plug.delta", "30");
			client.RegisterCallback("data_simulator_plug.ball_position");

			System.Threading.Thread.Sleep(5000);

			Assert.IsNotEmpty(callback.channelIds);
			Assert.IsNotEmpty(callback.states);
		}

		[Test]
		public void StressTest()
		{
			client.SetChannelValue("data_simulator_plug.delta", "30");
			client.RegisterCallback("data_simulator_plug.ball_position");

			const int ClientCount = 1000;
			DataUpdatedCallback[] callbacks = new DataUpdatedCallback[ClientCount];
			DataRetrieverClient[] clients = new DataRetrieverClient[ClientCount];

			for (int i = 0; i < ClientCount; i++)
			{
				callbacks[i] = new DataUpdatedCallback();
				EndpointAddress epAddress = new EndpointAddress(server.BaseAddress + "DataRetriever");
				clients[i] = new DataRetrieverClient(new InstanceContext(callbacks[i]), new WSDualHttpBinding(WSDualHttpSecurityMode.None), epAddress);
				clients[i].RegisterCallback("data_simulator_plug.ball_position");
			}
		
			System.Threading.Thread.Sleep(10000);

			for (int i = 0; i < ClientCount; i++)
			{
				clients[i].Abort();

				Assert.IsNotEmpty(callbacks[i].channelIds);
				Assert.IsNotEmpty(callbacks[i].states);
			}
		}
	}

	//Callback for testing
	class DataUpdatedCallback : IDataRetrieverCallback
	{
		public List<string> channelIds = new List<string>();
		public List<FreeSCADA.CLServer.ChannelState> states = new List<FreeSCADA.CLServer.ChannelState>();

		public void ValueChanged(string channelId, FreeSCADA.CLServer.ChannelState state)
		{
			channelIds.Add(channelId);
			states.Add(state);
		}
	}
}
