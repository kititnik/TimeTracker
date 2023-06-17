using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace TimeTracker.src
{
    public static class ProcessExtention
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(IntPtr hWnd);
        public static bool IsCorrect(this Process process)
        {
            try
            {
                return process.MainWindowHandle.ToInt32() != 0 &&
                       !String.IsNullOrEmpty(process.MainModule?.FileVersionInfo.FileDescription) &&
                       !IsIconic(process.MainWindowHandle) &&
                       !Constants.Exceptions.Contains(process.ProcessName);
            }
            catch (Win32Exception)
            {
                return false;
            }
        }
    }
}
