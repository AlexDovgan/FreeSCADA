﻿using System;
using System.ComponentModel;

namespace FreeSCADA.Communication.SimulatorPlug
{
	abstract class BaseChannel:ShellInterfaces.IChannel
	{
		protected string name;
		protected Type type;
		protected bool readOnly;
		protected Plugin plugin;
		private object tag;
		private object value = new object();
		private object valueLock = new object();

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler ValueChanged;

		public BaseChannel(string name, bool readOnly, Plugin plugin, Type type)
		{
			this.name = name;
			this.readOnly = readOnly;
			this.plugin = plugin;
			this.type = type;
		}

		#region IChannel Members

		

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
				if (!readOnly && plugin.IsConnected)
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
            if (PropertyChanged != null)
                   OnPropertyChanged("Value");
            if (ValueChanged!= null)
                ValueChanged(this, new EventArgs());
            
				
		}

		protected void InternalSetValue(object value)
		{
			if (value.GetType() == type)
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
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
 
	}
}
