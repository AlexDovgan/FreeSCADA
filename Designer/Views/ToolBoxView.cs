using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FreeSCADA.ShellInterfaces;

namespace FreeSCADA.Designer.Views
{
	/// <summary>
	/// Represents available tools and manipulators for active document window. Basically used for Schema editing.
	/// </summary>
    class ToolBoxView : ToolWindow
    {
		/// <summary>
		/// Notify that current tool has changed
		/// </summary>
		/// <param name="sender">Sender of the event. Typically it is ToolBoxView object instance.</param>
		/// <param name="tool">Instance of activated tool</param>
		public delegate void ToolActivatedHandler(Object sender, Type tool);
		/// <summary>Occurs when user selects a tool from the list</summary>
		public event ToolActivatedHandler ToolActivated;

        public ToolBoxView()
        {
            TabText = "ToolBox";
			AutoHidePortion = 0.15;
        }

        public void OnToolsCollectionChanged(List<ITool> tools, Type currentTool)
        {
            int pos = 0;
            SuspendLayout();
            Controls.Clear();
            if(tools!=null)
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
                if(ToolActivated != null)
					ToolActivated(this, (Type)b.Tag);
            }
        }

		public void Clean()
		{
			Controls.Clear();
		}
    }
}
