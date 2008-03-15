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

		List<ITool> toolsList;

		public ToolBoxView()
		{
			TabText = "ToolBox";
			AutoHidePortion = 0.15;
		}

		public void OnToolsCollectionChanged(List<ITool> tools, Type currentTool)
		{
			if (toolsList != tools)
			{
				toolsList = tools;
				int pos = 0;
				SuspendLayout();
				Controls.Clear();
				if (tools != null)
				{
					foreach (ITool tool in tools)
					{
						RadioButton b = new RadioButton();
						b.Text = tool.ToolName;
						if (tool.GetType() == currentTool)
						{
							b.Checked = true;
							b.Focus();
							b.Tag = currentTool;
							NotifyToolActivated((Type)b.Tag);
						}
						else b.Tag = tool.GetType();
						b.Location = new System.Drawing.Point(2, 7 + pos * 19);
						b.CheckedChanged += new EventHandler(ToolChanged);
						Controls.Add(b);

						pos++;
					}
				}
				ResumeLayout(false);
			}
			else
			{
				foreach (Control control in Controls)
				{
					SuspendLayout();
					if (control is RadioButton && (Type)control.Tag == currentTool)
					{
						RadioButton rb = (RadioButton)control;
						rb.Checked = true;
						rb.Focus();
						rb.Tag = currentTool;
						NotifyToolActivated((Type)rb.Tag);
					}
					ResumeLayout(false);
				}
			}
		}

		void ToolChanged(object sender, EventArgs e)
		{
			RadioButton b = (RadioButton)sender;
			if (b.Checked)
				NotifyToolActivated((Type)b.Tag);
		}

		private void NotifyToolActivated(Type tool)
		{
			if (ToolActivated != null)
				ToolActivated(this, tool);
		}

		public void Clean()
		{
			Controls.Clear();
		}
	}
}
