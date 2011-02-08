using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreeSCADA.CommonUI.Interfaces;

namespace FreeSCADA.Designer
{
    public class BaseUndoBuffer : IUndoBuffer
    {
        IDocumentView view;
        bool documentModifiedState;
        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }


        public BaseUndoBuffer(IDocumentView doc)
        {
            view = doc;
        }
        public void AddCommand(IUndoCommand command)
        {
            redoStack.Clear();
            command.Do(view);
            documentModifiedState = view.IsModified;
            view.IsModified = true;

            undoStack.Push(command);
            RaiseCanExecuteChanged();
        }

        public void UndoCommand()
        {
            if (undoStack.Count == 0)
                return;
            IUndoCommand cmd = undoStack.Pop();

            try
            {
                cmd.Undo();
                view.IsModified = documentModifiedState;

            }
            finally
            {
                redoStack.Push(cmd);
                RaiseCanExecuteChanged();

            }
        }


        public void RedoCommand()
        {
            if (redoStack.Count == 0)
                return;
            IUndoCommand cmd = redoStack.Pop();
            try
            {
                cmd.Redo();
                //if (!CanRedo()) (Env.Current.MainWindow as MainForm).redoButton.Enabled = false;
                documentModifiedState = view.IsModified;
                view.IsModified = true;

            }
            finally
            {
                undoStack.Push(cmd);
                RaiseCanExecuteChanged();
                //(Env.Current.MainWindow as MainForm).undoButton.Enabled = true;
            }
        }

        public bool CanUndo()
        {
            return undoStack.Count > 0;
        }

        public bool CanRedo()
        {
            return redoStack.Count > 0;
        }

        private Stack<IUndoCommand> undoStack = new Stack<IUndoCommand>();
        private Stack<IUndoCommand> redoStack = new Stack<IUndoCommand>();
    }
}
