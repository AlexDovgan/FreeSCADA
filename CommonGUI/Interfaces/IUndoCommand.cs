using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeSCADA.CommonUI.Interfaces
{
    public interface IUndoCommand
    {
        /// <summary>
        /// exexute command 
        /// exexuted when comman add to undo redo buffer
        /// </summary>
        void Do(IDocumentView doc);
        /// <summary>
        /// executed when undo redo buffer execute redo for document
        /// </summary>
        void Redo();
        /// <summary>
        /// executed when undo redo buffer execute undo for document
        /// </summary>
        void Undo();
    }
}
