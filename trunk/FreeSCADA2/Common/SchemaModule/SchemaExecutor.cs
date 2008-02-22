using System;
using System.Collections.Generic;
using System.Text;
using FreeSCADA.Schema;
namespace FreeSCADA.Schema
{
    class SchemaExecutor:SchemaViewer
    {
        public SchemaExecutor(SchemaDocument schema)
            : base(schema)
        {
        }
    }
}
