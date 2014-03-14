namespace MetroTwit.Extensions
{
    using System;
    using System.Runtime.InteropServices;

    public static class WinApi
    {
        public const int HWND_BROADCAST = 0xffff;
        public const int SW_SHOWNORMAL = 1;

        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);
        public static int RegisterWindowMessage(string format, params object[] args)
        {
            return RegisterWindowMessage(string.Format(format, args));
        }

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        public static void ShowToFront(IntPtr window)
        {
            ShowWindow(window, 1);
            SetForegroundWindow(window);
        }

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    }
}

