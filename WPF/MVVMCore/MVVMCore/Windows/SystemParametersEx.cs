using MVVMCore.Win32;
using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;

namespace MVVMCore.Windows
{
    public static class SystemParametersEx
    {
        private static int _dpiX;
        private static bool _setDpiX = true;
        private static BitArray _cacheValid = new BitArray((int)CacheSlot.NumSlots);

        private static object _dpiLock = new object();
        private static int _dpi;
        private static bool _dpiInitialized = false;

        private enum CacheSlot : int
        {
            DpiX,

            FocusBorderWidth,
            FocusBorderHeight,
            HighContrast,
            MouseVanish,

            DropShadow,
            FlatMenu,
            WorkAreaInternal,
            WorkArea,

            IconMetrics,

            KeyboardCues,
            KeyboardDelay,
            KeyboardPreference,
            KeyboardSpeed,
            SnapToDefaultButton,
            WheelScrollLines,
            MouseHoverTime,
            MouseHoverHeight,
            MouseHoverWidth,

            MenuDropAlignment,
            MenuFade,
            MenuShowDelay,

            ComboBoxAnimation,
            ClientAreaAnimation,
            CursorShadow,
            GradientCaptions,
            HotTracking,
            ListBoxSmoothScrolling,
            MenuAnimation,
            SelectionFade,
            StylusHotTracking,
            ToolTipAnimation,
            ToolTipFade,
            UIEffects,

            MinimizeAnimation,
            Border,
            CaretWidth,
            ForegroundFlashCount,
            DragFullWindows,
            NonClientMetrics,

            ThinHorizontalBorderHeight,
            ThinVerticalBorderWidth,
            CursorWidth,
            CursorHeight,
            ThickHorizontalBorderHeight,
            ThickVerticalBorderWidth,
            MinimumHorizontalDragDistance,
            MinimumVerticalDragDistance,
            FixedFrameHorizontalBorderHeight,
            FixedFrameVerticalBorderWidth,
            FocusHorizontalBorderHeight,
            FocusVerticalBorderWidth,
            FullPrimaryScreenWidth,
            FullPrimaryScreenHeight,
            HorizontalScrollBarButtonWidth,
            HorizontalScrollBarHeight,
            HorizontalScrollBarThumbWidth,
            IconWidth,
            IconHeight,
            IconGridWidth,
            IconGridHeight,
            MaximizedPrimaryScreenWidth,
            MaximizedPrimaryScreenHeight,
            MaximumWindowTrackWidth,
            MaximumWindowTrackHeight,
            MenuCheckmarkWidth,
            MenuCheckmarkHeight,
            MenuButtonWidth,
            MenuButtonHeight,
            MinimumWindowWidth,
            MinimumWindowHeight,
            MinimizedWindowWidth,
            MinimizedWindowHeight,
            MinimizedGridWidth,
            MinimizedGridHeight,
            MinimumWindowTrackWidth,
            MinimumWindowTrackHeight,
            PrimaryScreenWidth,
            PrimaryScreenHeight,
            WindowCaptionButtonWidth,
            WindowCaptionButtonHeight,
            ResizeFrameHorizontalBorderHeight,
            ResizeFrameVerticalBorderWidth,
            SmallIconWidth,
            SmallIconHeight,
            SmallWindowCaptionButtonWidth,
            SmallWindowCaptionButtonHeight,
            VirtualScreenWidth,
            VirtualScreenHeight,
            VerticalScrollBarWidth,
            VerticalScrollBarButtonHeight,
            WindowCaptionHeight,
            KanjiWindowHeight,
            MenuBarHeight,
            VerticalScrollBarThumbHeight,
            IsImmEnabled,
            IsMediaCenter,
            IsMenuDropRightAligned,
            IsMiddleEastEnabled,
            IsMousePresent,
            IsMouseWheelPresent,
            IsPenWindows,
            IsRemotelyControlled,
            IsRemoteSession,
            ShowSounds,
            IsSlowMachine,
            SwapButtons,
            IsTabletPC,
            VirtualScreenLeft,
            VirtualScreenTop,

            PowerLineStatus,

            IsGlassEnabled,
            UxThemeName,
            UxThemeColor,
            WindowCornerRadius,
            WindowGlassColor,
            WindowGlassBrush,
            WindowNonClientFrameThickness,
            WindowResizeBorderThickness,

            NumSlots
        }

        internal static int Dpi
        {
            [SecurityCritical, SecurityTreatAsSafe]
            get
            {
                if (!_dpiInitialized)
                {
                    lock (_dpiLock)
                    {
                        if (!_dpiInitialized)
                        {
                            HandleRef desktopWnd = new HandleRef(null, IntPtr.Zero);

                            // Win32Exception will get the Win32 error code so we don't have to
                            IntPtr dc = NativeMethods.GetDC(desktopWnd);

                            // Detecting error case from unmanaged call, required by PREsharp to throw a Win32Exception
                            if (dc == IntPtr.Zero)
                            {
                                throw new Win32Exception();
                            }

                            try
                            {
                                _dpi = NativeMethods.GetDeviceCaps(new HandleRef(null, dc), NativeMethods.LOGPIXELSY);
                                _dpiInitialized = true;
                            }
                            finally
                            {
                                NativeMethods.ReleaseDC(desktopWnd, new HandleRef(null, dc));
                            }
                        }
                    }
                }
                return _dpi;
            }
        }

        ///<SecurityNote>
        /// Critical as this accesses Native methods.
        /// TreatAsSafe - it would be ok to expose this information - DPI in partial trust
        ///</SecurityNote>
        internal static int DpiX
        {
            [SecurityCritical, SecurityTreatAsSafe]
            get
            {
                if (_setDpiX)
                {
                    lock (_cacheValid)
                    {
                        if (_setDpiX)
                        {
                            _setDpiX = false;
                            HandleRef desktopWnd = new HandleRef(null, IntPtr.Zero);

                            // Win32Exception will get the Win32 error code so we don't have to
#pragma warning disable 6523
                            IntPtr dc = NativeMethods.GetDC(desktopWnd);

                            // Detecting error case from unmanaged call, required by PREsharp to throw a Win32Exception
#pragma warning disable 6503
                            if (dc == IntPtr.Zero)
                            {
                                throw new Win32Exception();
                            }
#pragma warning restore 6503
#pragma warning restore 6523

                            try
                            {
                                _dpiX = NativeMethods.GetDeviceCaps(new HandleRef(null, dc), NativeMethods.LOGPIXELSX);
                                _cacheValid[(int)CacheSlot.DpiX] = true;
                            }
                            finally
                            {
                                NativeMethods.ReleaseDC(desktopWnd, new HandleRef(null, dc));
                            }
                        }
                    }
                }

                return _dpiX;
            }
        }
    }
}
