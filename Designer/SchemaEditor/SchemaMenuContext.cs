using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Designer.SchemaEditor.SchemaCommands
{
    
	class SchemaMenuContext:ICommandContext
	{
		ContextMenu menu;
        
		public SchemaMenuContext(ContextMenu menu)
		{
			this.menu = menu;
		}

		#region ICommandContext Members

		public void AddCommand(ICommand cmd)
		{
			cmd.CanExecuteChanged += new EventHandler(OnCommandCanExecuteChanged);

			if (menu != null)
			{
				MenuItem mi = new MenuItem();
				switch (cmd.Type)
				{
					case CommandType.Standard:
						mi.Text = cmd.Name;
						mi.Enabled = cmd.CanExecute;
						mi.Tag = cmd;
						mi.Click += new EventHandler(OnMenuItemClick);
						break;
					default:
						throw new NotImplementedException();
				}

				int pos = FindPositionToInsert(menu.MenuItems, cmd);
				if (pos >= 0)
					menu.MenuItems.Add(pos, mi);
				else
					menu.MenuItems.Add(mi);
			}
		}

		public void RemoveCommand(ICommand cmd)
		{
			cmd.CanExecuteChanged -= new EventHandler(OnCommandCanExecuteChanged);

			if (menu != null)
			{
				List<MenuItem> removalList = new List<MenuItem>();
				foreach (MenuItem item in menu.MenuItems)
				{
					if (item.Tag != null && item.Tag is ICommand)
					{
						ICommand menuCommand = item.Tag as ICommand;
						item.Click -= new EventHandler(OnMenuItemClick);
						removalList.Add(item);
					}
				}

				foreach (MenuItem item in removalList)
					menu.MenuItems.Remove(item);
			}
		}

		#endregion

		private int FindPositionToInsert(ContextMenu.MenuItemCollection items, ICommand cmd)
		{
			for (int i = 0; i < items.Count; i++)
			{
				if (items[i].Tag != null && items[i].Tag is ICommand)
				{
					int itemPriority = (items[i].Tag as ICommand).Priority;
					if (itemPriority > cmd.Priority)
						return i;
				}
			}
			return -1;
		}

		void OnCommandCanExecuteChanged(object sender, EventArgs e)
		{
			ICommand cmd = (ICommand)sender;

			if (menu != null)
			{
				foreach (MenuItem item in menu.MenuItems)
				{
					if (item.Tag != null && item.Tag is ICommand)
					{
						ICommand menuCommand = item.Tag as ICommand;
						if (menuCommand == cmd)
							item.Enabled = cmd.CanExecute;
					}
				}
			}
		}

		void OnMenuItemClick(object sender, EventArgs e)
		{
			MenuItem item = (MenuItem)sender;
			if (item.Tag != null)
			{
				ICommand cmd = (ICommand)item.Tag;
				if (cmd.CanExecute)
					cmd.Execute();
			}
		}
	}
}
