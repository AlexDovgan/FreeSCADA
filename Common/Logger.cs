using System;
using System.Collections.Generic;
using System.Text;

namespace FreeSCADA.Common
{
	public class Logger
	{
		public enum Severity
		{
			Information,
			Warning,
			Error
		}

		public virtual void Log(Severity severity, string message)
		{
			Console.WriteLine(string.Format("{0}: {1}", severity.ToString(), message));
		}

		public void LogInfo(string message)
		{
			Log(Severity.Information, message);
		}

		public void LogWarning(string message)
		{
			Log(Severity.Warning, message);
		}

		public void LogError(string message)
		{
			Log(Severity.Error, message);
		}
	}
}
