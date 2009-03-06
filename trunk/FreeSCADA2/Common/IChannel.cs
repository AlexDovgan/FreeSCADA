using System;
using System.ComponentModel;

namespace FreeSCADA.Interfaces
{
	public enum ChannelStatusFlags
	{
		Unknown = 0,
		Good	= 1,
		Bad		= 2,
        NotUsed = 0xFFFF
	}
        
	public interface IChannel:INotifyPropertyChanged
	{
        event EventHandler ValueChanged;
		string Name
		{
			get;
		}

		string PluginId
		{
			get;
		}

		Type Type
		{
			get;
		}

		bool IsReadOnly
		{
			get;
		}

		object Value
		{
			get;
			set;
		}

        DateTime ModifyTime
        {
            get;
        }

        string  Status
        {
            get;
        }

		ChannelStatusFlags StatusFlags
		{
			get;
			set;
		}

		//client's tmp_buff
		object Tag
		{
			get;
			set;
		}
	}
}
