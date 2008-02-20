using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FreeSCADA.Common;
using FreeSCADA.ShellInterfaces;


namespace FreeSCADA.Designer
{
    class ToolBoxView : ToolWindow
    {
        public delegate void ToolActivatedDelegate(Type tool);
        public event ToolActivatedDelegate ToolActivated;

        public ToolBoxView()
        {
            TabText = "ToolBox";
        }
        public void ToolsCollectionChanged(List<ITool> tools, Type currentTool)
        {
            int pos = 0;
            SuspendLayout();
            Controls.Clear();
            foreach (ITool tool in tools)
            {

                CheckBox b = new CheckBox();
                b.Text = tool.ToolName;
                if (tool.GetType() == currentTool)
                {
                    b.Checked = true;
                    b.Tag = currentTool;
                }
                else b.Tag = tool.GetType();
                b.Location = new System.Drawing.Point(2, 7 + pos * 19);
                b.CheckedChanged += new EventHandler(ToolChanged);
                Controls.Add(b);

                pos++;
            }
            ResumeLayout(false);
        }

        void ToolChanged(object sender, EventArgs e)
        {
            CheckBox b = (CheckBox)sender;
            if (b.Checked)
            {
                foreach (CheckBox ch in Controls)
                {
                    if (ch != b)
                        ch.Checked = false;
                }
                ToolActivated((Type)b.Tag);
            }
        }
    }
}
