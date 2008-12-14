using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Common
{
	public class BaseCommandContext : ICommandContext
	{
		ToolStrip menu;
		ToolStrip toolbar;

		public BaseCommandContext(ToolStrip menu, ToolStrip toolbar)
		{
			this.menu = menu;
			this.toolbar = toolbar;

			if (toolbar != null && toolbar.Items.Count == 0)
				toolbar.Visible = false;
		}

		public void AddCommand(ICommand cmd)
		{
			cmd.CanExecuteChanged += new EventHandler(OnCommandCanExecuteChanged);
			if (cmd.DropDownItems != null)
				cmd.DropDownItems.CurrentChanged += new EventHandler(OnDropDownCommandCurrentChanged);

			//We cannot add the same item object into different holders. Therefore we create two copies
			if (menu != null)
			{
				ToolStripItem tsi;
				switch (cmd.Type)
				{
					case CommandType.Separator: 
						tsi = new ToolStripSeparator();
						break;
					case CommandType.Standard: 
						tsi = new ToolStripMenuItem();
						InitializeStandardCommand(tsi, cmd);
						tsi.Text = cmd.Name;
						break;
					case CommandType.DropDownBox:
						tsi = new ToolStripComboBox();
						InitializeDropDownBoxCommand((ToolStripComboBox)tsi, cmd);
						break;
					default:
						throw new NotImplementedException();
				}
				tsi.Tag = cmd;

				int pos = FindPositionToInsert(menu.Items, cmd);
				if (pos >= 0)
					menu.Items.Insert(pos, tsi);
				else
				{
					if(!(menu.Items.Count == 0 && cmd.Type == CommandType.Separator)) //Don't add separator if there is nothing to separate
						menu.Items.Add(tsi);
				}
			}

			if (toolbar != null)
			{
				ToolStripItem tsi;
				switch (cmd.Type)
				{
					case CommandType.Separator: 
						tsi = new ToolStripSeparator();
						break;
					case CommandType.Standard: 
						tsi = new ToolStripButton();
						InitializeStandardCommand(tsi, cmd);
						tsi.Name = cmd.Name;
						break;
					case CommandType.DropDownBox: 
						tsi = new ToolStripComboBox();
						InitializeDropDownBoxCommand((ToolStripComboBox)tsi, cmd);
						break;
					default:
						throw new NotImplementedException();
				}
				tsi.Tag = cmd;

				int pos = FindPositionToInsert(toolbar.Items, cmd);
				if (pos >= 0)
					toolbar.Items.Insert(pos, tsi);
				else
					toolbar.Items.Add(tsi);

				toolbar.Visible = true;
			}
		}

		private int FindPositionToInsert(ToolStripItemCollection items, ICommand cmd)
		{
			for(int i=0;i<items.Count;i++)
			{
				if (items[i].Tag is ICommand)
				{
					int itemPriority = (items[i].Tag as ICommand).Priority;
					if (itemPriority > cmd.Priority)
						return i;
				}
			}
			return -1;
		}

		private void InitializeStandardCommand(ToolStripItem tsi, ICommand cmd)
		{
			tsi.ToolTipText = cmd.Description;
			tsi.Image = cmd.Icon;
			tsi.Enabled = cmd.CanExecute;

			tsi.Click += new EventHandler(OnCommandClick);
		}

		private void InitializeDropDownBoxCommand(ToolStripComboBox tsi, ICommand cmd)
		{
			tsi.ToolTipText = cmd.Description;
			tsi.Image = cmd.Icon;
			tsi.Enabled = cmd.CanExecute;

			foreach (object obj in cmd.DropDownItems.Items)
				tsi.Items.Add(obj);
			if(cmd.DropDownItems.Current != null)
				tsi.Text = cmd.DropDownItems.Current.ToString();

			tsi.KeyUp += new KeyEventHandler(OnDropDownCommandKeyUp);
			tsi.SelectedIndexChanged += new EventHandler(OnDropDownCommandSelectedChanged);
		}

		public void RemoveCommand(ICommand cmd)
		{
			cmd.CanExecuteChanged -= new EventHandler(OnCommandCanExecuteChanged);
			if(cmd.DropDownItems != null)
				cmd.DropDownItems.CurrentChanged -= new EventHandler(OnDropDownCommandCurrentChanged);

			if (menu != null)
				RemoveToolStripItems(menu, cmd);
			if (toolbar != null)
			{
				RemoveToolStripItems(toolbar, cmd);
				if (toolbar.Items.Count == 0)
					toolbar.Visible = false;
			}
		}

		private void RemoveToolStripItems(ToolStrip container, ICommand cmd)
		{
			List<ToolStripItem> removalList = new List<ToolStripItem>();
			foreach (ToolStripItem item in container.Items)
			{
				if (item.Tag == cmd)
					removalList.Add(item);
			}

			foreach (ToolStripItem item in removalList)
			{
				switch (cmd.Type)
				{
					case CommandType.Standard:
						item.Click -= new EventHandler(OnCommandClick);
						break;
					case CommandType.DropDownBox:
						(item as ToolStripComboBox).KeyUp -= new KeyEventHandler(OnDropDownCommandKeyUp);
						break;
				}

				container.Items.Remove(item);
			}
		}

		void OnCommandClick(object sender, EventArgs e)
		{
			ToolStripItem item = (ToolStripItem)sender;
			if (item.Tag != null)
			{
				ICommand cmd = (ICommand)item.Tag;
				if (cmd.CanExecute)
					cmd.Execute();
			}
		}

		void OnDropDownCommandKeyUp(object sender, KeyEventArgs e)
		{
			ToolStripComboBox item = sender as ToolStripComboBox;
			if (item.Tag != null && e.KeyCode == Keys.Return)
			{
				ICommand cmd = (ICommand)item.Tag;
				if (cmd.CanExecute)
				{
					if (cmd.DropDownItems != null)
					{
						cmd.DropDownItems.Current = item.Text;
						cmd.Execute();
					}
				}
			}
		}

		void OnDropDownCommandSelectedChanged(object sender, EventArgs e)
		{
			ToolStripComboBox item = sender as ToolStripComboBox;
			if (item.Tag != null)
			{
				ICommand cmd = (ICommand)item.Tag;
				if (cmd.CanExecute)
				{
					if (cmd.DropDownItems != null)
					{
						cmd.DropDownItems.Current = item.SelectedItem.ToString();
						cmd.Execute();
						if (cmd.DropDownItems.Current != null)
							item.Text = cmd.DropDownItems.Current.ToString();
					}
				}
			}
		}

		void OnCommandCanExecuteChanged(object sender, EventArgs e)
		{
			ICommand cmd = (ICommand)sender;

			if (menu != null)
			{
				foreach (ToolStripItem item in menu.Items)
				{
					if (item.Tag == cmd)
						item.Enabled = cmd.CanExecute;
				}
			}

			if (toolbar != null)
			{
				foreach (ToolStripItem item in toolbar.Items)
				{
					if (item.Tag == cmd)
						item.Enabled = cmd.CanExecute;
				}
			}
		}

		void OnDropDownCommandCurrentChanged(object sender, EventArgs e)
		{
			ICommand cmd = (ICommand)sender;

			if (menu != null)
			{
				foreach (ToolStripItem item in menu.Items)
				{
					if (item.Tag == cmd)
						item.Text = cmd.DropDownItems.Current.ToString();
				}
			}

			if (toolbar != null)
			{
				foreach (ToolStripItem item in toolbar.Items)
				{
					if (item.Tag == cmd)
						item.Text = cmd.DropDownItems.Current.ToString();
				}
			}
		}
	}
}
