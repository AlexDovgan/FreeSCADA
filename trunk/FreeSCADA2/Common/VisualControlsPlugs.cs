using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FreeSCADA.Interfaces;
using FreeSCADA.Interfaces.Plugins;

namespace FreeSCADA.Common
{
    public class VisualControlsPlugs
	{
        private List<IVisualControlsPlug> visualPlugs = new List<IVisualControlsPlug>();

		public void Load()
		{
			DirectoryInfo di = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            foreach (FileInfo fi in di.GetFiles("VisualControls.*.dll"))
			{
				Assembly lib = Assembly.LoadFrom(fi.FullName);
				foreach (Type t in lib.GetExportedTypes())
				{
                    if (t.GetInterface(typeof(IVisualControlsPlug).FullName) != null)
					{
                        IVisualControlsPlug plug = (IVisualControlsPlug)Activator.CreateInstance(t);
						InitializePlugin(Env.Current, plug);
					}
				}
			}
		}

        private void InitializePlugin(IEnvironment env, IVisualControlsPlug plug)
		{
			plug.Initialize(env);
            visualPlugs.Add(plug);
		}

		public List<string> PluginIds
		{
			get
			{
				List<string> list = new List<string>();
                foreach (IVisualControlsPlug plug in visualPlugs)
					list.Add(plug.PluginId);
				return list;
			}
		}

        public List<IVisualControlsPlug> Plugins
        {
            get
            {
                List<IVisualControlsPlug> list = new List<IVisualControlsPlug>();
                foreach (IVisualControlsPlug plug in visualPlugs)
                    list.Add(plug);
                return list;
            }
        }

        public IVisualControlsPlug this[string pluginId]
		{
			get
			{
                foreach (IVisualControlsPlug plug in visualPlugs)
					if (plug.PluginId == pluginId)
						return plug;
				return null;
			}
		}

        public IVisualControlDescriptor GetControl(string name)
        {                
            string[] splited = name.Split('.');
            if(splited.Length>1)
            {
                IVisualControlsPlug plug;
                if( (plug=this[splited[0]])!=null)
                {
                    foreach (IVisualControlDescriptor control in plug.Controls)
                    {
                        if (control.Name == splited[1])
                            return control;
                    }
                }
            }
			return null;
        }
	}
}
