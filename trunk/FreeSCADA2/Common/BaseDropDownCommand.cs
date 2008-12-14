using System;
using System.Collections.Generic;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Common
{
	public abstract class BaseDropDownCommand : BaseCommand, ICommandItems
	{
		public event EventHandler CurrentChanged;

		private object currentItem = null;
		private List<object> items = new List<object>();

		public override ICommandItems DropDownItems { get { return this; } }
		public override CommandType Type { get { return CommandType.DropDownBox; } }

		public virtual List<object> Items
		{
			get
			{
				return items;
			}
			protected set
			{
				items = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual object Current
		{
			get
			{
				return currentItem;
			}
			set
			{
				currentItem = value;
			}
		}

		protected void FireCurrentChanged()
		{
			if (CurrentChanged != null)
				CurrentChanged(this, new EventArgs());
		}
	}
}
