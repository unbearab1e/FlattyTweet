// Type: MetroTwit.Behaviors.Win32
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit.Extensions;
using System;
using System.Runtime.InteropServices;

namespace MetroTwit.Behaviors
{
  public class Win32
  {
    public const int GWL_STYLE = -16;
    public const int WS_SYSMENU = 524288;
    public const int SM_CXFRAME = 32;
    public const int SM_CXPADDEDBORDER = 92;

    [DllImport("shell32", CallingConvention = CallingConvention.StdCall)]
    internal static extern int SHAppBarMessage(int dwMessage, ref Win32.APPBARDATA pData);

    [DllImport("user32", SetLastError = true)]
    internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32")]
    internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

    [DllImport("User32")]
    internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

    [DllImport("User32")]
    internal static extern IntPtr MonitorFromPoint(POINT pt, int flags);

    [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true)]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    [DllImport("kernel32", EntryPoint = "GetProcAddress", CharSet = CharSet.Ansi, SetLastError = true)]
    private static extern IntPtr GetProcAddressByOrdinal(IntPtr hModule, ushort ordinal);

    [DllImport("kernel32", SetLastError = true)]
    private static extern IntPtr LoadLibrary(string lpFileName);

    public static bool IsDwmAvailable()
    {
      return !(Win32.LoadLibrary("dwmapi") == IntPtr.Zero);
    }

    [DllImport("dwmapi.dll", PreserveSig = false)]
    public static extern bool DwmIsCompositionEnabled();

    public static int DwmSetWindowAttribute(IntPtr hwnd, uint dwAttribute, ref IntPtr pvAttribute, uint cbAttribute)
    {
      IntPtr hModule = Win32.LoadLibrary("dwmapi");
      if (hModule == IntPtr.Zero)
        return 0;
      IntPtr procAddress = Win32.GetProcAddress(hModule, "DwmSetWindowAttribute");
      if (procAddress == IntPtr.Zero)
        return 0;
      else
        return ((Win32.DwmSetWindowAttributeDelegate) Marshal.GetDelegateForFunctionPointer(procAddress, typeof (Win32.DwmSetWindowAttributeDelegate)))(hwnd, dwAttribute, ref pvAttribute, cbAttribute);
    }

    public static int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref Win32.MARGINS margins)
    {
      IntPtr hModule = Win32.LoadLibrary("dwmapi");
      if (hModule == IntPtr.Zero)
        return 0;
      IntPtr procAddress = Win32.GetProcAddress(hModule, "DwmExtendFrameIntoClientArea");
      if (procAddress == IntPtr.Zero)
        return 0;
      else
        return ((Win32.DwmExtendFrameIntoClientAreaDelegate) Marshal.GetDelegateForFunctionPointer(procAddress, typeof (Win32.DwmExtendFrameIntoClientAreaDelegate)))(hwnd, ref margins);
    }

    public static uint DwmGetColorizationValue()
    {
      IntPtr hModule = Win32.LoadLibrary("dwmapi");
      uint pcfColorization = 0U;
      if (Environment.OSVersion.Version.Major >= 6 && Environment.OSVersion.Version.Minor >= 2)
      {
        Win32.DwmGetColorizationColorDelegate colorizationColorDelegate = (Win32.DwmGetColorizationColorDelegate) Marshal.GetDelegateForFunctionPointer(Win32.GetProcAddress(hModule, "DwmGetColorizationColor"), typeof (Win32.DwmGetColorizationColorDelegate));
        bool pfOpaqueBlend = false;
        int num = colorizationColorDelegate(ref pcfColorization, ref pfOpaqueBlend);
      }
      else
      {
        Win32.DwmGetColorizationParametersDelegate parametersDelegate = (Win32.DwmGetColorizationParametersDelegate) Marshal.GetDelegateForFunctionPointer(Win32.GetProcAddressByOrdinal(hModule, (ushort) sbyte.MaxValue), typeof (Win32.DwmGetColorizationParametersDelegate));
        Win32.DWMCOLORIZATIONPARAMS dwmColorParams = new Win32.DWMCOLORIZATIONPARAMS();
        int num = parametersDelegate(ref dwmColorParams);
        pcfColorization = dwmColorParams.ColorizationColor;
      }
      return pcfColorization;
    }

    [DllImport("user32.dll")]
    public static extern int GetSystemMetrics(int nIndex);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    public static extern IntPtr DefWindowProc(IntPtr hWnd, int uMsg, IntPtr wParam, IntPtr lParam);

    public struct MARGINS
    {
      public int leftWidth;
      public int rightWidth;
      public int topHeight;
      public int bottomHeight;
    }

    private delegate int DwmSetWindowAttributeDelegate(IntPtr hwnd, uint dwAttribute, ref IntPtr pvAttribute, uint cbAttribute);

    private delegate int DwmExtendFrameIntoClientAreaDelegate(IntPtr hwnd, ref Win32.MARGINS margins);

    private delegate int DwmGetColorizationParametersDelegate(ref Win32.DWMCOLORIZATIONPARAMS dwmColorParams);

    private delegate int DwmGetColorizationColorDelegate(ref uint pcfColorization, ref bool pfOpaqueBlend);

    public struct DWMCOLORIZATIONPARAMS
    {
      public uint ColorizationColor;
      public uint ColorizationAfterglow;
      public uint ColorizationColorBalance;
      public uint ColorizationAfterglowBalance;
      public uint ColorizationBlurBalance;
      public uint ColorizationGlassReflectionIntensity;
      public uint ColorizationOpaqueBlend;
    }

    public enum WindowMessages
    {
      WM_ERASEBKGND = 20,
      WM_GETMINMAXINFO = 36,
      WM_WINDOWPOSCHANGING = 70,
      WM_NCCALCSIZE = 131,
      WM_NCPAINT = 133,
      WM_NCACTIVATE = 134,
      WM_SYSCOMMAND = 274,
      WM_DWMCOMPOSITIONCHANGED = 798,
      WM_DWMCOLORIZATIONCOLORCHANGED = 800,
      SC_MINIMIZE = 61472,
      SC_RESTORE = 61728,
    }

    public enum wParams
    {
      SC_SIZE = 61440,
    }

    public enum ABEdge
    {
      ABE_LEFT,
      ABE_TOP,
      ABE_RIGHT,
      ABE_BOTTOM,
    }

    public enum ABMsg
    {
      ABM_NEW,
      ABM_REMOVE,
      ABM_QUERYPOS,
      ABM_SETPOS,
      ABM_GETSTATE,
      ABM_GETTASKBARPOS,
      ABM_ACTIVATE,
      ABM_GETAUTOHIDEBAR,
      ABM_SETAUTOHIDEBAR,
      ABM_WINDOWPOSCHANGED,
      ABM_SETSTATE,
    }

    public struct APPBARDATA
    {
      public int cbSize;
      public IntPtr hWnd;
      public int uCallbackMessage;
      public int uEdge;
      public RECT rc;
      public bool lParam;
    }
  }
}
