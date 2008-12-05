using System;
using System.ComponentModel;
using FreeSCADA.ShellInterfaces;
using FreeSCADA.ShellInterfaces.Plugins;

namespace FreeSCADA.Common
{
    public abstract class BaseChannel : IChannel
    {
		private ChannelStatusFlags status = ChannelStatusFlags.Unknown;
		private string name;
		private Type type;
		protected bool readOnly;
		private DateTime modifyTime;

        protected ICommunicationPlug plugin;
        private object tag;
        private object value = new object();

        public BaseChannel(string name, bool readOnly, ICommunicationPlug plugin, Type type)
        {
            this.name = name;
            this.readOnly = readOnly;
            this.plugin = plugin;
            this.type = type;
			if (type != value.GetType())
                if (type != typeof(string))     // This approach does not work with STRING channels!!!!!!!!!!!
				    value = System.Activator.CreateInstance(type);
                else
                    value = "";

            modifyTime = DateTime.MinValue;
            status = ChannelStatusFlags.Unknown;
        }

        #region IChannel Members

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler ValueChanged;

        public string Name
        {
            get 
			{ 
				lock(this)
					return name; 
			}
        }

        public string Type
        {
            get 
			{
				lock (this) 
					return type.Name;
			}
        }

        public bool IsReadOnly
        {
            get 
			{
				lock (this)
					return readOnly; 
			}
        }

        public virtual object Value
        {
            get
            {
                lock (this)
                    return value;

            }
            set
            {
				if (!IsReadOnly /*&& plugin.IsConnected*/)
                    DoUpdate(value);
            }
        }

        public DateTime ModifyTime
        {
            get
            {
				lock(this)
					return modifyTime;
            }
        }

        public string Status
        {
            get
            {
				switch (StatusFlags)
				{
					case ChannelStatusFlags.Good:
						return "Good";
					case ChannelStatusFlags.Bad:
						return "Bad";
					default:
						return "Unknown";
				}
            }
        }

		public ChannelStatusFlags StatusFlags
		{
			get
			{
				lock (this)
					return status;
			}
			set
			{
				lock (this)
					status = value;
			}
		}

        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        #endregion

        protected void FireValueChanged()
        {
            if (PropertyChanged != null)
            {
                OnPropertyChanged("Value");
                OnPropertyChanged("ModifyTime");
                OnPropertyChanged("Status");
				OnPropertyChanged("StatusFlags");
            }
            if (ValueChanged != null)
                ValueChanged(this, new EventArgs());
        }

        protected void InternalSetValue(object value, DateTime externalTime, ChannelStatusFlags status)
        {
            bool fire = false;
            lock (this)
            {
                object old = this.value;
                this.value = value;
				this.type = value.GetType();

                modifyTime = externalTime;
				this.status = status;
                if (old != null)
                    fire = !old.Equals(this.value);
                else
                    fire = true;
            }
            if (fire)
                FireValueChanged();
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

		public abstract void DoUpdate();
        public virtual void DoUpdate(object value)
        {
			InternalSetValue(value, DateTime.Now, ChannelStatusFlags.Good);
        }
		public virtual void DoUpdate(object value, DateTime externalTime, ChannelStatusFlags status)
		{
			InternalSetValue(value, externalTime, status);
		}
    }
}
