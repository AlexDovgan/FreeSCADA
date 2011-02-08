using System.Collections.Generic;

namespace FreeSCADA.Interfaces
{
    public static class PredefinedContexts
    {
        static public string GlobalMenu
        {
            get { return "GlobalMenu"; }
        }
        static public string GlobalToolbar
        {
            get { return "GlobalToolbar"; }
        }
        static public string Communication
        {
            get { return "Communication"; }
        }
        static public string VisualControls
        {
            get { return "VisualControls"; }
        }
        static public string Project
        {
            get{return "Project";}
        }
    }
    public interface ICommands
    {
        //void RemoveCommand(ICommand cmd);

        ICommandContext GetContext(string contextName);
        ICommands RegisterContext(string contextName, ICommandContext context);
        ICommand FindCommandByName(string contextName,string name);
        List<string> GetRegisteredContextes();
    }
    
}
