using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreeSCADA.Common;
using FreeSCADA.Interfaces;
using FreeSCADA.CommonUI.Interfaces;

namespace FreeSCADA.CommonUI.GlobalCommands
{

   
    public class LoadProjectCommand:BaseCommand
    {
        IWindowManager _windowManager;
        MRUManager _mruManager;
        
        public override string Description
        {
            get
            {
                return "Load Project command";
            }
        }
        public override string Name
        {
            get
            {
                return "Load Project";
            }
        }
        
        public override System.Drawing.Bitmap Icon
        {
            get
            {
                return global::FreeSCADA.CommonUI.Resources.open_file;
            }
        }
        public LoadProjectCommand(MRUManager mruManager,IWindowManager wm)
        {
            _windowManager = wm;
            _mruManager = mruManager;
            CanExecute = true;
            Priority = (int)CommandManager.Priorities.FileCommands;
        }
        public override void Execute()
        {
            var fd = new OpenFileDialog
            {
                Filter = CommonUI.StringResources.FileOpenDialogFilter,
                FilterIndex = 0,
                RestoreDirectory = true
            };

            if (fd.ShowDialog() != DialogResult.OK)
                return;

            _windowManager.Close();
            Env.Current.Project.Load(fd.FileName);
            _mruManager.Add(fd.FileName);
            
        }
    }

    public class MruLoadProjectCommand : BaseCommand
    {
        IWindowManager _windowManager;
        MRUManager _mruManager;
        string _fileName;
        public override string Description
        {
            get
            {
                return "Load Project command";
            }
        }
        public override string Name
        {
            get
            {
                return _fileName;
            }
        }

        public override System.Drawing.Bitmap Icon
        {
            get
            {
                return global::FreeSCADA.CommonUI.Resources.open_file;
            }
        }
        public MruLoadProjectCommand(MRUManager mruManager, IWindowManager wm,string fileName)
        {
            _windowManager = wm;
            _mruManager = mruManager;
            _fileName = fileName;
            CanExecute = true;
            Priority = (int)CommandManager.Priorities.MruCommands;
        }
        public override void Execute()
        {   
            _windowManager.Close();
            Env.Current.Project.Load(_fileName);
            _mruManager.Add(_fileName);

        }
    }

    public class ExitCommand : BaseCommand
    {
        public override string Description
        {
            get
            {
                return "Exit";
            }
        }
        public override string Name
        {
            get
            {
                return "Exit";
            }
        }

        public override System.Drawing.Bitmap Icon
        {
            get
            {
                return null;
            }
        }
        public ExitCommand()
        {
            
            Priority = (int)CommandManager.Priorities.MruCommandsEnd;
            CanExecute = true;
        }
        public override void Execute()
        {
            Application.Exit();
        }
    }


    public class NullCommand : BaseCommand
	{
		public NullCommand()
		{
		}
		public NullCommand(int priority)
		{
			Priority = priority;
		}

		public override string Name { get { return ""; } }
		public override string Description { get { return ""; } }
		public override CommandType Type { get { return CommandType.Separator; } }
	}
}
