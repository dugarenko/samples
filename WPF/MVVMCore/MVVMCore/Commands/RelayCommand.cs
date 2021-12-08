using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MVVMCore.Commands
{
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Never)]
	public class RelayCommand<T> : NotifyPropertyChangedDispatcherObject, IOwnedCommand
    {
        [NonSerialized]
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;
        private EventHandler _canExecuteChanged;
        private bool _executingCommand;

        public RelayCommand(Action execute)
            : this((T obj) => execute(), null)
        { }

        public RelayCommand(Action<T> execute)
            : this(execute, null)
        { }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return CanExecute((T)parameter);
        }

        [DebuggerStepThrough]
        public bool CanExecute(T parameter)
        {
            if (_executingCommand)
            {
                return false;
            }

            if (_canExecute == null)
            {
                return true;
            }

            try
            {
                return _canExecute(parameter);
            }
            catch
            {
                return false;
            }
        }

        public void Execute(object parameter)
        {
            Execute((T)parameter);
        }

        public void Execute(T parameter)
        {
            try
            {
                IsExecuting = true;
                _execute(parameter);
            }
            finally
            {
                IsExecuting = false;
            }
        }

        async public Task ExecuteAsync(T parameter)
        {
            try
            {
                IsExecuting = true;
                await Task.Run(() => _execute(parameter));
            }
            finally
            {
                IsExecuting = false;
            }
        }

        public void RaiseCanExecuteChanged()
        {
            _canExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool IsExecuting
        {
            get
            {
                return _executingCommand;
            }
            private set
            {
                if (SetAndRaisePropertyChanged(ref _executingCommand, value, nameof(IsExecuting)))
                {
                    RaiseCanExecuteChanged();
                }
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                VerifyAccess();
                _canExecuteChanged += value;
            }
            remove
            {
                _canExecuteChanged -= value;
            }
        }
    }
}
