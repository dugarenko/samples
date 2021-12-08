using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MVVMCore.Commands
{
    [Serializable]
	public abstract class NotifyPropertyChangedDispatcherObject : NotifyPropertyChangedDependencyProperty, INotifyPropertyChanged
    {
        [NonSerialized]
        private Dispatcher _dispatcher = null;

        [field: NonSerialized()]
        public event PropertyChangedEventHandler PropertyChanged;

        protected NotifyPropertyChangedDispatcherObject()
            : this(Dispatcher.CurrentDispatcher)
        { }

        protected NotifyPropertyChangedDispatcherObject(Dispatcher dispatcher)
        {
            Dispatcher = dispatcher ?? throw new ArgumentNullException("dispatcher");
        }

        protected override void RaisePropertyChanged<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, params string[] propertiesToExclude) //where TValue : IEnumerable<string>
        {
            TValue value;
            if (dictionary.TryGetValue(key, out value))
            {
                foreach (string propertyName in value.Except(propertiesToExclude))
                {
                    RaisePropertyChanged(propertyName);
                }
            }
        }

        protected override void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            VerifyAccess();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            base.RaisePropertyChanged(propertyName);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected bool SetAndRaisePropertyChanged<T>(ref T propertyDataField, T value, [CallerMemberName] string propertyName = null)
        {
            bool flag;
            IEquatable<T> equatable = value as IEquatable<T>;
            if (equatable == null)
            {
                flag = (!typeof(T).IsSubclassOf(typeof(Enum)) ? object.Equals(value, propertyDataField) : object.Equals(value, propertyDataField));
            }
            else
            {
                flag = equatable.Equals(propertyDataField);
            }

            if (!flag)
            {
                propertyDataField = value;
                RaisePropertyChanged(propertyName);
            }
            return !flag;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected bool SetAndRaisePropertyChanged(ref string propertyDataField, string value, [CallerMemberName] string propertyName = null)
        {
            if (string.Equals(propertyDataField, value, StringComparison.Ordinal))
            {
                return false;
            }
            propertyDataField = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected bool SetAndRaisePropertyChanged(ref int propertyDataField, int value, [CallerMemberName] string propertyName = null)
        {
            if (propertyDataField == value)
            {
                return false;
            }
            propertyDataField = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected bool SetAndRaisePropertyChanged(ref bool propertyDataField, bool value, [CallerMemberName] string propertyName = null)
        {
            if (propertyDataField == value)
            {
                return false;
            }
            propertyDataField = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected void VerifyAccess()
        {
            Dispatcher.VerifyAccess();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool CheckAccess()
        {
            return Dispatcher.CheckAccess();
        }

        public void CheckAccessInvoke(Action action)
        {
            ArgumentValidation.NotNull(action, "action");
            if (CheckAccess())
            {
                action();
                return;
            }
            Dispatcher.Invoke(action, DispatcherPriority.Normal);
        }

        public T CheckAccessInvoke<T>(Func<T> func)
        {
            ArgumentValidation.NotNull(func, "action");
            if (CheckAccess())
            {
                return func();
            }
            return Dispatcher.Invoke(func, DispatcherPriority.Normal);
        }

        public async Task CheckAccessInvokeAsync(Action action)
        {
            ArgumentValidation.NotNull(action, "action");
            if (CheckAccess())
            {
                action();
            }
            else
            {
                await Dispatcher.InvokeAsync(action, DispatcherPriority.Normal);
            }
        }

        public async Task<T> CheckAccessInvokeAsync<T>(Func<T> func)
        {
            ArgumentValidation.NotNull(func, "action");
            if (CheckAccess())
            {
                return func();
            }
            else
            {
                return await Dispatcher.InvokeAsync(func, DispatcherPriority.Normal);
            }
        }

        public Dispatcher Dispatcher
        {
            get => _dispatcher;
            private set
            {
                _dispatcher = value;
            }
        }
    }
}
