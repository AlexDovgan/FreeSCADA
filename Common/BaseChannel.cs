using System;
using System.ComponentModel;
using FreeSCADA.ShellInterfaces;
using FreeSCADA.ShellInterfaces.Plugins;

namespace FreeSCADA.Common
{
    public abstract class BaseChannel : IChannel
    {
        protected string bname;
        protected Type btype;
        protected bool breadOnly;
        protected DateTime bmodifyTime;
        protected string bstatus;
        //protected short quality;   // TODO - quality property
        protected ICommunicationPlug plugin;
        private object tag;
        private object value = null;
        private object valueLock = new object();

        public const short Q_GOOD = 0x03 << 6;
        


        public BaseChannel(string name, bool readOnly, ICommunicationPlug plugin, Type type)
        {
            this.bname = name;
            this.breadOnly = readOnly;
            this.plugin = plugin;
            this.btype = type;
            bmodifyTime = DateTime.MinValue;
            bstatus = "NotSet";
            if (value == null)
                if (type == typeof(string))
                {
                    value = new String(new char[0]);
                }
                else
                    value = System.Activator.CreateInstance(type);
        }

        #region IChannel Members

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler ValueChanged;

        public string Name
        {
            get { return bname; }
        }

        public string Type
        {
            get { return btype.Name; }
        }

        public bool IsReadOnly
        {
            get { return breadOnly; }
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
                if (!breadOnly && plugin.IsConnected)
                    ExternalSetValue(value);
            }
        }
        public DateTime ModifyTime
        {
            get
            {
                return bmodifyTime;
            }
        }

        public string Status
        {
            get
            {
                return bstatus;
            }
            set
            {
                bstatus = value;
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
            btype = value.GetType();
            bool fire = false;
            lock (valueLock)
            {
                object old = this.value;
                this.value = value;
                bmodifyTime = DateTime.Now;
                bstatus = "Good";
                fire = !old.Equals(this.value);
            }
            if (fire)
                FireValueChanged();

        }

        protected void InternalSetValue(object value, DateTime externalTime, short externalQuality)
        {
            btype = value.GetType();
            bool fire = false;
            lock (valueLock)
            {
                object old = this.value;
                this.value = value;
                bmodifyTime = externalTime;
                if (externalQuality == Q_GOOD)
                {
                    bstatus = "Good";
                }
                else
                {
                    bstatus = "Bad";
                }
                fire = !old.Equals(this.value);
            }
            if (fire)
                FireValueChanged();
        }

        public virtual void DoUpdate(object value, DateTime externalTime, short externalQuality)
        {
            InternalSetValue(value, externalTime, externalQuality);
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
        public abstract void DoUpdate();
        public virtual void DoUpdate(object value)
        {
            InternalSetValue(value);
        }
        public virtual void ExternalSetValue(object value)
        {
            DoUpdate(value);
        }

        
       
    }
}
