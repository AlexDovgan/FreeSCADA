using System;
using System.Drawing;

namespace FreeSCADA.Designer
{
    class ToolDescriptor
    {
        public ToolDescriptor(string name,string group,Bitmap icon,Type type)
        {
            ToolName = name;
            ToolGroup = group;
            ToolIcon = icon;
            ToolType = type;
        }
        public String ToolName
        {
            get;
            protected set;
        }
        public String ToolGroup
        {
            get;
            protected set;
        }
        public Bitmap ToolIcon
        {
            get;
            protected set;
        }
        public Type ToolType
        {
            get;
            protected set;
        }
    }
}

