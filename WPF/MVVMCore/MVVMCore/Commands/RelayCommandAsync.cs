using MVVMCore.Threading.Tasks;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MVVMCore.Commands
{
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Never)]
	public class RelayCommandAsync<T> : RelayCommand<T>
    {
        private static Action<T> WrapExecute(Func<T, Task> execute, bool ignoreCancellation)
        {
            if (ignoreCancellation)
            {
                return (T param) => TaskCancellationHelper.FireAndForget(execute, param);
            }
            return (T param) => execute(param);
        }

        public RelayCommandAsync(Func<Task> execute)
            : this((T param) => execute(), null, true)
        { }

        public RelayCommandAsync(Func<Task> execute, bool ignoreCancellation = true)
            : this((T param) => execute(), null, ignoreCancellation)
        { }

        public RelayCommandAsync(Func<T, Task> execute, Predicate<T> canExecute)
            : base(WrapExecute(execute, true), canExecute)
        { }

        public RelayCommandAsync(Func<T, Task> execute, Predicate<T> canExecute = null, bool ignoreCancellation = true)
            : base(WrapExecute(execute, ignoreCancellation), canExecute)
        { }
    }
}
