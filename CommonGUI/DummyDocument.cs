using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeSCADA.Common.Documents
{
    public class DummyDocument:FreeSCADA.Interfaces.IDocument
    {
        #region IDocument Members
        public DummyDocument(string name)
        {
            Name = name;
        }

        public object Content
        {
            get { throw new NotImplementedException(); }
        }

        public object Load(string schemaName)
        {
            throw new NotImplementedException();
        }

        public string Name
        {
            get;
            protected set;
        }

        public void Save(string name,object content)
        {
            throw new NotImplementedException();
        }

        public Common.ProjectEntityType Type
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IDocument Members


        public void Save(string name)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
