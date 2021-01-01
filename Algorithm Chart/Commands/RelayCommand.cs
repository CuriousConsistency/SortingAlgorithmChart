using System;
using System.Windows.Input;
using System.Collections.Generic;

namespace Sorting_Algorithm_Chart.Commands
{
    public class RelayCommand : ICommand
    {
        private Func<bool> _canExecute;
        private Action<object> _execute;
        private event EventHandler CanExecuteChangedInternal;

        public RelayCommand(Func<bool> canExecute, Action<object> execute)
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
                _CanExecuteChanged.Add(value);
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                _CanExecuteChanged.Remove(value);
                CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// Used to hang on to strong references to the event handlers, as
        /// CommandManager.RequerySuggested is static and only holds weak references
        /// (otherwise can get inappropriately garbage collected)
        /// </summary>
        private readonly List<EventHandler> _CanExecuteChanged = new List<EventHandler>();

        public bool CanExecute(object parameter)
        {
            return this._canExecute();
        }

        public void Execute(object parameter)
        {
            if (this.CanExecute(null))
            {
                this._execute(parameter);
            }
        }

        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            if (handler != null)
            {
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        public void Destroy()
        {
            this._canExecute = null;
            this._execute = _ => { return; };
        }
    }
}
