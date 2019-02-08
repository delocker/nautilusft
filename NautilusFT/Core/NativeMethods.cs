#region License

// ====================================================
// NautilusFT Project by shaliuno.
// 
// This program comes with ABSOLUTELY NO WARRANTY; This is free software,
// and you are welcome to redistribute it under certain conditions; See
// file LICENSE, which is part of this source code package, for details.
// 
// Let your braincells grow and neural connections never fade.
// Live long and prosper. (c) Spock
// ====================================================

#endregion

using System;
using System.Runtime.InteropServices;

namespace NautilusFT
{
    public static class NativeMethods
    {
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        #region memory

        [DllImport("kernel32.dll")]
        internal static extern IntPtr OpenProcess(int desiredAccess, bool inheritHandle, int processId);

        [DllImport("kernel32.dll")]
        internal static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll")]
        internal static extern bool ReadProcessMemory(int processHandle, long processBaseAddress, byte[] biffer, int size, ref int numberOfBytesRead);

        [DllImport("kernel32.dll")]
        internal static extern bool WriteProcessMemory(int processHandle, long processBaseAddress, byte[] biffer, int size, ref int numberOfBytesWritten);

        #endregion

        #region screen

        [DllImport("user32.dll")]
        internal static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        internal static extern bool GetWindowRect(IntPtr hwnd, out Rectangle rect);

        [DllImport("user32.dll")]
        internal static extern int ClientToScreen(IntPtr hwnd, out Point point);

        #endregion

        #region structs

        // easily overcomed by using System Drawing Point
        [StructLayout(LayoutKind.Sequential)]
        internal struct Rectangle
        {
            // https://msdn.microsoft.com/en-us/library/windows/desktop/dd162897(v=vs.85).aspx
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        // easily overcomed by using System Drawing Rectanble
        [StructLayout(LayoutKind.Sequential)]
        internal struct Point
        {
            // https://msdn.microsoft.com/en-us/library/windows/desktop/dd162805(v=vs.85).aspx
            public int x;
            public int y;
        }

        #endregion
    }
}
