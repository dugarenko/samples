namespace MVVMCore.Diagnostics
{
    /// <summary>
    /// Informacje o debuggerze.
    /// </summary>
	public static class DebugEx
    {
        /// <summary>
        /// Określa czy program uruchomiony jest w trybie DEBUG.
        /// </summary>
        public static bool IsDebug
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }
    }
}
