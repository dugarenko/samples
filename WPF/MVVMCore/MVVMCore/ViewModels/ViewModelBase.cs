using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace MVVMCore.ViewModels
{
    /// <summary>
    /// Reprezentuje bazową klasę modelu.
    /// </summary>
    [Serializable]
    public class ViewModelBase : DispatcherObject, INotifyPropertyChanged, INotifyDataErrorInfo, IDisposable
    {
        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();

        #region Private methods.

        private void RefreshErrors()
        {
            if (ErrorsChanged != null)
            {
                var properties = this.GetType().GetProperties();
                foreach (var p in properties)
                {
                    ErrorsChanged.Invoke(this, new DataErrorsChangedEventArgs(p.Name));
                }
            }
        }

        #endregion

        #region Protected methods.

        /// <summary>
        /// Wywołanie zdarzenia PropertyChanged.
        /// </summary>
        /// <param name="propertyName">Nazwa właściwości.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Wywołanie zdarzenia PropertyChanged.
        /// </summary>
        /// <typeparam name="T">Typ danych.</typeparam>
        /// <param name="property">Wyrażenie lambda.</param>
        protected void OnPropertyChanged<T>(Expression<Func<T>> property)
        {
            LambdaExpression lambdaExpression = property;
            MemberExpression memberExpression = (!(lambdaExpression.Body is UnaryExpression) ?
                (MemberExpression)lambdaExpression.Body : (MemberExpression)((UnaryExpression)lambdaExpression.Body).Operand);
            OnPropertyChanged(memberExpression.Member.Name);
        }

        protected void ValidateModel(bool refreshErrors)
        {
            _validationErrors.Clear();

            ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            ValidationContext validationContext = new ValidationContext(this, null, null);

            if (!Validator.TryValidateObject(this, validationContext, validationResults, true))
            {
                foreach (ValidationResult validationResult in validationResults)
                {
                    string property = validationResult.MemberNames.ElementAt(0);
                    if (_validationErrors.ContainsKey(property))
                    {
                        _validationErrors[property].Add(validationResult.ErrorMessage);
                    }
                    else
                    {
                        _validationErrors.Add(property, new List<string> { validationResult.ErrorMessage });
                    }
                }
            }

            if (refreshErrors)
            {
                RefreshErrors();
            }
        }

        protected void ValidateModelProperty(object value, string propertyName)
        {
            if (_validationErrors.ContainsKey(propertyName))
            {
                _validationErrors.Remove(propertyName);
            }

            ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            ValidationContext validationContext = new ValidationContext(this, null, null) { MemberName = propertyName };
            if (!Validator.TryValidateProperty(value, validationContext, validationResults))
            {
                _validationErrors.Add(propertyName, new List<string>());
                foreach (ValidationResult validationResult in validationResults)
                {
                    _validationErrors[propertyName].Add(validationResult.ErrorMessage);
                }
            }
        }

        #endregion

        #region Public properties.

        /// <summary>
        /// Pobiera lub ustawia wartość, która informuje czy zawartości okna została wyrenderowana.
        /// </summary>
        public bool IsContentRendered
        { get; protected set; }

        #endregion

        #region INotifyPropertyChanged members.

        /// <summary>
        /// Występuje, gdy zmienia się wartość właściwości.
        /// </summary>
        [field: NonSerialized()]
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region INotifyDataErrorInfo members.

        /// <summary>
        /// Występuje, gdy błędy sprawdzania poprawności uległy zmianie dla właściwości lub całej jednostki.
        /// </summary>
        [field: NonSerialized()]
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// Pobiera wartość wskazującą, czy jednostka ma błędy sprawdzania poprawności.
        /// </summary>
        public bool HasErrors
        {
            get
            {
                if (IsContentRendered)
                {
                    ValidateModel(false);
                }
                return (_validationErrors.Count > 0);
            }
        }

        /// <summary>
        /// Pobiera błędy sprawdzania poprawności dla określonej właściwości lub dla całej encji.
        /// </summary>
        /// <param name="propertyName">Nazwa właściwości, dla której należy pobrać błędy sprawdzania poprawności lub null, lub System.String.Empty, aby pobrać błędy na poziomie encji.</param>
        /// <returns>Błędy sprawdzania poprawności właściwości lub obiektu.</returns>
        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (!_validationErrors.ContainsKey(propertyName))
            {
                return null;
            }
            return _validationErrors[propertyName];
        }

        #endregion

        #region IDisposable members.

        [NonSerialized()]
        private bool _isDisposed = false;
        [NonSerialized()]
        private bool _isDisposing = false;

#if DOC_LANG_EN
        /// <summary>
        /// Destructor based on which the Finalize () method will be generated.
        /// </summary>
#else
        /// <summary>
        /// Destruktor na podstawie którego zostanie wygenerowana metoda Finalize().
        /// </summary>
#endif
        ~ViewModelBase()
        {
            Dispose(false);
        }

#if DOC_LANG_EN
        /// <summary>
        /// Cleaning method. Releases the managed resources.
        /// </summary>
#else
        /// <summary>
        /// Metoda sprzątająca. Zwalnia zasoby zarządzane.
        /// </summary>
#endif
        protected virtual void Cleaner()
        {
            _validationErrors.Clear();
            PropertyChanged = null;
            ErrorsChanged = null;
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

#if DOC_LANG_EN
        /// <summary>
        /// Release of resources occupied by the object and its effective destruction.
        /// </summary>
#else
        /// <summary>
        /// Zwolnienie zajmowanych zasobów przez obiekt i efektywne jego zniszczenie.
        /// </summary>
#endif
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

#if DOC_LANG_EN
        /// <summary>
        /// Gets a value indicating whether the control has been disposed of.
        /// </summary>
        /// <returns>true if the control has been disposed of; otherwise, false.</returns>
#else
        /// <summary>
        /// Pobiera wartość wskazującą, czy kontrolka została usunięta.
        /// </summary>
        /// <returns>true, jeśli kontrola została usunięta, w przeciwnym razie false.</returns>
#endif
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

#if DOC_LANG_EN
        /// <summary>
        /// Gets a value indicating whether the control is in the process of being disposed.
        /// </summary>
        /// <returns>true if the control is being disposed; otherwise false.</returns>
#else
        /// <summary>
        /// Pobiera wartość wskazującą, czy kontrolka jest w trakcie usuwania.
        /// </summary>
        /// <returns>true, jeśli kontrolka jest w trakcie usuwania, w przeciwnym razie false.</returns>
#endif
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
