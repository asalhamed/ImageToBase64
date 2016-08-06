using System;
using System.Windows.Input;

namespace ImgToBase64
{
    internal interface IDelegateCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }

    internal class DelegateCommand : IDelegateCommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        // Event required by ICommand
        public event EventHandler CanExecuteChanged;
        // Two constructors
        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public DelegateCommand(Action<object> execute)
        {
            _execute = execute;
            _canExecute = AlwaysCanExecute;
        }

        // Methods required by ICommand
        public void Execute(object param)
        {
            _execute(param);
        }
        public bool CanExecute(object param)
        {
            return _canExecute(param);
        }
        // Method required by IDelegateCommand
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        // Default CanExecute method
        private static bool AlwaysCanExecute(object param)
        {
            return true;
        }
    }
}
