using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
namespace FreeSCADA.Designer.SchemaEditor.PropertyGridTypeEditors
{
    public class OriginalPropertyAttribute:Attribute
    {
        public OriginalPropertyAttribute(Type tp,string name)
        {
            ObjectType = tp;
            PropertyName = name;

        }
        public Type ObjectType
        {
            get;
            protected set;
        }
        public string PropertyName
        {
            get;
            protected set;
        }
    }
}
