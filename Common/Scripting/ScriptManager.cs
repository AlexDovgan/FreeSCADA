using System;
using System.Collections.Generic;
using System.IO;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace FreeSCADA.Common.Scripting
{
	public class ScriptManager
	{
		public delegate void NewScriptCreatedHandler(Object sender, string name);
		public event NewScriptCreatedHandler NewScriptCreated;
		public event EventHandler ScriptsUpdated;

		public const string ChannelsScriptName = "ChannelHandlers";

		ScriptEngine python = InitializePython();
		List<Script> scripts = new List<Script>();
		ChannelsScriptHandlers channelsScriptHandlers = new ChannelsScriptHandlers();
		Application application = new Application();

		public ChannelsScriptHandlers ChannelsHandlers
		{
			get { return channelsScriptHandlers; }
		}

		public Application ScriptApplication
		{
			get { return application; }
		}

		/// <summary>
		/// Initialize Script manager. This method should be called after all plugins are loaded. (In order to set right handlers for channels)
		/// </summary>
		internal void Initialize()
		{
			Env.Current.Project.ProjectClosed += new EventHandler(OnProjectClosed);
			Env.Current.Project.ProjectLoaded += new EventHandler(OnProjectLoaded);
			Env.Current.Project.EntitySetChanged += new EventHandler(OnProjectEntitySetChanged);
		}

		void OnProjectEntitySetChanged(object sender, EventArgs e)
		{
			OnProjectClosed(sender, e);
			OnProjectLoaded(sender, e);
		}

		void OnProjectLoaded(object sender, EventArgs e)
		{
			Dictionary<string, string> scriptTexts = new Dictionary<string, string>();

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
				scriptTexts[name] = scriptText;
			}

			while (scriptTexts.Count > 0)
			{
				//Get first script to load
				string scriptName = "";
				foreach(string key in scriptTexts.Keys)
				{
					scriptName = key;
					break;
				}

				if (!string.IsNullOrEmpty(scriptName))
				{
					if (LoadScript(scriptTexts, scriptName, new List<string>()) == false)
						return;
				}
			}

			channelsScriptHandlers.Load();
			channelsScriptHandlers.InstallHandlers();
		}

		/// <summary>
		/// Load scripts with checking for dependencies (e.g. script_1 depends on script_2, then script_2 should be loaded first)
		/// </summary>
		/// <param name="scriptTexts"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		bool LoadScript(Dictionary<string, string> scriptTexts, string name, List<string> loadingStack)
		{
			if (loadingStack.Contains(name))
			{
				Env.Current.Logger.LogError(string.Format("Circular dependency of module {0}: {1}", name, loadingStack.ToString()));
				return false;
			}
			loadingStack.Add(name);

			List<string> modules = Script.GetImportedModules(scriptTexts[name]);
			foreach (string module in modules)
			{
				if (scriptTexts.ContainsKey(module))
				{
					if (LoadScript(scriptTexts, module, loadingStack) == false)
						return false;
				}
			}

			if (scriptTexts.ContainsKey(name))
			{
				Script script = new Script(scriptTexts[name], python, name);
				scripts.Add(script);
				scriptTexts.Remove(name);
			}

			return true;
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
					NewScriptCreated(this, script.Name);

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
