
using System.Windows.Input;
using System.Drawing;
using System;
namespace FreeSCADA.Common
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
