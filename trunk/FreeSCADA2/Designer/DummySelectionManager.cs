using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreeSCADA.Common;

namespace FreeSCADA.Designer
{
    class DummySelectionManager: ISelectionManager
    {

        #region ISelectionManager Members

        public void AddObject(object el)
        {
            throw new NotImplementedException();
        }

        public void DeleteObject(object el)
        {
            throw new NotImplementedException();
        }

        public List<object> SelectedObjects
        {
            get { return  new List<object>(); }
        }

        public event ObjectSelectedDelegate SelectionChanged;

        public void SelectObject(object el)
        {
         
        }

        public void UpdateManipulator()
        {
            throw new NotImplementedException();
        }

        public Type ManipulatorType
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        
        #endregion
    }
}
