using System;
using System.Collections.Generic;
using FreeSCADA.Common;
using FreeSCADA.Interfaces;
using WeifenLuo.WinFormsUI.Docking;

namespace FreeSCADA.RunTime.Views
{
	class DocumentView : DockContent
	{
        string documentName = "";

		public struct CommandInfo
		{
			public ICommand command;
			public ICommandContext defaultContext;

			public CommandInfo(ICommand command)
			{
				this.command = command;
				this.defaultContext = null;
			}

			public CommandInfo(ICommand command, ICommandContext defaultContext)
			{
				this.command = command;
				this.defaultContext = defaultContext;
			}
		}

		public virtual List<CommandInfo> DocumentCommands
        {
            get;
            protected set;
        }

		public DocumentView()
		{
			DocumentCommands = new List<CommandInfo>();
			DockAreas = DockAreas.Float | DockAreas.Document;
			documentName = "Document";
			UpdateCaption();
		}

		public string DocumentName
		{
			get { return documentName; }
			set { documentName = value; UpdateCaption();}
		}

		public virtual void OnActivated()
		{
			foreach (CommandInfo cmdInfo in DocumentCommands)
				Env.Current.Commands.AddCommand(cmdInfo.defaultContext, cmdInfo.command);
		}

		public virtual void OnDeactivated()
		{
			foreach (CommandInfo cmdInfo in DocumentCommands)
				Env.Current.Commands.RemoveCommand(cmdInfo.command);
                 
        }

		private void UpdateCaption()
		{
			TabText = DocumentName;
		}

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            
        }
    }
}
