using System;
namespace FreeSCADA.Common
{
    public interface IUndoBuffer
    {
        void AddCommand(FreeSCADA.Common.IUndoCommand command);
        event EventHandler CanExecuteChanged;
        bool CanRedo();
        bool CanUndo();
        void RaiseCanExecuteChanged();
        void RedoCommand();
        void UndoCommand();
    }
}
