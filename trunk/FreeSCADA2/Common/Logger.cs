using System;
using System.IO;

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

			try
			{
				string tmpFile = Path.Combine(Path.GetTempPath(), "FreeSCADA2_log_file.txt");
				using (Stream stream = new FileStream(tmpFile, FileMode.Append, FileAccess.Write, FileShare.Read))
				using (TextWriter writer = new StreamWriter(stream))
				{
					writer.WriteLine(string.Format("[{0}] {1}: {2}", DateTime.Now, severity.ToString(), message));
					writer.Flush();
				}
			}
			catch { }
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
