using MVVMCore.Win32;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Versioning;
using System.Security.Permissions;
using System.Text;

namespace MVVMCore.Windows
{
    /// <summary>
    /// Rozszerzenie klasy Application.
    /// </summary>
    public static class ApplicationEx
    {
        private static string executablePath;

        /// <summary>
        /// Gets the path for the executable file that started the application, including the executable name.
        /// </summary>
        public static string ExecutablePath
        {
            [ResourceExposure(ResourceScope.Machine)]
            [ResourceConsumption(ResourceScope.Machine)]
            get
            {
                if (executablePath == null)
                {
                    Assembly asm = Assembly.GetEntryAssembly();
                    if (asm == null)
                    {
                        StringBuilder sb = NativeMethods.GetModuleFileNameLongPath(NativeMethods.NullHandleRef);
                        executablePath = IntSecurity.UnsafeGetFullPath(sb.ToString());
                    }
                    else
                    {
                        String cb = asm.CodeBase;
                        Uri codeBase = new Uri(cb);
                        if (codeBase.IsFile)
                        {
                            executablePath = codeBase.LocalPath + Uri.UnescapeDataString(codeBase.Fragment);
                        }
                        else
                        {
                            executablePath = codeBase.ToString();
                        }
                    }
                }
                Uri exeUri = new Uri(executablePath);
                if (exeUri.Scheme == "file")
                {
                    Debug.WriteLineIf(IntSecurity.SecurityDemand.TraceVerbose, "FileIO(" + executablePath + ") Demanded");
                    new FileIOPermission(FileIOPermissionAccess.PathDiscovery, executablePath).Demand();
                }
                return executablePath;
            }
        }

        /// <summary>
        /// Pobiera wartość wskazującą czy System.ComponentModel.Component jest obecnie w trybie projektowy.
        /// </summary>
        /// <returns>true jeśli System.ComponentModel.Component jest w trybie projektowym, w przeciwnym razie false.</returns>
        public static bool DesignMode()
        {
            return ExecutablePath.ToLower().EndsWith("xdesproc.exe") || ExecutablePath.ToLower().EndsWith("devenv.exe");
        }
    }
}
