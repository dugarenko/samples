using MVVMCore.Reflection;
using MVVMCore.Commands;
using MVVMCore.Properties;
using MVVMCore.ViewModels;
using MVVMCore.Windows;
using System;
using System.Data.Linq;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using MVVMCore.Data.SqlClient;
using MVVMCore.Text;
using MVVMCore.Windows.Controls;

namespace MVVMCore
{
    /// <summary>
    /// Reprezentuje bazową klasę modelu aplikacji.
    /// </summary>
    [Serializable]
    public class AppViewModel : ViewModelBase
    {
        private bool? _isAdministrator = null;
        private bool _isBusy = false;
        private Guid[] _tokensBusy = null;
        private object _syncBusy = new object();
        //
        private object _syncCursor = new object();
        private Guid _cursorToken = Guid.Empty;

        #region Protected methods.

        /// <summary>
        /// Rozwiązuje konflikty współbieżności danych.
        /// </summary>
        /// <param name="dataContext">Kontekst danych.</param>
        /// <returns>Odpowiedź użytkownika.</returns>
        protected MessageBoxResult ResolveConflicts(DataContext dataContext)
        {
            // W międzyczasie ustawienia zostały zmienione przez innego użytkownika.
            // Czy chcesz kontynuować zapis i nadpisać dane wprowadzone przez innego użytkownika?
            // Kliknij TAK i nadpisz dane lub kliknij NIE, wówczas dane na formularzu zostaną przeładowane.
            var result = MessageBox.Show(Resources.Msg_Warning_DataConcurrency, Title,
                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

            try
            {
                if (result == MessageBoxResult.Yes)
                {
                    foreach (var occ in dataContext.ChangeConflicts)
                    {
                        // Bieżące wartości zastępują wartości w bazie danych.
                        occ.Resolve(RefreshMode.KeepCurrentValues);
                    }

                    // Ponowne wykonanie polecenia.
                    dataContext.SubmitChanges();
                }
                else if (result == MessageBoxResult.No)
                {
                    foreach (var occ in dataContext.ChangeConflicts)
                    {
                        // Wszystkie wartości bazy danych zastępują bieżące wartości.
                        occ.Resolve(RefreshMode.OverwriteCurrentValues);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionFormater.AppendMethod(ex, MethodBase.GetCurrentMethod()), Title);
            }

            return result;

            // ===============================================================
            // Nie używać, przy scaleniu powstaje zmiksowany zapis danych!
            // ===============================================================
            // Scalenie! Zachowuje bieżące wartości, które zostały zmienione,
            // ale aktualizuje inne wartości przy użyciu wartości bazy danych.
            // ===============================================================
            //foreach (var occ in dataContext.ChangeConflicts)
            //{
            //    occ.Resolve(RefreshMode.KeepChanges);
            //}
            //// Ponowne wykonanie polecenia.
            //dataContext.SubmitChanges();
            // ===============================================================
        }

        protected void MoveValues<T>(T source, T destination) //where T: IIdColumn
        {
            // ====================================================================================
            // Uwaga! poniższy algorytm wyklucza aktualizację kolumn, które należą do klucza obcego
            // ponieważ te kolumny można aktualizować tylko jeśli powiązanie jeszcze nie istnieje.
            // ------------------------------------------------------------------------------------
            //if ((this._IdKraj != value))
            //{
            //    if (this._Kraj.HasLoadedOrAssignedValue)
            //    {
            //        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
            //    }
            //    this.OnIdKrajChanging(value);
            //    this.SendPropertyChanging();
            //    this._IdKraj = value;
            //    this.SendPropertyChanged("IdKraj");
            //    this.OnIdKrajChanged();
            //}
            // =================================================================================================================

            // Nazwy kolumn kluczy obcych do wykluczenia.
            var foreignKeys = typeof(T)
                                        .GetProperties()
                                        .SelectMany(x =>
                                                    x.CustomAttributes.Where(ca =>
                                                                                    // Szuka powiązania referencyjne.
                                                                                    ca.AttributeType == typeof(System.Data.Linq.Mapping.AssociationAttribute) ||
                                                                                    // Szuka kolumny, które należą do klucza obcego np: IdKraj, IdWojewodztwo...
                                                                                    ca.NamedArguments.Any(na => na.MemberName == "IsForeignKey" && (bool)na.TypedValue.Value == true))
                                                )
                                        .SelectMany(x =>
                                                    x.NamedArguments.Where(na => na.MemberName == "ThisKey"))
                                        .Select(x => (string)x.TypedValue.Value)
                                        .ToList();

            var properties = typeof(T)
                                        .GetProperties()
                                        .Where(x =>
                                                    x.CanRead &&
                                                    x.CanWrite &&
                                                    !x.CustomAttributes.Any(ca =>
                                                                                    // Szuka kolumny, których wartość jest generowana po stronie bazy danych:
                                                                                    // Id, Znacznik, Utworzony, UtworzonyPrzez, Zmodyfikowany, ZmodyfikowanyPrzez.
                                                                                    ca.NamedArguments.Any(na => na.MemberName == "IsDbGenerated" && (bool)na.TypedValue.Value == true))
                                                )
                                        // Wyklucza kolumny klucza obcego.
                                        .Where(x => !foreignKeys.Contains(x.Name))
                                        .ToList();

            foreach (PropertyInfo pi in properties)
            {
                pi.SetValue(destination, pi.GetValue(source));
            }
        }

        /// <summary>
        /// Ustawia kursor oczekiwania (lub klepsydry) dla całej aplikacji.
        /// </summary>
        /// <returns>Token kursora, którego można użyć do przywrócenia normalnego kursora.</returns>
        protected Guid SetWaitCursor()
        {
            lock (_syncCursor)
            {
                if (_cursorToken == Guid.Empty)
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    _cursorToken = Guid.NewGuid();
                    return _cursorToken;
                }
                return Guid.Empty;
            }
        }

        /// <summary>
        /// Ustawia dla całej aplikacji normalny kursor.
        /// </summary>
        /// <param name="token">Token kursora, którego można użyć do przywrócenia normalnego kursora.</param>
        protected void SetNormalCursor(Guid token)
        {
            lock (_syncCursor)
            {
                if (_cursorToken != Guid.Empty && _cursorToken == token)
                {
                    Mouse.OverrideCursor = null;
                    _cursorToken = Guid.Empty;
                }
            }
        }

        /// <summary>
        /// Ustawia wartość, która określa zajętość (IsBusy=true).
        /// </summary>
        /// <param name="setWaitCursor">Określa czy ustawić kursor oczekiwania (lub klepsydry) dla całej aplikacji.</param>
        /// <returns>Tokeny, które przywracają poprzednie stany elementów.</returns>
        protected Guid[] SetBusy(bool setWaitCursor)
        {
            lock (_syncBusy)
            {
                if (_tokensBusy == null)
                {
                    // IsBusy, WaitCursor, VisibleCircularProgress1, VisibleCircularProgress2, VisibleCircularProgress3, VisibleCircularProgress4, VisibleCircularProgress5.
                    _tokensBusy = new Guid[] { Guid.NewGuid(), Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty };

                    IsBusy = true;

                    if (setWaitCursor)
                    {
                        _tokensBusy[1] = SetWaitCursor();
                    }

                    return _tokensBusy;
                }
                return null;
            }
        }

        /// <summary>
        /// Ustawia wartość, która określa zajętość (IsBusy=false).
        /// </summary>
        /// <param name="tokens">Tokeny, które przywracają poprzednie stany elementów.</param>
        protected void SetNoBusy(Guid[] tokens)
        {
            lock (_syncBusy)
            {
                if (tokens != null && _tokensBusy != null)
                {
                    if (tokens.Length >= 1 && tokens[0] == _tokensBusy[0] && _tokensBusy[0] != Guid.Empty)
                    {
                        IsBusy = false;
                    }

                    if (tokens.Length >= 2 && tokens[1] == _tokensBusy[1] && _tokensBusy[1] != Guid.Empty)
                    {
                        SetNormalCursor(_tokensBusy[1]);
                    }

                    _tokensBusy = null;
                }
            }
        }

        /// <summary>
        /// Zwraca domyślny dla systemu obiekt kodowania znaków wskazujący informację jakiej użyto strony kodowej do zapisania danych w podanym pliku.
        /// </summary>
        /// <returns>Strona kodowa znaków.</returns>
        protected Encoding GetDefaultEncoding()
        {
            return Encoding.Default; // Encoding.UTF8;
        }

        /// <summary>
        /// Zwraca obiekt kodowania znaków wskazujący informację jakiej użyto strony kodowej do zapisania danych w podanym pliku.
        /// </summary>
        /// <param name="filePath">Ścieżka do pliku.</param>
        /// <returns>Strona kodowa znaków.</returns>
        protected Encoding GetEncodingFile(string filePath)
        {
            return EncodingEx.GetEncoding(filePath, GetDefaultEncoding());
        }

        /// <summary>
        /// Kopiuje pojedynczą komórkę do schowka.
        /// </summary>
        protected void KopiujKomorke(DataGridExtensionInvoker invoker)
        {
            invoker.CopyCellContentToClipboard();
        }

        /// <summary>
        /// Kopiuje zaznaczoną zawartość do schowka.
        /// </summary>
        protected void KopiujZaznaczenie(DataGridExtensionInvoker invoker)
        {
            invoker.CopySelectedContentToClipboard();
        }

        #endregion

        #region Internal methods.

        /// <summary>
        /// Wykonuje przeładowanie połączenia. Wykorzystać w przypadku zmiany połączenia.
        /// </summary>
        internal virtual void Reconnection()
        { }

        /// <summary>
        /// Zwraca informację czy aplikacja została uruchomiona z uprawnieniami administratora.
        /// </summary>
        internal static bool CheckAdministrator()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        #endregion

        #region Protected properties.

        /// <summary>
        /// Pobiera lub ustawia okno.
        /// </summary>
        protected Window WindowInvoker
        { get; set; }

        #endregion

        #region Public properties.

        /// <summary>
        /// Pobiera lub ustawia informacje o połączeniu do serwera SQL.
        /// </summary>
        public SqlConnectionInfo ConnectionInfo
        {
            get => Settings.Default.ConnectionInfo;
        }

        /// <summary>
        /// Pobiera wartość, która określa czy bieżący użytkownik jest administratorem komputera.
        /// </summary>
        public bool IsAdministrator
        {
            get
            {
                if (!_isAdministrator.HasValue)
                {
                    _isAdministrator = CheckAdministrator();
                }
                return _isAdministrator.Value;
            }
        }

        /// <summary>
        /// Pobiera nazwę aplikacji (AssemblyProduct).
        /// </summary>
        public virtual string AppName
        {
            get
            {
                return AssemblyEx.CallingAssemblyProduct;
            }
        }

        /// <summary>
        /// Pobiera wersję aplikacji (AssemblyVersion).
        /// </summary>
        public virtual Version AppVersion
        {
            get
            {
                return AssemblyEx.CallingAssemblyVersion;
            }
        }

        /// <summary>
        /// Pobiera tytuł aplikacji.
        /// </summary>
        public virtual string Title
        {
            get
            {
                return AssemblyEx.CallingAssemblyProduct;
            }
        }

        /// <summary>
        /// Pobiera tytuł aplikacji i wersję aplikacji.
        /// </summary>
        public virtual string TitleAndVersion
        {
            get
            {
                return string.Format("{0} {1}",
                    AssemblyEx.CallingAssemblyProduct,
                    AssemblyEx.CallingAssemblyVersion.ToString());
            }
        }

        /// <summary>
        /// Pobiera lub ustawia wartość, która określa zajętość, wykonywanie jakiś operacji, najczęściej ładowanie danych oraz zapis.
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            internal set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Events - Initialized, Loaded.

        /// <summary>
        /// Występuje po zainicjowaniu System.Windows.FrameworkElement. To zdarzenie pokrywa się z przypadkami,
        /// w których wartość właściwości System.Windows.FrameworkElement.IsInitialized zmienia się z false (lub undefined) na true.
        /// </summary>
        /// <param name="windowInvoker">Interfejs sterowania oknem.</param>
        protected virtual void OnInitialized(Window windowInvoker)
        { }

        public RelayCommand<Window> Initialized => new RelayCommand<Window>((e) =>
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    WindowInvoker = e;
                });
                OnInitialized(WindowInvoker);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionFormater.AppendMethod(ex, MethodBase.GetCurrentMethod()));
            }
        });

        /// <summary>
        /// Występuje, gdy element jest ułożony, renderowany i gotowy do interakcji.
        /// </summary>
        /// <param name="windowInvoker">Interfejs sterowania oknem.</param>
        protected virtual void OnLoaded(Window windowInvoker)
        { }

        public RelayCommand<Window> Loaded => new RelayCommand<Window>((e) =>
        {
            try
            {
                // Sprawdzamy 'WindowInvoker', metoda 'Initialized' mogła już wczytać wartość 'WindowInvoker';
                if (WindowInvoker == null)
                {
                    Dispatcher.Invoke(() =>
                    {
                        WindowInvoker = e;
                    });
                }
                OnLoaded(WindowInvoker);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionFormater.AppendMethod(ex, MethodBase.GetCurrentMethod()));
            }
        });

        /// <summary>
        /// Występuje po wyrenderowaniu zawartości okna.
        /// </summary>
        /// <param name="windowInvoker">Interfejs sterowania oknem.</param>
        protected virtual void OnContentRendered(Window windowInvoker)
        { }

        public RelayCommand<object> ContentRendered => new RelayCommand<object>((e) =>
        {
            try
            {
                IsContentRendered = true;
                OnContentRendered(WindowInvoker);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionFormater.AppendMethod(ex, MethodBase.GetCurrentMethod()));
            }
        });

        /// <summary>
        /// Występuje po zainicjowaniu System.Windows.FrameworkElement. To zdarzenie pokrywa się z przypadkami,
        /// w których wartość właściwości System.Windows.FrameworkElement.IsInitialized zmienia się z false (lub undefined) na true.
        /// </summary>
        /// <param name="userControl">Kontrolka użytkownika.</param>
        protected virtual void OnInitialized(System.Windows.Controls.UserControl userControl)
        { }

        /// <summary>
        /// Uwaga metoda nie jest wywoływana z poziomu XAML-a.
        /// </summary>
        public RelayCommand<System.Windows.Controls.UserControl> InitializedUserControl => new RelayCommand<System.Windows.Controls.UserControl>((e) =>
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    WindowInvoker = e.GetWindow();
                });
                OnInitialized(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionFormater.AppendMethod(ex, MethodBase.GetCurrentMethod()));
            }
        });

        /// <summary>
        /// Występuje, gdy element jest układany, renderowany i gotowy do interakcji.
        /// </summary>
        /// <param name="userControl">Kontrolka użytkownika.</param>
        protected virtual void OnLoaded(System.Windows.Controls.UserControl userControl)
        { }

        public RelayCommand<System.Windows.Controls.UserControl> LoadedUserControl => new RelayCommand<System.Windows.Controls.UserControl>((e) =>
        {
            try
            {
                // Sprawdzamy 'WindowInvoker', metoda 'InitializedUserControl' mogła już wczytać wartość 'WindowInvoker';
                if (WindowInvoker == null)
                {
                    Dispatcher.Invoke(() =>
                    {
                        WindowInvoker = e.GetWindow();
                    });
                }
                OnLoaded(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionFormater.AppendMethod(ex, MethodBase.GetCurrentMethod()));
            }
        });

        #endregion

        #region Events - InitializedAsync, LoadedAsync.

        /// <summary>
        /// Występuje po zainicjowaniu System.Windows.FrameworkElement. To zdarzenie pokrywa się z przypadkami,
        /// w których wartość właściwości System.Windows.FrameworkElement.IsInitialized zmienia się z false (lub undefined) na true.
        /// </summary>
        /// <param name="windowInvoker">Interfejs sterowania oknem.</param>
        protected virtual async Task OnInitializedAsync(Window windowInvoker)
        {
            await Dispatcher.InvokeAsync(() => { });
        }

        public RelayCommandAsync<Window> InitializedAsync => new RelayCommandAsync<Window>(new Func<Window, Task>(async (e) =>
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    WindowInvoker = e;
                });
                await OnInitializedAsync(WindowInvoker);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionFormater.AppendMethod(ex, MethodBase.GetCurrentMethod()));
            }
        }));

        /// <summary>
        /// Występuje, gdy element jest ułożony, renderowany i gotowy do interakcji.
        /// </summary>
        /// <param name="windowInvoker">Interfejs sterowania oknem.</param>
        protected virtual async Task OnLoadedAsync(Window windowInvoker)
        {
            await Dispatcher.InvokeAsync(() => { });
        }

        public RelayCommandAsync<Window> LoadedAsync => new RelayCommandAsync<Window>(new Func<Window, Task>(async (e) =>
        {
            try
            {
                // Sprawdzamy 'WindowInvoker', metoda 'Initialized' mogła już wczytać wartość 'WindowInvoker';
                if (WindowInvoker == null)
                {
                    Dispatcher.Invoke(() =>
                    {
                        WindowInvoker = e;
                    });
                }
                await OnLoadedAsync(WindowInvoker);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionFormater.AppendMethod(ex, MethodBase.GetCurrentMethod()));
            }
        }));

        /// <summary>
        /// Występuje po zainicjowaniu System.Windows.FrameworkElement. To zdarzenie pokrywa się z przypadkami,
        /// w których wartość właściwości System.Windows.FrameworkElement.IsInitialized zmienia się z false (lub undefined) na true.
        /// </summary>
        /// <param name="userControl">Kontrolka użytkownika.</param>
        protected virtual async Task OnInitializedAsync(System.Windows.Controls.UserControl userControl)
        {
            await Dispatcher.InvokeAsync(() => { });
        }

        /// <summary>
        /// Uwaga metoda nie jest wywoływana z poziomu XAML-a.
        /// </summary>
        public RelayCommandAsync<System.Windows.Controls.UserControl> InitializedUserControlAsync => new RelayCommandAsync<System.Windows.Controls.UserControl>(new Func<System.Windows.Controls.UserControl, Task>(async (e) =>
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    WindowInvoker = e.GetWindow();
                });
                await OnInitializedAsync(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionFormater.AppendMethod(ex, MethodBase.GetCurrentMethod()));
            }
        }));

        /// <summary>
        /// Występuje, gdy element jest układany, renderowany i gotowy do interakcji.
        /// </summary>
        /// <param name="userControl">Kontrolka użytkownika.</param>
        protected virtual async Task OnLoadedAsync(System.Windows.Controls.UserControl userControl)
        {
            await Dispatcher.InvokeAsync(() => { });
        }

        public RelayCommandAsync<System.Windows.Controls.UserControl> LoadedUserControlAsync => new RelayCommandAsync<System.Windows.Controls.UserControl>(new Func<System.Windows.Controls.UserControl, Task>(async (e) =>
        {
            try
            {
                // Sprawdzamy 'WindowInvoker', metoda 'InitializedUserControl' mogła już wczytać wartość 'WindowInvoker';
                if (WindowInvoker == null)
                {
                    Dispatcher.Invoke(() =>
                    {
                        WindowInvoker = e.GetWindow();
                    });
                }
                await OnLoadedAsync(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionFormater.AppendMethod(ex, MethodBase.GetCurrentMethod()));
            }
        }));

        #endregion
    }
}
