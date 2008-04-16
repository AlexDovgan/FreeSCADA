using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FreeSCADA.ShellInterfaces;
using Silver.UI;

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
        ToolBox toolBox;
		public ToolBoxView()
		{
			TabText = "ToolBox";
			AutoHidePortion = 0.15;
            AddToolBox();
            
		}

		public void OnToolsCollectionChanged(List<ITool> tools, Type currentTool)
		{
            
            
			if (toolsList != tools)
			{
				toolsList = tools;
				
				SuspendLayout();

                toolBox.DeleteAllTabs(true);
                            
    			if (tools != null)
				{
					foreach (ITool tool in tools)
					{
                        ToolBoxTab tbt;
                        ToolBoxItem tbi;
						if((tbt=toolBox[tool.ToolGroup])==null)
                        {
                            tbt = new ToolBoxTab(tool.ToolGroup, -1);
                            toolBox.AddTab(tbt);
                        }
                        tbi=new ToolBoxItem(tool.ToolName,-1);
                        tbt.AddItem(tbi);

                        tbi.Selected = false;
						if (tool.GetType() == currentTool)
						{
							tbi.Selected = true;
							tbi.Object = currentTool;
							NotifyToolActivated((Type)tbi.Object);
						}
						else
                           
                            tbi.Object= tool.GetType();
						
						
					}
				}
				ResumeLayout(false);
			}
			else
			{
			/*	foreach (Control control in Controls)
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
             */
			}
            
		}

		void ToolChanged(object sender, EventArgs e)
		{
            ToolBoxItem tbi = (ToolBoxItem)sender;
            if (tbi.Selected)
            {
               
                NotifyToolActivated((Type)tbi.Object);
            }
		}

		private void NotifyToolActivated(Type tool)
		{
			if (ToolActivated != null)
				ToolActivated(this, tool);
		}

		public void Clean()
		{
            toolBox.DeleteAllTabs(true);
		}

        void AddToolBox()
        {
            toolBox= new ToolBox();

            toolBox.BackColor = System.Drawing.SystemColors.Control;
            toolBox.Dock = System.Windows.Forms.DockStyle.Fill;
            toolBox.TabHeight = 18;
            toolBox.ItemHeight = 20;
            toolBox.ItemSpacing = 1;

            toolBox.ItemHoverColor = System.Drawing.Color.BurlyWood;
            toolBox.ItemNormalColor = System.Drawing.SystemColors.Control;
            toolBox.ItemSelectedColor = System.Drawing.Color.Linen;

            toolBox.Name = "_toolBox";
            toolBox.ItemSelectionChanged+=new ItemSelectionChangedHandler(ToolChanged);
            Controls.Add(toolBox);

            
        }

	}
}
