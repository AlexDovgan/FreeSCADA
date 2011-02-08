using System;


namespace FreeSCADA.CommonUI.Interfaces
{
    public interface IUndoBuffer
    {
        void AddCommand(IUndoCommand command);
        event EventHandler CanExecuteChanged;
        bool CanRedo();
        bool CanUndo();
        void RaiseCanExecuteChanged();
        void RedoCommand();
        void UndoCommand();
    }
}
