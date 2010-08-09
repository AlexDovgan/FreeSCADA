using System;
using System.Collections.Generic;
using FreeSCADA.Common;
using FreeSCADA.Designer.SchemaEditor;
using FreeSCADA.Interfaces;
using WeifenLuo.WinFormsUI.Docking;

namespace FreeSCADA.Designer.Views
{
	class DocumentView : DockContent
	{

        public delegate void ObjectSelectedDelegate(object sender);
        public event ObjectSelectedDelegate ObjectSelected;

        
        string documentName = "";
		bool modifiedFlag;
		bool handleModifiedFlagOnClose = true;

        public event EventHandler IsModifiedChanged;


      
        
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

        public BasicUndoBuffer UndoBuff
        {
            get;
            protected set;
        }

		public DocumentView(string documentName)
		{
			DocumentCommands = new List<CommandInfo>();
			DockAreas = DockAreas.Float | DockAreas.Document;
            this.documentName = documentName;
			UpdateCaption();
		}

		public virtual string DocumentName
		{
			get { return documentName; }
			set { documentName = value; UpdateCaption();}
		}

		/// <summary>
		/// This property should be set to "true" for new documents and set to "false" after saving the document.
		/// </summary>
		public virtual bool IsModified
		{
			get { return modifiedFlag; }
			set { modifiedFlag = value;
                if (IsModifiedChanged != null)
                    IsModifiedChanged(this, new EventArgs());
                UpdateCaption(); }
		}

		public bool HandleModifiedOnClose
		{
			get { return handleModifiedFlagOnClose; }
			set { handleModifiedFlagOnClose = value; }
		}

        public virtual void OnToolActivated(object sender, Type tool)
        {
        }

		public virtual void OnActivated()
		{
			foreach (CommandInfo cmdInfo in DocumentCommands)
			{
				if(cmdInfo.defaultContext != null)
					Env.Current.Commands.AddCommand(cmdInfo.defaultContext, cmdInfo.command);
				else
					Env.Current.Commands.AddCommand(CommandManager.documentContext, cmdInfo.command);
			}
		}

		public virtual void OnDeactivated()
		{
			foreach (CommandInfo cmdInfo in DocumentCommands)
				Env.Current.Commands.RemoveCommand(cmdInfo.command);
        }

		public virtual bool SaveDocument()
		{
			return false;
		}
	

        public void RaiseObjectSelected(object sender )
        {
			if(ObjectSelected != null)
				ObjectSelected(sender);
        }

		protected virtual void UpdateCaption()
		{
			TabText = DocumentName;
			if (IsModified)
				TabText += " *";
		}

        
        protected override void OnClosed(EventArgs e)
        {
            
            RaiseObjectSelected(null);
            ObjectSelected = null;
            base.OnClosed(e);
        }

        public virtual void OnPropertiesBrowserChanged(object el)
        {
            IsModified = true;
        }
    }
}
