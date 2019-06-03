using System;
using System.Windows.Input;

namespace AudioRecorder.Utils
{
    class RelayCommand : ICommand
    {
        private Action execute;
        private Func<bool> canExecute;
        private bool isEnabled;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object args)
        {
            return this.canExecute == null || this.canExecute();
        }
        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                }
            }
        }
        public void Execute(object parameter)
        {
            this.execute();
        }
    }
}
