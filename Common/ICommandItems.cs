using System;
using System.Collections.Generic;

namespace FreeSCADA.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
	public interface ICommandItems
    {
		event EventHandler CurrentChanged;

        /// <summary>
        /// 
        /// </summary>
        List<object> Items
        {
            get;
        }

		/// <summary>
		/// 
		/// </summary>
		object Current
		{
			get;
			set;
		}
    }
}
