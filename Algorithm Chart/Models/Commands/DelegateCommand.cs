using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Algorithm_Chart.Models.Commands
{
    /// <summary>
    /// An implementation of ICommand that allows the ViewModel to pass its methods
    /// to the command
    /// </summary>
    public class DelegateCommand : ICommand
{
    /// <summary>
    /// The delegate to call to see if the command can executed
    /// </summary>
    private readonly Func<bool> _CanExecute;

    /// <summary>
    /// The delegate to call to when the command is executed
    /// </summary>
    private readonly Action _Execute;

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
    private List<EventHandler> _CanExecuteChanged = new List<EventHandler>();

    /// <summary>
    /// Instantiates an instance of DelegateCommand, the command can always 
    /// be run
    /// </summary>
    /// <param name="execute">Defines the method to be called when the 
    /// command is invoked</param>
    public DelegateCommand(Action execute)
            : this(execute, null)
    {
    }

    /// <summary>
    /// Instantiates an instance of DelegateCommand
    /// </summary>
    /// <param name="execute">Defines the method to be called when the 
    /// command is invoked</param>
    /// <param name="canExecute">Defines the method that determines whether 
    /// the command can execute in its current state.</param>
    public DelegateCommand(Action execute,
            Func<bool> canExecute)
    {
        _Execute = execute;
        _CanExecute = canExecute;
    }

    /// <summary>
    /// Defines the method that determines whether the command can execute 
    /// in its current state.
    /// </summary>
    /// <param name="parameter">Data used by the command. If the command 
    /// does not require data to be passed, this object can be set to null.</param>
    /// <returns>true if this command can be executed; otherwise, false.</returns>
    public bool CanExecute(object parameter)
    {
        if (_CanExecute == null)
        {
            return true;
        }

        return _CanExecute();
    }

    /// <summary>
    /// Defines the method to be called when the command is invoked.
    /// </summary>
    /// <param name="parameter">Data used by the command. If the command 
    /// does not require data to be passed, this object can be set to null.</param>
    public void Execute(object parameter)
    {
        _Execute();
    }
}

/// <summary>
/// A generic implementation of ICommand that allows the ViewModel to pass its methods
/// to the command
/// </summary>
public class DelegateCommand<T> : ICommand
{
    /// <summary>
    /// The delegate to call to see if the command can executed
    /// </summary>
    private readonly Predicate<T> _CanExecute;

    /// <summary>
    /// The delegate to call to when the command is executed
    /// </summary>
    private readonly Action<T> _Execute;

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
    private List<EventHandler> _CanExecuteChanged = new List<EventHandler>();

    /// <summary>
    /// Instantiates an instance of DelegateCommand, the command can always 
    /// be run
    /// </summary>
    /// <param name="execute">Defines the method to be called when the 
    /// command is invoked</param>
    public DelegateCommand(Action<T> execute)
        : this(execute, null)
    {
        var type = typeof(T);
        if ((!type.IsGenericType ||
                !type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                && !type.IsClass)
        {
            // enforced here as can't do in where on class type
            // have to enforce as ICommand takes an object, and having a value type 
            // for T would cause unexpected behavior when CanExecute(null) 
            // is called during XAML initialization for command bindings
            throw new ArgumentException("The DelegateCommand doesn't support value types.");
        }
    }

    /// <summary>
    /// Instantiates an instance of DelegateCommand
    /// </summary>
    /// <param name="execute">Defines the method to be called when the 
    /// command is invoked</param>
    /// <param name="canExecute">Defines the method that determines whether 
    /// the command can execute in its current state.</param>
    public DelegateCommand(Action<T> execute,
            Predicate<T> canExecute)
    {
        _Execute = execute;
        _CanExecute = canExecute;
    }

    /// <summary>
    /// Defines the method that determines whether the command can execute 
    /// in its current state.
    /// </summary>
    /// <param name="parameter">Data used by the command. If the command 
    /// does not require data to be passed, this object can be set to null.</param>
    /// <returns>true if this command can be executed; otherwise, false.</returns>
    public bool CanExecute(object parameter)
    {
        if (_CanExecute == null)
        {
            return true;
        }

        return _CanExecute((T)parameter);
    }

    /// <summary>
    /// Defines the method to be called when the command is invoked.
    /// </summary>
    /// <param name="parameter">Data used by the command. If the command 
    /// does not require data to be passed, this object can be set to null.</param>
    public void Execute(object parameter)
    {
        _Execute((T)parameter);
    }
}
}
