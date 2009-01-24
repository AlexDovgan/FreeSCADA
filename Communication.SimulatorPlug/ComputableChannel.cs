using System.Collections.Generic;
using FreeSCADA.Common;
using FreeSCADA.Interfaces;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace FreeSCADA.Communication.SimulatorPlug
{
	class ComputableChannel : BaseChannel
	{
		string expression;
		static ScriptEngine python = InitializePython();
		ScriptSource source;
		ScriptScope scope;

		public ComputableChannel(string name, Plugin plugin, string expression)
			: base(name, true, plugin, typeof(int))
		{
			Expression = expression;
		}

		static ScriptEngine InitializePython()
		{
			Dictionary<string, object> options = new Dictionary<string, object>();
			options["DivisionOptions"] = IronPython.PythonDivisionOptions.New;
			return Python.CreateEngine(options);
		}

        public override  void DoUpdate()
		{
			foreach (IChannel ch in plugin.Channels)
				scope.SetVariable(ch.Name, ch.Value);

			if (source != null)
			{
				try
				{
					source.Execute(scope);
				}
				catch (System.Exception e)
				{
					Env.Current.Logger.LogWarning(string.Format("Channel '{0}' fail to execute script: {1}", this.Name, e.Message));
					return;
				}
				object val = null;
				if(scope.TryGetVariable("result", out val) == true)
					DoUpdate(val);
			}
		}

		public string Expression
		{
			get { return expression; }
			internal set 
			{ 
				expression = value;
				if (string.IsNullOrEmpty(expression) == false && expression.Contains("result"))
				{
					string pyExpr = "from math import *\n" + expression;
					source = python.CreateScriptSourceFromString(pyExpr, Microsoft.Scripting.SourceCodeKind.Statements);
					scope = python.CreateScope();
				}
			}
		}
	}
}
