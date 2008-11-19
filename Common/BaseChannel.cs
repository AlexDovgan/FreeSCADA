using System;
using System.Collections.Generic;
using System.Text;
using FreeSCADA.ShellInterfaces;
using System.ComponentModel;
using FreeSCADA.ShellInterfaces.Plugins;

namespace FreeSCADA.Common
{
    public abstract class BaseChannel : IChannel
    {
        protected string name;
        protected Type type;
        protected bool readOnly;
        protected DateTime modifyTime;
        protected string status;
        protected ICommunicationPlug plugin;
        private object tag;
        private object value = null;
        private object valueLock = new object();

        

        public BaseChannel(string name, bool readOnly, ICommunicationPlug plugin, Type type)
        {
            this.name = name;
            this.readOnly = readOnly;
            this.plugin = plugin;
            this.type = type;
            modifyTime = DateTime.MinValue;
            status = "NotSet";
            if(value==null)
                value=System.Activator.CreateInstance(type);
        }

        #region IChannel Members

        public event PropertyChangedEventHandler PropertyChanged;
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
                lock (valueLock)
                    return value;

            }
            set
            {
                if (!readOnly && plugin.IsConnected)
                    InternalSetValue(value);
            }
        }
        public DateTime ModifyTime
        {
            get
            {
                return modifyTime;
            }
        }

        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status=value;
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
            }
            if (ValueChanged != null)
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
                    modifyTime = DateTime.Now;
                    status = "Good";
                    fire = !old.Equals(this.value);
                }
                if (fire)
                    FireValueChanged();
            }
        }
        public abstract void DoUpdate();
        public virtual void DoUpdate(object value)
        {
            InternalSetValue(value);
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
