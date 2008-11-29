using System;
using System.Windows.Forms;
using FreeSCADA.ShellInterfaces;

namespace FreeSCADA.Designer
{
	class CommandManager
	{
		static CommandManager commandManagerInstance = null;

		ToolStrip documentToolbar;

		#region Initialization and singleton implementation

		public static void Initialize(ToolStrip documentToolbar)
		{
			if (commandManagerInstance == null)
			{
				commandManagerInstance = new CommandManager();

				commandManagerInstance.documentToolbar = documentToolbar;
			}
		}
        
		public static void Deinitialize()
		{
			commandManagerInstance = null;
		}

		public static CommandManager Current
		{
			get
			{
				if (commandManagerInstance == null)
					throw new System.NullReferenceException();

				return commandManagerInstance;
			}
		}

		CommandManager() { }

		#endregion

        public ToolStripItem AddDocumentCommand(ICommandData cmd)
        {
            ToolStripItem tsi = (ToolStripItem)Activator.CreateInstance(cmd.ToolStripItemType);
            tsi.Name = cmd.CommandName;
            tsi.Image = cmd.CommandIcon;
            tsi.Tag = cmd.Tag;
            tsi.Click += new EventHandler(cmd.EvtHandler);
            tsi.Enabled = cmd.CanExecute(null);
			tsi.ToolTipText = cmd.CommandDescription;
            cmd.CommandToolStripItem = tsi;
			if (documentToolbar != null) 
				documentToolbar.Items.Add(tsi);
            return tsi;
        }

        public void RemoveDocumentCommand(ToolStripItem tsi)
        {
			if (documentToolbar != null) 
				documentToolbar.Items.Remove(tsi);
        }
	}
}
