using System;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Common
{
    public struct CommandInfo
    {
        public ICommand command;
        public ICommandContext defaultContext;

        public CommandInfo(ICommand command)
        {
            this.command = command;
            this.defaultContext = null;
        }

        public CommandInfo(ICommand command, ICommandContext defaultContext)
        {
            this.command = command;
            this.defaultContext = defaultContext;
        }
    }
    public interface IDocumentView
    {
        FreeSCADA.Common.BaseTool ActiveTool 
        { get; set; }
        System.Collections.Generic.List<CommandInfo> DocumentCommands 
        { get; }
        string DocumentName 
        { get; set; }
        bool HandleModifiedOnClose 
        { get; set; }
        bool IsModified 
        { get; set; }
        event EventHandler IsModifiedChanged;
        System.Windows.Controls.Panel MainPanel 
        { get; }
        void OnActivated();
        void OnDeactivated();
        void OnPropertiesBrowserChanged(object el);
        void OnToolActivated(object sender, Type tool);
        bool SaveDocument();
        ISelectionManager SelectionManager 
        { get; }
        FreeSCADA.Common.IUndoBuffer UndoBuff 
        { get; }
        FreeSCADA.Common.Gestures.MapZoom ZoomManager 
        { get; }
    }
}
