using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreeSCADA.Common;
using FreeSCADA.Interfaces;
using FreeSCADA.CommonUI.Interfaces;
using FreeSCADA.CommonUI;

namespace FreeSCADA.Designer.Commands
{

    class NewSchemaCommand : BaseCommand
    {
        IWindowManager _windowManager;
        public override string Description
        {
            get
            {
                return "New Schema Command";
            }
        }
        public override string Name
        {
            get
            {
                return "New Schema";
            }
        }
        public override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resources.open_schema;
            }
        }
        public NewSchemaCommand(IWindowManager wm)
        {
            CanExecute = true;
            _windowManager = wm;
        }
        public override void Execute()
        {
           
            
            var view = new Views.SchemaView(new Common.Documents.SchemaDocument());
            _windowManager.ActivateDocument(view);            
            
        }
    }

    class RunProjectCommand:BaseCommand
    {
        public override string Description
        {
            get
            {
                return "Run Project Command";
            }
        }
        public override string Name
        {
            get
            {
                return "Run Project";
            }
        }
        public override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resources.run;
            }
        }
        public RunProjectCommand()
        {
            CanExecute = true;
        }
        public override void Execute()
        {
            ProcessStartInfo psi = new ProcessStartInfo(Application.StartupPath + @"\\RunTime.exe");
            psi.Arguments = "\"" + Env.Current.Project.FileName + "\"";
            Process.Start(psi);
        }
    }

    class NewProjectCommand : BaseCommand
    {
        IWindowManager _windowManager;
        public override string Description
        {
            get
            {
                return "New Project command";
            }
        }
        public override string Name
        {
            get
            {
                return "New Project";
            }
        }
        public override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resources.new_file;
            }
        }
        
        public NewProjectCommand(IWindowManager wm)
        {
            CanExecute = true;
            _windowManager = wm;
        }
        public override void Execute()
        {
            if (_windowManager.Close())
            {
                Env.Current.CreateNewProject();
            }
        }
    }


    class SaveProjectCommand : BaseCommand
    {
        IWindowManager _windowManager;
        MRUManager _mruManager;
        public override string Description
        {
            get
            {
                return "Save Project command";
            }
        }
        public override string Name
        {
            get
            {
                return "Save Project";
            }
        }

        public override System.Drawing.Bitmap Icon
        {
            get
            {
                return global::FreeSCADA.CommonUI.Resources.save_file;
            }
        }
        public SaveProjectCommand(MRUManager mruManager, IWindowManager wm)
        {
            _windowManager = wm;
            _mruManager = mruManager;
            CanExecute = true;
            Priority = (int)CommandManager.Priorities.FileCommands;
        }
        public override void Execute()
        {
            string projectFileName = Env.Current.Project.FileName;
            if (projectFileName == "")
            {
                var fd = new SaveFileDialog
                {
                    Filter = CommonUI.StringResources.FileOpenDialogFilter,
                    FilterIndex = 0,
                    RestoreDirectory = true
                };

                if (fd.ShowDialog() == DialogResult.OK)
                    projectFileName = fd.FileName;
                else
                    return;
            }

            _windowManager.SaveAllDocuments();
            Env.Current.Project.Save(projectFileName);
            _windowManager.Refresh();
            _mruManager.Add(projectFileName);
            
        }
    }
}
