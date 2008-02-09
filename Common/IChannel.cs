using System;
using System.Collections.Generic;
using System.Text;

namespace FreeSCADA.ShellInterfaces
{
	public interface IChannel
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

		//client's data
		object Tag
		{
			get;
			set;
		}
	}
}
