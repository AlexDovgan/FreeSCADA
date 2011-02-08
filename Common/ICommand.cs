using System;
using System.Drawing;

namespace FreeSCADA.Interfaces
{
	public enum CommandType
	{
		Separator,
        Submenu,
		Standard,
		DropDownBox
	}
    /// <summary>
    /// 
    /// </summary>
	public interface ICommand
    {
		event EventHandler CanExecuteChanged;

        /// <summary>
        /// 
        /// </summary>
        string Name
        {
            get;
        }
		/// <summary>
		/// 
		/// </summary>
		string Description
		{
			get;
		}
        /// <summary>
        /// 
        /// </summary>
        Bitmap Icon
        {
            get;
        }
        /// <summary>
        /// 
        /// </summary>
		CommandType Type
        {
            get;
        }

        bool CanExecute
        {
            get;
            set;
        }

		int Priority
		{
			get;
			set;
		}

		ICommandItems DropDownItems
		{
			get;
		}

		void Execute();
    }
}
