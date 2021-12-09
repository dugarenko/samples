using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Text;
using Internal_ = MVVMCore.Internal;

namespace MVVMCore.Win32
{
    public static class NativeMethods
    {
        #region Constans.

        public static HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);

        public const int ERROR_ACCESS_DENIED = 5;
        public const int ERROR_ALREADY_EXISTS = 0xb7;
        public const int ERROR_BAD_IMPERSONATION_LEVEL = 0x542;
        public const int ERROR_BAD_LENGTH = 0x18;
        public const int ERROR_BAD_PATHNAME = 0xa1;
        public const int ERROR_CALL_NOT_IMPLEMENTED = 120;
        public const int ERROR_CANT_OPEN_ANONYMOUS = 0x543;
        public const int ERROR_DLL_INIT_FAILED = 0x45a;
        public const int ERROR_ENVVAR_NOT_FOUND = 0xcb;
        public const int ERROR_FILE_EXISTS = 80;
        public const int ERROR_FILE_NOT_FOUND = 2;
        public const int ERROR_FILENAME_EXCED_RANGE = 0xce;
        public const int ERROR_INSUFFICIENT_BUFFER = 0x7a; //https://msdn.microsoft.com/en-us/library/windows/desktop/ms681382(v=vs.85).aspx
        public const int ERROR_INVALID_ACL = 0x538;
        public const int ERROR_INVALID_DATA = 13;
        public const int ERROR_INVALID_DRIVE = 15;
        public const int ERROR_INVALID_FUNCTION = 1;
        public const int ERROR_INVALID_HANDLE = 6;
        public const int ERROR_INVALID_NAME = 0x7b;
        public const int ERROR_INVALID_OWNER = 0x51b;
        public const int ERROR_INVALID_PARAMETER = 0x57;
        public const int ERROR_INVALID_PRIMARY_GROUP = 0x51c;
        public const int ERROR_INVALID_SECURITY_DESCR = 0x53a;
        public const int ERROR_INVALID_SID = 0x539;
        public const int ERROR_MORE_DATA = 0xea;
        public const int ERROR_NO_DATA = 0xe8;
        public const int ERROR_NO_MORE_FILES = 0x12;
        public const int ERROR_NO_SECURITY_ON_OBJECT = 0x546;
        public const int ERROR_NO_SUCH_PRIVILEGE = 0x521;
        public const int ERROR_NO_TOKEN = 0x3f0;
        public const int ERROR_NON_ACCOUNT_SID = 0x4e9;
        public const int ERROR_NONE_MAPPED = 0x534;
        public const int ERROR_NOT_ALL_ASSIGNED = 0x514;
        public const int ERROR_NOT_ENOUGH_MEMORY = 8;
        public const int ERROR_NOT_READY = 0x15;
        public const int ERROR_NOT_SUPPORTED = 50;
        public const int ERROR_OPERATION_ABORTED = 0x3e3;
        public const int ERROR_PATH_NOT_FOUND = 3;
        public const int ERROR_PIPE_NOT_CONNECTED = 0xe9;
        public const int ERROR_PRIVILEGE_NOT_HELD = 0x522;
        public const int ERROR_SHARING_VIOLATION = 0x20;
        public const int ERROR_SUCCESS = 0;
        public const int ERROR_TRUSTED_RELATIONSHIP_FAILURE = 0x6fd;
        public const int ERROR_UNKNOWN_REVISION = 0x519;

        public const int
           LOGPIXELSX = 88,
           LOGPIXELSY = 90,
           LB_ERR = (-1),
           LB_ERRSPACE = (-2),
           LBN_SELCHANGE = 1,
           LBN_DBLCLK = 2,
           LB_ADDSTRING = 0x0180,
           LB_INSERTSTRING = 0x0181,
           LB_DELETESTRING = 0x0182,
           LB_RESETCONTENT = 0x0184,
           LB_SETSEL = 0x0185,
           LB_SETCURSEL = 0x0186,
           LB_GETSEL = 0x0187,
           LB_GETCARETINDEX = 0x019F,
           LB_GETCURSEL = 0x0188,
           LB_GETTEXT = 0x0189,
           LB_GETTEXTLEN = 0x018A,
           LB_GETTOPINDEX = 0x018E,
           LB_FINDSTRING = 0x018F,
           LB_GETSELCOUNT = 0x0190,
           LB_GETSELITEMS = 0x0191,
           LB_SETTABSTOPS = 0x0192,
           LB_SETHORIZONTALEXTENT = 0x0194,
           LB_SETCOLUMNWIDTH = 0x0195,
           LB_SETTOPINDEX = 0x0197,
           LB_GETITEMRECT = 0x0198,
           LB_SETITEMHEIGHT = 0x01A0,
           LB_GETITEMHEIGHT = 0x01A1,
           LB_FINDSTRINGEXACT = 0x01A2,
           LB_ITEMFROMPOINT = 0x01A9,
           LB_SETLOCALE = 0x01A5;

        public const int
            MEMBERID_NIL = (-1),
            MAX_PATH = 260,
            MAX_UNICODESTRING_LEN = short.MaxValue, // maximum unicode string length 
            MA_ACTIVATE = 0x0001,
            MA_ACTIVATEANDEAT = 0x0002,
            MA_NOACTIVATE = 0x0003,
            MA_NOACTIVATEANDEAT = 0x0004,
            MM_TEXT = 1,
            MM_ANISOTROPIC = 8,
            MK_LBUTTON = 0x0001,
            MK_RBUTTON = 0x0002,
            MK_SHIFT = 0x0004,
            MK_CONTROL = 0x0008,
            MK_MBUTTON = 0x0010,
            MNC_EXECUTE = 2,
            MNC_SELECT = 3,
            MIIM_STATE = 0x00000001,
            MIIM_ID = 0x00000002,
            MIIM_SUBMENU = 0x00000004,
            MIIM_TYPE = 0x00000010,
            MIIM_DATA = 0x00000020,
            MIIM_STRING = 0x00000040,
            MIIM_BITMAP = 0x00000080,
            MIIM_FTYPE = 0x00000100,
            MB_OK = 0x00000000,
            MF_BYCOMMAND = 0x00000000,
            MF_BYPOSITION = 0x00000400,
            MF_ENABLED = 0x00000000,
            MF_GRAYED = 0x00000001,
            MF_POPUP = 0x00000010,
            MF_SYSMENU = 0x00002000,
            MDIS_ALLCHILDSTYLES = 0x0001,
            MDITILE_VERTICAL = 0x0000,
            MDITILE_HORIZONTAL = 0x0001,
            MDITILE_SKIPDISABLED = 0x0002,
            MCM_SETMAXSELCOUNT = (0x1000 + 4),
            MCM_SETSELRANGE = (0x1000 + 6),
            MCM_GETMONTHRANGE = (0x1000 + 7),
            MCM_GETMINREQRECT = (0x1000 + 9),
            MCM_SETCOLOR = (0x1000 + 10),
            MCM_SETTODAY = (0x1000 + 12),
            MCM_GETTODAY = (0x1000 + 13),
            MCM_HITTEST = (0x1000 + 14),
            MCM_SETFIRSTDAYOFWEEK = (0x1000 + 15),
            MCM_SETRANGE = (0x1000 + 18),
            MCM_SETMONTHDELTA = (0x1000 + 20),
            MCM_GETMAXTODAYWIDTH = (0x1000 + 21),
            MCHT_TITLE = 0x00010000,
            MCHT_CALENDAR = 0x00020000,
            MCHT_TODAYLINK = 0x00030000,
            MCHT_TITLEBK = (0x00010000),
            MCHT_TITLEMONTH = (0x00010000 | 0x0001),
            MCHT_TITLEYEAR = (0x00010000 | 0x0002),
            MCHT_TITLEBTNNEXT = (0x00010000 | 0x01000000 | 0x0003),
            MCHT_TITLEBTNPREV = (0x00010000 | 0x02000000 | 0x0003),
            MCHT_CALENDARBK = (0x00020000),
            MCHT_CALENDARDATE = (0x00020000 | 0x0001),
            MCHT_CALENDARDATENEXT = ((0x00020000 | 0x0001) | 0x01000000),
            MCHT_CALENDARDATEPREV = ((0x00020000 | 0x0001) | 0x02000000),
            MCHT_CALENDARDAY = (0x00020000 | 0x0002),
            MCHT_CALENDARWEEKNUM = (0x00020000 | 0x0003),
            MCSC_TEXT = 1,
            MCSC_TITLEBK = 2,
            MCSC_TITLETEXT = 3,
            MCSC_MONTHBK = 4,
            MCSC_TRAILINGTEXT = 5,
            MCN_VIEWCHANGE = (0 - 750), // MCN_SELECT -4  - give state of calendar view
            MCN_SELCHANGE = ((0 - 750) + 1),
            MCN_GETDAYSTATE = ((0 - 750) + 3),
            MCN_SELECT = ((0 - 750) + 4),
            MCS_DAYSTATE = 0x0001,
            MCS_MULTISELECT = 0x0002,
            MCS_WEEKNUMBERS = 0x0004,
            MCS_NOTODAYCIRCLE = 0x0008,
            MCS_NOTODAY = 0x0010,
            MSAA_MENU_SIG = (unchecked((int)0xAA0DF00D));

        public const int URLZONE_LOCAL_MACHINE = 0;
        public const int URLZONE_INTRANET = URLZONE_LOCAL_MACHINE + 1;
        public const int URLZONE_TRUSTED = URLZONE_INTRANET + 1;
        public const int URLZONE_INTERNET = URLZONE_TRUSTED + 1;
        public const int URLZONE_UNTRUSTED = URLZONE_INTERNET + 1;

        public const int S_OK = 0x00000000;
        public const int S_FALSE = 0x00000001;

        public const int URLACTION_FEATURE_ZONE_ELEVATION = 0x00002101;
        public const int PUAF_NOUI = 0x00000001;
        public const int MUTZ_NOSAVEDFILECHECK = 0x00000001;

        #endregion

        public static class CommonHandles
        {
            public readonly static int Accelerator;
            public readonly static int Cursor;
            public readonly static int EMF;
            public readonly static int Find;
            public readonly static int GDI;
            public readonly static int HDC;
            public readonly static int Icon;
            public readonly static int Kernel;
            public readonly static int Menu;
            public readonly static int Window;

            static CommonHandles()
            {
                Accelerator = Internal_.HandleCollector.RegisterType("Accelerator", 80, 50);
                Cursor = Internal_.HandleCollector.RegisterType("Cursor", 20, 500);
                EMF = Internal_.HandleCollector.RegisterType("EnhancedMetaFile", 20, 500);
                Find = Internal_.HandleCollector.RegisterType("Find", 0, 1000);
                GDI = Internal_.HandleCollector.RegisterType("GDI", 50, 500);
                HDC = Internal_.HandleCollector.RegisterType("HDC", 100, 2);
                Icon = Internal_.HandleCollector.RegisterType("Icon", 20, 500);
                Kernel = Internal_.HandleCollector.RegisterType("Kernel", 0, 1000);
                Menu = Internal_.HandleCollector.RegisterType("Menu", 30, 1000);
                Window = Internal_.HandleCollector.RegisterType("Window", 5, 1000);
            }
        }

        [ComImport, Guid("79eac9ee-baf9-11ce-8c82-00aa004ba90b"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IInternetSecurityMgrSite
        {
            void GetWindow(/* [out] */ ref IntPtr phwnd);
            void EnableModeless(/* [in] */ bool fEnable);
        }

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [ComImport, ComVisible(false), Guid("79eac9ee-baf9-11ce-8c82-00aa004ba90b"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IInternetSecurityManager
        {
            void SetSecuritySite(IInternetSecurityMgrSite pSite);
            unsafe void GetSecuritySite( /* [out] */ void** ppSite);

            [SecurityCritical, SuppressUnmanagedCodeSecurity]
            void MapUrlToZone(
                [In, MarshalAs(UnmanagedType.BStr)] string pwszUrl,
                [Out] out int pdwZone,
                [In] int dwFlags);

            unsafe void GetSecurityId(  /* [in] */ string pwszUrl,
                                /* [size_is][out] */ byte* pbSecurityId,
                                /* [out][in] */ int* pcbSecurityId,
                                /* [in] */ int dwReserved);

            unsafe void ProcessUrlAction(
                                /* [in] */ string pwszUrl,
                                /* [in] */ int dwAction,
                                /* [size_is][out] */ byte* pPolicy,
                                /* [in] */ int cbPolicy,
                                /* [in] */ byte* pContext,
                                /* [in] */ int cbContext,
                                /* [in] */ int dwFlags,
                                /* [in] */ int dwReserved);

            unsafe void QueryCustomPolicy(
                                /* [in] */ string pwszUrl,
                                /* [in] */ /*REFGUID*/ void* guidKey,
                                /* [size_is][size_is][out] */ byte** ppPolicy,
                                /* [out] */ int* pcbPolicy,
                                /* [in] */ byte* pContext,
                                /* [in] */ int cbContext,
                                /* [in] */ int dwReserved);

            unsafe void SetZoneMapping( /* [in] */ int dwZone, /* [in] */ string lpszPattern, /* [in] */ int dwFlags);
            unsafe void GetZoneMappings( /* [in] */ int dwZone, /* [out] */ /*IEnumString*/ void** ppenumString, /* [in] */ int dwFlags);
        }

        ///<SecurityNote>
        /// Critical as this code performs an elevation.
        ///</SecurityNote>
        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", SetLastError = true, ExactSpelling = true, EntryPoint = "GetDC", CharSet = CharSet.Auto)]
        public static extern IntPtr IntGetDC(
            HandleRef hWnd);

        ///<SecurityNote>
        /// Critical as this code performs an elevation.
        ///</SecurityNote>
        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport("gdi32.dll", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern int GetDeviceCaps(
            HandleRef hDC,
            int nIndex);

        ///<SecurityNote>
        /// Critical as this code performs an elevation.The call to handle collector
        /// is by itself not dangerous because handle collector simply
        /// stores a count of the number of instances of a given handle and not the handle itself.
        ///</SecurityNote>
        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", ExactSpelling = true, EntryPoint = "ReleaseDC", CharSet = CharSet.Auto)]
        private static extern int IntReleaseDC(
            HandleRef hWnd,
            HandleRef hDC);

        ///<SecurityNote>
        /// Critical - performs an elevation.
        ///</SecurityNote>
        [SecurityCritical, SuppressUnmanagedCodeSecurity]
        [DllImport("urlmon.dll", ExactSpelling = true)]
        public static extern int CoInternetCreateSecurityManager(
            [MarshalAs(UnmanagedType.Interface)] object pIServiceProvider,
            [MarshalAs(UnmanagedType.Interface)] out object ppISecurityManager,
            int dwReserved);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [ResourceExposure(ResourceScope.None)]
        public static extern int GetModuleFileName(
            HandleRef hModule,
            StringBuilder buffer,
            int length);

        ///<SecurityNote>
        /// Critical as this code performs an elevation. The call to handle collector is
        /// by itself not dangerous because handle collector simply
        /// stores a count of the number of instances of a given
        /// handle and not the handle itself.
        ///</SecurityNote>
        [SecurityCritical]
        public static IntPtr GetDC(HandleRef hWnd)
        {
            IntPtr hDc = IntGetDC(hWnd);
            if (hDc == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            return Internal_.HandleCollector.Add(hDc, CommonHandles.HDC);
        }

        ///<SecurityNote>
        /// Critical as this code performs an elevation.
        ///</SecurityNote>
        [SecurityCritical]
        public static int ReleaseDC(HandleRef hWnd, HandleRef hDC)
        {
            Internal_.HandleCollector.Remove((IntPtr)hDC, CommonHandles.HDC);
            return IntReleaseDC(hWnd, hDC);
        }

        public static bool Failed(int hr)
        {
            return (hr < 0);
        }

        public static StringBuilder GetModuleFileNameLongPath(HandleRef hModule)
        {
            StringBuilder buffer = new StringBuilder(MAX_PATH);
            int noOfTimes = 1;
            int length = 0;
            // Iterating by allocating chunk of memory each time we find the length is not sufficient.
            // Performance should not be an issue for current MAX_PATH length due to this change.
            while (((length = GetModuleFileName(hModule, buffer, buffer.Capacity)) == buffer.Capacity)
                && Marshal.GetLastWin32Error() == ERROR_INSUFFICIENT_BUFFER
                && buffer.Capacity < MAX_UNICODESTRING_LEN)
            {
                noOfTimes += 2; // Increasing buffer size by 520 in each iteration.
                int capacity = noOfTimes * MAX_PATH < MAX_UNICODESTRING_LEN ? noOfTimes * MAX_PATH : MAX_UNICODESTRING_LEN;
                buffer.EnsureCapacity(capacity);
            }
            buffer.Length = length;
            return buffer;
        }
    }
}
