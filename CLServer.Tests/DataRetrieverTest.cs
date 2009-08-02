using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.IO;
using NUnit.Framework;
using FreeSCADA.CLServer;

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
			client = new DataRetrieverClient(new InstanceContext(callback), new WSDualHttpBinding(), epAddress);
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
			Assert.IsNotEmpty(callback.values);
		}
	}

	//Callback for testing
	class DataUpdatedCallback : IDataRetrieverCallback
	{
		public List<string> channelIds = new List<string>();
		public List<string> values = new List<string>();
		
		public void ValueChanged(string channelId, string value)
		{
			channelIds.Add(channelId);
			values.Add(value);
		}
	}
}
