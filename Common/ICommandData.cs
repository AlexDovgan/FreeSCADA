using System;
using System.Drawing;
using System.Windows.Input;

namespace FreeSCADA.ShellInterfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICommandData : ICommand
    {
        /// <summary>
        /// 
        /// </summary>
        string CommandName
        {
            get;
        }
		/// <summary>
		/// 
		/// </summary>
		string CommandDescription
		{
			get;
		}
        /// <summary>
        /// 
        /// </summary>
        Bitmap CommandIcon
        {
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        System.Windows.Forms.ToolStripItem CommandToolStripItem
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        Type ToolStripItemType
        {
            get;
        }
        Object Tag
        {
            get;
            set;
        }
        bool Enabled
        {
            get;
            set;
        }
        void EvtHandler(object sender, System.EventArgs e);
    }
}
