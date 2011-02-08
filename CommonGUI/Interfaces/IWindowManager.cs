using System;

namespace FreeSCADA.CommonUI.Interfaces
{
    public interface IWindowManager
    {
        bool Close();
        void ActivateDocument(IDocumentView view);
        void ForceWindowsClose();
        void SaveAllDocuments();
        void SaveDocument();
        void Refresh();

    }
}
