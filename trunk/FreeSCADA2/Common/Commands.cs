using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FreeSCADA.ShellInterfaces;

namespace FreeSCADA.Common
{
	public class Commands : ICommands
	{
		MenuStrip mainMenu;
		struct CmdId
		{
			public int Id;
			public string PluginId;
		}
		Dictionary<int, CmdId> items = new Dictionary<int,CmdId>();

		public delegate void PluginCommandHandler(Object sender, int id, string pluginId);
		public event PluginCommandHandler PluginCommand;


		public Commands(MenuStrip mainMenu)
		{
			this.mainMenu = mainMenu;
		}


		public int RegisterCommand(string pluginId, string name, string group)
		{
			ToolStripMenuItem group_item = OpenGroupItem(group);
			ToolStripMenuItem new_item = new ToolStripMenuItem(name);
			group_item.DropDown.Items.Add(new_item);

			CmdId cmdId = new CmdId();
			cmdId.Id = items.Count;
			cmdId.PluginId = pluginId;
			items.Add(cmdId.Id, cmdId);

			new_item.Tag = cmdId.Id;
			new_item.Click += new EventHandler(OnItemClick);
			return cmdId.Id;
		}

		private ToolStripMenuItem OpenGroupItem(string group)
		{
			foreach (ToolStripItem item in mainMenu.Items)
			{
				if (group == item.Text && item is ToolStripMenuItem)
					return (ToolStripMenuItem)item;
			}
			ToolStripMenuItem new_item = new ToolStripMenuItem(group);
			mainMenu.Items.Add(new_item);
			return new_item;
		}

		void OnItemClick(object sender, EventArgs e)
		{
			CmdId cmdId = items[(int)(sender as ToolStripMenuItem).Tag];
			if(PluginCommand != null)
				PluginCommand(this, cmdId.Id, cmdId.PluginId);
		}
	}
}
