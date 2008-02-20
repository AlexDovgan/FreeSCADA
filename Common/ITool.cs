using System;
using System.Drawing;
namespace FreeSCADA.ShellInterfaces
{
    public interface ITool
    {
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

