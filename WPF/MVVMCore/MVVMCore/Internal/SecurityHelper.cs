using MVVMCore.Win32;
using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace MVVMCore.Internal
{
    internal static class SecurityHelper
    {
        /// <SecurityNote>
        /// Critical - Exceptions raised by a demand may contain security sensitive information that should not be passed to transparent callers
        /// Safe     - The method denies the caller access to the full exception object.
        /// </SecurityNote>
        [SecuritySafeCritical]       
        internal static bool CheckUnmanagedCodePermission()
        {
            try
            {
                SecurityHelper.DemandUnmanagedCode();
            }
            catch (SecurityException)
            {
                return false;
            }

            return true;
        }

        /// <SecurityNote>
        /// Critical - Exceptions raised by a demand may contain security sensitive information that should not be passed to transparent callers
        /// </SecurityNote>
        [SecurityCritical]
        internal static void DemandUnmanagedCode()
        {
            if (_unmanagedCodePermission == null)
            {
                _unmanagedCodePermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
            }
            _unmanagedCodePermission.Demand();
        }
        static SecurityPermission _unmanagedCodePermission = null;

        ///<summary>
        /// Check to see if the caller is fully trusted.
        ///</summary>
        /// <returns>true if call stack has unrestricted permission</returns>
        /// <SecurityNote>
        /// Critical - Exceptions raised by a demand may contain security sensitive information that should not be passed to transparent callers
        /// Safe     - The method denies the caller access to the full exception object.
        /// </SecurityNote>
        [SecuritySafeCritical]
        internal static bool IsFullTrustCaller()
        {
            try
            {
                if (_fullTrustPermissionSet == null)
                {
                    _fullTrustPermissionSet = new PermissionSet(PermissionState.Unrestricted);
                }
                _fullTrustPermissionSet.Demand();
            }
            catch (SecurityException)
            {
                return false;
            }

            return true;
        }
        static PermissionSet _fullTrustPermissionSet = null;

        ///<summary>
        /// Return true if the caller has the correct permission set to get a folder
        /// path.
        ///</summary>
        ///<remarks>
        /// This function exists solely as a an optimazation for the debugger scenario
        ///</remarks>
        /// <SecurityNote>
        ///    Critical: This code extracts the permission set associated with an appdomain by elevating
        ///    TreatAsSafe: The information is not exposed
        /// </SecurityNote>
        [SecuritySafeCritical]
        //
        internal static bool CallerHasPermissionWithAppDomainOptimization(params IPermission[] permissionsToCheck)
        {
            // in case of passing null return true
            if (permissionsToCheck == null)
                return true;
            PermissionSet psToCheck = new PermissionSet(PermissionState.None);
            for (int i = 0; i < permissionsToCheck.Length; i++)
            {
                psToCheck.AddPermission(permissionsToCheck[i]);
            }
            PermissionSet permissionSetAppDomain = AppDomain.CurrentDomain.PermissionSet;
            if (psToCheck.IsSubsetOf(permissionSetAppDomain))
            {
                return true;
            }
            return false;
        }

        /// <summary> Enables an efficient check for a specific permisison in the AppDomain's permission grant
        /// without having to catch a SecurityException in the case the permission is not granted.
        /// <summary>
        /// <SecurityNote>
        /// Caveat: This is not a generally valid substitute for doing a full Demand. The main cases not
        /// covered are:
        ///   1) call from PT AppDomain into full-trust one;
        ///   2) captured PT callstack (via ExecutionContext) from another thread or context. Our Dispatcher
        ///     does this.
        ///
        /// Critical: Accesses the Critical AppDomain.PermissionSet, which might contain sensitive information
        ///     such as file paths.
        /// Safe: Does not expose the permission object to the caller.
        /// </SecurityNote>
        [SecuritySafeCritical]
        internal static bool AppDomainHasPermission(IPermission permissionToCheck)
        {
            PermissionSet psToCheck = new PermissionSet(PermissionState.None);
            psToCheck.AddPermission(permissionToCheck);
            return psToCheck.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet);
        }

        /// <SecurityNote>
        /// Critical: Elevates to extract the AppDomain BaseDirectory and returns it, which is sensitive information.
        /// </SecurityNote>
        [SecurityCritical]
        internal static Uri GetBaseDirectory(AppDomain domain)
        {
            Uri appBase = null;
            new FileIOPermission(PermissionState.Unrestricted).Assert();// BlessedAssert
            try
            {
                appBase = new Uri(domain.BaseDirectory);
            }
            finally
            {
                FileIOPermission.RevertAssert();
            }
            return (appBase);
        }

        /// <SecurityNote>
        ///   Critical: This code elevates to call MapUrlToZone in the form of a SUC
        /// </SecurityNote>
        [SecurityCritical]
        internal static int MapUrlToZoneWrapper(Uri uri)
        {
            int targetZone = NativeMethods.URLZONE_LOCAL_MACHINE; // fail securely this is the most priveleged zone
            int hr = NativeMethods.S_OK;
            object curSecMgr = null;
            hr = NativeMethods.CoInternetCreateSecurityManager(
                                                                     null,
                                                                     out curSecMgr,
                                                                     0);
            if (NativeMethods.Failed(hr))
                throw new Win32Exception(hr);

            NativeMethods.IInternetSecurityManager pSec = (NativeMethods.IInternetSecurityManager)curSecMgr;

            string uriString = BindUriHelper.UriToString(uri);
            //
            // special case the condition if file is on local machine or UNC to ensure that content with mark of the web
            // does not yield with an internet zone result
            //
            if (uri.IsFile)
            {
                pSec.MapUrlToZone(uriString, out targetZone, NativeMethods.MUTZ_NOSAVEDFILECHECK);
            }
            else
            {
                pSec.MapUrlToZone(uriString, out targetZone, 0);
            }
            //
            // This is the condition for Invalid zone
            //
            if (targetZone < 0)
            {
                throw new SecurityException("The URI specified is invalid.");
            }
            pSec = null;
            curSecMgr = null;
            return targetZone;
        }

        /// <SecurityNote>
        /// Critical - Exceptions raised by a demand may contain security sensitive information that should not be passed to transparent callers
        /// </SecurityNote>
        [SecurityCritical]
        internal static void DemandFilePathDiscoveryWriteRead()
        {
            FileIOPermission permobj = new FileIOPermission(PermissionState.None);
            permobj.AllFiles = FileIOPermissionAccess.Write | FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery;
            permobj.Demand();
        }

        /// <summary>
        /// determines if the current call stack has the serialization formatter
        /// permission. This is one of the few CLR checks that doesn't have a
        /// bool version - you have to let the check fail and catch the exception.
        ///
        /// Because this is a check *at that point*, you may not cache this value.
        /// </summary>
        /// <returns>true if call stack has the serialization permission</returns>
        /// <SecurityNote>
        /// Critical - Exceptions raised by a demand may contain security sensitive information that should not be passed to transparent callers
        /// Safe     - The method denies the caller access to the full exception object.
        /// </SecurityNote>
        [SecuritySafeCritical]
        internal static bool CallerHasSerializationPermission()
        {
            try
            {
                if (_serializationSecurityPermission == null)
                {
                    _serializationSecurityPermission = new SecurityPermission(SecurityPermissionFlag.SerializationFormatter);
                }
                _serializationSecurityPermission.Demand();
            }
            catch (SecurityException)
            {
                return false;
            }
            return true;
        }
        static SecurityPermission _serializationSecurityPermission = null;

        /// <summary>
        /// determines if the current call stack has the all clipboard
        /// permission. This is one of the few CLR checks that doesn't have a
        /// bool version - you have to let the check fail and catch the exception.
        ///
        /// Because this is a check *at that point*, you may not cache this value.
        /// </summary>
        /// <returns>true if call stack has the all clipboard</returns>
        /// <SecurityNote>
        /// Critical - Exceptions raised by a demand may contain security sensitive information that should not be passed to transparent callers
        /// Safe     - The method denies the caller access to the full exception object.
        /// </SecurityNote>
        [SecuritySafeCritical]
        internal static bool CallerHasAllClipboardPermission()
        {
            try
            {
                SecurityHelper.DemandAllClipboardPermission();
            }
            catch (SecurityException)
            {
                return false;
            }
            return true;
        }

        /// <SecurityNote>
        /// Critical - Exceptions raised by a demand may contain security sensitive information that should not be passed to transparent callers
        /// </SecurityNote>
        [SecurityCritical]
        internal static void DemandAllClipboardPermission()
        {
            if (_uiPermissionAllClipboard == null)
            {
                _uiPermissionAllClipboard = new UIPermission(UIPermissionClipboard.AllClipboard);
            }
            _uiPermissionAllClipboard.Demand();
        }
        static UIPermission _uiPermissionAllClipboard = null;

        /// <SecurityNote>
        /// Critical - Exceptions raised by a demand may contain security sensitive information that should not be passed to transparent callers
        /// </SecurityNote>
        [SecurityCritical]
        internal static void DemandPathDiscovery(string path)
        {
            new FileIOPermission(FileIOPermissionAccess.PathDiscovery, path).Demand();
        }

        /// <SecurityNote>
        /// Critical - Exceptions raised by a demand may contain security sensitive information that should not be passed to transparent callers
        /// Safe     - The method denies the caller access to the full exception object.
        /// </SecurityNote>
        [SecuritySafeCritical]
        internal static bool CheckEnvironmentPermission()
        {
            try
            {
                SecurityHelper.DemandEnvironmentPermission();
            }
            catch (SecurityException)
            {
                return false;
            }

            return true;
        }

        /// <SecurityNote>
        /// Critical - Exceptions raised by a demand may contain security sensitive information that should not be passed to transparent callers
        /// </SecurityNote>
        [SecurityCritical]
        internal static void DemandEnvironmentPermission()
        {
            if (_unrestrictedEnvironmentPermission == null)
            {
                _unrestrictedEnvironmentPermission = new EnvironmentPermission(PermissionState.Unrestricted);
            }
            _unrestrictedEnvironmentPermission.Demand();
        }
        static EnvironmentPermission _unrestrictedEnvironmentPermission = null;

        /// <SecurityNote>
        /// Critical - Exceptions raised by a demand may contain security sensitive information that should not be passed to transparent callers
        /// </SecurityNote>
        [SecurityCritical]
        internal static void DemandUriDiscoveryPermission(Uri uri)
        {
            CodeAccessPermission permission = CreateUriDiscoveryPermission(uri);
            if (permission != null)
                permission.Demand();
        }

        ///<SecurityNote>
        ///  Critical: Returns a permission object, which can be misused.
        ///</SecurityNote>
        [SecurityCritical]
        internal static CodeAccessPermission CreateUriDiscoveryPermission(Uri uri)
        {
            // explicitly disallow sub-classed Uris to guard against
            // exploits where we "lie" about some of the properties on the Uri.
            // and then later change the value returned
            //      ( e.g. supply a different uri from what checked here)
            if (uri.GetType().IsSubclassOf(typeof(Uri)))
            {
                DemandInfrastructurePermission();
            }

            if (uri.IsFile)
                return new FileIOPermission(FileIOPermissionAccess.PathDiscovery, uri.LocalPath);

            // Add appropriate demands for other Uri types here.
            return null;
        }

        ///<SecurityNote>
        ///  Critical: Returns a permission object, which can be misused.
        ///</SecurityNote>
        [SecurityCritical]
        internal static CodeAccessPermission CreateUriReadPermission(Uri uri)
        {
            // explicitly disallow sub-classed Uris to guard against
            // exploits where we "lie" about some of the properties on the Uri.
            // and then later change the value returned
            //      ( e.g. supply a different uri from what checked here)
            if (uri.GetType().IsSubclassOf(typeof(Uri)))
            {
                DemandInfrastructurePermission();
            }

            if (uri.IsFile)
                return new FileIOPermission(FileIOPermissionAccess.Read, uri.LocalPath);

            // Add appropriate demands for other Uri types here.
            return null;
        }

        /// <SecurityNote>
        /// Critical - Exceptions raised by a demand may contain security sensitive information that should not be passed to transparent callers
        /// </SecurityNote>
        [SecurityCritical]
        internal static void DemandUriReadPermission(Uri uri)
        {
            CodeAccessPermission permission = CreateUriReadPermission(uri);
            if (permission != null)
                permission.Demand();
        }

        /// <summary>
        /// Checks whether the caller has path discovery permission for the input path.
        /// </summary>
        /// <param name="path">Full path to a file or a directory.</param>
        /// <returns>true if the caller has the discovery permission, false otherwise.</returns>
        /// <SecurityNote>
        /// Critical - Exceptions raised by a demand may contain security sensitive information that should not be passed to transparent callers
        /// Safe     - The method denies the caller access to the full exception object.
        /// </SecurityNote>
        [SecuritySafeCritical]
        internal static bool CallerHasPathDiscoveryPermission(string path)
        {
            try
            {
                DemandPathDiscovery(path);
                return true;
            }
            catch (SecurityException)
            {
                return false;
            }
        }

        /// <SecurityNote>
        /// Critical: This calls into Marshal.GetExceptionForHR which is critical
        ///           it populates the exception object from data stored in a per thread IErrorInfo
        ///           the IErrorInfo may have security sensitive information like file paths stored in it
        /// TreatAsSafe: Uses overload of GetExceptionForHR that omits IErrorInfo information from exception message
        /// </SecurityNote>
        [SecuritySafeCritical]
        internal static Exception GetExceptionForHR(int hr)
        {
            return Marshal.GetExceptionForHR(hr, new IntPtr(-1));
        }

        /// <SecurityNote>
        /// Critical: This calls into Marshal.ThrowExceptionForHR which is critical because 
        ///           it populates the exception object from data stored in a per thread IErrorInfo
        ///           the IErrorInfo may have security sensitive information like file paths stored in it
        /// TreatAsSafe: Uses overload of ThrowExceptionForHR that omits IErrorInfo information from exception message
        /// </SecurityNote>
        [SecuritySafeCritical]
        internal static void ThrowExceptionForHR(int hr)
        {
            Marshal.ThrowExceptionForHR(hr, new IntPtr(-1));
        }

        /// <SecurityNote>
        /// Critical: This calls into Marshal.GetHRForException which is critical
        ///           because it fills a per thread IErrorInfo with data in the Exception
        ///           the Exception may have security sensitive data like file paths stored in it
        /// TreatAsSafe: Clears the per thread IErrorInfo by calling GetHRForException a second time 
        ///              with an exception object with no security sensitive information
        /// </SecurityNote>
        [SecuritySafeCritical]
        internal static int GetHRForException(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }

            // GetHRForException fills a per thread IErrorInfo object with data from the exception
            // The exception may contain security sensitive data like full file paths that we do not
            // want to leak into an IErrorInfo
            int hr = Marshal.GetHRForException(exception);

            // Call GetHRForException a second time with a security safe exception object
            // to make sure the per thread IErrorInfo is cleared of security sensitive data
            Marshal.GetHRForException(new Exception());

            return hr;
        }

        /// <SecurityNote>
        /// Critical - Exceptions raised by a demand may contain security sensitive information that should not be passed to transparent callers
        /// </SecurityNote>
        [SecurityCritical]
        internal static void DemandRegistryPermission()
        {
            if (_unrestrictedRegistryPermission == null)
            {
                _unrestrictedRegistryPermission = new RegistryPermission(PermissionState.Unrestricted);
            }
            _unrestrictedRegistryPermission.Demand();
        }
        static RegistryPermission _unrestrictedRegistryPermission = null;

        /// <SecurityNote>
        /// Critical - Exceptions raised by a demand may contain security sensitive information that should not be passed to transparent callers
        /// </SecurityNote>
        [SecurityCritical]
        internal static void DemandUIWindowPermission()
        {
            if (_allWindowsUIPermission == null)
            {
                _allWindowsUIPermission = new UIPermission(UIPermissionWindow.AllWindows);
            }
            _allWindowsUIPermission.Demand();
        }
        static UIPermission _allWindowsUIPermission = null;

        /// <SecurityNote>
        /// Critical - Exceptions raised by a demand may contain security sensitive information that should not be passed to transparent callers
        /// </SecurityNote>
        [SecurityCritical]
        internal static void DemandInfrastructurePermission()
        {
            if (_infrastructurePermission == null)
            {
                _infrastructurePermission = new SecurityPermission(SecurityPermissionFlag.Infrastructure);
            }
            _infrastructurePermission.Demand();
        }
        static SecurityPermission _infrastructurePermission = null;

        /// <SecurityNote>
        /// Critical - Exceptions raised by a demand may contain security sensitive information that should not be passed to transparent callers
        /// </SecurityNote>
        [SecurityCritical]
        internal static void DemandMediaPermission(MediaPermissionAudio audioPermissionToDemand,
                                                   MediaPermissionVideo videoPermissionToDemand,
                                                   MediaPermissionImage imagePermissionToDemand)
        {
            // Demand the appropriate permission
            (new MediaPermission(audioPermissionToDemand,
                                 videoPermissionToDemand,
                                 imagePermissionToDemand)).Demand();
        }


        ///<summary>
        /// Check whether the call stack has the permissions needed for safe media.
        ///
        /// </summary>
        /// <SecurityNote>
        /// Critical - Exceptions raised by a demand may contain security sensitive information that should not be passed to transparent callers
        /// Safe     - The method denies the caller access to the full exception object.
        /// </SecurityNote>
        [SecuritySafeCritical]   
        internal static bool CallerHasMediaPermission(MediaPermissionAudio audioPermissionToDemand,
                                                      MediaPermissionVideo videoPermissionToDemand,
                                                      MediaPermissionImage imagePermissionToDemand)
        {
            try
            {
                (new MediaPermission(audioPermissionToDemand, videoPermissionToDemand, imagePermissionToDemand)).Demand();
                return true;
            }
            catch (SecurityException)
            {
                return false;
            }
        }

        // don't include this in the compiler - avoid compiler changes when we can.
        /// <SecurityNote>
        /// Critical - Exceptions raised by a demand may contain security sensitive information that should not be passed to transparent callers
        /// </SecurityNote>
        [SecurityCritical]
        internal static void DemandUnrestrictedUIPermission()
        {
            if (_unrestrictedUIPermission == null)
            {
                _unrestrictedUIPermission = new UIPermission(PermissionState.Unrestricted);
            }
            _unrestrictedUIPermission.Demand();
        }
        static UIPermission _unrestrictedUIPermission = null;

        internal static bool AppDomainGrantedUnrestrictedUIPermission
        {
            [SecurityCritical]
            get
            {
                if (!_appDomainGrantedUnrestrictedUIPermission.HasValue)
                {
                    _appDomainGrantedUnrestrictedUIPermission = AppDomainHasPermission(new UIPermission(PermissionState.Unrestricted));
                }

                return _appDomainGrantedUnrestrictedUIPermission.Value;
            }
        }
        [SecurityCritical]
        private static bool? _appDomainGrantedUnrestrictedUIPermission;

        /// <SecurityNote>
        /// Critical - Exceptions raised by a demand may contain security sensitive information that should not be passed to transparent callers
        /// </SecurityNote>
        [SecurityCritical]
        internal static void DemandFileIOReadPermission(string fileName)
        {
            new FileIOPermission(FileIOPermissionAccess.Read, fileName).Demand();
        }

        ///<summary>
        ///   Check caller has web-permission. for a given Uri.
        /// </summary>
        /// <SecurityNote>
        /// Critical - Exceptions raised by a demand may contain security sensitive information that should not be passed to transparent callers
        /// Safe     - The method denies the caller access to the full exception object.
        /// </SecurityNote>
        [SecuritySafeCritical]       
        internal static bool CallerHasWebPermission(Uri uri)
        {
            try
            {
                SecurityHelper.DemandWebPermission(uri);
                return true;
            }
            catch (SecurityException)
            {
                return false;
            }
        }

        /// <SecurityNote>
        /// Critical - Exceptions raised by a demand may contain security sensitive information that should not be passed to transparent callers
        /// </SecurityNote>
        [SecurityCritical]
        internal static void DemandWebPermission(Uri uri)
        {
            // We do this first as a security measure since the call below
            // checks for derivatives. Please note we need to extract the
            // string to call into WebPermission anyways, the only thing that
            // doing this early gains us is a defense in depth measure. The call
            // is required nevertheless.
            string finalUri = BindUriHelper.UriToString(uri);

            if (uri.IsFile)
            {
                // If the scheme is file: demand file io
                string toOpen = uri.LocalPath;
                (new FileIOPermission(FileIOPermissionAccess.Read, toOpen)).Demand();
            }
            else
            {
                // else demand web permissions
                new WebPermission(NetworkAccess.Connect, finalUri).Demand();
            }
        }

        /// <SecurityNote>
        /// Critical - Exceptions raised by a demand may contain security sensitive information that should not be passed to transparent callers
        /// </SecurityNote>
        /// <summary>
        /// By default none of the plug-in serializer code must succeed for partially trusted callers
        /// </summary>
        [SecurityCritical]
        internal static void DemandPlugInSerializerPermissions()
        {
            if (_plugInSerializerPermissions == null)
            {
                _plugInSerializerPermissions = new PermissionSet(PermissionState.Unrestricted);
            }
            _plugInSerializerPermissions.Demand();
        }
        static PermissionSet _plugInSerializerPermissions = null;

        /// <SecurityNote>
        /// This method has security implications: Mime\ContentTypes\Uri schemes types are served in a case-insensitive fashion; they MUST be compared that way
        /// </SecurityNote>
        internal static bool AreStringTypesEqual(string m1, string m2)
        {
            return (String.Compare(m1, m2, StringComparison.OrdinalIgnoreCase) == 0);
        }
    }
}
