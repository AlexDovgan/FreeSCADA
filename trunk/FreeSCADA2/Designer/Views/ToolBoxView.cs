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


        ToolBox toolBox;
        public ToolBoxView()
        {
            TabText = "ToolBox";
            AutoHidePortion = 0.15;
            AddToolBox();

        }

        public void SetCurrentTool(ToolDescriptor toolToSet)
        {
            ToolBoxTab tbt;
            ToolBoxItem tbi;
            tbt = toolBox[toolToSet.ToolGroup];
            tbt.Selected = true;
            tbi = tbt[toolToSet.ToolName];
            tbi.Selected = true;
            tbt.SelectedItem = tbi;
            //NotifyToolActivated(toolToSet);
            ToolChanged(tbi, null);
            //OnToolsCollectionChanged(tools, toolToSet);
        }

        public void OnToolsCollectionChanged(List<ToolDescriptor> tools, Type currentTool)
        {

            ToolBoxTab tbtoselect = null;
            ToolBoxItem tbitoselect = null;


            SuspendLayout();

            toolBox.DeleteAllTabs(true);
            toolBox.SmallImageList = new ImageList();

            if (tools != null)
            {
                int imgnum = 0;
                foreach (ToolDescriptor tool in tools)
                {
                    ToolBoxTab tbt;
                    ToolBoxItem tbi;
                    if ((tbt = toolBox[tool.ToolGroup]) == null)
                    {
                        tbt = new ToolBoxTab(tool.ToolGroup, -1);
                        toolBox.AddTab(tbt);
                    }
                    tbi = new ToolBoxItem(tool.ToolName, imgnum++);
                    toolBox.SmallImageList.Images.Add(tool.ToolIcon);
                    tbt.AddItem(tbi);

                    tbi.Selected = false;
                    if (tool.ToolType == currentTool)
                    {
                        tbi.Selected = true;
                        tbtoselect = tbt;
                        tbitoselect = tbi;
                        tbi.Object = currentTool;
                        NotifyToolActivated((Type)tbi.Object);
                    }
                    else

                        tbi.Object = tool.ToolType;
                }
            }
            ResumeLayout(false);
            if (tbtoselect != null)
            {
                tbtoselect.Selected = true;
                if (tbitoselect != null)
                {
                    tbitoselect.Selected = true;
                    tbtoselect.SelectedItem = tbitoselect;
                }
            }
            //SetCurrentTool( currentTool);
        }

        void ToolChanged(object sender, EventArgs e)
        {
            if (sender is ToolBoxTab)
            {
                ToolBoxTab tbt = (ToolBoxTab)sender;
                if (tbt.SelectedItem != null)
                    NotifyToolActivated((Type)tbt.SelectedItem.Object);
                //tbt.SelectedItem = null;
            }
            else
            {
                ToolBoxItem tbi = (ToolBoxItem)sender;
                if (tbi.Selected)
                {
                    NotifyToolActivated((Type)tbi.Object);
                }
                //MessageBox.Show(sender.ToString());
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
            toolBox = new ToolBox();

            toolBox.BackColor = System.Drawing.SystemColors.Control;
            toolBox.Dock = System.Windows.Forms.DockStyle.Fill;
            toolBox.TabHeight = 18;
            toolBox.ItemHeight = 20;
            toolBox.ItemSpacing = 1;

            toolBox.ItemHoverColor = System.Drawing.Color.BurlyWood;
            toolBox.ItemNormalColor = System.Drawing.SystemColors.Control;
            toolBox.ItemSelectedColor = System.Drawing.Color.Linen;

            toolBox.Name = "_toolBox";
            toolBox.ItemSelectionChanged += new ItemSelectionChangedHandler(ToolChanged);
            toolBox.TabSelectionChanged += new TabSelectionChangedHandler(ToolChanged);
            Controls.Add(toolBox);
        }
    }
}
