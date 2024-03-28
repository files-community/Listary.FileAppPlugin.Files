using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Listary.FileAppPlugin.Files
{
    internal static class Win32Utils
    {
        private enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool QueryFullProcessImageName([In] IntPtr hProcess, [In] int dwFlags, [Out] StringBuilder lpExeName, ref int lpdwSize);

        public static string GetProcessPathFromHwnd(IntPtr hWnd)
        {
            GetWindowThreadProcessId(hWnd, out uint pid);
            
            StringBuilder buffer = new StringBuilder(1024);
            IntPtr process = OpenProcess(ProcessAccessFlags.QueryLimitedInformation, false, (int)pid);
            if (process != IntPtr.Zero)
            {
                try
                {
                    int size = buffer.Capacity;
                    if (QueryFullProcessImageName(process, 0, buffer, ref size))
                    {
                        return buffer.ToString();
                    }
                }
                finally
                {
                    CloseHandle(process);
                }
            }
            
            return string.Empty;
        }
    }
}
