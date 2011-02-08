﻿using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using FreeSCADA.Common;

namespace FreeSCADA.CLServer
{
    class Commands:Interfaces.ICommands
    {
        #region ICommands Members
        public Commands()
        {
        }
        public FreeSCADA.Interfaces.ICommandContext GetContext(string contextName)
        {
            throw new NotImplementedException();
        }

        public bool RegisterContext(string contextName, FreeSCADA.Interfaces.ICommandContext context)
        {
            throw new NotImplementedException();
        }

        public FreeSCADA.Interfaces.ICommand FindCommandByName(string contextName, string name)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICommands Members


        FreeSCADA.Interfaces.ICommands FreeSCADA.Interfaces.ICommands.RegisterContext(string contextName, FreeSCADA.Interfaces.ICommandContext context)
        {
            throw new NotImplementedException();
        }

        public System.Collections.Generic.List<string> GetRegisteredContextes()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
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

			Console.Write("Initializing communication plugins... ");
			Env.Initialize(null, new Commands(), FreeSCADA.Interfaces.EnvironmentMode.Runtime);
			Env.Current.Project.Load(options.ProjectFile);
			CommunationPlugs plugs = Env.Current.CommunicationPlugins;
			if (plugs.Connect() == false)
			{
				Env.Deinitialize();
				return -1;
			}
			Console.WriteLine("Done.");

			Uri baseAddress = new Uri(string.Format("http://{0}:{1}/", System.Windows.Forms.SystemInformation.ComputerName, options.Port));
			ServiceHost host = new ServiceHost(typeof(Service), baseAddress);
            try
            {
				host.AddServiceEndpoint(typeof(IChannelInformationRetriever), new WSDualHttpBinding(WSDualHttpSecurityMode.None), "ChannelInformationRetriever");
				host.AddServiceEndpoint(typeof(IDataRetriever), new WSDualHttpBinding(WSDualHttpSecurityMode.None), "DataRetriever");

				ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
				smb.HttpGetEnabled = true;
				host.Description.Behaviors.Add(smb);

				ServiceThrottlingBehavior throttlingBehavior = new ServiceThrottlingBehavior();
				throttlingBehavior.MaxConcurrentCalls = Int32.MaxValue;
				throttlingBehavior.MaxConcurrentInstances = Int32.MaxValue;
				throttlingBehavior.MaxConcurrentSessions = Int32.MaxValue;
				host.Description.Behaviors.Add(throttlingBehavior);

				host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.None;
				host.Open();

				Console.WriteLine("Server address {0}", baseAddress.AbsoluteUri);
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
