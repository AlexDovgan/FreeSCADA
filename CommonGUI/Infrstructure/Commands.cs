using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Common
{
    public class Commands : ICommands
    {

        Dictionary<string, ICommandContext> contextes;
        #region ICommands implementation

        public Commands(MenuStrip menu,ToolStrip toolbar )
        {   
            contextes = new Dictionary<string, ICommandContext>();
			if (menu != null && toolbar != null)
			{
				contextes[PredefinedContexts.GlobalMenu] = new MenuCommandContext(menu);
                contextes[PredefinedContexts.GlobalToolbar] = new ToolbarCommandContext(toolbar);
                ToolStrip communicationMenu = GetGroupItem(menu,StringResources.CommunicationCommandGroupName);

                contextes[PredefinedContexts.Communication] = new MenuCommandContext(communicationMenu);

				//ToolStrip visualControlsMenu = GetGroupItem(menu, "User Controls");
				//visualControlsContext = new BaseCommandContext(visualControlsMenu, null);

				ToolStrip projectMenu = GetGroupItem(menu, StringResources.ProjectCommandGroupName);
				contextes[PredefinedContexts.Project]= new MenuCommandContext(projectMenu);
			}
        }

        public List<string> GetRegisteredContextes()
        {
            return contextes.Keys.ToList();
        }
        public ICommands RegisterContext(string contextName, ICommandContext context)
        {
            if (contextes.ContainsKey(contextName))
                throw new Exception("Context is already exists");
            contextes[contextName] = context;
            return this;
        }
        public ICommand FindCommandByName(string contextName, string name)
        {
            if (contextes.ContainsKey(contextName))
                return contextes[contextName].GetCommands().First(c => c.Name == name);
            return null;

        }

        public ICommandContext GetContext(string contextName)
        {
            if (contextes.ContainsKey(contextName))
                return contextes[contextName];
            return null;
        }
        #endregion
        
		private ToolStrip GetGroupItem(ToolStrip root, string name)
		{
			foreach (ToolStripItem item in root.Items)
			{
				ToolStripMenuItem tmp = (ToolStripMenuItem)item;
				if (name == item.Text && tmp != null)
					return tmp.DropDown;
			}
			ToolStripMenuItem newItem = new ToolStripMenuItem(name);
			root.Items.Add(newItem);
			return newItem.DropDown;
		}
        

    }
             
}
