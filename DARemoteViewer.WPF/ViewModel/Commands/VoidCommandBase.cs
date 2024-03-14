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

        private Action ExecuteMethod;
        private Func<bool> canExecute;
        public VoidCommandBase(Action ToExecute, Func<bool> canExecute)
        {
            ExecuteMethod = ToExecute;
            this.canExecute = canExecute;
        }
        public VoidCommandBase(Action ToExecute)
            : this(ToExecute, null)
        {
        }

        public bool CanExecute(object parameter)
        {
            if (this.canExecute == null)
            {
                return true;
            }
            else
            {
                bool result = this.canExecute.Invoke();
                return result;
            }
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
