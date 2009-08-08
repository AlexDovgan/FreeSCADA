﻿using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using FreeSCADA.Common;

namespace FreeSCADA.CLServer
{
	class Program
	{
		static int Main(string[] args)
		{
			Options options = new Options();
			Plossum.CommandLine.CommandLineParser parser = new Plossum.CommandLine.CommandLineParser(options);
			parser.Parse();
			Console.WriteLine(parser.UsageInfo.GetHeaderAsString(78));

			if (options.Help)
			{
				Console.WriteLine(parser.UsageInfo.GetOptionsAsString(20, 56));
				return 0;
			}
			else if (parser.HasErrors)
			{
				Console.WriteLine(parser.UsageInfo.GetErrorsAsString(78));
				Console.WriteLine("type --help for list of available options.");
				return -1;
			}

			Env.Initialize(null, null, null, FreeSCADA.Interfaces.EnvironmentMode.Runtime);
			Env.Current.Project.Load(options.ProjectFile);
			CommunationPlugs plugs = Env.Current.CommunicationPlugins;
			if (plugs.Connect() == false)
			{
				Env.Deinitialize();
				return -1;
			}

			Uri baseAddress = new Uri(string.Format("http://localhost:{0}/", options.Port));
			ServiceHost host = new ServiceHost(typeof(Service), baseAddress);
            try
            {
				host.AddServiceEndpoint(typeof(IChannelInformationRetriever), new WSDualHttpBinding(), "ChannelInformationRetriever");
				host.AddServiceEndpoint(typeof(IDataRetriever), new WSDualHttpBinding(), "DataRetriever");

				ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
				smb.HttpGetEnabled = true;
				host.Description.Behaviors.Add(smb);
				host.Open();

				Console.WriteLine("Server address {0}", string.Format("http://localhost:{0}/", options.Port));
                Console.WriteLine("Press <ENTER> to terminate");
                Console.ReadLine();
				Console.WriteLine("Terminating. Please wait...");

                host.Close();
            }
            catch (CommunicationException cex)
            {
                Console.WriteLine("Error occured: {0}", cex.Message);
                host.Abort();
            }

			plugs.Disconnect();
			Env.Deinitialize();

			return 0;
        }

	}
}
