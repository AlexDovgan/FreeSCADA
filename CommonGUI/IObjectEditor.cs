using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeSCADA.Common
{
    public delegate void ObjectChanged(IUndoCommand cmd);
    public interface IObjectEditor
    {
        event ObjectChanged ObjectChangedEvent;
    }
}
