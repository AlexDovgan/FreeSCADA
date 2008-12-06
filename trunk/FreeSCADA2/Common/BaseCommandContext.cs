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

			//We cannot add the same item object into different holders. Therefore we create two copies
			if (menu != null)
			{
				ToolStripItem tsi;
				switch (cmd.Type)
				{
					case CommandType.Separator: tsi = new ToolStripSeparator(); break;
					case CommandType.Standard: tsi = new ToolStripMenuItem(); break;
					default:
						throw new NotImplementedException();
				}
				tsi.Text = cmd.Name;
				tsi.ToolTipText = cmd.Description;
				tsi.Image = cmd.Icon;
				tsi.Tag = cmd;
				tsi.Enabled = cmd.CanExecute;
				tsi.Click += new EventHandler(OnCommandClick);

				menu.Items.Add(tsi);
			}

			if (toolbar != null)
			{
				ToolStripItem tsi;
				switch (cmd.Type)
				{
					case CommandType.Separator: tsi = new ToolStripSeparator(); break;
					case CommandType.Standard: tsi = new ToolStripButton(); break;
					default:
						throw new NotImplementedException();
				}
				tsi.Name = cmd.Name;
				tsi.ToolTipText = cmd.Description;
				tsi.Image = cmd.Icon;
				tsi.Tag = cmd;
				tsi.Enabled = cmd.CanExecute;
				tsi.Click += new EventHandler(OnCommandClick);

				toolbar.Items.Add(tsi);
				toolbar.Visible = true;
			}
		}

		public void RemoveCommand(ICommand cmd)
		{
			cmd.CanExecuteChanged -= new EventHandler(OnCommandCanExecuteChanged);

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
				item.Click -= new EventHandler(OnCommandClick);
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
	}
}
