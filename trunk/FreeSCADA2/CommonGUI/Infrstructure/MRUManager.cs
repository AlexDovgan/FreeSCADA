using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using FreeSCADA.Interfaces;
using FreeSCADA.Common;


namespace FreeSCADA.CommonUI
{
	/// <summary>
	/// This class is to support Most Frequently Used (MRU) lists in a menu
	/// </summary>
	public class MRUManager:IDisposable
	{
		const int Size = 4;
        ICommandContext _context;
        Interfaces.IWindowManager _windowManager;
		//ToolStripItem firstPosition;
		//ToolStripItem lastPosition;

		public MRUManager(ICommandContext context,Interfaces.IWindowManager wm)
		{
            if (context == null)
                throw new ArgumentNullException("context");

            if (wm== null)
                throw new ArgumentNullException("windowManager");
            _windowManager = wm;

            _context = context;
			List<string> strings = new List<string>();
			try
			{
				string cfgFile = ConfigurationManager.GetUserConfigFile("MRUlist");
				using (Stream stream = new FileStream(cfgFile, FileMode.Open, FileAccess.Read, FileShare.Read))
				using (TextReader reader = new StreamReader(stream))
				{
					XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
					strings = (List<string>)serializer.Deserialize(reader);
				}
			}
			catch
			{
			}
			CreateItems(strings);
		}

		public void Add(string file)
		{
			
			List<string> strings = new List<string>();
			strings.Add(file);
            int i = 0;
            foreach (ICommand cmd in _context.GetCommands())
            {
                if (cmd is GlobalCommands.MruLoadProjectCommand)
                {
                    strings.Add(cmd.Name);
                    if (i++ >= Size)
                        break;
                }
            }

			try
			{
				string cfgFile = ConfigurationManager.GetUserConfigFile("MRUlist");
				if (!Directory.Exists(Path.GetDirectoryName(cfgFile)))
					Directory.CreateDirectory(Path.GetDirectoryName(cfgFile));

				using (Stream stream = new FileStream(cfgFile, FileMode.Create, FileAccess.Write, FileShare.Read))
				using (TextWriter writer = new StreamWriter(stream))
				{
					XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
					serializer.Serialize(writer, strings);
				}
			}
			catch(Exception e)
			{
				Env.Current.Logger.LogError(string.Format("Cannot save MRU list ({0})", e.Message));
			}
            CreateItems(strings);
		}

		void CreateItems(List<string> strings)
		{
            int i = 0;
			foreach(ICommand cmd in _context.GetCommands() )
			{
                if (cmd is GlobalCommands.MruLoadProjectCommand)
                    _context.RemoveCommand(cmd);
			}
			foreach (string str in strings)
			{
                if (i++ >=Size)
                    break;
                _context.AddCommand(new GlobalCommands.MruLoadProjectCommand(this, _windowManager, str));
                
			}
            
			
		}

		
		#region IDisposable Members

		public void Dispose()
		{
			CreateItems(new List<string>());
		}

		#endregion
	}
}
