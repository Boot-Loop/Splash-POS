using System;
using System.Windows.Input;

namespace UI.ViewModels.Commands
{
    public class RelayCommand : ICommand
    {
        private Action<object> _execute;
        private Func<object, bool> _can_ececute;

        public RelayCommand(Action<object> execute) : this(execute, null) {

        }

        public RelayCommand(Action<object> execute, Func<object, bool> can_execute) {
            if (execute == null) throw new ArgumentNullException("execute argument is must");
            _execute = execute;
            _can_ececute = can_execute;
        }
        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) {
            return _can_ececute == null ? true : _can_ececute((object)parameter);
        }

        public void Execute(object parameter) {
            _execute((object)parameter);
        }
    }
}
