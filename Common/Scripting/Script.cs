using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace FreeSCADA.Common.Scripting
{
	public class Script
	{
		string text;
		ScriptEngine python;
		string name;

		ScriptSource source;
		ScriptScope scope;

		public event EventHandler TextUpdated;

		public class ErrorInfo
		{
			public enum SeverityType
			{
				Error,
				Warning
			};

			public string Message;
			public int Line;
			public SeverityType Severity;
		}

		class ErrorDump:ErrorListener
		{
			List<ErrorInfo> errors = new List<ErrorInfo>();

			public List<ErrorInfo> Errors
			{
				get { return errors; }
			}

			public override void ErrorReported(ScriptSource source, string message, SourceSpan span, int errorCode, Severity severity)
			{
				ErrorInfo error = new ErrorInfo();
				error.Message = message;
				error.Line = span.Start.Line - 1;

				switch (severity)
				{
					case Severity.Error:
					case Severity.FatalError:
						error.Severity = ErrorInfo.SeverityType.Error;
						break;
					case Severity.Ignore:
					case Severity.Warning:
						error.Severity = ErrorInfo.SeverityType.Warning;
						break;
				}

				errors.Add(error);
			}
		}
		public Script(string text, ScriptEngine python, string name)
		{
			this.text = text;
			this.python = python;
			this.name = name;

			CreatePythonObjects();
		}

		public string Text
		{
			get { return text; }
			set 
			{ 
				text = value;
				CreatePythonObjects();

				if (TextUpdated != null)
					TextUpdated(this, new EventArgs());
			}
		}

		public string Name
		{
			get { return name; }
		}

		public static List<string> GetImportedModules(string text)
		{
			List<string> modules = new List<string>();

			using (StringReader reader = new StringReader(text))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					Regex rx = new Regex(@".*import (?<name>.*).*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
					Match match = rx.Match(line);
					if (match.Success && match.Groups["name"].Value != "*")
					{
						if (modules.IndexOf(match.Groups["name"].Value) < 0)
							modules.Add(match.Groups["name"].Value);
					}

					//from System.Windows.Media import *
					rx = new Regex(@".*from (?<name>.*).* import *.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
					match = rx.Match(line);
					if (match.Success)
					{
						if (modules.IndexOf(match.Groups["name"].Value) < 0)
							modules.Add(match.Groups["name"].Value);
					}
				}
			}

			return modules;
		}

		public void Save()
		{
			using (MemoryStream stream = new MemoryStream())
			using (StreamWriter writer = new StreamWriter(stream))
			{
				writer.Write(text);
				writer.Flush();
				Env.Current.Project.SetData(ProjectEntityType.Script, name, stream);
			}
		}

		public void Execute(string functionName, object[] args)
		{
			if (source != null)
			{
				try
				{
					object func = scope.GetVariable(functionName);
					if (func == null)
					{
						Env.Current.Logger.LogWarning(string.Format("Fail to execute script '{0}': Cannot find handler '{1}'", this.Name, functionName));
						return;
					}

					python.Operations.Call(func, args);
				}
				catch (System.Exception e)
				{
					Env.Current.Logger.LogWarning(string.Format("Fail to execute script '{0}': {1}", this.Name, e.Message));
					return;
				}
			}
		}

		private void CreatePythonObjects()
		{
			if (string.IsNullOrEmpty(text) == false)
			{
				source = python.CreateScriptSourceFromString(text, Microsoft.Scripting.SourceCodeKind.Statements);
				scope = python.CreateScope();
				scope.SetVariable("Application", Env.Current.ScriptManager.ScriptApplication);

				//Try to execute script to get all its functions into 'scope'
				try
				{
					source.Execute(scope);
				}
				catch (System.Exception e)
				{
					Env.Current.Logger.LogWarning(string.Format("Fail to execute script '{0}': {1}", this.Name, e.Message));
					return;
				}

				ScriptScope sysModule = python.GetSysModule();
				IronPython.Runtime.PythonDictionary modules = sysModule.GetVariable<IronPython.Runtime.PythonDictionary>("modules");
				modules[name] = scope;
			}
		}

		public void AddHandlerTemplate(string functionName, EventInfo evnt)
		{
			Type ehType = evnt.EventHandlerType;
			MethodInfo mi = ehType.GetMethod("Invoke");
			ParameterInfo[] eventParams = mi.GetParameters();

			//Add comment
			string template = "\n\n# Arguments: ";
			for(int i=0;i< eventParams.Length;i++)
			{
				if(i > 0)
					template += ", ";

				template += string.Format("{0} {1}", eventParams[i].ParameterType.Name, eventParams[i].Name);
			}

			template += "\n";

			//Create function definition
			template += string.Format("def {0}(", functionName);
			for (int i = 0; i < eventParams.Length; i++)
			{
				if (i > 0)
					template += ", ";

				template += eventParams[i].Name;
			}
			template += "):\n\t";

			Text = Text + template;
		}

		public List<ErrorInfo> Validate()
		{
			ErrorDump listener = new ErrorDump();
			source.Compile(listener);

			List<ErrorInfo> errors = listener.Errors;

			//Try to execute script to get more errors
			if (errors.Count == 0)
			{
				try
				{
					source.Execute(scope);
				}
				catch (System.Exception e)
				{
					ErrorInfo error = new ErrorInfo();
					error.Message = e.Message;
					error.Line = -1;
					error.Severity = ErrorInfo.SeverityType.Error;

					errors.Add(error);
				}
			}

			return errors;
		}
	}
}
