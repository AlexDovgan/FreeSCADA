using System;
using System.ComponentModel;
namespace FreeSCADA.ShellInterfaces
{
	public interface IChannel:INotifyPropertyChanged
	{
        event EventHandler ValueChanged;
		string Name
		{
			get;
		}

		string Type
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
