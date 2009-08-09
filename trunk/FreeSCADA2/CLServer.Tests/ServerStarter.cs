using System;
using System.IO;

namespace CLServer.Tests
{
	class ServerStarter
	{
		System.Diagnostics.Process process;

		public string BaseAddress
		{
			get
			{
				return "http://localhost:8082/";
			}
		}
		public bool Start(string project)
		{
			//return true;
			string serverExecutable = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CLServer.exe");

			process = new System.Diagnostics.Process();
			process.StartInfo.FileName = serverExecutable;
			process.StartInfo.Arguments = string.Format("--project-file=\"{0}\" --port=8082", project);
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardInput = true;
			process.StartInfo.RedirectStandardError = true;

			if (process.Start())
			{
				string line;
				while ((line = process.StandardOutput.ReadLine()) != null)
				{
					if (line == "Press <ENTER> to terminate")
						return process.HasExited == false;
				}
				if (process.HasExited)
					return false;
			}

			return false;
		}

		public void Stop()
		{
			if (process != null && process.HasExited == false)
			{
				process.StandardInput.WriteLine("");
				System.Threading.Thread.Sleep(500);
				if(process.HasExited == false)
					process.Kill();
				process.Dispose();
			}
		}
	}
}
