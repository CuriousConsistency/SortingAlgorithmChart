using System;
using System.Windows.Input;
using System.Collections.Generic;

namespace Algorithm_Chart.Commands
{
    public class GenerateNewDataSetRelayCommand : ICommand
    {
        private readonly Func<bool> _canExecute;
        private readonly Action _execute;
        private event EventHandler CanExecuteChangedInternal;

        public GenerateNewDataSetRelayCommand(Func<bool> canExecute, Action execute)
        {
            this._canExecute = canExecute;
            this._execute = execute;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command 
        /// should execute
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                this._CanExecuteChanged.Add(value);
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                this._CanExecuteChanged.Remove(value);
                CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// Used to hang on to strong references to the event handlers, as
        /// CommandManager.RequerySuggested is static and only holds weak references
        /// (otherwise can get inappropriately garbage collected)
        /// </summary>
        private List<EventHandler> _CanExecuteChanged = new List<EventHandler>();

        public bool CanExecute(object parameter)
        {
            return this._canExecute();
        }

        public void Execute(object parameter)
        {
            this._execute();
        }

        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            if (handler != null)
            {
                handler.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
