using System;

namespace FreeSCADA.Communication.OPCPlug
{
	class Channel:ShellInterfaces.IChannel
	{
		string name;
		string opcChannel;
		string opcServer;
		string opcHost;
		Type type;
		Plugin plugin;
		object tag;
		object value = new object();
		object valueLock = new object();

		public Channel(string name, Plugin plugin, string opcChannel, string opcServer, string opcHost)
		{
			this.name = name;
			this.plugin = plugin;
			this.type = typeof(object);
			this.opcChannel = opcChannel;
			this.opcHost = opcHost;
			this.opcServer = opcServer;
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
			get { return false; }
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
				if (plugin.IsConnected)
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

		protected void InternalSetValue(object value)
		{
			type = value.GetType();
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

		public void DoUpdate(object value)
		{
			InternalSetValue(value);
		}

		public string OpcChannel
		{
			get { return opcChannel; }
		}
		public string OpcServer
		{
			get { return opcServer; }
		}

		public string OpcHost
		{
			get { return opcHost; }
		}
	}
}
