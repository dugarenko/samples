using System;
using System.Text;

namespace MVVMCore.Internal
{
    /// <summary>
    /// Methods in this partial class are shared by PresentationFramework and PresentationBuildTasks. See also WpfWebRequestHelper.
    /// </summary>
    internal static partial class BindUriHelper
    {
        private const int MAX_PATH_LENGTH = 2048;
        private const int MAX_SCHEME_LENGTH = 32;
        public const int MAX_URL_LENGTH = MAX_PATH_LENGTH + MAX_SCHEME_LENGTH + 3; /*=sizeof("://")*/

        //
        // Uri-toString does 3 things over the standard .toString()
        //
        //  1) We don't unescape special control characters. The default Uri.ToString() 
        //     will unescape a character like ctrl-g, or ctrl-h so the actual char is emitted. 
        //     However it's considered safer to emit the escaped version. 
        //
        //  2) We truncate urls so that they are always <= MAX_URL_LENGTH
        // 
        // This method should be called whenever you are taking a Uri
        // and performing a p-invoke on it. 
        //
        internal static string UriToString(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            return new StringBuilder(
                uri.GetComponents(
                    uri.IsAbsoluteUri ? UriComponents.AbsoluteUri : UriComponents.SerializationInfoString,
                    UriFormat.SafeUnescaped),
                MAX_URL_LENGTH).ToString();
        }

        internal static bool DoSchemeAndHostMatch(Uri first, Uri second)
        {
            // Check that both the scheme and the host match. 
            return (SecurityHelper.AreStringTypesEqual(first.Scheme, second.Scheme) && first.Host.Equals(second.Host) == true);
        }
    }
}
