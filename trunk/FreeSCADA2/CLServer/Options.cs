using System;
using System.IO;
using Plossum.CommandLine;

namespace FreeSCADA.CLServer
{
	[CommandLineManager(	ApplicationName = "Command line FreeSCADA server", 
							Copyright = "Copyright (c) FreeSCADA project",
							Version="2.0",
							EnabledOptionStyles = OptionStyles.Group | OptionStyles.LongUnix | OptionStyles.Windows)]
	[CommandLineOptionGroup("commands", Name = "Commands")]
	[CommandLineOptionGroup("options", Name = "Options")]
	class Options
	{
		public Options()
		{
			Port = 8080;
		}

		[CommandLineOption(GroupId="commands", Name="help", Aliases="h,?", Description = "Displays this help text")]
		public bool Help { get; set; }

		private string projectFile;
		[CommandLineOption(GroupId = "options", Name = "f", Aliases = "project-file", Description = "Specifies project file (.fs2).\nThe server will get all information on communication channels from this file.", MinOccurs=1)]
		public string ProjectFile
		{
			get { return projectFile; }
			set
			{
				if (String.IsNullOrEmpty(value))
					throw new InvalidOptionValueException("The file name must not be empty", false);
				if (!File.Exists(value))
					throw new InvalidOptionValueException("The file must exists", false);
				projectFile = value;
			}
		}

		[CommandLineOption(GroupId = "options", Name = "p", Aliases = "port", Description = "Specifies TCP port for the server. Default value is 8080")]
		public int Port { get; set; }
	}
}
