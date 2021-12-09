using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Threading.Tasks;

namespace MVVMCore
{
    /// <summary>
    /// Formatuje komunikaty błędów.
    /// </summary>
    public static class ExceptionFormater
    {
        #region Private methods.

        /// <summary>
        /// Próbuje wyodrębnić i wywołać wyjątek WebException.
        /// </summary>
        /// <param name="ex">Wyjątek.</param>
        private static void ThrowWebException(Exception ex)
        {
            if (ex != null)
            {
                if (ex is WebException)
                {
                    if (ex.InnerException != null)
                    {
                        ThrowWebException(ex.InnerException);
                    }
                    throw ex;
                }
                else if (ex.InnerException is WebException)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        ThrowWebException(ex.InnerException.InnerException);
                    }
                    throw ex.InnerException;
                }
            }
        }

        /// <summary>
        /// Próbuje wyodrębnić i wywołać wyjątek SocketException lub WebException.
        /// </summary>
        /// <param name="ex">Wyjątek.</param>
        private static void ThrowSocketOrWebException(Exception ex)
        {
            if (ex != null)
            {
                if ((ex is SocketException) || (ex is WebException))
                {
                    if (ex.InnerException != null)
                    {
                        ThrowSocketOrWebException(ex.InnerException);
                    }
                    throw ex;
                }
                else if ((ex.InnerException is SocketException) || (ex.InnerException is WebException))
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        ThrowSocketOrWebException(ex.InnerException.InnerException);
                    }
                    throw ex.InnerException;
                }
            }
        }

        #endregion

        /// <summary>
        /// Zwraca pełną ścieżkę do metody.
        /// </summary>
        /// <param name="method">Informacje o metodzie.</param>
        public static string NamespaceMethod(MethodBase method)
        {
            if (method != null)
                return string.Format("Method: {0}.{1}", method.DeclaringType.FullName, method.Name);
            return "";
        }

        /// <summary>
        /// Formatuje komunikat błędu wywołany w metodzie. Dodaje do komunikatu miejsce wywołania (przestrzeń nazw).
        /// </summary>
        /// <param name="ex">Wyjątek.</param>
        /// <param name="methodNamespace">Miejsce wywołania - pełna ścieżka do metody.</param>
        /// <returns>Sformatowany komunikat.</returns>
        public static string AppendMethod(Exception ex, string methodNamespace)
        {
            return string.Format("{0}\r\n\r\nMethod: {1}", ex.Message, methodNamespace);
        }

        /// <summary>
        /// Formatuje komunikat błędu wywołany w metodzie. Dodaje do komunikatu miejsce wywołania (przestrzeń nazw).
        /// </summary>
        /// <param name="ex">Wyjątek.</param>
        /// <param name="method">Informacje o metodzie.</param>
        /// <returns>Sformatowany komunikat.</returns>
        public static string AppendMethod(Exception ex, MethodBase method)
        {
            return string.Format("{0}\r\n\r\n{1}", ex.Message, NamespaceMethod(method));
        }

        /// <summary>
        /// Zwraca informację czy to wyjątek anulowania żądania web.
        /// </summary>
        /// <param name="ex">Wyjątek.</param>
        public static bool IsRequestCanceled(Exception ex)
        {
            try
            {
                ThrowWebException(ex);
            }
            catch (WebException webException)
            {
                if (webException.Status == WebExceptionStatus.RequestCanceled)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Zwraca informację czy to wyjątek autoryzacji logowania.
        /// </summary>
        /// <param name="ex">Wyjątek.</param>
        public static bool IsRequestUnauthorized(Exception ex)
        {
            try
            {
                ThrowWebException(ex);
            }
            catch (WebException webException)
            {
                HttpWebResponse httpWebResponse = webException.Response as HttpWebResponse;
                if (httpWebResponse != null && httpWebResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Zwraca informację czy to wyjątek anulowania operacji przez użytkownika.
        /// </summary>
        /// <param name="ex">Wyjątek.</param>
        public static bool IsTaskCanceledException(Exception ex)
        {
            TaskCanceledException taskCanceledException = ex as TaskCanceledException;

            if (taskCanceledException == null)
            {
                AggregateException aggregateException = ex as AggregateException;
                if (aggregateException != null)
                {
                    taskCanceledException = aggregateException.InnerExceptions.FirstOrDefault(
                        item => item is TaskCanceledException) as TaskCanceledException;
                }
            }

            return (taskCanceledException != null);
        }

        /// <summary>
        /// Wyodrębnia i wywołuje właściwy wyjątek.
        /// </summary>
        /// <param name="ex">Wyjątek.</param>
        /// <param name="ignoreFaultException">Określa czy zignorować wyjątek FaultException.</param>
        public static void ThrowExtractException(Exception ex, bool ignoreFaultException)
        {
            if (ex is FaultException)
            {
                if (ignoreFaultException)
                    return;
                throw ex;
            }
            else if (ex is MessageSecurityException)
            {
                // ================================================== //
                // Autoryzacja z usługą Web Service nie powiodła się.
                // ================================================== //
                ThrowSocketOrWebException(ex);
                throw ex;
            }

            if (ex is UnauthorizedAccessException)
            {
                // ================================================== //
                // Autoryzacja z usługą Web Service nie powiodła się.
                // ================================================== //
                throw ex;
            }
            else if (ex is SocketException)
            {
                ThrowSocketOrWebException(ex);
                throw ex;
            }
            else if (ex is WebException)
            {
                ThrowSocketOrWebException(ex);
                throw ex;
            }
            else if (ex is ProtocolException)
            {
                // ========================================================================= //
                // 1. The remote server returned an unexpected response: (401) Unauthorized. //
                // ========================================================================= //
                // Błąd może wskazywać na to, że usługa ma włączone takie protokoły
                // uwierzytelniania, których aplikacja nie obsługuje.
                // ========================================================================= //
                throw ex;
            }
            else if (ex is CommunicationObjectAbortedException)
            {
                throw ex;
            }
            else if (ex is CommunicationException)
            {
                // ========================================================================= //
                // 1. The remote server returned an error: NotFound.
                // ========================================================================= //
                // a) Błąd może wskazywać na to, że usługa jest zatrzymana.
                // b) Urządzenie nie ma połączenia z internetem.
                // ========================================================================= //

                ThrowSocketOrWebException(ex);
                throw ex;
            }
            else if (ex is TimeoutException)
            {
                throw ex;
            }
            else if (ex is IOException)
            {
                ThrowSocketOrWebException(ex);
                throw ex;
            }
            else if (ex is Exception)
            {
                ThrowSocketOrWebException(ex);
                throw ex;
            }
            throw ex;
        }
    }
}
