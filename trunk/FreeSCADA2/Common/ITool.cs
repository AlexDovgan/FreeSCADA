using System;
using System.Drawing;
namespace FreeSCADA.ShellInterfaces
{
    public interface ITool
    {
		event EventHandler ToolFinished;
		event EventHandler ToolStarted;
		event EventHandler ToolWorking;

        String ToolName
        {
            get;
        }
        String ToolGroup
        {
            get;
        }
        Bitmap ToolIcon
        {
            get;
        }
    }
}

