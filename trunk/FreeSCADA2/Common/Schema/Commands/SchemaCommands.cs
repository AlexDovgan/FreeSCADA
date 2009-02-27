using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Collections.Generic;
using System.Text;

namespace FreeSCADA.Common.Schema.Commands
{
    public class SchemaOpenCommand : ICommand
    {
        
        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
