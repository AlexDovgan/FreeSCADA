using System;
using System.Collections.Generic;
using FreeSCADA.Interfaces;
using FreeSCADA.Common;

namespace FreeSCADA.CommonUI.Interfaces
{
    public struct CommandInfo
    {
        public ICommand command;
        public IEnumerable<String> defaultContextes;

        public CommandInfo(ICommand command)
        {
            this.command = command;
            List<String> l= new List<String>();
            l.Add("DocumentContext");
            this.defaultContextes=l;
            
        }

        public CommandInfo(ICommand command, IEnumerable<String> contextes)
        {
            this.command = command;
            this.defaultContextes = contextes;
        }
    }
    public interface IDocumentView
    {
        ITool ActiveTool 
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
        IUndoBuffer UndoBuff 
        { get; }
        FreeSCADA.Common.Gestures.MapZoom ZoomManager 
        { get; }
    }
}
