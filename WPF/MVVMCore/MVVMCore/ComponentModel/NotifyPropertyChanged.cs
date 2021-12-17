using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace MVVMCore.ComponentModel
{
    /// <summary>
    /// Reprezentuje klasę powiadomień.
    /// </summary>
    [Serializable]
    public class NotifyPropertyChanged : INotifyPropertyChanged, IDisposable
    {
        [NonSerialized()]
        private Dispatcher _dispatcher = null;

        /// <summary>
        /// Reprezentuje metodę wywołania zdarzenia PropertyChanged.
        /// </summary>
        [field: NonSerialized()]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Wywołuje zdarzenie PropertyChanged.
        /// </summary>
        /// <param name="propertyName">Nazwa właściwości.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Wywołuje zdarzenie PropertyChanged.
        /// </summary>
        /// <param name="property">Wyrażenie lambda.</param>
        protected void OnPropertyChanged<T>(Expression<Func<T>> property)
        {
            LambdaExpression lambdaExpression = property;
            MemberExpression memberExpression = (!(lambdaExpression.Body is UnaryExpression) ?
                (MemberExpression)lambdaExpression.Body : (MemberExpression)((UnaryExpression)lambdaExpression.Body).Operand);
            OnPropertyChanged(memberExpression.Member.Name);
        }

        /// <summary>
        /// Pobiera System.Windows.Threading.Dispatcher, z którym jest skojarzony z System.Windows.Threading.DispatcherObject.
        /// </summary>
        protected Dispatcher Dispatcher
        {
            get
            {
                if (_dispatcher == null)
                {
                    _dispatcher = Application.Current.Dispatcher;
                }
                return _dispatcher;
            }
        }

        #region IDisposable members.

        [NonSerialized()]
        private bool _isDisposed = false;
        [NonSerialized()]
        private bool _isDisposing = false;

        /// <summary>
        /// Destruktor na podstawie którego zostanie wygenerowana metoda Finalize().
        /// </summary>
        ~NotifyPropertyChanged()
        {
            Dispose(false);
        }

        /// <summary>
        /// Metoda sprzątająca. Zwalnia zasoby zarządzane.
        /// </summary>
        protected virtual void Cleaner()
        {
            PropertyChanged = null;
        }

        /// <summary>
        /// Utylizacja wykonuje się w dwóch różnych scenariuszach. Jeśli parametr 'disposing'
        /// równa się (true) to metoda została wywołana bezpośrednio lub pośrednio przez kod
        /// użytkownika. W tym przypadku zarządzane i niezarządzane zasoby zostaną zniszczone.
        /// Jeśli parametr 'disposing' równa się (false) metoda została wywołana przez
        /// środowisko wykonawcze (runtime) z wnętrza finalizatora i nie należy odwoływać się
        /// do innych obiektów tylko zniszczyć zasoby niezarządzane.
        /// </summary>
        /// <param name="disposing">Wywołano bezpośrednio (true) lub pośrednio (false).</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!IsDisposed)
                {
                    IsDisposing = true;
                    Cleaner();
                    IsDisposing = false;
                    IsDisposed = true;
                }
            }
        }

        /// <summary>
        /// Zwolnienie zajmowanych zasobów przez obiekt i efektywne jego zniszczenie.
        /// </summary>
        public void Dispose()
        {
            // Wywołanie utylizacji z poziomu kodu użytkownika.
            Dispose(true);

            // Ten obiekt zostanie zniszczony przez metodę 'Dispose'.
            // W związku z tym, należy powiadomić (Garbage Collector)
            // GC.SupressFinalize aby wyłączył ten obiekt z kolejki
            // finalizacji. Skutek będzie taki, że kod finalizacji
            // nie zostanie wykonany po raz drugi.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Pobiera wartość wskazującą, czy obiekt został usunięty.
        /// </summary>
        /// <returns>true, jeśli obiekt został usunięty, w przeciwnym razie false.</returns>
        public bool IsDisposed
        {
            get
            {
                return _isDisposed;
            }
            private set
            {
                _isDisposed = value;
            }
        }

        /// <summary>
        /// Pobiera wartość wskazującą, czy obiekt jest w trakcie usuwania.
        /// </summary>
        /// <returns>true, jeśli obiekt jest w trakcie usuwania, w przeciwnym razie false.</returns>
        public bool IsDisposing
        {
            get
            {
                return _isDisposing;
            }
            private set
            {
                _isDisposing = value;
            }
        }

        #endregion
    }
}
