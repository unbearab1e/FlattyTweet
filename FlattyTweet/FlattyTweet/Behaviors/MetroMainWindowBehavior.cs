
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet;
using FlattyTweet.Extensions;
using FlattyTweet.Model;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using System.Diagnostics;

namespace FlattyTweet.Behaviors
{
  public class MetroMainWindowBehavior : Behavior<Window>
  {
    private bool m_shadowAttempted = false;
    private const int MONITOR_DEFAULTTOPRIMARY = 1;
    private const int MONITOR_DEFAULTTONEAREST = 2;
    public HwndSource hwndSource;
    private static double minWidth;
    private static double minHeight;
    private static POINT lastGoodMaxTrackSize;
    private static IntPtr lastGoodMonitor;
    private DispatcherTimer ChangeThemeTimer;
    private SharedResourceDictionary dict;

    public bool ShadowApplicationAttempted
    {
      get
      {
        return this.m_shadowAttempted;
      }
    }

    public void TryApplyShadow()
    {
      if (Environment.OSVersion.Version.Major >= 6 && Environment.OSVersion.Version.Minor < 2 && (Win32.IsDwmAvailable() && Win32.DwmIsCompositionEnabled()) && SystemParameters.DropShadow)
      {
        Win32.MARGINS margins;
        margins.bottomHeight = 1;
        margins.leftWidth = 1;
        margins.rightWidth = 1;
        margins.topHeight = 1;
        Win32.DwmExtendFrameIntoClientArea(new WindowInteropHelper(this.AssociatedObject).Handle, ref margins);
        this.AssociatedObject.BorderThickness = new Thickness(0.0);
      }
      else if (Environment.OSVersion.Version.Major >= 6 && Environment.OSVersion.Version.Minor >= 2 && (Win32.IsDwmAvailable() && Win32.DwmIsCompositionEnabled()) && SystemParameters.DropShadow)
      {
        Win32.MARGINS margins;
        margins.bottomHeight = 1;
        margins.leftWidth = 1;
        margins.rightWidth = 1;
        margins.topHeight = 1;
        Win32.DwmExtendFrameIntoClientArea(new WindowInteropHelper(this.AssociatedObject).Handle, ref margins);
        this.AssociatedObject.BorderThickness = new Thickness(1.0);
      }
      else
        this.AssociatedObject.BorderThickness = new Thickness(1.0);
      this.m_shadowAttempted = true;
    }

    protected override void OnAttached()
    {
      if (SettingsData.Instance.Accent == "Aero")
        CommonCommands.ChangeTheme(this.TryUpdateColorization());
      if (this.AssociatedObject.IsInitialized)
        this.AddHwndHook();
      else
        this.AssociatedObject.SourceInitialized += new EventHandler(this.AssociatedObject_SourceInitialized);
      this.AssociatedObject.WindowStyle = WindowStyle.None;
      this.AssociatedObject.ResizeMode = ResizeMode.CanResizeWithGrip;
      MetroMainWindowBehavior.minWidth = this.AssociatedObject.MinWidth;
      MetroMainWindowBehavior.minHeight = this.AssociatedObject.MinHeight;
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) ViewModelMessages.ChangeThemeToAero, (Action<GenericMessage<object>>) (o => CommonCommands.ChangeTheme(this.TryUpdateColorization())));
      base.OnAttached();
    }

    protected override void OnDetaching()
    {
      this.RemoveHwndHook();
      base.OnDetaching();
    }

    private void AddHwndHook()
    {
      try
      {
        this.hwndSource = PresentationSource.FromVisual((Visual) this.AssociatedObject) as HwndSource;
        this.hwndSource.AddHook(new HwndSourceHook(this.WndProc));
      }
      catch
      {
      }
    }

    private void RemoveHwndHook()
    {
      this.AssociatedObject.SourceInitialized -= new EventHandler(this.AssociatedObject_SourceInitialized);
      this.hwndSource.RemoveHook(new HwndSourceHook(this.WndProc));
    }

    private void AssociatedObject_SourceInitialized(object sender, EventArgs e)
    {
      this.AddHwndHook();
      PresentationSource presentationSource = PresentationSource.FromVisual((Visual) this.AssociatedObject);
      App.DpiXfactor = presentationSource.CompositionTarget.TransformToDevice.M11;
      App.DpiYfactor = presentationSource.CompositionTarget.TransformToDevice.M22;
    }

    [DebuggerStepThrough]
    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
      switch (msg)
      {
        case 70:
          if (!this.ShadowApplicationAttempted)
          {
            this.TryApplyShadow();
            break;
          }
          else
            break;
        case 131:
          if (wParam == new IntPtr(1))
          {
            handled = true;
            break;
          }
          else
            break;
        case 133:
          this.TryApplyShadow();
          break;
        case 134:
          IntPtr num = Win32.DefWindowProc(hwnd, msg, wParam, new IntPtr(-1));
          handled = true;
          return num;
        case 798:
          this.TryApplyShadow();
          handled = true;
          break;
        case 800:
          if (SettingsData.Instance.Accent == "Aero")
          {
            this.dict = this.TryUpdateColorization();
            if (this.ChangeThemeTimer == null)
            {
              this.ChangeThemeTimer = new DispatcherTimer(DispatcherPriority.Background, Application.Current.Dispatcher);
              this.ChangeThemeTimer.Interval = TimeSpan.FromMilliseconds(1000.0);
              this.ChangeThemeTimer.Tick += (EventHandler) ((s, a) =>
              {
                (s as DispatcherTimer).Stop();
                CommonCommands.ChangeTheme(this.dict);
              });
            }
            this.ChangeThemeTimer.Start();
          }
          handled = true;
          break;
        case 20:
          try
          {
            Graphics graphics = Graphics.FromHdc(wParam);
            if (SettingsData.Instance.Theme == "Light")
              graphics.Clear(System.Drawing.Color.White);
            else
              graphics.Clear(System.Drawing.Color.Black);
            handled = true;
            break;
          }
          catch
          {
            break;
          }
        case 36:
          MetroMainWindowBehavior.WmGetMinMaxInfo(hwnd, lParam);
          handled = true;
          break;
      }
      return IntPtr.Zero;
    }

    private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
    {
      MINMAXINFO mmi = (MINMAXINFO) Marshal.PtrToStructure(lParam, typeof (MINMAXINFO));
      IntPtr num = Win32.MonitorFromWindow(hwnd, 1);
      if (num != IntPtr.Zero)
      {
        MONITORINFO lpmi = new MONITORINFO();
        Win32.GetMonitorInfo(num, lpmi);
        FlattyTweet.Extensions.RECT rect1 = lpmi.rcWork;
        FlattyTweet.Extensions.RECT rect2 = lpmi.rcMonitor;
        mmi.ptMaxPosition.x = Math.Abs(rect1.left - rect2.left);
        mmi.ptMaxPosition.y = Math.Abs(rect1.top - rect2.top);
        mmi.ptMaxSize.x = Math.Abs(rect1.right - rect1.left);
        mmi.ptMaxSize.y = Math.Abs(rect1.bottom - rect1.top);
        mmi.ptMinTrackSize.x = (int) (MetroMainWindowBehavior.minWidth * App.DpiXfactor);
        mmi.ptMinTrackSize.y = (int) (MetroMainWindowBehavior.minHeight * App.DpiYfactor);
        if (Application.Current.MainWindow.WindowState != WindowState.Minimized)
        {
          mmi.ptMaxTrackSize.x = Math.Abs(rect1.right - rect1.left);
          mmi.ptMaxTrackSize.y = Math.Abs(rect1.bottom - rect1.top);
          MetroMainWindowBehavior.lastGoodMaxTrackSize = mmi.ptMaxTrackSize;
          MetroMainWindowBehavior.lastGoodMonitor = num;
        }
        else if (MetroMainWindowBehavior.lastGoodMaxTrackSize.x > 0 && MetroMainWindowBehavior.lastGoodMaxTrackSize.y > 0)
          mmi.ptMaxTrackSize = MetroMainWindowBehavior.lastGoodMaxTrackSize;
        mmi = MetroMainWindowBehavior.AdjustWorkingAreaForAutoHide(num, mmi);
      }
      Marshal.StructureToPtr((object) mmi, lParam, true);
    }

    private static MINMAXINFO AdjustWorkingAreaForAutoHide(IntPtr monitorContainingApplication, MINMAXINFO mmi)
    {
      IntPtr window = Win32.FindWindow("Shell_TrayWnd", (string) null);
      IntPtr num = Win32.MonitorFromWindow(window, 2);
      if (!monitorContainingApplication.Equals((object) num))
        return mmi;
      Win32.APPBARDATA pData = new Win32.APPBARDATA();
      pData.cbSize = Marshal.SizeOf((object) pData);
      pData.hWnd = window;
      Win32.SHAppBarMessage(5, ref pData);
      int edge = MetroMainWindowBehavior.GetEdge(pData.rc);
      if (!Convert.ToBoolean(Win32.SHAppBarMessage(4, ref pData)))
        return mmi;
      switch (edge)
      {
        case 0:
          mmi.ptMaxPosition.x += 2;
          mmi.ptMaxTrackSize.x -= 2;
          mmi.ptMaxSize.x -= 2;
          break;
        case 1:
          mmi.ptMaxPosition.y += 2;
          mmi.ptMaxTrackSize.y -= 2;
          mmi.ptMaxSize.y -= 2;
          break;
        case 2:
          mmi.ptMaxSize.x -= 2;
          mmi.ptMaxTrackSize.x -= 2;
          break;
        case 3:
          mmi.ptMaxSize.y -= 2;
          mmi.ptMaxTrackSize.y -= 2;
          break;
        default:
          return mmi;
      }
      return mmi;
    }

    private static int GetEdge(FlattyTweet.Extensions.RECT rc)
    {
      return rc.top != rc.left || rc.bottom <= rc.right ? (rc.top != rc.left || rc.bottom >= rc.right ? (rc.top <= rc.left ? 2 : 3) : 1) : 0;
    }

    public SharedResourceDictionary TryUpdateColorization()
    {
      if (Environment.OSVersion.Version.Major < 6 || !Win32.IsDwmAvailable() || !Win32.DwmIsCompositionEnabled())
        return (SharedResourceDictionary) null;
      uint colorizationValue = Win32.DwmGetColorizationValue();
      System.Windows.Media.Color color = System.Windows.Media.Color.FromArgb((byte) (colorizationValue >> 24), (byte) (colorizationValue >> 16), (byte) (colorizationValue >> 8), (byte) colorizationValue);
      double num1 = (double) color.A / (double) byte.MaxValue;
      color.A = byte.MaxValue;
      color.R = (byte) (int) (num1 * (double) color.R + (1.0 - num1) * (double) Colors.White.R);
      color.G = (byte) (int) (num1 * (double) color.G + (1.0 - num1) * (double) Colors.White.G);
      color.B = (byte) (int) (num1 * (double) color.B + (1.0 - num1) * (double) Colors.White.B);
      SharedResourceDictionary resourceDictionary = new SharedResourceDictionary();
      resourceDictionary.Add((object) "ModernColorFeature", (object) color);
      //color.A = (byte) ((double) color.A * 0.5);
      //resourceDictionary.Add((object) "ModernColorFeatureFade", (object) System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
      double num2 = 0.299 * (double) color.R + 0.587 * (double) color.G + 0.114 * (double) color.B;
      resourceDictionary.Add((object) "ModernColorFeatureContrast", (object) (num2 > 205.0 ? Colors.Black : Colors.White));
      return resourceDictionary;
    }
  }
}
