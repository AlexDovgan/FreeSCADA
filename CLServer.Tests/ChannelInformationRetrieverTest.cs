using System;
using System.IO;
using System.ServiceModel;
using FreeSCADA.CLServer;
using NUnit.Framework;

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
			client = new ChannelInformationRetrieverClient(new WSDualHttpBinding(WSDualHttpSecurityMode.None), epAddress);			
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

		[Test]
		public void GetChannelInfoIterative()
		{
			Assert.IsTrue(client.GetChannelsCount() == 4);

			ChannelInfo channel;
			channel = client.GetChannel(0);
			Assert.IsNotNull(channel);
			Assert.IsTrue(channel.Name == "phase");

			channel = client.GetChannel(1);
			Assert.IsNotNull(channel);
			Assert.IsTrue(channel.Name == "delta");

			channel = client.GetChannel(2);
			Assert.IsNotNull(channel);
			Assert.IsTrue(channel.Name == "ball_position");

			channel = client.GetChannel(3);
			Assert.IsNotNull(channel);
			Assert.IsTrue(channel.Name == "ball_height");

			channel = client.GetChannel(4);
			Assert.IsNull(channel);
		}
	}
}
