using System;
using System.Drawing;

namespace FreeSCADA.Interfaces
{
	public enum CommandType
	{
		Separator,
		Standard
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

		void Execute();
    }
}
