using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

			System.Threading.Thread.Sleep(1000); //Wait for server to load up

			return process.Start();
		}

		public void Stop()
		{
			if (process != null && process.HasExited == false)
			{
				process.Kill();
				process.Dispose();
			}
		}
	}
}
