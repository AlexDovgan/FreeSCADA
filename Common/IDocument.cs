using System;

namespace FreeSCADA.Interfaces
{
    public interface IDocument
    {
        object Content { get; }
        object Load(string schemaName);
        string Name { get; }
        void Save(string name,object content);
        void Save(string name);
        FreeSCADA.Common.ProjectEntityType Type { get; }
    }
}
