// Type: MetroTwit.View.NotificationView
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight.Messaging;
using MetroTwit;
using MetroTwit.Extensions;
using MetroTwit.Model;
using MetroTwit.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

namespace MetroTwit.View
{
    public partial class NotificationView : Window, IComponentConnector
  {
    private static Thread thread = (Thread) null;
    public static readonly DependencyProperty NotificationsProperty = DependencyProperty.Register("Notifications", typeof (ObservableCollection<NotificationControlViewModel>), typeof (NotificationView), (PropertyMetadata) new UIPropertyMetadata((object) new ObservableCollection<NotificationControlViewModel>()));
    private bool IsShowing = false;
    private MediaPlayer player = new MediaPlayer();
    private static NotificationView notificationwindow;
    private double dpiXfactor;
    private double dpiYfactor;
    private static Window w;
    private static IntPtr desktopHandle;
    private static IntPtr shellHandle;
    private static bool ThreadStarting;

    public ObservableCollection<NotificationControlViewModel> Notifications
    {
      get
      {
        return (ObservableCollection<NotificationControlViewModel>) this.GetValue(NotificationView.NotificationsProperty);
      }
      set
      {
        this.SetValue(NotificationView.NotificationsProperty, (object) value);
      }
    }

    static NotificationView()
    {
    }

    public NotificationView()
    {
      this.InitializeComponent();
      this.InitializeDPI();
      this.player.Open(new Uri("Resources/beep.mp3", UriKind.Relative));
      NotificationView.desktopHandle = NotificationView.GetDesktopWindow();
      NotificationView.shellHandle = NotificationView.GetShellWindow();
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) ViewModelMessages.CloseNotification, (Action<GenericMessage<object>>) (o =>
      {
        int local_0 = -1;
        if (o.Content.GetType() == typeof (NotificationControlViewModel))
        {
          local_0 = this.Notifications.IndexOf((NotificationControlViewModel) o.Content);
        }
        else
        {
          foreach (NotificationControlViewModel item_0 in (Collection<NotificationControlViewModel>) this.Notifications)
          {
            ++local_0;
            if (item_0.GetType() != typeof (NotificationControlViewModel))
              break;
          }
        }
        if (local_0 > -1)
        {
          this.Notifications[local_0] = (NotificationControlViewModel) null;
          this.Notifications.RemoveAt(local_0);
        }
        if (Enumerable.Count<NotificationControlViewModel>((IEnumerable<NotificationControlViewModel>) this.Notifications) != 0)
          return;
        this.IsShowing = false;
        this.Hide();
      }));
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) ViewModelMessages.Exit, (Action<GenericMessage<object>>) (o => NotificationView.w.Dispatcher.BeginInvoke((Action) (() =>
      {
        this.Close();
        NotificationView.w.Close();
        if (NotificationView.thread == null)
          return;
        NotificationView.thread.Abort();
      }), DispatcherPriority.ContextIdle, new object[0])));
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern IntPtr GetDesktopWindow();

    [DllImport("user32.dll")]
    private static extern IntPtr GetShellWindow();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowRect(IntPtr hwnd, out NotificationView.RECT rc);

    private void InitializeDPI()
    {
      System.Windows.Application.Current.Dispatcher.BeginInvoke((Action) (() =>
      {
        PresentationSource local_0 = CommonCommands.CurrentSource();
        this.dpiXfactor = local_0.CompositionTarget.TransformToDevice.M11;
        this.dpiYfactor = local_0.CompositionTarget.TransformToDevice.M22;
      }), DispatcherPriority.Normal, new object[0]);
    }

    public void AddNotification(List<MetroTwitStatusBase> newTweets, string notificationName, bool showtoasts, bool playsound)
    {
      List<MetroTwitStatusBase> newTweets1 = this.FilterTweets(newTweets);
      if (newTweets1.Count > 0 && showtoasts)
      {
        if (Enumerable.Count<NotificationControlViewModel>(Enumerable.Where<NotificationControlViewModel>((IEnumerable<NotificationControlViewModel>) this.Notifications, (Func<NotificationControlViewModel, bool>) (x => x.NotificationName == notificationName))) > 0)
        {
          foreach (NotificationControlViewModel controlViewModel in (Collection<NotificationControlViewModel>) this.Notifications)
          {
            if (controlViewModel.NotificationName == notificationName)
              controlViewModel.AddNewTweets(newTweets);
          }
        }
        else
        {
          this.Notifications.Insert(0, new NotificationControlViewModel(newTweets1)
          {
            NewCount = Enumerable.Count<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>) newTweets1),
            NotificationName = notificationName
          });
          Screen screen;
          try
          {
            screen = Enumerable.ToArray<Screen>((IEnumerable<Screen>) Enumerable.OrderBy<Screen, int>((IEnumerable<Screen>) Screen.AllScreens, (Func<Screen, int>) (x => x.Bounds.Left)))[SettingsData.Instance.NotificationScreen];
          }
          catch
          {
            screen = Enumerable.ToArray<Screen>((IEnumerable<Screen>) Enumerable.OrderBy<Screen, int>((IEnumerable<Screen>) Screen.AllScreens, (Func<Screen, int>) (x => x.Bounds.Left)))[0];
          }
          ItemsControl itemsControl = this.NotificationList;
          Rectangle workingArea = screen.WorkingArea;
          double width1 = (double) workingArea.Width;
          workingArea = screen.WorkingArea;
          double height1 = (double) workingArea.Height;
          System.Windows.Size availableSize = new System.Windows.Size(width1, height1);
          itemsControl.Measure(availableSize);
          if (this.NotificationList.DesiredSize.Height != double.NaN && this.NotificationList.DesiredSize.Width != double.NaN)
          {
            try
            {
                base.Height = this.NotificationList.DesiredSize.Height;
                base.Width = this.NotificationList.DesiredSize.Width;
            }
            catch
            {
            }
          }
          if (this.dpiXfactor == 0.0)
            this.dpiXfactor = 1.0;
          if (this.dpiYfactor == 0.0)
            this.dpiYfactor = 1.0;
          switch (SettingsData.Instance.NotificationPosition)
          {
            case NotificationPosition.TopLeft:
              NotificationView notificationView3 = this;
              workingArea = screen.WorkingArea;
              double num1 = (double) workingArea.Left / this.dpiXfactor;
              notificationView3.Left = num1;
              NotificationView notificationView4 = this;
              workingArea = screen.WorkingArea;
              double num2 = (double) workingArea.Top / this.dpiYfactor;
              notificationView4.Top = num2;
              break;
            case NotificationPosition.TopRight:
              NotificationView notificationView5 = this;
              workingArea = screen.WorkingArea;
              double num3 = ((double) workingArea.Right - this.Width * this.dpiXfactor) / this.dpiXfactor;
              notificationView5.Left = num3;
              NotificationView notificationView6 = this;
              workingArea = screen.WorkingArea;
              double num4 = (double) workingArea.Top / this.dpiYfactor;
              notificationView6.Top = num4;
              break;
            case NotificationPosition.BottomLeft:
              NotificationView notificationView7 = this;
              workingArea = screen.WorkingArea;
              double num5 = (double) workingArea.Left / this.dpiXfactor;
              notificationView7.Left = num5;
              NotificationView notificationView8 = this;
              workingArea = screen.WorkingArea;
              double num6 = ((double) workingArea.Bottom - this.Height * this.dpiYfactor) / this.dpiYfactor;
              notificationView8.Top = num6;
              break;
            case NotificationPosition.BottomRight:
              NotificationView notificationView9 = this;
              workingArea = screen.WorkingArea;
              double num7 = ((double) workingArea.Right - this.Width * this.dpiXfactor) / this.dpiXfactor;
              notificationView9.Left = num7;
              NotificationView notificationView10 = this;
              workingArea = screen.WorkingArea;
              double num8 = ((double) workingArea.Bottom - this.Height * this.dpiYfactor) / this.dpiYfactor;
              notificationView10.Top = num8;
              break;
          }
        }
      }
      if (!playsound)
        return;
      this.PlaySound();
    }

    private List<MetroTwitStatusBase> FilterTweets(List<MetroTwitStatusBase> newTweets)
    {
      return Enumerable.ToList<MetroTwitStatusBase>(Enumerable.Where<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>) newTweets, (Func<MetroTwitStatusBase, int, bool>) ((x, r) => Enumerable.Count<UserAccountViewModel>(Enumerable.Where<UserAccountViewModel>((IEnumerable<UserAccountViewModel>) App.AppState.Accounts, (Func<UserAccountViewModel, bool>) (a => a.TwitterAccountID == (x.RetweetUser == null ? x.User.Id : x.RetweetUser.Id)))) == 0 && x.UnRead && Enumerable.Count<NotificationControlViewModel>(Enumerable.Where<NotificationControlViewModel>((IEnumerable<NotificationControlViewModel>) this.Notifications, (Func<NotificationControlViewModel, int, bool>) ((y, r2) => Enumerable.Count<MetroTwitStatusBase>(Enumerable.Where<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>) y.newTweets, (Func<MetroTwitStatusBase, int, bool>) ((z, r3) => z != null && z.ID == x.ID))) > 0))) == 0)));
    }

    private void PlaySound()
    {
      if (SettingsData.Instance.PlayingSound)
        return;
      SettingsData.Instance.PlayingSound = true;
      this.player.Stop();
      this.player.Play();
      System.Timers.Timer soundtimer = new System.Timers.Timer(5000.0);
      soundtimer.Elapsed += (ElapsedEventHandler) ((sender, e) =>
      {
        soundtimer.Stop();
        SettingsData.Instance.PlayingSound = false;
      });
      soundtimer.Start();
    }

    public static void Notify(List<MetroTwitStatusBase> newTweets, string notificationName, bool showtoasts, bool playsound)
    {
      bool runningFullScreen = false;
      IntPtr foregroundWindow = NotificationView.GetForegroundWindow();
      if (!foregroundWindow.Equals((object) IntPtr.Zero))
      {
        IntPtr num1 = NotificationView.desktopHandle;
        IntPtr num2 = NotificationView.shellHandle;
        bool flag = 0 == 0;
        IntPtr handle = (PresentationSource.FromVisual((Visual) System.Windows.Application.Current.MainWindow) as HwndSource).Handle;
        if (!foregroundWindow.Equals((object) NotificationView.desktopHandle) && !foregroundWindow.Equals((object) NotificationView.shellHandle) && !foregroundWindow.Equals((object) handle))
        {
          NotificationView.RECT rc;
          NotificationView.GetWindowRect(foregroundWindow, out rc);
          Rectangle bounds = Screen.FromHandle(foregroundWindow).Bounds;
          runningFullScreen = rc.Bottom - rc.Top == bounds.Height && rc.Right - rc.Left == bounds.Width;
        }
        else
          runningFullScreen = false;
      }
      if (NotificationView.notificationwindow == null && NotificationView.w == null && !NotificationView.ThreadStarting)
      {
        NotificationView.ThreadStarting = true;
        Thread thread = new Thread((ThreadStart) (() =>
        {
          NotificationView.w = new Window();
          NotificationView.w.Top = -100.0;
          NotificationView.w.Left = -100.0;
          NotificationView.w.Width = 1.0;
          NotificationView.w.Height = 1.0;
          NotificationView.w.WindowStyle = WindowStyle.ToolWindow;
          NotificationView.w.Show();
          NotificationView.notificationwindow = new NotificationView();
          NotificationView.notificationwindow.Owner = NotificationView.w;
          NotificationView.w.Hide();
          NotificationView.w.Closed += (EventHandler) ((sender2, e2) => NotificationView.w.Dispatcher.InvokeShutdown());
          NotificationView.ThreadStarting = false;
          Dispatcher.Run();
        }));
        thread.SetApartmentState(ApartmentState.STA);
        thread.Name = "NotificationUIThread";
        thread.Start();
      }
      if (NotificationView.w == null || NotificationView.w.Dispatcher == null)
        return;
      NotificationView.w.Dispatcher.BeginInvoke((Action) (() =>
      {
        if (NotificationView.notificationwindow == null)
          return;
        if (!NotificationView.notificationwindow.IsShowing && !runningFullScreen)
        {
          NotificationView.notificationwindow.IsShowing = true;
          NotificationView.notificationwindow.Show();
        }
        NotificationView.notificationwindow.AddNotification(newTweets, notificationName, showtoasts, playsound);
      }), DispatcherPriority.ContextIdle, new object[0]);
    }

    

    public struct RECT
    {
      public int Left;
      public int Top;
      public int Right;
      public int Bottom;
    }
  }
}
