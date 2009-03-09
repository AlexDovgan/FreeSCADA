using System;
using System.Collections.Generic;
using System.IO;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace FreeSCADA.Common.Scripting
{
	public class ScriptManager
	{
		public delegate void NewScriptCreatedHandler(Object sender, Script script);
		public event NewScriptCreatedHandler NewScriptCreated;
		public event EventHandler ScriptsUpdated;


		ScriptEngine python = InitializePython();
		List<Script> scripts = new List<Script>();

		internal ScriptManager()
		{
			Env.Current.Project.ProjectClosed += new EventHandler(OnProjectClosed);
			Env.Current.Project.ProjectLoaded += new EventHandler(OnProjectLoaded);
		}

		void OnProjectLoaded(object sender, EventArgs e)
		{
			foreach (string name in Env.Current.Project.GetEntities(ProjectEntityType.Script))
			{
				string scriptText = "";
				using (Stream stream = Env.Current.Project.GetData(ProjectEntityType.Script, name))
				using (StreamReader reader = new StreamReader(stream))
				{
					string line;
					while ((line = reader.ReadLine()) != null)
					{
						if (scriptText.Length > 0)
							scriptText += "\n";
						scriptText += line;
					}
				}

				Script script = new Script(scriptText, python, name);
				scripts.Add(script);
			}
		}

		void OnProjectClosed(object sender, EventArgs e)
		{
			scripts.Clear();
		}

		static ScriptEngine InitializePython()
		{
			Dictionary<string, object> options = new Dictionary<string, object>();
			options["DivisionOptions"] = IronPython.PythonDivisionOptions.New;
			return Python.CreateEngine(options);
		}

		public Script CreateNewScript(string name)
		{
			using (MemoryStream stream = new MemoryStream())
			using (StreamWriter writer = new StreamWriter(stream))
			{
				writer.WriteLine("");
				writer.Flush();
				Env.Current.Project.SetData(ProjectEntityType.Script, name, stream);

				Script script = new Script("", python, name);
				scripts.Add(script);

				if(NewScriptCreated != null)
					NewScriptCreated(this, script);

				if (ScriptsUpdated != null)
					ScriptsUpdated(this, new EventArgs());

				return script;
			}
		}

		public Script GetScript(string name)
		{
			foreach (Script s in scripts)
			{
				if (s.Name == name)
					return s;
			}

			return null;
		}
	}
}
