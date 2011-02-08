using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreeSCADA.Interfaces;
using FreeSCADA.Designer.Views;

namespace FreeSCADA.Designer
{
    internal class ToolboxContext :ICommandContext
    {
        ToolBoxView toolBox;
        public ToolboxContext(ToolBoxView view)
        {
            toolBox = view;
        }

        #region ICommandContext Members

        public void AddCommand(ICommand cmd)
        {
            toolBox.AddTool(cmd); 
        }

        public void RemoveCommand(ICommand cmd)
        {
            toolBox.DeleteTool(cmd);      
        }

        #endregion

        #region ICommandContext Members


        public List<ICommand> GetCommands()
        {
            return toolBox.GetTools();
        }

        #endregion
    }
}
