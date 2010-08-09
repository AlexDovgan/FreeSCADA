using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using Silver.UI;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Designer.Views
{
    /// <summary>
    /// Represents available tools and manipulators for active document window. Basically used for Schema editing.
    /// </summary>
    internal class ToolBoxView : ToolWindow
    {
        ToolBox _toolBox;
        public ToolBoxView()
        {
            TabText = "ToolBox";
            AutoHidePortion = 0.15;
            AddToolBox();

        }


        public void AddTool(ICommand cmd)
        {
            var command = cmd as SchemaEditor.SchemaCommands.ToolCommand;
            if (command == null)
                return;

            _toolBox.ItemSelectionChanged -= new ItemSelectionChangedHandler(ToolChanged);
            _toolBox.TabSelectionChanged -= new TabSelectionChangedHandler(ToolChanged);



            var imgnum = 0;
            ToolBoxTab tbt;
            if ((tbt = _toolBox[command.ToolGroup]) == null)
            {
                tbt = new ToolBoxTab(command.ToolGroup, -1);
                _toolBox.AddTab(tbt);
                tbt.Selected = false;
            }

            _toolBox.SmallImageList.Images.Add(command.ToolIcon);
            var tbi = new ToolBoxItem(command.ToolName, _toolBox.SmallImageList.Images.Count - 1);
            tbt.AddItem(tbi);
            tbi.Object = command;
            tbi.Selected = false;

            _toolBox.ItemSelectionChanged += new ItemSelectionChangedHandler(ToolChanged);
            _toolBox.TabSelectionChanged += new TabSelectionChangedHandler(ToolChanged);


            for (int i = 0; i < _toolBox.Tabs.Count; i++)
                for (int j = 0; j < _toolBox.Tabs[i].ItemCount; j++)
                    if (((SchemaEditor.SchemaCommands.ToolCommand)_toolBox.Tabs[i][j].Object).IsActive)
                    {
                        _toolBox.Tabs[i][j].Selected = true;
                        _toolBox.Tabs[i].Selected = true;
                        return;
                    }

        }
        public void DeleteTool(ICommand cmd)
        {
            var command = cmd as SchemaEditor.SchemaCommands.ToolCommand;
            _toolBox.ItemSelectionChanged -= new ItemSelectionChangedHandler(ToolChanged);
            _toolBox.TabSelectionChanged -= new TabSelectionChangedHandler(ToolChanged);

            var tbt = _toolBox[command.ToolGroup];
            for (var i = 0; i < tbt.ItemCount; i++)
                if (tbt[i].Object == cmd)
                {
                    tbt[i].Object = null;
                    tbt[i].Selected = false;
                    tbt.DeleteItem(tbt[i]);
                    break;
                }
            if (tbt.ItemCount == 0)
                _toolBox.DeleteTab(tbt);
            _toolBox.ItemSelectionChanged += new ItemSelectionChangedHandler(ToolChanged);
            _toolBox.TabSelectionChanged += new TabSelectionChangedHandler(ToolChanged);
        }

        void ToolChanged(object sender, EventArgs e)
        {

            if (sender is ToolBoxTab)
            {
                var tbt = (ToolBoxTab)sender;
                if (tbt.SelectedItem != null && tbt.SelectedItem.Object != null)
                    ((ICommand)tbt.SelectedItem.Object).Execute();
            }
            else if (sender is ToolBoxItem)
            {
                var tbi = (ToolBoxItem)sender;
                if (tbi.Selected && tbi.Object != null)
                    ((ICommand)tbi.Object).Execute();

            }
        }


        void AddToolBox()
        {
            _toolBox = new ToolBox
                           {
                               BackColor = System.Drawing.SystemColors.Control,
                               Dock = System.Windows.Forms.DockStyle.Fill,
                               TabHeight = 18,
                               ItemHeight = 20,
                               ItemSpacing = 1,
                               ItemHoverColor = System.Drawing.Color.BurlyWood,
                               ItemNormalColor = System.Drawing.SystemColors.Control,
                               ItemSelectedColor = System.Drawing.Color.Linen,
                               Name = "_toolBox"
                           };

            _toolBox.ItemSelectionChanged += new ItemSelectionChangedHandler(ToolChanged);
            _toolBox.TabSelectionChanged += new TabSelectionChangedHandler(ToolChanged);
            _toolBox.DeleteAllTabs();
            _toolBox.SmallImageList = new ImageList();
            Controls.Add(_toolBox);
        }
    }
}
