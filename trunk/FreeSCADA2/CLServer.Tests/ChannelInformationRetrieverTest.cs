using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.IO;
using NUnit.Framework;
using FreeSCADA.CLServer;

namespace CLServer.Tests
{
	[TestFixture]
	public class ChannelInformationRetrieverTest
	{
		ChannelInformationRetrieverClient client;
		ServerStarter server = new ServerStarter();

		[SetUp]
		public void Init()
		{
			string projectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Samples\bouncing_ball.fs2");
			projectPath = Path.GetFullPath(projectPath);
			Assert.IsTrue(server.Start(projectPath));
			EndpointAddress epAddress = new EndpointAddress(server.BaseAddress + "ChannelInformationRetriever");
			client = new ChannelInformationRetrieverClient(new WSDualHttpBinding(), epAddress);			
		}
		[TearDown]
		public void DeInit()
		{
			client.Close();
			server.Stop();
		}

		[Test]
		public void ConnectToServer()
		{
			ChannelInfo[] channels = client.GetChannels();
		}

		[Test]
		public void GetChannelInfo()
		{
			ChannelInfo[] channels = client.GetChannels();
			Assert.IsNotNull(channels);
			Assert.IsNotEmpty(channels);

			Assert.IsTrue(channels.Length == 4);
			Assert.IsTrue(channels[0].Name == "phase");
			Assert.IsTrue(channels[1].Name == "delta");
			Assert.IsTrue(channels[2].Name == "ball_position");
			Assert.IsTrue(channels[3].Name == "ball_height");
			
		}
	}
}
