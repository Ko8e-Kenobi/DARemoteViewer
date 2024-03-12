using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DARemoteViewer.WPF.ViewModel.Commands
{
    public class VoidCommandBase : ICommand
    {
        public delegate void SomeVoidMethod();
        SomeVoidMethod ExecuteMethod;
        bool canExecute;
        public VoidCommandBase(SomeVoidMethod ToExecute, bool canExecute)
        {
            ExecuteMethod = ToExecute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            ExecuteMethod();
        }
    }
}
