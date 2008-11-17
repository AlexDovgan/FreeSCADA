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
        protected ICommunicationPlug plugin;
        private object tag;
        private object value = new object();
        private object valueLock = new object();

        

        public BaseChannel(string name, bool readOnly, ICommunicationPlug plugin, Type type)
        {
            this.name = name;
            this.readOnly = readOnly;
            this.plugin = plugin;
            this.type = type;
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
            get { return tag; }
            set { tag = value; }
        }

        #endregion

        protected void FireValueChanged()
        {
            if (PropertyChanged != null)
                OnPropertyChanged("Value");
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
