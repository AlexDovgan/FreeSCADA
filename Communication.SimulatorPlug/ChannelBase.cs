using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeSCADA.Communication.SimulatorPlug
{
	abstract class ChannelBase:ShellInterfaces.IChannel
	{
		protected string name;
		protected Type type;
		protected bool readOnly;
		protected Plugin plugin;
		private object tag;
		private object value = new object();
		private object valueLock = new object();

		public enum InternalChannelType { SimpleGeneric, RandomInteger};
		InternalChannelType internalType;

		public ChannelBase(string name, bool readOnly, Plugin plugin, Type type, InternalChannelType internalType)
		{
			this.name = name;
			this.readOnly = readOnly;
			this.plugin = plugin;
			this.InternalType = internalType;
			this.type = type;
		}

		#region IChannel Members

		public event EventHandler ValueChanged;

		public string Name
		{
			get { return name; }
		}

		public string Type
		{
			get { return type.Name; }
		}

		public bool IsReadOnly
		{
			get { return readOnly; }
		}

		public virtual object Value
		{
			get
			{
				if (plugin.IsConnected)
				{
					lock (valueLock)
						return value;
				}
				else
					return null;
			}
			set
			{
				if (!readOnly)
					InternalSetValue(value);
			}
		}

		public object Tag
		{
			get{return tag;}
			set{tag = value;}
		}

		#endregion

		protected void FireValueChanged()
		{
			if (ValueChanged != null)
				ValueChanged(this, new EventArgs());
		}

		public InternalChannelType InternalType
		{
			get { return internalType; }
			set { internalType = value; }
		}

		protected void InternalSetValue(object value)
		{
			if (value.GetType() == type && plugin.IsConnected)
			{
				bool fire = false;
				lock (valueLock)
				{
					object old = this.value;
					this.value = value;
					fire = !old.Equals(this.value);
				}
				if (fire)
					FireValueChanged();
			}
		}

		public virtual void DoUpdate()
		{
		}
	}
}
